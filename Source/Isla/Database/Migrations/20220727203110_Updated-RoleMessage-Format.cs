using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Isla.Database.Migrations
{
    public partial class UpdatedRoleMessageFormat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RoleMessages_Type",
                table: "RoleMessages");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "RoleMessages");

            migrationBuilder.AddColumn<long>(
                name: "Created",
                table: "RoleMessages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "RoleMessages");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "RoleMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RoleMessages_Type",
                table: "RoleMessages",
                column: "Type",
                unique: true);
        }
    }
}
