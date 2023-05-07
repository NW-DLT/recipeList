﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using recipeList.Data;

#nullable disable

namespace recipeList.Migrations
{
    [DbContext(typeof(AppDBContext))]
    partial class AppDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProductRecipe", b =>
                {
                    b.Property<int>("Recipesid")
                        .HasColumnType("int");

                    b.Property<int>("productsid")
                        .HasColumnType("int");

                    b.HasKey("Recipesid", "productsid");

                    b.HasIndex("productsid");

                    b.ToTable("ProductRecipe");
                });

            modelBuilder.Entity("recipeList.Models.Product", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("products");
                });

            modelBuilder.Entity("recipeList.Models.Recipe", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("Userid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("Userid");

                    b.ToTable("recipes");
                });

            modelBuilder.Entity("recipeList.Models.User", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("ProductRecipe", b =>
                {
                    b.HasOne("recipeList.Models.Recipe", null)
                        .WithMany()
                        .HasForeignKey("Recipesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("recipeList.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("productsid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("recipeList.Models.Recipe", b =>
                {
                    b.HasOne("recipeList.Models.User", null)
                        .WithMany("recipes")
                        .HasForeignKey("Userid");
                });

            modelBuilder.Entity("recipeList.Models.User", b =>
                {
                    b.Navigation("recipes");
                });
#pragma warning restore 612, 618
        }
    }
}