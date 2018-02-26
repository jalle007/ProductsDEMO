using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ProductsApp.Dal;
using ProductsApp.Dal.Entity;

namespace ProductsApp.Dal.Migrations
{
    [DbContext(typeof(OnFeetContext))]
    [Migration("20170619060651_changeset02")]
    partial class changeset02
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProductsApp.Dal.Entity.Image", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("DeviceToken");

                    b.Property<int>("DeviceType");

                    b.Property<string>("ImageUrl");

                    b.Property<int>("Platform");

                    b.Property<string>("ProfileUrl");

                    b.Property<string>("Sku");

                    b.Property<string>("Title");

                    b.Property<long>("UserId");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("ProductsApp.Dal.Entity.Like", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("ImageId");

                    b.Property<int>("Platform");

                    b.Property<DateTimeOffset>("Timestamp");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("ProductsApp.Dal.Entity.Like", b =>
                {
                    b.HasOne("ProductsApp.Dal.Entity.Image", "Image")
                        .WithMany("Likes")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
