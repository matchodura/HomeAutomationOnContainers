﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RPI.API.Data;

#nullable disable

namespace RPI.API.Migrations
{
    [DbContext(typeof(RpiDataContext))]
    [Migration("20220109162500_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Entities.Mijia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Battery")
                        .HasColumnType("integer");

                    b.Property<float>("Humidity")
                        .HasColumnType("real");

                    b.Property<string>("MacAddress")
                        .HasColumnType("text");

                    b.Property<string>("SensorName")
                        .HasColumnType("text");

                    b.Property<float>("Temperature")
                        .HasColumnType("real");

                    b.Property<double>("Timestamp")
                        .HasColumnType("double precision");

                    b.Property<float>("Voltage")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Mijias");
                });
#pragma warning restore 612, 618
        }
    }
}
