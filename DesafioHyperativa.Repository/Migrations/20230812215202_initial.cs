using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DesafioHyperativa.Repository.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cartao",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "current_timestamp"),
                    DataUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "current_timestamp"),
                    NumeroCartao = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lote",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "current_timestamp"),
                    DataUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "current_timestamp"),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "current_timestamp"),
                    RegistroLote = table.Column<string>(type: "text", nullable: false),
                    QuantidadeRegistro = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lote", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lote_status",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "current_timestamp"),
                    DataUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "current_timestamp"),
                    Guid = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Erro = table.Column<string>(type: "text", nullable: false),
                    CodigoErro = table.Column<int>(type: "integer", nullable: true),
                    File = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lote_status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cartao_lote",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "current_timestamp"),
                    DataUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "current_timestamp"),
                    LoteId = table.Column<long>(type: "int8", nullable: false),
                    CartaoId = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartao_lote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cartao_lote_cartao_CartaoId",
                        column: x => x.CartaoId,
                        principalTable: "cartao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cartao_lote_lote_LoteId",
                        column: x => x.LoteId,
                        principalTable: "lote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cartao_NumeroCartao",
                table: "cartao",
                column: "NumeroCartao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cartao_lote_CartaoId",
                table: "cartao_lote",
                column: "CartaoId");

            migrationBuilder.CreateIndex(
                name: "IX_cartao_lote_LoteId",
                table: "cartao_lote",
                column: "LoteId");

            migrationBuilder.CreateIndex(
                name: "IX_lote_status_Guid",
                table: "lote_status",
                column: "Guid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cartao_lote");

            migrationBuilder.DropTable(
                name: "lote_status");

            migrationBuilder.DropTable(
                name: "cartao");

            migrationBuilder.DropTable(
                name: "lote");
        }
    }
}
