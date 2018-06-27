using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Ordering.API.Migrations
{
    public partial class AdressAsValueObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_address_AddressId",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_paymentmethods_PaymentMethodId",
                table: "orders");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropIndex(
                name: "IX_orders_AddressId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "orders");

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "orders",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "orders",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                
                table: "orders",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Street",
                table: "orders",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_ZipCode",
                table: "orders",
                type: "nvarchar(500)",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_paymentmethods_PaymentMethodId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "Address_State",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "Address_Street",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "Address_ZipCode",
                table: "orders");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "orders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_AddressId",
                table: "orders",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_address_AddressId",
                table: "orders",
                column: "AddressId",
                principalTable: "address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
