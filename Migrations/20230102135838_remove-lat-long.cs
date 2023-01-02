using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeighborGoodAPI.Migrations
{
    public partial class removelatlong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Profiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Profiles",
                type: "decimal(8,6)",
                precision: 8,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Profiles",
                type: "decimal(8,6)",
                precision: 8,
                scale: 6,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
