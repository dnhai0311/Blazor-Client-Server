using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class CreateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AuthorName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BookSales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "double", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    ImgUrl = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookSales_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false),
                    Password = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bills_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BillDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BillId = table.Column<int>(type: "int", nullable: false),
                    BookSaleId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillDetails_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillDetails_BookSales_BookSaleId",
                        column: x => x.BookSaleId,
                        principalTable: "BookSales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "AuthorName" },
                values: new object[,]
                {
                    { 1, "Dương Ngọc Hải" },
                    { 2, "Trần Hiếu Nghĩa" },
                    { 3, "Tô Hoài" },
                    { 4, "Tố Hữu" },
                    { 5, "Nguyễn Trần Phương Tứng" },
                    { 6, "Hồ Quỳnh Hương" },
                    { 7, "Tun Phạm" },
                    { 8, "Tuấn Trần" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Staff" },
                    { 3, "Seller" }
                });

            migrationBuilder.InsertData(
                table: "BookSales",
                columns: new[] { "Id", "AuthorId", "Description", "ImgUrl", "Price", "Quantity", "Title" },
                values: new object[,]
                {
                    { 1, 1, "", "[]", 120000.0, 1000, "Đường Hầm Mùa Hạ Tập 1" },
                    { 2, 1, "", "[]", 120000.0, 1500, "Đường Hầm Mùa Hạ Tập 2" },
                    { 3, 1, "", "[]", 120000.0, 500, "Đường Hầm Mùa Hạ Tập 3" },
                    { 4, 1, "", "[]", 120000.0, 2000, "Đường Hầm Mùa Hạ Tập 4" },
                    { 5, 1, "", "[]", 120000.0, 1000, "Đường Hầm Mùa Hạ Tập 5" },
                    { 6, 1, "", "[]", 120000.0, 1500, "Đường Hầm Mùa Hạ Tập 6" },
                    { 7, 1, "", "[]", 120000.0, 500, "Đường Hầm Mùa Hạ Tập 7" },
                    { 8, 1, "", "[]", 120000.0, 2000, "Đường Hầm Mùa Hạ Tập 8" },
                    { 9, 1, "", "[]", 120000.0, 1000, "Đường Hầm Mùa Hạ Tập 9" },
                    { 10, 1, "", "[]", 120000.0, 1500, "Đường Hầm Mùa Hạ Tập 10" },
                    { 11, 2, "", "[]", 130000.0, 500, "Dế Mèo Phiêu Lưu Ký Tập 1" },
                    { 12, 2, "", "[]", 130000.0, 1000, "Dế Mèo Phiêu Lưu Ký Tập 2" },
                    { 13, 2, "", "[]", 130000.0, 1500, "Dế Mèo Phiêu Lưu Ký Tập 3" },
                    { 14, 2, "", "[]", 130000.0, 2000, "Dế Mèo Phiêu Lưu Ký Tập 4" },
                    { 15, 2, "", "[]", 130000.0, 500, "Dế Mèo Phiêu Lưu Ký Tập 5" },
                    { 16, 2, "", "[]", 130000.0, 1000, "Dế Mèo Phiêu Lưu Ký Tập 6" },
                    { 17, 2, "", "[]", 130000.0, 1500, "Dế Mèo Phiêu Lưu Ký Tập 7" },
                    { 18, 2, "", "[]", 130000.0, 2000, "Dế Mèo Phiêu Lưu Ký Tập 8" },
                    { 19, 2, "", "[]", 130000.0, 500, "Dế Mèo Phiêu Lưu Ký Tập 9" },
                    { 20, 2, "", "[]", 130000.0, 1000, "Dế Mèo Phiêu Lưu Ký Tập 10" },
                    { 21, 3, "", "[]", 140000.0, 1500, "Hành Trình Đến Bắc Cực Tập 1" },
                    { 22, 3, "", "[]", 140000.0, 2000, "Hành Trình Đến Bắc Cực Tập 2" },
                    { 23, 3, "", "[]", 140000.0, 500, "Hành Trình Đến Bắc Cực Tập 3" },
                    { 24, 3, "", "[]", 140000.0, 1000, "Hành Trình Đến Bắc Cực Tập 4" },
                    { 25, 3, "", "[]", 140000.0, 1500, "Hành Trình Đến Bắc Cực Tập 5" },
                    { 26, 3, "", "[]", 140000.0, 2000, "Hành Trình Đến Bắc Cực Tập 6" },
                    { 27, 3, "", "[]", 140000.0, 500, "Hành Trình Đến Bắc Cực Tập 7" },
                    { 28, 3, "", "[]", 140000.0, 1000, "Hành Trình Đến Bắc Cực Tập 8" },
                    { 29, 3, "", "[]", 140000.0, 1500, "Hành Trình Đến Bắc Cực Tập 9" },
                    { 30, 3, "", "[]", 140000.0, 2000, "Hành Trình Đến Bắc Cực Tập 10" },
                    { 31, 4, "", "[]", 150000.0, 500, "Thế Giới Huyền Bí Tập 1" },
                    { 32, 4, "", "[]", 150000.0, 1000, "Thế Giới Huyền Bí Tập 2" },
                    { 33, 4, "", "[]", 150000.0, 1500, "Thế Giới Huyền Bí Tập 3" },
                    { 34, 4, "", "[]", 150000.0, 2000, "Thế Giới Huyền Bí Tập 4" },
                    { 35, 4, "", "[]", 150000.0, 500, "Thế Giới Huyền Bí Tập 5" },
                    { 36, 4, "", "[]", 150000.0, 1000, "Thế Giới Huyền Bí Tập 6" },
                    { 37, 4, "", "[]", 150000.0, 1500, "Thế Giới Huyền Bí Tập 7" },
                    { 38, 4, "", "[]", 150000.0, 2000, "Thế Giới Huyền Bí Tập 8" },
                    { 39, 4, "", "[]", 150000.0, 500, "Thế Giới Huyền Bí Tập 9" },
                    { 40, 4, "", "[]", 150000.0, 1000, "Thế Giới Huyền Bí Tập 10" }
                });

            migrationBuilder.CreateIndex(
                name: "UX_Author_AuthorName",
                table: "Authors",
                column: "AuthorName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_BillId",
                table: "BillDetails",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_BookSaleId",
                table: "BillDetails",
                column: "BookSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_UserId",
                table: "Bills",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookSales_AuthorId",
                table: "BookSales",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "UX_BookSale_Title",
                table: "BookSales",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UX_Role_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UX_User_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_User_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillDetails");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "BookSales");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
