using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebVer.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointSingerDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SignerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointSingerDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointSingerDocuments_AspNetUsers_SignerId",
                        column: x => x.SignerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointSingerDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionDescription",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: true),
                    Action = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionDescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionDescription_AspNetUsers_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionDescription_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DescriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IssuerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionDescription_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "TransactionDescription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a0347f0f-831a-4852-a2c2-ff19d4906e8d"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "702a7fc1-04f8-4cf0-b512-f196a458bbf2", "AQAAAAIAAYagAAAAEJufvhK9I+W+xk5WPg7RNj8KiJc4by1PaHgy5dQbh/CPxdWe4Mtd5Sb9UOnummraIg==", "2497834b-285d-4568-921e-d2921f98574b" });

            migrationBuilder.CreateIndex(
                name: "IX_AppointSingerDocuments_DocumentId",
                table: "AppointSingerDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointSingerDocuments_SignerId",
                table: "AppointSingerDocuments",
                column: "SignerId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDescription_DocumentId",
                table: "TransactionDescription",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDescription_SubjectId",
                table: "TransactionDescription",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DescriptionId",
                table: "Transactions",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_IssuerId",
                table: "Transactions",
                column: "IssuerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointSingerDocuments");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionDescription");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a0347f0f-831a-4852-a2c2-ff19d4906e8d"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "79ae7b90-0d2e-4dff-bfef-69abe21e3373", "AQAAAAIAAYagAAAAENAdmL6KTSwtXv/1aqfWBJVKq9JtMV7NdjHCvt+mB6UhM2V1bOnlO8cH3PiSxxjhVA==", "248e8b3b-63ab-4743-9c70-7d79c1eccc41" });
        }
    }
}
