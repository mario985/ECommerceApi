using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceApi.Migrations
{
    /// <inheritdoc />
    public partial class inventoryupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Inventories",
                newName: "Reserved");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Order",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentConfirmedAt",
                table: "Order",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "Order",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OnHand",
                table: "Inventories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentConfirmedAt",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OnHand",
                table: "Inventories");

            migrationBuilder.RenameColumn(
                name: "Reserved",
                table: "Inventories",
                newName: "Quantity");
        }
    }
}
