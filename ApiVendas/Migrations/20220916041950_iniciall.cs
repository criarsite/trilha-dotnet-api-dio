using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiVendas.Migrations
{
    public partial class iniciall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VendedorId",
                table: "Vendedores",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "StatusDisponíveis",
                table: "Pedidos",
                newName: "StatusDisponiveis");

            migrationBuilder.RenameColumn(
                name: "PedidoId",
                table: "Pedidos",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Vendedores",
                newName: "VendedorId");

            migrationBuilder.RenameColumn(
                name: "StatusDisponiveis",
                table: "Pedidos",
                newName: "StatusDisponíveis");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Pedidos",
                newName: "PedidoId");
        }
    }
}
