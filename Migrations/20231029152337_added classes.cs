using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogixTask.Migrations
{
    public partial class addedclasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "PhoneNumber",
                table: "WebUsers",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserClasses",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "WebUserId",
                table: "UserClasses",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserClasses_WebUserId",
                table: "UserClasses",
                column: "WebUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClasses_WebUsers_WebUserId",
                table: "UserClasses",
                column: "WebUserId",
                principalTable: "WebUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClasses_WebUsers_WebUserId",
                table: "UserClasses");

            migrationBuilder.DropIndex(
                name: "IX_UserClasses_WebUserId",
                table: "UserClasses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserClasses");

            migrationBuilder.DropColumn(
                name: "WebUserId",
                table: "UserClasses");

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber",
                table: "WebUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
