using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    public partial class StoreOwnerName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TodoLists",
                newName: "Owner");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Tasks",
                newName: "Owner");

            migrationBuilder.RenameColumn(
                name: "Assignee",
                table: "Tasks",
                newName: "AssignedTo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Owner",
                table: "TodoLists",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Owner",
                table: "Tasks",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "AssignedTo",
                table: "Tasks",
                newName: "Assignee");
        }
    }
}
