using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Query.Domain.Entities;
using Query.Persistence.Constants;

namespace Query.Persistence.Configurations
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable(TableName.PostTable);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("post_id");
            builder.Property(x => x.PostTitle).HasColumnName("post_title");
            builder.Property(x => x.PostThumbnail).HasColumnName("post_thumbnail");
            builder.Property(x => x.PostSummary).HasColumnName("post_summary");
            builder.Property(x => x.PostTextId).HasColumnName("post_text_id");
            builder.Property(x => x.TotalReactions).HasColumnName("total_reactions");
            builder.Property(x => x.TotalComments).HasColumnName("total_comments");
            builder.Property(x => x.TotalReads).HasColumnName("total_reads");
            builder.Property(x => x.IsPublished).HasColumnName("is_published");
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.IsDeleted).HasColumnName("is_deleted");

            // ForeignKey Configuration
            // 1 - n blog - user
            builder.HasOne(x => x.User)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.UserId);

            // 1 - 1 blog - blog_content
            builder.HasOne(x => x.PostText)
                .WithOne(x => x.Post)
                .HasForeignKey<Post>(x => x.PostTextId);

            builder.HasData(
                    new Post
                    {
                        Id = 1,
                        PostTitle = "blog-title-1",
                        PostSummary = "blog-description-1",
                        TotalComments = 1,
                        TotalReads = 1,
                        IsPublished = true,
                        TotalReactions = 1,
                        UserId = 1,
                        PostThumbnail = "banner.png",
                        PostTextId = 1,
                        IsDeleted = false
                    },
                    new Post
                    {
                        Id = 2,
                        PostTitle = "blog-title-2",
                        PostSummary = "blog-description-2",
                        TotalComments = 2,
                        TotalReads = 2,
                        IsPublished = true,
                        TotalReactions = 2,
                        UserId = 2,
                        PostThumbnail = "banner.png",
                        PostTextId = 2,
                        IsDeleted = false
                    }
                );
        }
    }
}
