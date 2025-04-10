using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Command.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SnapShot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "post_text",
                columns: table => new
                {
                    post_text_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_text", x => x.post_text_id);
                });

            migrationBuilder.CreateTable(
                name: "reactions",
                columns: table => new
                {
                    reaction_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reaction_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reaction_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reaction_icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reactions", x => x.reaction_id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tag_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    class_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    full_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    is_login_google = table.Column<bool>(type: "bit", nullable: false),
                    is_actived = table.Column<bool>(type: "bit", nullable: false),
                    is_email_verified = table.Column<bool>(type: "bit", nullable: false),
                    last_login_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_user_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "black_list_token",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    token_revoked = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_black_list_token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_black_list_token_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "email_token",
                columns: table => new
                {
                    email_token_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email_token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expired_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    is_used = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_token", x => x.email_token_id);
                    table.ForeignKey(
                        name: "FK_email_token_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "follow",
                columns: table => new
                {
                    follow_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    follower_id = table.Column<int>(type: "int", nullable: false),
                    followed_id = table.Column<int>(type: "int", nullable: false),
                    followed_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follow", x => x.follow_id);
                    table.ForeignKey(
                        name: "FK_follow_user_followed_id",
                        column: x => x.followed_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_follow_user_follower_id",
                        column: x => x.follower_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "post",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    post_thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    post_summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    post_text_id = table.Column<int>(type: "int", nullable: false),
                    total_reactions = table.Column<int>(type: "int", nullable: false),
                    total_comments = table.Column<int>(type: "int", nullable: false),
                    total_reads = table.Column<int>(type: "int", nullable: false),
                    is_published = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post", x => x.post_id);
                    table.ForeignKey(
                        name: "FK_post_post_text_post_text_id",
                        column: x => x.post_text_id,
                        principalTable: "post_text",
                        principalColumn: "post_text_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_post_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_token",
                columns: table => new
                {
                    refresh_token_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_revoked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_token", x => x.refresh_token_id);
                    table.ForeignKey(
                        name: "FK_refresh_token_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    comment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comment_text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parent_comment_id = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    post_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_comment_post_post_id",
                        column: x => x.post_id,
                        principalTable: "post",
                        principalColumn: "post_id");
                    table.ForeignKey(
                        name: "FK_comment_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "post_reaction",
                columns: table => new
                {
                    post_reaction_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    reaction_id = table.Column<int>(type: "int", nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_reaction", x => x.post_reaction_id);
                    table.ForeignKey(
                        name: "FK_post_reaction_post_post_id",
                        column: x => x.post_id,
                        principalTable: "post",
                        principalColumn: "post_id");
                    table.ForeignKey(
                        name: "FK_post_reaction_reactions_reaction_id",
                        column: x => x.reaction_id,
                        principalTable: "reactions",
                        principalColumn: "reaction_id");
                    table.ForeignKey(
                        name: "FK_post_reaction_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "post_saved",
                columns: table => new
                {
                    post_saved_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    post_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_saved", x => x.post_saved_id);
                    table.ForeignKey(
                        name: "FK_post_saved_post_post_id",
                        column: x => x.post_id,
                        principalTable: "post",
                        principalColumn: "post_id");
                    table.ForeignKey(
                        name: "FK_post_saved_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "post_tag",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "int", nullable: false),
                    tag_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_tag", x => new { x.post_id, x.tag_id });
                    table.ForeignKey(
                        name: "FK_post_tag_post_post_id",
                        column: x => x.post_id,
                        principalTable: "post",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_post_tag_tag_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tag",
                        principalColumn: "tag_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    notification_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    notification_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    comment_id = table.Column<int>(type: "int", nullable: true),
                    replay_for_comment_id = table.Column<int>(type: "int", nullable: true),
                    recipient_user_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    post_id = table.Column<int>(type: "int", nullable: true),
                    seen = table.Column<bool>(type: "bit", nullable: false),
                    notification_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.notification_id);
                    table.ForeignKey(
                        name: "FK_notification_comment_comment_id",
                        column: x => x.comment_id,
                        principalTable: "comment",
                        principalColumn: "comment_id");
                    table.ForeignKey(
                        name: "FK_notification_comment_replay_for_comment_id",
                        column: x => x.replay_for_comment_id,
                        principalTable: "comment",
                        principalColumn: "comment_id");
                    table.ForeignKey(
                        name: "FK_notification_post_post_id",
                        column: x => x.post_id,
                        principalTable: "post",
                        principalColumn: "post_id");
                    table.ForeignKey(
                        name: "FK_notification_user_recipient_user_id",
                        column: x => x.recipient_user_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_notification_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.InsertData(
                table: "post_text",
                columns: new[] { "post_text_id", "post_text" },
                values: new object[,]
                {
                    { 1, "Content Blog 1" },
                    { 2, "Content Blog 2" }
                });

            migrationBuilder.InsertData(
                table: "reactions",
                columns: new[] { "reaction_id", "created_at", "IsDeleted", "reaction_description", "reaction_icon", "reaction_name", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 7, 10, 38, 56, 31, DateTimeKind.Utc).AddTicks(5532), false, "reaction-description-1", "icon-1.png", "Haha", null },
                    { 2, new DateTime(2025, 4, 7, 10, 38, 56, 31, DateTimeKind.Utc).AddTicks(5537), false, "reaction-description-2", "icon-2.png", "Tim", null },
                    { 3, new DateTime(2025, 4, 7, 10, 38, 56, 31, DateTimeKind.Utc).AddTicks(5538), false, "reaction-description-3", "icon-3.png", "Thích", null },
                    { 4, new DateTime(2025, 4, 7, 10, 38, 56, 31, DateTimeKind.Utc).AddTicks(5539), false, "reaction-description-4", "icon-4.png", "Phẩn nộ", null }
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "role_id", "role_name" },
                values: new object[,]
                {
                    { 1, "ADMIN" },
                    { 2, "USER" }
                });

            migrationBuilder.InsertData(
                table: "tag",
                columns: new[] { "tag_id", "class_name", "created_at", "is_deleted", "tag_name", "updated_at" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 7, 10, 38, 56, 32, DateTimeKind.Utc).AddTicks(3333), false, "tag-name-test-1", null },
                    { 2, null, new DateTime(2025, 4, 7, 10, 38, 56, 32, DateTimeKind.Utc).AddTicks(3339), false, "tag-name-test-2", null }
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "user_id", "avatar", "bio", "created_at", "email", "full_name", "is_actived", "is_deleted", "is_email_verified", "is_login_google", "last_login_at", "password_hash", "role_id", "updated_at" },
                values: new object[,]
                {
                    { 1, "default-avatar.png", "bio-admin", new DateTime(2025, 4, 7, 10, 38, 56, 32, DateTimeKind.Utc).AddTicks(7450), "admin@gmail.com", "admin", true, false, true, false, null, "$2a$11$QU6qsm2oygfPiz28h5KAl.FRRpjsOWpZOd8UCa3/jINCGANpSEwYy", 1, null },
                    { 2, "default-avatar.png", "bio-user", new DateTime(2025, 4, 7, 10, 38, 56, 152, DateTimeKind.Utc).AddTicks(7169), "user@gmail.com", "user", true, false, true, false, null, "$2a$11$W7sa8hOoiEZ6BqwXU7wus.nXluSmzzz/lxibVPpGbi/VFMr4Jh9Nu", 2, null }
                });

            migrationBuilder.InsertData(
                table: "follow",
                columns: new[] { "follow_id", "followed_at", "followed_id", "follower_id" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 7, 10, 38, 56, 22, DateTimeKind.Utc).AddTicks(6741), 2, 1 },
                    { 2, new DateTime(2025, 4, 7, 10, 38, 56, 22, DateTimeKind.Utc).AddTicks(6745), 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "notification",
                columns: new[] { "notification_id", "comment_id", "notification_at", "post_id", "recipient_user_id", "replay_for_comment_id", "seen", "notification_type", "user_id" },
                values: new object[] { 2, null, new DateTime(2025, 4, 7, 17, 38, 56, 28, DateTimeKind.Local).AddTicks(5161), null, 2, null, false, "Follow", 1 });

            migrationBuilder.InsertData(
                table: "post",
                columns: new[] { "post_id", "created_at", "is_deleted", "is_published", "post_summary", "post_text_id", "post_thumbnail", "post_title", "total_comments", "total_reactions", "total_reads", "updated_at", "user_id" },
                values: new object[,]
                {
                    { 1, null, false, true, "blog-description-1", 1, "banner.png", "blog-title-1", 1, 1, 1, null, 1 },
                    { 2, null, false, true, "blog-description-2", 2, "banner.png", "blog-title-2", 2, 2, 2, null, 2 }
                });

            migrationBuilder.InsertData(
                table: "comment",
                columns: new[] { "comment_id", "comment_text", "created_at", "is_deleted", "parent_comment_id", "post_id", "updated_at", "user_id" },
                values: new object[,]
                {
                    { 1, "content-1", new DateTime(2025, 4, 7, 10, 38, 56, 18, DateTimeKind.Utc).AddTicks(8786), false, 0, 1, null, 1 },
                    { 2, "content-2", new DateTime(2025, 4, 7, 10, 38, 56, 18, DateTimeKind.Utc).AddTicks(8793), false, 0, 2, null, 2 },
                    { 3, "content-3", new DateTime(2025, 4, 7, 10, 38, 56, 18, DateTimeKind.Utc).AddTicks(8794), false, 0, 2, null, 2 },
                    { 4, "content-4", new DateTime(2025, 4, 7, 10, 38, 56, 18, DateTimeKind.Utc).AddTicks(8796), false, 0, 1, null, 1 }
                });

            migrationBuilder.InsertData(
                table: "notification",
                columns: new[] { "notification_id", "comment_id", "notification_at", "post_id", "recipient_user_id", "replay_for_comment_id", "seen", "notification_type", "user_id" },
                values: new object[] { 3, null, new DateTime(2025, 4, 7, 17, 38, 56, 28, DateTimeKind.Local).AddTicks(5162), 2, 1, null, false, "CreatePost", 2 });

            migrationBuilder.InsertData(
                table: "post_reaction",
                columns: new[] { "post_reaction_id", "created_at", "IsActived", "post_id", "reaction_id", "updated_at", "user_id" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 7, 10, 38, 56, 30, DateTimeKind.Utc).AddTicks(183), false, 1, 1, null, 1 },
                    { 2, new DateTime(2025, 4, 7, 10, 38, 56, 30, DateTimeKind.Utc).AddTicks(187), false, 2, 4, null, 2 }
                });

            migrationBuilder.InsertData(
                table: "post_saved",
                columns: new[] { "post_saved_id", "post_id", "user_id" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "post_tag",
                columns: new[] { "post_id", "tag_id" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "notification",
                columns: new[] { "notification_id", "comment_id", "notification_at", "post_id", "recipient_user_id", "replay_for_comment_id", "seen", "notification_type", "user_id" },
                values: new object[] { 1, 1, new DateTime(2025, 4, 7, 17, 38, 56, 28, DateTimeKind.Local).AddTicks(5140), 1, 1, null, false, "Comment", 2 });

            migrationBuilder.CreateIndex(
                name: "IX_black_list_token_user_id",
                table: "black_list_token",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_post_id",
                table: "comment",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_user_id",
                table: "comment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_email_token_user_id",
                table: "email_token",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_follow_followed_id",
                table: "follow",
                column: "followed_id");

            migrationBuilder.CreateIndex(
                name: "IX_follow_follower_id",
                table: "follow",
                column: "follower_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_comment_id",
                table: "notification",
                column: "comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_post_id",
                table: "notification",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_recipient_user_id",
                table: "notification",
                column: "recipient_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_replay_for_comment_id",
                table: "notification",
                column: "replay_for_comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_user_id",
                table: "notification",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_post_text_id",
                table: "post",
                column: "post_text_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_post_user_id",
                table: "post",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_reaction_post_id_user_id_reaction_id",
                table: "post_reaction",
                columns: new[] { "post_id", "user_id", "reaction_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_post_reaction_reaction_id",
                table: "post_reaction",
                column: "reaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_reaction_user_id",
                table: "post_reaction",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_saved_post_id",
                table: "post_saved",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_saved_user_id",
                table: "post_saved",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_tag_tag_id",
                table: "post_tag",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_token_token",
                table: "refresh_token",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_refresh_token_user_id",
                table: "refresh_token",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_id",
                table: "user",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "black_list_token");

            migrationBuilder.DropTable(
                name: "email_token");

            migrationBuilder.DropTable(
                name: "follow");

            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropTable(
                name: "post_reaction");

            migrationBuilder.DropTable(
                name: "post_saved");

            migrationBuilder.DropTable(
                name: "post_tag");

            migrationBuilder.DropTable(
                name: "refresh_token");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "reactions");

            migrationBuilder.DropTable(
                name: "tag");

            migrationBuilder.DropTable(
                name: "post");

            migrationBuilder.DropTable(
                name: "post_text");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
