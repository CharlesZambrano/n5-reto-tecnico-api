using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N5.Permissions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeToPermissionsType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "PermissionTypes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionTypes_Code",
                table: "PermissionTypes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PermissionTypes_Code",
                table: "PermissionTypes");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PermissionTypes");
        }
    }
}
