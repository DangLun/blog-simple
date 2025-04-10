using Query.Domain.Entities;
using Query.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Query.Persistence.Configurations
{
    public class ReactionConfig : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.ToTable(TableName.ReactionTable);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("reaction_id");
            builder.Property(x => x.ReactionName).HasColumnName("reaction_name");
            builder.Property(x => x.ReactionDescription).HasColumnName("reaction_description");
            builder.Property(x => x.ReactionIcon).HasColumnName("reaction_icon");
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");


            builder.HasData(
                    new Reaction
                    {
                        Id = 1,
                        CreatedAt = DateTime.UtcNow,
                        ReactionIcon = "icon-1.png",
                        ReactionName = "Haha",
                        ReactionDescription = "reaction-description-1"
                    },
                    new Reaction
                    {
                        Id = 2,
                        CreatedAt = DateTime.UtcNow,
                        ReactionIcon = "icon-2.png",
                        ReactionName = "Tim",
                        ReactionDescription = "reaction-description-2"
                    },
                    new Reaction
                    {
                        Id = 3,
                        CreatedAt = DateTime.UtcNow,
                        ReactionIcon = "icon-3.png",
                        ReactionName = "Thích",
                        ReactionDescription = "reaction-description-3"
                    },
                    new Reaction
                    {
                        Id = 4,
                        CreatedAt = DateTime.UtcNow,
                        ReactionIcon = "icon-4.png",
                        ReactionName = "Phẩn nộ",
                        ReactionDescription = "reaction-description-4"
                    }
                );
        }
    }
}
