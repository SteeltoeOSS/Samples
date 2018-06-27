using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.API.Migrations
{
    public partial class Domain_events : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_buyers_BuyerId",
                table: "orders");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethodId",
                table: "orders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "BuyerId",
                table: "orders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_orders_buyers_BuyerId",
                
                table: "orders",
                column: "BuyerId",
                principalTable: "buyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_buyers_BuyerId",
                table: "orders");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethodId",
                table: "orders",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuyerId",
                table: "orders",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_buyers_BuyerId",
                
                table: "orders",
                column: "BuyerId",
                principalTable: "buyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
