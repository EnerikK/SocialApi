using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addfriendrequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    FriendRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequesterUserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReceiverUserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateResponded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Response = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.FriendRequestId);
                    table.ForeignKey(
                        name: "FK_FriendRequests_UserProfiles_ReceiverUserProfileId",
                        column: x => x.ReceiverUserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId");
                    table.ForeignKey(
                        name: "FK_FriendRequests_UserProfiles_RequesterUserProfileId",
                        column: x => x.RequesterUserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId");
                });

            migrationBuilder.CreateTable(
                name: "FriendStatus",
                columns: table => new
                {
                    FriendshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstFriendUserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SecondFriendUserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateEstablished = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FriendshipStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendStatus", x => x.FriendshipId);
                    table.ForeignKey(
                        name: "FK_FriendStatus_UserProfiles_FirstFriendUserProfileId",
                        column: x => x.FirstFriendUserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId");
                    table.ForeignKey(
                        name: "FK_FriendStatus_UserProfiles_SecondFriendUserProfileId",
                        column: x => x.SecondFriendUserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_ReceiverUserProfileId",
                table: "FriendRequests",
                column: "ReceiverUserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_RequesterUserProfileId",
                table: "FriendRequests",
                column: "RequesterUserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendStatus_FirstFriendUserProfileId",
                table: "FriendStatus",
                column: "FirstFriendUserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendStatus_SecondFriendUserProfileId",
                table: "FriendStatus",
                column: "SecondFriendUserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropTable(
                name: "FriendStatus");
        }
    }
}
