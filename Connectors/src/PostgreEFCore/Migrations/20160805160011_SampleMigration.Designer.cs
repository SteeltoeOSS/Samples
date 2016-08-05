using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PostgreEFCore;

namespace PostgreEFCore.Migrations
{
    [DbContext(typeof(TestContext))]
    [Migration("20160805160011_SampleMigration")]
    partial class SampleMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("PostgreEFCore.TestData", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Data");

                    b.HasKey("Id");

                    b.ToTable("TestData");
                });
        }
    }
}
