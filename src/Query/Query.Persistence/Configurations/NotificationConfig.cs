using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Query.Domain.Entities;
using Query.Persistence.Constants;

namespace Query.Persistence.Configurations
{
    public class NotificationConfig : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable(TableName.NotificationTable);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("notification_id");
            builder.Property(x => x.Type).HasColumnName("notification_type");
            builder.Property(x => x.CommentId).HasColumnName("comment_id");
            builder.Property(x => x.RecipientUserId).HasColumnName("recipient_user_id");
            builder.Property(x => x.Seen).HasColumnName("seen");
            builder.Property(x => x.NotificationAt).HasColumnName("notification_at");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.ReplayForCommentId).HasColumnName("replay_for_comment_id");
            builder.Property(x => x.PostId).HasColumnName("post_id");

            builder.HasOne(x => x.Comment)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.CommentId);

            builder.HasOne(x => x.RecipientUser)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.RecipientUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Post)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasData(
                new Notification
                {
                    Id = 1,
                    CommentId = 1,
                    NotificationAt = DateTime.Now,
                    PostId = 1,
                    RecipientUserId = 1,
                    ReplayForCommentId = null,
                    Seen = false,
                    Type = "Comment",
                    UserId = 2,
                },
                new Notification
                {
                    Id = 2,
                    CommentId = null,
                    NotificationAt = DateTime.Now,
                    PostId = null,
                    RecipientUserId = 2,
                    ReplayForCommentId = null,
                    Seen = false,
                    Type = "Follow",
                    UserId = 1,
                },
                new Notification
                {
                    Id = 3,
                    CommentId = null,
                    NotificationAt = DateTime.Now,
                    PostId = 2,
                    RecipientUserId = 1,
                    ReplayForCommentId = null,
                    Seen = false,
                    Type = "CreatePost",
                    UserId = 2,
                }
                );
        }
    }
}
