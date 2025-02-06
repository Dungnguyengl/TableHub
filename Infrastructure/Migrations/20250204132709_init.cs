using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NORMALIZED_NAME = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CONCURRENCY_STAMP = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    REFRESH_TOKEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REFRESH_TOKEN_EXPIRE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STORE_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    STORE_SLUG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ROLE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GENDER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOB = table.Column<DateOnly>(type: "date", nullable: true),
                    USER_NAME = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NORMALIZED_USER_NAME = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EMAIL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NORMALIZED_EMAIL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EMAIL_CONFIRMED = table.Column<bool>(type: "bit", nullable: false),
                    PASSWORD_HASH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SECURITY_STAMP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CONCURRENCY_STAMP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PHONE_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PHONE_NUMBER_CONFIRMED = table.Column<bool>(type: "bit", nullable: false),
                    TWO_FACTOR_ENABLED = table.Column<bool>(type: "bit", nullable: false),
                    LOCKOUT_END = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LOCKOUT_ENABLED = table.Column<bool>(type: "bit", nullable: false),
                    ACCESS_FAILED_COUNT = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ORDER",
                schema: "dbo",
                columns: table => new
                {
                    ORDER_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TABLE_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CUSTOMER_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    TOTAL_PRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    INS_USER_CODE = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    INS_DATE = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    UPD_USER_CODE = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UPD_DATE = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    IS_DELETE = table.Column<bool>(type: "bit", nullable: false),
                    STORE_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER", x => x.ORDER_ID);
                });

            migrationBuilder.CreateTable(
                name: "ORDERITEM",
                schema: "dbo",
                columns: table => new
                {
                    ORDERITEM_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ORDER_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PRODUCT_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PRODUCT_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UNIT_PRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QUANTITY = table.Column<int>(type: "int", nullable: true),
                    INS_USER_CODE = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    INS_DATE = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    UPD_USER_CODE = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UPD_DATE = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    IS_DELETE = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDERITEM", x => x.ORDERITEM_ID);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT",
                schema: "dbo",
                columns: table => new
                {
                    PRODUCT_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IMAGE_LINK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INS_USER_CODE = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    INS_DATE = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    UPD_USER_CODE = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UPD_DATE = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    IS_DELETE = table.Column<bool>(type: "bit", nullable: false),
                    STORE_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT", x => x.PRODUCT_ID);
                });

            migrationBuilder.CreateTable(
                name: "STORE",
                schema: "dbo",
                columns: table => new
                {
                    STORE_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LOGO_LINK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INS_USER_CODE = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    INS_DATE = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    UPD_USER_CODE = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UPD_DATE = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    IS_DELETE = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STORE", x => x.STORE_ID);
                });

            migrationBuilder.CreateTable(
                name: "TABLE",
                schema: "dbo",
                columns: table => new
                {
                    TABLE_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    INS_USER_CODE = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    INS_DATE = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    UPD_USER_CODE = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UPD_DATE = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    IS_DELETE = table.Column<bool>(type: "bit", nullable: false),
                    STORE_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TABLE", x => x.TABLE_ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROLE_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CLAIM_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CLAIM_VALUE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USER_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CLAIM_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CLAIM_VALUE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_USER_ID",
                        column: x => x.USER_ID,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "dbo",
                columns: table => new
                {
                    LOGIN_PROVIDER = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PROVIDER_KEY = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PROVIDER_DISPLAY_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USER_ID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LOGIN_PROVIDER, x.PROVIDER_KEY });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_USER_ID",
                        column: x => x.USER_ID,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "dbo",
                columns: table => new
                {
                    USER_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ROLE_ID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.USER_ID, x.ROLE_ID });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_USER_ID",
                        column: x => x.USER_ID,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "dbo",
                columns: table => new
                {
                    USER_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LOGIN_PROVIDER = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VALUE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.USER_ID, x.LOGIN_PROVIDER, x.NAME });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_USER_ID",
                        column: x => x.USER_ID,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_ROLE_ID",
                schema: "dbo",
                table: "AspNetRoleClaims",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "dbo",
                table: "AspNetRoles",
                column: "NORMALIZED_NAME",
                unique: true,
                filter: "[NORMALIZED_NAME] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_USER_ID",
                schema: "dbo",
                table: "AspNetUserClaims",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_USER_ID",
                schema: "dbo",
                table: "AspNetUserLogins",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_ROLE_ID",
                schema: "dbo",
                table: "AspNetUserRoles",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NORMALIZED_EMAIL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NORMALIZED_USER_NAME",
                unique: true,
                filter: "[NORMALIZED_USER_NAME] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ORDER",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ORDERITEM",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PRODUCT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "STORE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TABLE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "dbo");
        }
    }
}
