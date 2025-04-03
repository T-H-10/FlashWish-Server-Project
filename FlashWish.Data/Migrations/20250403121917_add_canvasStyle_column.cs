using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashWish.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_canvasStyle_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CanvasStyle",
                table: "GreetingCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanvasStyle",
                table: "GreetingCards");
        }
    }
}
