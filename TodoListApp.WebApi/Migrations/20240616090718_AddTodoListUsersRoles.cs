using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    public partial class AddTodoListUsersRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "TodoLists");

            migrationBuilder.CreateTable(
                name: "TodoListsUsers",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TodoListId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoListsUsers", x => new { x.TodoListId, x.UserName });
                    table.ForeignKey(
                        name: "FK_TodoListsUsers_TodoLists_TodoListId",
                        column: x => x.TodoListId,
                        principalTable: "TodoLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoListsUsers");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "TodoLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
