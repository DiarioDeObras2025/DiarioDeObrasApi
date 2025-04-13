using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiarioObras.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCamposRegistroDiario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AreaExecutada",
                table: "RegistroDiarios",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ConsumoCimento",
                table: "RegistroDiarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Equipamentos",
                table: "RegistroDiarios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Etapa",
                table: "RegistroDiarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HorasTrabalhadas",
                table: "RegistroDiarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Ocorrencias",
                table: "RegistroDiarios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "PercentualConcluido",
                table: "RegistroDiarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Precipitacao",
                table: "RegistroDiarios",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Temperatura",
                table: "RegistroDiarios",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentoRegistro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomeArquivo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CaminhoArquivo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegistroDiarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoRegistro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentoRegistro_RegistroDiarios_RegistroDiarioId",
                        column: x => x.RegistroDiarioId,
                        principalTable: "RegistroDiarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MaterialUtilizado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegistroDiarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialUtilizado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialUtilizado_RegistroDiarios_RegistroDiarioId",
                        column: x => x.RegistroDiarioId,
                        principalTable: "RegistroDiarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoRegistro_RegistroDiarioId",
                table: "DocumentoRegistro",
                column: "RegistroDiarioId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialUtilizado_RegistroDiarioId",
                table: "MaterialUtilizado",
                column: "RegistroDiarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentoRegistro");

            migrationBuilder.DropTable(
                name: "MaterialUtilizado");

            migrationBuilder.DropColumn(
                name: "AreaExecutada",
                table: "RegistroDiarios");

            migrationBuilder.DropColumn(
                name: "ConsumoCimento",
                table: "RegistroDiarios");

            migrationBuilder.DropColumn(
                name: "Equipamentos",
                table: "RegistroDiarios");

            migrationBuilder.DropColumn(
                name: "Etapa",
                table: "RegistroDiarios");

            migrationBuilder.DropColumn(
                name: "HorasTrabalhadas",
                table: "RegistroDiarios");

            migrationBuilder.DropColumn(
                name: "Ocorrencias",
                table: "RegistroDiarios");

            migrationBuilder.DropColumn(
                name: "PercentualConcluido",
                table: "RegistroDiarios");

            migrationBuilder.DropColumn(
                name: "Precipitacao",
                table: "RegistroDiarios");

            migrationBuilder.DropColumn(
                name: "Temperatura",
                table: "RegistroDiarios");
        }
    }
}
