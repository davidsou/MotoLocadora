using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable


namespace MotoLocadora.Infrastructure.Migrations.Application
{
    public partial class AlteracaoTipoAnoNaTabelaMotorcycle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""Motorcycles"" 
                SET ""Ano"" = '2024' 
                WHERE ""Ano"" !~ '^\d+$' OR ""Ano"" IS NULL;
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Motorcycles"" 
                ALTER COLUMN ""Ano"" TYPE integer USING ""Ano""::integer;
            ");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Motorcycles"" 
                ALTER COLUMN ""Ano"" TYPE text USING ""Ano""::text;
            ");
        }
    }
}