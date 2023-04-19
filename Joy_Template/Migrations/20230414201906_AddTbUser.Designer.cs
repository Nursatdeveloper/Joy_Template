﻿// <auto-generated />
using System;
using MVCTemplate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Joy_Template.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230414201906_AddTbUser")]
    partial class AddTbUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Joy_Template.Data.Tables.TbExceptions", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Exception")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsProcessed")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("tbexceptions", "system");
                });

            modelBuilder.Entity("Joy_Template.Data.Tables.TbLogs", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("tblogs", "system");
                });

            modelBuilder.Entity("Joy_Template.Data.Tables.TbRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RowVersion")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("tbrole", "user");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2023, 4, 15, 2, 19, 6, 466, DateTimeKind.Local).AddTicks(6726),
                            Role = "User",
                            RowVersion = 1
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2023, 4, 15, 2, 19, 6, 466, DateTimeKind.Local).AddTicks(6739),
                            Role = "Moderator",
                            RowVersion = 1
                        },
                        new
                        {
                            Id = 3L,
                            CreatedAt = new DateTime(2023, 4, 15, 2, 19, 6, 466, DateTimeKind.Local).AddTicks(6740),
                            Role = "Admin",
                            RowVersion = 1
                        });
                });

            modelBuilder.Entity("Joy_Template.Data.Tables.TbUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Fathername")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Iin")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RowVersion")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("tbusers", "user");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            BirthDate = new DateTime(2004, 4, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedAt = new DateTime(2023, 4, 14, 20, 19, 6, 661, DateTimeKind.Utc).AddTicks(4300),
                            Email = "kznursat#gmail.com",
                            Fathername = "Erzatuly",
                            Firstname = "Nursat",
                            Iin = "040524501037",
                            Lastname = "Zeinolla",
                            Password = "$2a$11$XRB011ZMPT8KK1IlW01lxuHp2aEnXcZIGbV/BDmVkiSURye.WtyQm",
                            Roles = "Moderator, Admin, User",
                            RowVersion = 1
                        });
                });
#pragma warning restore 612, 618
        }
    }
}