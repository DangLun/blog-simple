using Query.Domain.Entities;
using Query.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Query.Persistence.Configurations
{
    public class PostTagConfig : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> builder)
        {
            builder.ToTable(TableName.PostTagTable);
            builder.HasKey(x => new { x.PostId, x.TagId });
            builder.Property(x => x.PostId).HasColumnName("post_id");
            builder.Property(x => x.TagId).HasColumnName("tag_id");

            builder.HasOne(x => x.Post)
                .WithMany(x => x.PostTags)
                .HasForeignKey(x => x.PostId);

            builder.HasOne(x => x.Tag)
                .WithMany(x => x.PostTags)
                .HasForeignKey(x => x.TagId);

            builder.HasData(
                    new PostTag { PostId = 1, TagId = 1},
                    new PostTag { PostId = 2, TagId = 2}
                );
        }
    }
}
