using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeighborGoodAPI.Migrations
{
    public partial class modeltweaks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Profiles_UserProfileId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "UserProfileId",
                table: "Items",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_UserProfileId",
                table: "Items",
                newName: "IX_Items_OwnerId");

            migrationBuilder.AddColumn<string>(
                name: "Auth0Id",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ItemId",
                table: "Comments",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Items_ItemId",
                table: "Comments",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Profiles_OwnerId",
                table: "Items",
                column: "OwnerId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Items_ItemId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Profiles_OwnerId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ItemId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Auth0Id",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Items",
                newName: "UserProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_OwnerId",
                table: "Items",
                newName: "IX_Items_UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Profiles_UserProfileId",
                table: "Items",
                column: "UserProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
