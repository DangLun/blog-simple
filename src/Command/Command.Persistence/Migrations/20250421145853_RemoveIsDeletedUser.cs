using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Command.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsDeletedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "user");

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 21, 14, 58, 52, 509, DateTimeKind.Utc).AddTicks(6185));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 21, 14, 58, 52, 509, DateTimeKind.Utc).AddTicks(6194));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 21, 14, 58, 52, 509, DateTimeKind.Utc).AddTicks(6196));

            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "comment_id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 4, 21, 14, 58, 52, 509, DateTimeKind.Utc).AddTicks(6197));

            migrationBuilder.UpdateData(
                table: "follow",
                keyColumn: "follow_id",
                keyValue: 1,
                column: "followed_at",
                value: new DateTime(2025, 4, 21, 14, 58, 52, 513, DateTimeKind.Utc).AddTicks(2234));

            migrationBuilder.UpdateData(
                table: "follow",
                keyColumn: "follow_id",
                keyValue: 2,
                column: "followed_at",
                value: new DateTime(2025, 4, 21, 14, 58, 52, 513, DateTimeKind.Utc).AddTicks(2237));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 1,
                column: "notification_at",
                value: new DateTime(2025, 4, 21, 21, 58, 52, 518, DateTimeKind.Local).AddTicks(9900));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 2,
                column: "notification_at",
                value: new DateTime(2025, 4, 21, 21, 58, 52, 518, DateTimeKind.Local).AddTicks(9963));

            migrationBuilder.UpdateData(
                table: "notification",
                keyColumn: "notification_id",
                keyValue: 3,
                column: "notification_at",
                value: new DateTime(2025, 4, 21, 21, 58, 52, 518, DateTimeKind.Local).AddTicks(9965));

            migrationBuilder.UpdateData(
                table: "post_reaction",
                keyColumn: "post_reaction_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 21, 14, 58, 52, 520, DateTimeKind.Utc).AddTicks(4169));

            migrationBuilder.UpdateData(
                table: "post_reaction",
                keyColumn: "post_reaction_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 21, 14, 58, 52, 520, DateTimeKind.Utc).AddTicks(4172));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 21, 14, 58, 52, 521, DateTimeKind.Utc).AddTicks(8845));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 21, 14, 58, 52, 521, DateTimeKind.Utc).AddTicks(8850));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 21, 14, 58, 52, 521, DateTimeKind.Utc).AddTicks(8851));

            migrationBuilder.UpdateData(
                table: "reactions",
                keyColumn: "reaction_id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 4, 21, 14, 58, 52, 521, DateTimeKind.Utc).AddTicks(8852));

            migrationBuilder.UpdateData(
                table: "tag",
                keyColumn: "tag_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 21, 14, 58, 52, 522, DateTimeKind.Utc).AddTicks(6700));

            migrationBuilder.UpdateData(
                table: "tag",
                keyColumn: "tag_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 21, 14, 58, 52, 522, DateTimeKind.Utc).AddTicks(6707));

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 21, 14, 58, 52, 523, DateTimeKind.Utc).AddTicks(792), "$2a$11$GQv/68NdXu.5zUJzUcO8pewLK1WRPlVV.kUlC2vTFjx6woseh7VJa" });

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 21, 14, 58, 52, 643, DateTimeKind.Utc).AddTicks(6982), "$2a$11$NX8cA0BcdCCQx8f6aRlAzeLZ5fCZUrxweS8UuPcs1lAl0vl/LW92S" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "user",
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
                columns: new[] { "created_at", "is_deleted", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 14, 7, 28, 29, 517, DateTimeKind.Utc).AddTicks(9335), false, "$2a$11$4ViFbFey8KEm.rqv5gwM9.wS0jbnEkU1oy9VIdn8K6sVDFfkY8eI6" });

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created_at", "is_deleted", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 14, 7, 28, 29, 639, DateTimeKind.Utc).AddTicks(5603), false, "$2a$11$2p0349noPq3CXSZjGtT0HeT238MalPN0WgVVBbGqdWw6QLReDMA3e" });
        }
    }
}
