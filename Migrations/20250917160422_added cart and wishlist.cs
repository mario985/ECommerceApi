using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceApi.Migrations
{
    /// <inheritdoc />
    public partial class addedcartandwishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_AspNetUsers_UserId",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Cart_CartId",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_WishList_AspNetUsers_UserId",
                table: "WishList");

            migrationBuilder.DropForeignKey(
                name: "FK_WishListItem_WishList_WishListId",
                table: "WishListItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WishList",
                table: "WishList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cart",
                table: "Cart");

            migrationBuilder.RenameTable(
                name: "WishList",
                newName: "wishList");

            migrationBuilder.RenameTable(
                name: "Cart",
                newName: "cart");

            migrationBuilder.RenameIndex(
                name: "IX_WishList_UserId",
                table: "wishList",
                newName: "IX_wishList_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_UserId",
                table: "cart",
                newName: "IX_cart_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_wishList",
                table: "wishList",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cart",
                table: "cart",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_AspNetUsers_UserId",
                table: "cart",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_cart_CartId",
                table: "CartItem",
                column: "CartId",
                principalTable: "cart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_wishList_AspNetUsers_UserId",
                table: "wishList",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WishListItem_wishList_WishListId",
                table: "WishListItem",
                column: "WishListId",
                principalTable: "wishList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_AspNetUsers_UserId",
                table: "cart");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_cart_CartId",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_wishList_AspNetUsers_UserId",
                table: "wishList");

            migrationBuilder.DropForeignKey(
                name: "FK_WishListItem_wishList_WishListId",
                table: "WishListItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_wishList",
                table: "wishList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cart",
                table: "cart");

            migrationBuilder.RenameTable(
                name: "wishList",
                newName: "WishList");

            migrationBuilder.RenameTable(
                name: "cart",
                newName: "Cart");

            migrationBuilder.RenameIndex(
                name: "IX_wishList_UserId",
                table: "WishList",
                newName: "IX_WishList_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_cart_UserId",
                table: "Cart",
                newName: "IX_Cart_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WishList",
                table: "WishList",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cart",
                table: "Cart",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_AspNetUsers_UserId",
                table: "Cart",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Cart_CartId",
                table: "CartItem",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WishList_AspNetUsers_UserId",
                table: "WishList",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WishListItem_WishList_WishListId",
                table: "WishListItem",
                column: "WishListId",
                principalTable: "WishList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
