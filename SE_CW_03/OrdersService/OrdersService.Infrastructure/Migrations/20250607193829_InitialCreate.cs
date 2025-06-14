using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrdersService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "new-order-messages",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    payload = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    reserved = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_new-order-messages", x => x.order_id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    bill = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                    table.CheckConstraint("CK_orders_status_valid", "LOWER(status) IN ('neworder', 'awaitforpayment', 'completed', 'cancelednouserfound', 'cancelednofunds')");
                });

            migrationBuilder.CreateTable(
                name: "paid-order-messages",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    reserved = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paid-order-messages", x => x.order_id);
                    table.CheckConstraint("CK_paid-order-messages_status_valid", "LOWER(status) IN ('neworder', 'awaitforpayment', 'completed', 'cancelednouserfound', 'cancelednofunds')");
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_user_id",
                table: "orders",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "new-order-messages");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "paid-order-messages");
        }
    }
}
