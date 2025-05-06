using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashWish.Data.Migrations
{
    /// <inheritdoc />
    public partial class addmarkedToDtioninGreetingMessagetbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MarkedForDeletion",
                table: "GreetingMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkedForDeletion",
                table: "GreetingMessages");
        }
    }
}
