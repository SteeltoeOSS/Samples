using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure;

namespace Catalog.API.Infrastructure.Migrations
{
    [DbContext(typeof(CatalogContext))]
    [Migration("20161103152832_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure.CatalogBrand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("Id");

                    b.ToTable("CatalogBrand");
                });

            modelBuilder.Entity("Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure.CatalogItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CatalogBrandId");

                    b.Property<int>("CatalogTypeId");

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("PictureUri");

                    b.Property<decimal>("Price");

                    b.HasKey("Id");

                    b.HasIndex("CatalogBrandId");

                    b.HasIndex("CatalogTypeId");

                    b.ToTable("Catalog");
                });

            modelBuilder.Entity("Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure.CatalogType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("Id");

                    b.ToTable("CatalogTypes");
                });

            modelBuilder.Entity("Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure.CatalogItem", b =>
                {
                    b.HasOne("Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure.CatalogBrand", "CatalogBrand")
                        .WithMany()
                        .HasForeignKey("CatalogBrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure.CatalogType", "CatalogType")
                        .WithMany()
                        .HasForeignKey("CatalogTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
