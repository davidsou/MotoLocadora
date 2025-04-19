using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoLocadora.Infrastructure.Migrations.Application
{
    /// <inheritdoc />
    public partial class AlteracaoNovoCampoRent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedPrice",
                table: "Rents",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedPrice",
                table: "Rents");
        }
    }
}
