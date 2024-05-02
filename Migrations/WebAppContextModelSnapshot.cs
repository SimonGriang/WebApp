﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Data;

#nullable disable

namespace WebApp.Migrations
{
    [DbContext(typeof(WebAppContext))]
    partial class WebAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApp.Models.Language", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isOriginLanguage")
                        .HasColumnType("bit");

                    b.Property<bool>("isTargetLanguage")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.ToTable("Language");
                });

            modelBuilder.Entity("WebApp.Models.Translation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int?>("OriginalLanguageID")
                        .HasColumnType("int");

                    b.Property<string>("OriginalText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TranslatedLanguageID")
                        .HasColumnType("int");

                    b.Property<string>("TranslatedText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("translated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("OriginalLanguageID");

                    b.HasIndex("TranslatedLanguageID");

                    b.ToTable("Translation");
                });

            modelBuilder.Entity("WebApp.Models.Translation", b =>
                {
                    b.HasOne("WebApp.Models.Language", "OriginalLanguage")
                        .WithMany()
                        .HasForeignKey("OriginalLanguageID");

                    b.HasOne("WebApp.Models.Language", "TranslatedLanguage")
                        .WithMany()
                        .HasForeignKey("TranslatedLanguageID");

                    b.Navigation("OriginalLanguage");

                    b.Navigation("TranslatedLanguage");
                });
#pragma warning restore 612, 618
        }
    }
}
