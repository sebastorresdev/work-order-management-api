using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkOrderManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixPendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_work_orders_branch_id_status_created",
                schema: "business",
                table: "work_orders",
                columns: new[] { "branch_id", "status", "created" });

            migrationBuilder.CreateIndex(
                name: "ix_work_orders_created_by_user_id_status_created",
                schema: "business",
                table: "work_orders",
                columns: new[] { "created_by_user_id", "status", "created" });

            migrationBuilder.CreateIndex(
                name: "ix_work_orders_status_request_type_created",
                schema: "business",
                table: "work_orders",
                columns: new[] { "status", "request_type", "created" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_work_orders_branch_id_status_created",
                schema: "business",
                table: "work_orders");

            migrationBuilder.DropIndex(
                name: "ix_work_orders_created_by_user_id_status_created",
                schema: "business",
                table: "work_orders");

            migrationBuilder.DropIndex(
                name: "ix_work_orders_status_request_type_created",
                schema: "business",
                table: "work_orders");
        }
    }
}
