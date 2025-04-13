using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiarioObras.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCamposRegistroDiarioss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentoRegistro_RegistroDiarios_RegistroDiarioId",
                table: "DocumentoRegistro");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialUtilizado_RegistroDiarios_RegistroDiarioId",
                table: "MaterialUtilizado");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaterialUtilizado",
                table: "MaterialUtilizado");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentoRegistro",
                table: "DocumentoRegistro");

            migrationBuilder.RenameTable(
                name: "MaterialUtilizado",
                newName: "MaterialUtilizados");

            migrationBuilder.RenameTable(
                name: "DocumentoRegistro",
                newName: "DocumentoRegistros");

            migrationBuilder.RenameIndex(
                name: "IX_MaterialUtilizado_RegistroDiarioId",
                table: "MaterialUtilizados",
                newName: "IX_MaterialUtilizados_RegistroDiarioId");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentoRegistro_RegistroDiarioId",
                table: "DocumentoRegistros",
                newName: "IX_DocumentoRegistros_RegistroDiarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaterialUtilizados",
                table: "MaterialUtilizados",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentoRegistros",
                table: "DocumentoRegistros",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentoRegistros_RegistroDiarios_RegistroDiarioId",
                table: "DocumentoRegistros",
                column: "RegistroDiarioId",
                principalTable: "RegistroDiarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialUtilizados_RegistroDiarios_RegistroDiarioId",
                table: "MaterialUtilizados",
                column: "RegistroDiarioId",
                principalTable: "RegistroDiarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentoRegistros_RegistroDiarios_RegistroDiarioId",
                table: "DocumentoRegistros");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialUtilizados_RegistroDiarios_RegistroDiarioId",
                table: "MaterialUtilizados");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaterialUtilizados",
                table: "MaterialUtilizados");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentoRegistros",
                table: "DocumentoRegistros");

            migrationBuilder.RenameTable(
                name: "MaterialUtilizados",
                newName: "MaterialUtilizado");

            migrationBuilder.RenameTable(
                name: "DocumentoRegistros",
                newName: "DocumentoRegistro");

            migrationBuilder.RenameIndex(
                name: "IX_MaterialUtilizados_RegistroDiarioId",
                table: "MaterialUtilizado",
                newName: "IX_MaterialUtilizado_RegistroDiarioId");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentoRegistros_RegistroDiarioId",
                table: "DocumentoRegistro",
                newName: "IX_DocumentoRegistro_RegistroDiarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaterialUtilizado",
                table: "MaterialUtilizado",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentoRegistro",
                table: "DocumentoRegistro",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentoRegistro_RegistroDiarios_RegistroDiarioId",
                table: "DocumentoRegistro",
                column: "RegistroDiarioId",
                principalTable: "RegistroDiarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialUtilizado_RegistroDiarios_RegistroDiarioId",
                table: "MaterialUtilizado",
                column: "RegistroDiarioId",
                principalTable: "RegistroDiarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
