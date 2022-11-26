using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboxInboxParticipants");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "InboxParticipants");

            migrationBuilder.DropTable(
                name: "Inboxes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inboxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    InboxUId = table.Column<int>(type: "integer", nullable: false),
                    LastMessage = table.Column<string>(type: "text", nullable: false),
                    LastSendUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inboxes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InboxParticipants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    InboxUId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InboxParticipants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    InboxId = table.Column<int>(type: "integer", nullable: true),
                    Messages = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Inboxes_InboxId",
                        column: x => x.InboxId,
                        principalTable: "Inboxes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InboxInboxParticipants",
                columns: table => new
                {
                    InboxParticipantsId = table.Column<int>(type: "integer", nullable: false),
                    InboxesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxInboxParticipants", x => new { x.InboxParticipantsId, x.InboxesId });
                    table.ForeignKey(
                        name: "FK_InboxInboxParticipants_Inboxes_InboxesId",
                        column: x => x.InboxesId,
                        principalTable: "Inboxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InboxInboxParticipants_InboxParticipants_InboxParticipantsId",
                        column: x => x.InboxParticipantsId,
                        principalTable: "InboxParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InboxInboxParticipants_InboxesId",
                table: "InboxInboxParticipants",
                column: "InboxesId");

            migrationBuilder.CreateIndex(
                name: "IX_InboxParticipants_UserId",
                table: "InboxParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_InboxId",
                table: "Messages",
                column: "InboxId");
        }
    }
}
