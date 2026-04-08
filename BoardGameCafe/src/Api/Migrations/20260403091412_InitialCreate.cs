using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    MaxPlayers = table.Column<int>(type: "integer", nullable: false),
                    PlaytimeMin = table.Column<int>(type: "integer", nullable: false),
                    Complexity = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EmailAddress = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    ForcePasswordChange = table.Column<bool>(type: "boolean", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameCopies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCopies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameCopies_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "Complexity", "CreatedAt", "Description", "ImageUrl", "MaxPlayers", "PlaytimeMin", "Title" },
                values: new object[] { new Guid("d1bd1bfa-4d38-4d69-a68d-0fd1599095ef"), 2, 0, "A game of trading and building.", "", 4, 60, "Catan" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "EmailAddress", "ForcePasswordChange", "Name", "PasswordHash", "Phone", "Role" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 0, "admin@test.com", false, "Admin User", "admin", "", 2 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 0, "customer@test.com", false, "Customer User", "customer", "", 0 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 0, "steward@test.com", false, "Steward User", "steward", "", 1 }
                });

            migrationBuilder.InsertData(
                table: "GameCopies",
                columns: new[] { "Id", "CreatedAt", "GameId" },
                values: new object[] { new Guid("b1b1b1b1-4d38-4d69-a68d-0fd1599095ef"), 0, new Guid("d1bd1bfa-4d38-4d69-a68d-0fd1599095ef") });

            migrationBuilder.CreateIndex(
                name: "IX_GameCopies_GameId",
                table: "GameCopies",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameCopies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
