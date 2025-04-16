using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Command.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldSavedPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActived",
                table: "post_saved",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 14, 7, 28, 29, 503, DateTimeKind.Utc).AddTicks(4966));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 14, 7, 28, 29, 503, DateTimeKind.Utc).AddTicks(4975));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 14, 7, 28, 29, 503, DateTimeKind.Utc).AddTicks(4977));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 4, 14, 7, 28, 29, 503, DateTimeKind.Utc).AddTicks(4978));

            migrationBuilder.UpdateData(
                table: "follow",
                keyColumn: "follow_id",
                keyValue: 1,
                column: "followed_at",
                value: new DateTime(2025, 4, 14, 7, 28, 29, 507, DateTimeKind.Utc).AddTicks(3147));

            migrationBuilder.UpdateData(
                table: "follow",
                keyColumn: "follow_id",
                keyValue: 2,
                column: "followed_at",
                value: new DateTime(2025, 4, 14, 7, 28, 29, 507, DateTimeKind.Utc).AddTicks(3151));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 1,
                column: "notification_at",
                value: new DateTime(2025, 4, 14, 14, 28, 29, 513, DateTimeKind.Local).AddTicks(7902));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 2,
                column: "notification_at",
                value: new DateTime(2025, 4, 14, 14, 28, 29, 513, DateTimeKind.Local).AddTicks(7921));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 3,
                column: "notification_at",
                value: new DateTime(2025, 4, 14, 14, 28, 29, 513, DateTimeKind.Local).AddTicks(7923));

            migrationBuilder.UpdateData(
                table: "post_reaction",
                keyColumn: "post_reaction_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 14, 7, 28, 29, 515, DateTimeKind.Utc).AddTicks(2576));

            migrationBuilder.UpdateData(
                table: "post_reaction",
                keyColumn: "post_reaction_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 14, 7, 28, 29, 515, DateTimeKind.Utc).AddTicks(2579));

            migrationBuilder.UpdateData(
                table: "post_saved",
                keyColumn: "post_saved_id",
                keyValue: 1,
                column: "IsActived",
                value: false);

            migrationBuilder.UpdateData(
                table: "post_saved",
                keyColumn: "post_saved_id",
                keyValue: 2,
                column: "IsActived",
                value: false);

            migrationBuilder.UpdateData(
                table: "post_saved",
                keyColumn: "post_saved_id",
                keyValue: 3,
                column: "IsActived",
                value: false);

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 14, 7, 28, 29, 516, DateTimeKind.Utc).AddTicks(7589));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 14, 7, 28, 29, 516, DateTimeKind.Utc).AddTicks(7594));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 14, 7, 28, 29, 516, DateTimeKind.Utc).AddTicks(7595));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 4, 14, 7, 28, 29, 516, DateTimeKind.Utc).AddTicks(7596));

            migrationBuilder.UpdateData(
                table: "tag",
                keyColumn: "tag_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 14, 7, 28, 29, 517, DateTimeKind.Utc).AddTicks(5245));

            migrationBuilder.UpdateData(
                table: "tag",
                keyColumn: "tag_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 14, 7, 28, 29, 517, DateTimeKind.Utc).AddTicks(5252));

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 14, 7, 28, 29, 517, DateTimeKind.Utc).AddTicks(9335), "$2a$11$4ViFbFey8KEm.rqv5gwM9.wS0jbnEkU1oy9VIdn8K6sVDFfkY8eI6" });

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 14, 7, 28, 29, 639, DateTimeKind.Utc).AddTicks(5603), "$2a$11$2p0349noPq3CXSZjGtT0HeT238MalPN0WgVVBbGqdWw6QLReDMA3e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActived",
                table: "post_saved");

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 10, 12, 13, 29, 627, DateTimeKind.Utc).AddTicks(6931));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 10, 12, 13, 29, 627, DateTimeKind.Utc).AddTicks(6940));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 10, 12, 13, 29, 627, DateTimeKind.Utc).AddTicks(6941));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 4, 10, 12, 13, 29, 627, DateTimeKind.Utc).AddTicks(6942));

            migrationBuilder.UpdateData(
                table: "follow",
                keyColumn: "follow_id",
                keyValue: 1,
                column: "followed_at",
                value: new DateTime(2025, 4, 10, 12, 13, 29, 631, DateTimeKind.Utc).AddTicks(3071));

            migrationBuilder.UpdateData(
                table: "follow",
                keyColumn: "follow_id",
                keyValue: 2,
                column: "followed_at",
                value: new DateTime(2025, 4, 10, 12, 13, 29, 631, DateTimeKind.Utc).AddTicks(3081));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 1,
                column: "notification_at",
                value: new DateTime(2025, 4, 10, 19, 13, 29, 637, DateTimeKind.Local).AddTicks(2479));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 2,
                column: "notification_at",
                value: new DateTime(2025, 4, 10, 19, 13, 29, 637, DateTimeKind.Local).AddTicks(2515));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 3,
                column: "notification_at",
                value: new DateTime(2025, 4, 10, 19, 13, 29, 637, DateTimeKind.Local).AddTicks(2517));

            migrationBuilder.UpdateData(
                table: "post_reaction",
                keyColumn: "post_reaction_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 10, 12, 13, 29, 639, DateTimeKind.Utc).AddTicks(2598));

            migrationBuilder.UpdateData(
                table: "post_reaction",
                keyColumn: "post_reaction_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 10, 12, 13, 29, 639, DateTimeKind.Utc).AddTicks(2602));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 10, 12, 13, 29, 640, DateTimeKind.Utc).AddTicks(7890));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 10, 12, 13, 29, 640, DateTimeKind.Utc).AddTicks(7896));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 10, 12, 13, 29, 640, DateTimeKind.Utc).AddTicks(7898));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 4, 10, 12, 13, 29, 640, DateTimeKind.Utc).AddTicks(7899));

            migrationBuilder.UpdateData(
                table: "tag",
                keyColumn: "tag_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 10, 12, 13, 29, 641, DateTimeKind.Utc).AddTicks(5780));

            migrationBuilder.UpdateData(
                table: "tag",
                keyColumn: "tag_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 10, 12, 13, 29, 641, DateTimeKind.Utc).AddTicks(5786));

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 10, 12, 13, 29, 642, DateTimeKind.Utc).AddTicks(2462), "$2a$11$xVOeErb.QaWUx00npUBX9eYyZgvgdpNoUVzzENfVFe1DLq3gZeibK" });

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 10, 12, 13, 29, 764, DateTimeKind.Utc).AddTicks(8838), "$2a$11$0sHFqyUE64rVciMvqIkOK.ixuFAdMEeueUQsfN4Y/z8tVZ8OdSTv2" });
        }
    }
}
