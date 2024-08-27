using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class changed_template_to_one_to_many : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "message_template_clinics");

            migrationBuilder.AddColumn<Guid>(
                name: "clinic_id",
                table: "message_templates",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_message_templates_clinic_id",
                table: "message_templates",
                column: "clinic_id");

            migrationBuilder.AddForeignKey(
                name: "FK_message_templates_clinics_clinic_id",
                table: "message_templates",
                column: "clinic_id",
                principalTable: "clinics",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_message_templates_clinics_clinic_id",
                table: "message_templates");

            migrationBuilder.DropIndex(
                name: "IX_message_templates_clinic_id",
                table: "message_templates");

            migrationBuilder.DropColumn(
                name: "clinic_id",
                table: "message_templates");

            migrationBuilder.CreateTable(
                name: "message_template_clinics",
                columns: table => new
                {
                    clinic_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    message_template_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message_template_clinics", x => new { x.clinic_id, x.message_template_id });
                    table.ForeignKey(
                        name: "FK_message_template_clinics_clinics_clinic_id",
                        column: x => x.clinic_id,
                        principalTable: "clinics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_message_template_clinics_message_templates_message_template_~",
                        column: x => x.message_template_id,
                        principalTable: "message_templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_message_template_clinics_message_template_id",
                table: "message_template_clinics",
                column: "message_template_id");
        }
    }
}
