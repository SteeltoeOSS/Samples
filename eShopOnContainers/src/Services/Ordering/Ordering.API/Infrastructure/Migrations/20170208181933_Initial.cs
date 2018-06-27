using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ordering.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

           migrationBuilder.CreateTable(
                name: "buyers",
                
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false).Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityGuid = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_buyers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cardtypes",
                
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cardtypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "address",
                
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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

            migrationBuilder.CreateTable(
                name: "orderstatus",
                
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderstatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "paymentmethods",
                
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false).Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Alias = table.Column<string>(maxLength: 200, nullable: false),
                    BuyerId = table.Column<int>(nullable: false),
                    CardHolderName = table.Column<string>(maxLength: 200, nullable: false),
                    CardNumber = table.Column<string>(maxLength: 25, nullable: false),
                    CardTypeId = table.Column<int>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentmethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_paymentmethods_buyers_BuyerId",
                        column: x => x.BuyerId,
                        
                        principalTable: "buyers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_paymentmethods_cardtypes_CardTypeId",
                        column: x => x.CardTypeId,
                        
                        principalTable: "cardtypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false).Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AddressId = table.Column<int>(nullable: true),
                    BuyerId = table.Column<int>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    OrderStatusId = table.Column<int>(nullable: false),
                    PaymentMethodId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orders_address_AddressId",
                        column: x => x.AddressId,
                        
                        principalTable: "address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_buyers_BuyerId",
                        column: x => x.BuyerId,
                        
                        principalTable: "buyers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orders_orderstatus_OrderStatusId",
                        column: x => x.OrderStatusId,
                        
                        principalTable: "orderstatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orders_paymentmethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        
                        principalTable: "paymentmethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "orderItems",
                
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false).Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Discount = table.Column<decimal>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    PictureUrl = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    Units = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orderItems_orders_OrderId",
                        column: x => x.OrderId,
                        
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_buyers_IdentityGuid",
                
                table: "buyers",
                column: "IdentityGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_paymentmethods_BuyerId",
                
                table: "paymentmethods",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_paymentmethods_CardTypeId",
                
                table: "paymentmethods",
                column: "CardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_AddressId",
                
                table: "orders",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_BuyerId",
                
                table: "orders",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_OrderStatusId",
                
                table: "orders",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_PaymentMethodId",
                
                table: "orders",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_orderItems_OrderId",
                
                table: "orderItems",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orderItems");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "orderstatus");

            migrationBuilder.DropTable(
                name: "paymentmethods");

            migrationBuilder.DropTable(
                name: "buyers");

            migrationBuilder.DropTable(
                name: "cardtypes");

          
        }
    }
}
