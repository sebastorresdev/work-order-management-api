using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkOrderManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkOrdersAndSupervisor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_branch_users_users_user_id",
                table: "branch_users");

            migrationBuilder.EnsureSchema(
                name: "business");

            migrationBuilder.AddColumn<Guid>(
                name: "supervisor_id",
                schema: "identity",
                table: "users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "work_orders",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticket_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    request_type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    client_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    client_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    client_phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    client_secondary_phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    district = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    address_reference = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    scheduled_date = table.Column<DateOnly>(type: "date", nullable: true),
                    scheduled_slot = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    assigned_technician_id = table.Column<Guid>(type: "uuid", nullable: true),
                    completed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    completion_notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    observation_notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    rejection_reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    cancellation_reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    last_modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    last_modified_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_work_orders_application_user_assigned_technician_id",
                        column: x => x.assigned_technician_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_work_orders_application_user_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_work_orders_branches_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "work_order_schedule_history",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    work_order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    scheduled_date = table.Column<DateOnly>(type: "date", nullable: false),
                    scheduled_slot = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    assigned_technician_id = table.Column<Guid>(type: "uuid", nullable: true),
                    notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    scheduled_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    scheduled_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_order_schedule_history", x => x.id);
                    table.ForeignKey(
                        name: "fk_work_order_schedule_history_users_assigned_technician_id",
                        column: x => x.assigned_technician_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_work_order_schedule_history_users_scheduled_by_user_id",
                        column: x => x.scheduled_by_user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_work_order_schedule_history_work_orders_work_order_id",
                        column: x => x.work_order_id,
                        principalSchema: "business",
                        principalTable: "work_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "work_order_status_history",
                schema: "business",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    work_order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status_from = table.Column<int>(type: "integer", nullable: false),
                    status_to = table.Column<int>(type: "integer", nullable: false),
                    comments = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    changed_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_order_status_history", x => x.id);
                    table.ForeignKey(
                        name: "fk_work_order_status_history_users_changed_by_user_id",
                        column: x => x.changed_by_user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_work_order_status_history_work_orders_work_order_id",
                        column: x => x.work_order_id,
                        principalSchema: "business",
                        principalTable: "work_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_users_supervisor_id",
                schema: "identity",
                table: "users",
                column: "supervisor_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_order_schedule_history_assigned_technician_id",
                schema: "business",
                table: "work_order_schedule_history",
                column: "assigned_technician_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_order_schedule_history_scheduled_by_user_id",
                schema: "business",
                table: "work_order_schedule_history",
                column: "scheduled_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_order_schedule_history_work_order_id",
                schema: "business",
                table: "work_order_schedule_history",
                column: "work_order_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_order_status_history_changed_by_user_id",
                schema: "business",
                table: "work_order_status_history",
                column: "changed_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_order_status_history_work_order_id",
                schema: "business",
                table: "work_order_status_history",
                column: "work_order_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_orders_assigned_technician_id",
                schema: "business",
                table: "work_orders",
                column: "assigned_technician_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_orders_branch_id",
                schema: "business",
                table: "work_orders",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_orders_created",
                schema: "business",
                table: "work_orders",
                column: "created");

            migrationBuilder.CreateIndex(
                name: "ix_work_orders_created_by_user_id",
                schema: "business",
                table: "work_orders",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_orders_request_type",
                schema: "business",
                table: "work_orders",
                column: "request_type");

            migrationBuilder.CreateIndex(
                name: "ix_work_orders_status",
                schema: "business",
                table: "work_orders",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_work_orders_ticket_number",
                schema: "business",
                table: "work_orders",
                column: "ticket_number",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_branch_users_application_user_user_id",
                table: "branch_users",
                column: "user_id",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_users_users_supervisor_id",
                schema: "identity",
                table: "users",
                column: "supervisor_id",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_branch_users_application_user_user_id",
                table: "branch_users");

            migrationBuilder.DropForeignKey(
                name: "fk_users_users_supervisor_id",
                schema: "identity",
                table: "users");

            migrationBuilder.DropTable(
                name: "work_order_schedule_history",
                schema: "business");

            migrationBuilder.DropTable(
                name: "work_order_status_history",
                schema: "business");

            migrationBuilder.DropTable(
                name: "work_orders",
                schema: "business");

            migrationBuilder.DropIndex(
                name: "ix_users_supervisor_id",
                schema: "identity",
                table: "users");

            migrationBuilder.DropColumn(
                name: "supervisor_id",
                schema: "identity",
                table: "users");

            migrationBuilder.AddForeignKey(
                name: "fk_branch_users_users_user_id",
                table: "branch_users",
                column: "user_id",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
