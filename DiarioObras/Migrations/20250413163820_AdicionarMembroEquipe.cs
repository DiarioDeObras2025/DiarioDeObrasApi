using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiarioObras.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarMembroEquipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalFuncionarios",
                table: "RegistroDiarios");

            migrationBuilder.DropColumn(
                name: "TotalTerceirizados",
                table: "RegistroDiarios");

            migrationBuilder.CreateTable(
                name: "MembroEquipe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cargo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observacao = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Terceirizado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RegistroDiarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembroEquipe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembroEquipe_RegistroDiarios_RegistroDiarioId",
                        column: x => x.RegistroDiarioId,
                        principalTable: "RegistroDiarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MembroEquipe_RegistroDiarioId",
                table: "MembroEquipe",
                column: "RegistroDiarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MembroEquipe");

            migrationBuilder.AddColumn<int>(
                name: "TotalFuncionarios",
                table: "RegistroDiarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalTerceirizados",
                table: "RegistroDiarios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
