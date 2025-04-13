using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiarioObras.Migrations
{
    /// <inheritdoc />
    public partial class CreateEmpresaUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
             name: "EmpresaId",
             table: "AspNetUsers",
             type: "int",
             nullable: false,
             defaultValue: 0);

            // Cria a FK com a tabela Empresas
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmpresaId",
                table: "AspNetUsers",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Empresas_EmpresaId",
                table: "AspNetUsers",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
               name: "FK_AspNetUsers_Empresas_EmpresaId",
               table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmpresaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "AspNetUsers");
        }
    }
}
