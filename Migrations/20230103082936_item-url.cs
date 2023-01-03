using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeighborGoodAPI.Migrations
{
    public partial class itemurl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Items",
                newName: "ImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Items",
                newName: "ImageName");
        }
    }
}
