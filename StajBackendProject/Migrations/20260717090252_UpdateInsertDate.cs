using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajBackendProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInsertDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "InsertDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Users",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "InsertDate",
                table: "Users",
                newName: "CreatedAt");
        }
    }
}
