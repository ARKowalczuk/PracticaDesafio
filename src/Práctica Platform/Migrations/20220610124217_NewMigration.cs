using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practica.API.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroDePedido = table.Column<long>(type: "bigint", nullable: true),
                    CicloDelPedido = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoDeContratoInterno = table.Column<long>(type: "bigint", nullable: false),
                    EstadoDelPedido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CuentaCorriente = table.Column<long>(type: "bigint", nullable: false),
                    Cuando = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pedidos");
        }
    }
}
