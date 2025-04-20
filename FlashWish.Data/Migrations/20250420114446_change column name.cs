using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashWish.Data.Migrations
{
    /// <inheritdoc />
    public partial class changecolumnname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsToDelete",
                table: "Templates",
                newName: "MarkedForDeletion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MarkedForDeletion",
                table: "Templates",
                newName: "IsToDelete");
        }
    }
}
