using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoLocadora.Infrastructure.Migrations.Application
{
    /// <inheritdoc />
    public partial class AddUserIdToRider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Riders_Id",
                table: "Riders");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Riders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Riders_CommpanyId",
                table: "Riders",
                column: "CommpanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Riders_Id",
                table: "Riders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Riders_LicenseDrive",
                table: "Riders",
                column: "LicenseDrive",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Riders_UserId",
                table: "Riders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Riders_CommpanyId",
                table: "Riders");

            migrationBuilder.DropIndex(
                name: "IX_Riders_Id",
                table: "Riders");

            migrationBuilder.DropIndex(
                name: "IX_Riders_LicenseDrive",
                table: "Riders");

            migrationBuilder.DropIndex(
                name: "IX_Riders_UserId",
                table: "Riders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Riders");

            migrationBuilder.CreateIndex(
                name: "IX_Riders_Id",
                table: "Riders",
                column: "Id",
                unique: true);
        }
    }
}
