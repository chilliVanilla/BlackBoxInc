using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackBoxInc.Migrations
{
    /// <inheritdoc />
    public partial class IDUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Carts_CartID",
                table: "CartItem");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Products",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Carts",
                newName: "CartId");

            migrationBuilder.RenameColumn(
                name: "CartID",
                table: "CartItem",
                newName: "CartId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CartItem",
                newName: "CartItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItem_CartID",
                table: "CartItem",
                newName: "IX_CartItem_CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Carts_CartId",
                table: "CartItem",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "CartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Carts_CartId",
                table: "CartItem");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Products",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "Carts",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "CartItem",
                newName: "CartID");

            migrationBuilder.RenameColumn(
                name: "CartItemId",
                table: "CartItem",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem",
                newName: "IX_CartItem_CartID");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Carts_CartID",
                table: "CartItem",
                column: "CartID",
                principalTable: "Carts",
                principalColumn: "ID");
        }
    }
}
