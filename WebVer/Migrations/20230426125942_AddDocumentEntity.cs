using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebVer.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Data = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a0347f0f-831a-4852-a2c2-ff19d4906e8d"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "79ae7b90-0d2e-4dff-bfef-69abe21e3373", "AQAAAAIAAYagAAAAENAdmL6KTSwtXv/1aqfWBJVKq9JtMV7NdjHCvt+mB6UhM2V1bOnlO8cH3PiSxxjhVA==", "248e8b3b-63ab-4743-9c70-7d79c1eccc41" });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserId",
                table: "Documents",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a0347f0f-831a-4852-a2c2-ff19d4906e8d"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "98f198fa-6528-4d1f-9dc7-c02b2870ab34", "AQAAAAIAAYagAAAAEHjYsIsF7b2W6sS/3zbP0/Zkr9cTMnBVw2IOOmj6NyDTRnUV0Gq0SvyhMCORia953w==", "8f28c643-586a-4dcf-8d4d-bb0f60faffbd" });
        }
    }
}
