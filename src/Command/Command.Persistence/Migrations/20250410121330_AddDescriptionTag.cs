using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Command.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tag_description",
                table: "tag",
                type: "nvarchar(max)",
                nullable: true);

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
                columns: new[] { "created_at", "tag_description" },
                values: new object[] { new DateTime(2025, 4, 10, 12, 13, 29, 641, DateTimeKind.Utc).AddTicks(5780), null });

            migrationBuilder.UpdateData(
                table: "tag",
                keyColumn: "tag_id",
                keyValue: 2,
                columns: new[] { "created_at", "tag_description" },
                values: new object[] { new DateTime(2025, 4, 10, 12, 13, 29, 641, DateTimeKind.Utc).AddTicks(5786), null });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tag_description",
                table: "tag");

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 9, 11, 12, 40, 825, DateTimeKind.Utc).AddTicks(5778));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 9, 11, 12, 40, 825, DateTimeKind.Utc).AddTicks(5786));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 9, 11, 12, 40, 825, DateTimeKind.Utc).AddTicks(5787));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 4, 9, 11, 12, 40, 825, DateTimeKind.Utc).AddTicks(5789));

            migrationBuilder.UpdateData(
                table: "follow",
                keyColumn: "follow_id",
                keyValue: 1,
                column: "followed_at",
                value: new DateTime(2025, 4, 9, 11, 12, 40, 829, DateTimeKind.Utc).AddTicks(1933));

            migrationBuilder.UpdateData(
                table: "follow",
                keyColumn: "follow_id",
                keyValue: 2,
                column: "followed_at",
                value: new DateTime(2025, 4, 9, 11, 12, 40, 829, DateTimeKind.Utc).AddTicks(1936));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 1,
                column: "notification_at",
                value: new DateTime(2025, 4, 9, 18, 12, 40, 835, DateTimeKind.Local).AddTicks(227));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 2,
                column: "notification_at",
                value: new DateTime(2025, 4, 9, 18, 12, 40, 835, DateTimeKind.Local).AddTicks(247));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 3,
                column: "notification_at",
                value: new DateTime(2025, 4, 9, 18, 12, 40, 835, DateTimeKind.Local).AddTicks(249));

            migrationBuilder.UpdateData(
                table: "post_reaction",
                keyColumn: "post_reaction_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 9, 11, 12, 40, 836, DateTimeKind.Utc).AddTicks(5121));

            migrationBuilder.UpdateData(
                table: "post_reaction",
                keyColumn: "post_reaction_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 9, 11, 12, 40, 836, DateTimeKind.Utc).AddTicks(5124));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 9, 11, 12, 40, 838, DateTimeKind.Utc).AddTicks(1828));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 9, 11, 12, 40, 838, DateTimeKind.Utc).AddTicks(1832));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 9, 11, 12, 40, 838, DateTimeKind.Utc).AddTicks(1834));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 4, 9, 11, 12, 40, 838, DateTimeKind.Utc).AddTicks(1835));

            migrationBuilder.UpdateData(
                table: "tag",
                keyColumn: "tag_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 9, 11, 12, 40, 838, DateTimeKind.Utc).AddTicks(9673));

            migrationBuilder.UpdateData(
                table: "tag",
                keyColumn: "tag_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 9, 11, 12, 40, 838, DateTimeKind.Utc).AddTicks(9676));

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 9, 11, 12, 40, 839, DateTimeKind.Utc).AddTicks(3801), "$2a$11$d2jQ7gBpdCbi.eToWS5S/eD0/zxjAioS0TekbomqVhBEIBkKth9SW" });

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 9, 11, 12, 40, 953, DateTimeKind.Utc).AddTicks(7541), "$2a$11$44MLGnx5WKRy7Sd/LeJdQuzwmjCXPw8vQjma8MaKY8rXpYjGxB.82" });
        }
    }
}
