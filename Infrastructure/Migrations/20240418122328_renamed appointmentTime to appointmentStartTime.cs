using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class renamedappointmentTimetoappointmentStartTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "appointment_time",
                table: "appointments",
                newName: "appointment_start_time");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "appointment_start_time",
                table: "appointments",
                newName: "appointment_time");
        }
    }
}
