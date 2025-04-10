using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Command.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTablePost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "post_thumbnail",
                table: "post",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "post_summary",
                table: "post",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "post_thumbnail",
                table: "post",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "post_summary",
                table: "post",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 7, 10, 38, 56, 18, DateTimeKind.Utc).AddTicks(8786));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 7, 10, 38, 56, 18, DateTimeKind.Utc).AddTicks(8793));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 7, 10, 38, 56, 18, DateTimeKind.Utc).AddTicks(8794));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 4, 7, 10, 38, 56, 18, DateTimeKind.Utc).AddTicks(8796));

            migrationBuilder.UpdateData(
                table: "follow",
                keyColumn: "follow_id",
                keyValue: 1,
                column: "followed_at",
                value: new DateTime(2025, 4, 7, 10, 38, 56, 22, DateTimeKind.Utc).AddTicks(6741));

            migrationBuilder.UpdateData(
                table: "follow",
                keyColumn: "follow_id",
                keyValue: 2,
                column: "followed_at",
                value: new DateTime(2025, 4, 7, 10, 38, 56, 22, DateTimeKind.Utc).AddTicks(6745));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 1,
                column: "notification_at",
                value: new DateTime(2025, 4, 7, 17, 38, 56, 28, DateTimeKind.Local).AddTicks(5140));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 2,
                column: "notification_at",
                value: new DateTime(2025, 4, 7, 17, 38, 56, 28, DateTimeKind.Local).AddTicks(5161));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 3,
                column: "notification_at",
                value: new DateTime(2025, 4, 7, 17, 38, 56, 28, DateTimeKind.Local).AddTicks(5162));

            migrationBuilder.UpdateData(
                table: "post_reaction",
                keyColumn: "post_reaction_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 7, 10, 38, 56, 30, DateTimeKind.Utc).AddTicks(183));

            migrationBuilder.UpdateData(
                table: "post_reaction",
                keyColumn: "post_reaction_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 7, 10, 38, 56, 30, DateTimeKind.Utc).AddTicks(187));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 7, 10, 38, 56, 31, DateTimeKind.Utc).AddTicks(5532));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 7, 10, 38, 56, 31, DateTimeKind.Utc).AddTicks(5537));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 7, 10, 38, 56, 31, DateTimeKind.Utc).AddTicks(5538));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 4, 7, 10, 38, 56, 31, DateTimeKind.Utc).AddTicks(5539));

            migrationBuilder.UpdateData(
                table: "tag",
                keyColumn: "tag_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 7, 10, 38, 56, 32, DateTimeKind.Utc).AddTicks(3333));

            migrationBuilder.UpdateData(
                table: "tag",
                keyColumn: "tag_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 7, 10, 38, 56, 32, DateTimeKind.Utc).AddTicks(3339));

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 7, 10, 38, 56, 32, DateTimeKind.Utc).AddTicks(7450), "$2a$11$QU6qsm2oygfPiz28h5KAl.FRRpjsOWpZOd8UCa3/jINCGANpSEwYy" });

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 7, 10, 38, 56, 152, DateTimeKind.Utc).AddTicks(7169), "$2a$11$W7sa8hOoiEZ6BqwXU7wus.nXluSmzzz/lxibVPpGbi/VFMr4Jh9Nu" });
        }
    }
}
