﻿// <auto-generated />
using System;
using FlexDevSagas.Services.Cinemas.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlexDevSagas.Services.Cinemas.Migrations
{
    [DbContext(typeof(CinemaContext))]
    partial class CinemaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FlexDevSagas.Services.Cinemas.Entities.Auditorium", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<Guid>("CinemaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CinemaId");

                    b.ToTable("Auditoriums");
                });

            modelBuilder.Entity("FlexDevSagas.Services.Cinemas.Entities.Cinema", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cinemas");
                });

            modelBuilder.Entity("FlexDevSagas.Services.Cinemas.Entities.Row", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<Guid>("Row_Auditorium")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Row_Auditorium");

                    b.ToTable("Rows");
                });

            modelBuilder.Entity("FlexDevSagas.Services.Cinemas.Entities.Seat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<Guid>("Seat_Row")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Seat_Row");

                    b.ToTable("Seats");
                });

            modelBuilder.Entity("FlexDevSagas.Services.Cinemas.Entities.Auditorium", b =>
                {
                    b.HasOne("FlexDevSagas.Services.Cinemas.Entities.Cinema", "Cinema")
                        .WithMany("Auditoriums")
                        .HasForeignKey("CinemaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cinema");
                });

            modelBuilder.Entity("FlexDevSagas.Services.Cinemas.Entities.Row", b =>
                {
                    b.HasOne("FlexDevSagas.Services.Cinemas.Entities.Auditorium", "Auditoirum")
                        .WithMany("Rows")
                        .HasForeignKey("Row_Auditorium")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auditoirum");
                });

            modelBuilder.Entity("FlexDevSagas.Services.Cinemas.Entities.Seat", b =>
                {
                    b.HasOne("FlexDevSagas.Services.Cinemas.Entities.Row", "Row")
                        .WithMany("Seats")
                        .HasForeignKey("Seat_Row")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Row");
                });

            modelBuilder.Entity("FlexDevSagas.Services.Cinemas.Entities.Auditorium", b =>
                {
                    b.Navigation("Rows");
                });

            modelBuilder.Entity("FlexDevSagas.Services.Cinemas.Entities.Cinema", b =>
                {
                    b.Navigation("Auditoriums");
                });

            modelBuilder.Entity("FlexDevSagas.Services.Cinemas.Entities.Row", b =>
                {
                    b.Navigation("Seats");
                });
#pragma warning restore 612, 618
        }
    }
}
