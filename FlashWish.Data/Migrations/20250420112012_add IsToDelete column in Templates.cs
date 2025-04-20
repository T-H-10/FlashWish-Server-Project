using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashWish.Data.Migrations
{
    /// <inheritdoc />
    public partial class addIsToDeletecolumninTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsToDelete",
                table: "Templates",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsToDelete",
                table: "Templates");
        }
    }
}
