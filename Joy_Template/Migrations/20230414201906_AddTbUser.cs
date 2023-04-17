using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Joy_Template.Migrations
{
    public partial class AddTbUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbtest",
                schema: "application");

            migrationBuilder.EnsureSchema(
                name: "user");

            migrationBuilder.CreateTable(
                name: "tbrole",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Role = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RowVersion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbrole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbusers",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Firstname = table.Column<string>(type: "text", nullable: false),
                    Lastname = table.Column<string>(type: "text", nullable: false),
                    Fathername = table.Column<string>(type: "text", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Iin = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Roles = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RowVersion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbusers", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "user",
                table: "tbrole",
                columns: new[] { "Id", "CreatedAt", "Role", "RowVersion", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(2023, 4, 15, 2, 19, 6, 466, DateTimeKind.Utc).AddTicks(6726), "User", 1, null },
                    { 2L, new DateTime(2023, 4, 15, 2, 19, 6, 466, DateTimeKind.Utc).AddTicks(6739), "Moderator", 1, null },
                    { 3L, new DateTime(2023, 4, 15, 2, 19, 6, 466, DateTimeKind.Utc).AddTicks(6740), "Admin", 1, null }
                });

            migrationBuilder.InsertData(
                schema: "user",
                table: "tbusers",
                columns: new[] { "Id", "BirthDate", "CreatedAt", "Email", "Fathername", "Firstname", "Iin", "Lastname", "Password", "Roles", "RowVersion", "UpdatedAt" },
                values: new object[] { 1L, new DateTime(2004, 4, 24, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 4, 14, 20, 19, 6, 661, DateTimeKind.Utc).AddTicks(4300), "kznursat@gmail.com", "Erzatuly", "Nursat", "040524501037", "Zeinolla", "$2a$11$XRB011ZMPT8KK1IlW01lxuHp2aEnXcZIGbV/BDmVkiSURye.WtyQm", "Moderator, Admin, User", 1, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbrole",
                schema: "user");

            migrationBuilder.DropTable(
                name: "tbusers",
                schema: "user");

            migrationBuilder.EnsureSchema(
                name: "application");

            migrationBuilder.CreateTable(
                name: "tbtest",
                schema: "application",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RowVersion = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbtest", x => x.Id);
                });
        }
    }
}
