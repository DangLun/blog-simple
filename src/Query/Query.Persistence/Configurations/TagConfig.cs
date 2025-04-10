using Query.Domain.Entities;
using Query.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Query.Persistence.Configurations
{
    public class TagConfig : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable(TableName.TagTable);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("tag_id");
            builder.Property(x => x.TagName).HasColumnName("tag_name");
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            builder.Property(x => x.IsDeleted).HasColumnName("is_deleted");
            builder.Property(x => x.ClassName).HasColumnName("class_name");
            builder.Property(x => x.Description).HasColumnName("tag_description");


            builder.HasData(
                
                new Tag
                {
                    Id = 1,
                    CreatedAt = DateTime.UtcNow,
                    TagName = "tag-name-test-1",
                    IsDeleted = false
                },
                new Tag
                {
                    Id = 2,
                    CreatedAt = DateTime.UtcNow,
                    TagName = "tag-name-test-2",
                    IsDeleted = false
                }
                );
        }
    }
}
