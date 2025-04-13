using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiarioObras.Migrations
{
    /// <inheritdoc />
    public partial class AddIndicesToRegistroDiario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aprovado",
                table: "RegistroDiarios");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroDiarios_Data",
                table: "RegistroDiarios",
                column: "Data");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroDiarios_ObraId_Data",
                table: "RegistroDiarios",
                columns: new[] { "ObraId", "Data" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RegistroDiarios_Data",
                table: "RegistroDiarios");

            migrationBuilder.DropIndex(
                name: "IX_RegistroDiarios_ObraId_Data",
                table: "RegistroDiarios");

            migrationBuilder.AddColumn<bool>(
                name: "Aprovado",
                table: "RegistroDiarios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
