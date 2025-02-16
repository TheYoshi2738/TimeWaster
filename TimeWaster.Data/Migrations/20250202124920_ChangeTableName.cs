using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeWaster.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Intervals_Users_user_id",
                table: "Intervals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Intervals",
                table: "Intervals");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Intervals",
                newName: "intervals");

            migrationBuilder.RenameIndex(
                name: "IX_Intervals_user_id",
                table: "intervals",
                newName: "IX_intervals_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_intervals",
                table: "intervals",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_intervals_users_user_id",
                table: "intervals",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_intervals_users_user_id",
                table: "intervals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_intervals",
                table: "intervals");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "intervals",
                newName: "Intervals");

            migrationBuilder.RenameIndex(
                name: "IX_intervals_user_id",
                table: "Intervals",
                newName: "IX_Intervals_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Intervals",
                table: "Intervals",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Intervals_Users_user_id",
                table: "Intervals",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
