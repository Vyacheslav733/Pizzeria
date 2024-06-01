using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaDatabaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReaded",
                table: "MessageInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReply",
                table: "MessageInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReplyMessageId",
                table: "MessageInfos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageInfos_ReplyMessageId",
                table: "MessageInfos",
                column: "ReplyMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageInfos_MessageInfos_ReplyMessageId",
                table: "MessageInfos",
                column: "ReplyMessageId",
                principalTable: "MessageInfos",
                principalColumn: "MessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageInfos_MessageInfos_ReplyMessageId",
                table: "MessageInfos");

            migrationBuilder.DropIndex(
                name: "IX_MessageInfos_ReplyMessageId",
                table: "MessageInfos");

            migrationBuilder.DropColumn(
                name: "IsReaded",
                table: "MessageInfos");

            migrationBuilder.DropColumn(
                name: "IsReply",
                table: "MessageInfos");

            migrationBuilder.DropColumn(
                name: "ReplyMessageId",
                table: "MessageInfos");
        }
    }
}
