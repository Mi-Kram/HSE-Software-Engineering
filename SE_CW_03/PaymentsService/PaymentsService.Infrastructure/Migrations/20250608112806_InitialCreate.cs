using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PaymentsService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    caption = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    balance = table.Column<decimal>(type: "numeric(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "order-to-pay-messages",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    bill = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    reserved = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order-to-pay-messages", x => x.order_id);
                });

            migrationBuilder.CreateTable(
                name: "paid-order-messages",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payload = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    reserved = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paid-order-messages", x => x.order_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "order-to-pay-messages");

            migrationBuilder.DropTable(
                name: "paid-order-messages");
        }
    }
}
