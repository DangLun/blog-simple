using Query.Domain.Entities;
using Query.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Query.Persistence.Configurations
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable(TableName.CommentTable);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("comment_id");
            builder.Property(x => x.CommentText).HasColumnName("comment_text");
            builder.Property(x => x.PostId).HasColumnName("post_id");
            builder.Property(x => x.ParentCommentId).HasColumnName("parent_comment_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            builder.Property(x => x.IsDeleted).HasColumnName("is_deleted");

            builder.HasOne(x => x.Post)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.HasData(
                    new Comment 
                    { 
                        Id = 1, 
                        PostId = 1, 
                        CreatedAt = DateTime.UtcNow, 
                        ParentCommentId = 0, 
                        UserId = 1,
                        CommentText = "content-1",
                        IsDeleted = false
                    },
                    new Comment 
                    { 
                        Id = 2,
                        PostId = 2, 
                        CreatedAt = DateTime.UtcNow, 
                        ParentCommentId = 0, 
                        UserId = 2,
                        CommentText = "content-2",
                        IsDeleted = false
                    },
                    new Comment
                    {
                        Id = 3,
                        PostId = 2,
                        CreatedAt = DateTime.UtcNow,
                        ParentCommentId = 0,
                        UserId = 2,
                        CommentText = "content-3",
                        IsDeleted = false
                    },
                    new Comment
                    {
                        Id = 4,
                        PostId = 1,
                        CreatedAt = DateTime.UtcNow,
                        ParentCommentId = 0,
                        UserId = 1,
                        CommentText = "content-4",
                        IsDeleted = false
                    }
                );
        }
    }
}
