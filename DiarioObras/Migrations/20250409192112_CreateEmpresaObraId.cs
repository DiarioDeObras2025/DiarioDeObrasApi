using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiarioObras.Migrations
{
    /// <inheritdoc />
    public partial class CreateEmpresaObraId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adiciona a coluna EmpresaId (caso ainda não exista)
            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Obras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Cria o índice para a coluna EmpresaId
            migrationBuilder.CreateIndex(
                name: "IX_Obras_EmpresaId",
                table: "Obras",
                column: "EmpresaId");

            // Adiciona a foreign key relacionando com Empresas.Id
            migrationBuilder.AddForeignKey(
                name: "FK_Obras_Empresas_EmpresaId",
                table: "Obras",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
               name: "FK_Obras_Empresas_EmpresaId",
               table: "Obras");

            migrationBuilder.DropIndex(
                name: "IX_Obras_EmpresaId",
                table: "Obras");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Obras");
        }
    }
}
