using Query.Domain.Entities;
using Query.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Query.Persistence.Configurations
{
    public class PostTextConfig : IEntityTypeConfiguration<PostText>
    {
        public void Configure(EntityTypeBuilder<PostText> builder)
        {
            builder.ToTable(TableName.PostTextTable);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("post_text_id");
            builder.Property(x => x.Content).HasColumnName("post_text");

            builder.HasData(
                new PostText { Id = 1, Content = "Content Blog 1" },
                new PostText { Id = 2, Content = "Content Blog 2" }
                );
        }
    }
}
