using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashWish.Data.Migrations
{
    /// <inheritdoc />
    public partial class addnullabletocategotyIDintemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Categories_CategoryID",
                table: "Templates");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "Templates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Categories_CategoryID",
                table: "Templates",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Categories_CategoryID",
                table: "Templates");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "Templates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Categories_CategoryID",
                table: "Templates",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
