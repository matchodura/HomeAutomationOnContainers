﻿// <auto-generated />
using System;
using Logging.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Logging.API.Migrations
{
    [DbContext(typeof(RpiDataContext))]
    partial class RpiDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Entities.Configuration.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FriendlyName")
                        .HasColumnType("text");

                    b.Property<int>("Function")
                        .HasColumnType("integer");

                    b.Property<string>("IPAddress")
                        .HasColumnType("text");

                    b.Property<string>("MosquittoPassword")
                        .HasColumnType("text");

                    b.Property<string>("MosquittoUsername")
                        .HasColumnType("text");

                    b.Property<string>("Room")
                        .HasColumnType("text");

                    b.Property<string>("TasmotaDevice")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Entities.DHT.DHT", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("DewPoint")
                        .HasColumnType("double precision");

                    b.Property<double>("Humidity")
                        .HasColumnType("double precision");

                    b.Property<string>("SensorName")
                        .HasColumnType("text");

                    b.Property<double>("Temperature")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("DHTs");
                });

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

                    b.Property<double>("TimestampLinux")
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
