using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentsService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialUtils : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION try_reserve_order_to_pay_message(interval_seconds INT)
RETURNS TABLE(order_id UUID)
LANGUAGE plpgsql
AS $$
DECLARE
    rec RECORD;
    v_reserved_until TIMESTAMP := now() + make_interval(secs => interval_seconds);
BEGIN
    FOR rec IN
        WITH grouped AS (
            SELECT user_id, MIN(created_at) AS min_created
            FROM ""order-to-pay-messages""
            GROUP BY user_id
        ),
        earliest_per_user AS (
            SELECT o.order_id
            FROM ""order-to-pay-messages"" o
            JOIN grouped g ON o.user_id = g.user_id AND o.created_at = g.min_created
            WHERE o.reserved <= now()
        )
        SELECT o.order_id
        FROM ""order-to-pay-messages"" o
        WHERE o.order_id IN (SELECT e.order_id FROM earliest_per_user e)
        FOR UPDATE SKIP LOCKED
    LOOP
        UPDATE ""order-to-pay-messages"" o
        SET reserved = v_reserved_until
        WHERE o.order_id = rec.order_id
        RETURNING o.order_id::uuid INTO order_id;

        IF FOUND THEN
            RETURN QUERY SELECT order_id;
            RETURN;
        END IF;
    END LOOP;

    RETURN;
END;
$$;");

            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION try_reserve_paid_order_message(interval_seconds INT)
RETURNS TABLE(order_id UUID)
LANGUAGE plpgsql
AS $$
DECLARE
    rec RECORD;
    v_reserved_until TIMESTAMP := now() + make_interval(secs => interval_seconds);
BEGIN
    FOR rec IN
        WITH earliest AS (
            SELECT o.order_id
            FROM ""paid-order-messages"" o
            WHERE o.reserved <= now()
        )
        SELECT o.order_id
        FROM ""paid-order-messages"" o
        WHERE o.order_id IN (SELECT e.order_id FROM earliest e)
        FOR UPDATE SKIP LOCKED
    LOOP
        UPDATE ""paid-order-messages"" o
        SET reserved = v_reserved_until
        WHERE o.order_id = rec.order_id
        RETURNING o.order_id::uuid INTO order_id;

        IF FOUND THEN
            RETURN QUERY SELECT order_id;
            RETURN;
        END IF;
    END LOOP;

    RETURN;
END;
$$;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS try_reserve_order_to_pay_message;");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS try_reserve_paid_order_message;");
        }
    }
}
