using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class added_clinicid_to_appointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "clinic_id",
                table: "appointments",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_clinic_id",
                table: "appointments",
                column: "clinic_id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_clinics_clinic_id",
                table: "appointments",
                column: "clinic_id",
                principalTable: "clinics",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_clinics_clinic_id",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_clinic_id",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "clinic_id",
                table: "appointments");
        }
    }
}
