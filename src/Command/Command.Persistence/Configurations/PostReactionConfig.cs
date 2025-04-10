using Command.Domain.Entities;
using Command.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Command.Persistence.Configurations
{
    public class PostReactionConfig : IEntityTypeConfiguration<PostReaction>
    {
        public void Configure(EntityTypeBuilder<PostReaction> builder)
        {
            builder.ToTable(TableName.PostReactionTable);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("post_reaction_id");
            builder.Property(x => x.PostId).HasColumnName("post_id");
            builder.Property(x => x.UserId).HasColumnName("user_id"); 
            builder.Property(x => x.ReactionId).HasColumnName("reaction_id");
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

            builder.HasOne(x => x.User)
                .WithMany(x => x.PostReactions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Reaction)
                .WithMany(x => x.PostReactions)
                .HasForeignKey(x => x.ReactionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Post)
                .WithMany(x => x.PostReactions)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => new { x.PostId, x.UserId, x.ReactionId })
                    .IsUnique();

            builder.HasData(
                    new PostReaction { Id = 1, PostId = 1, ReactionId = 1, UserId = 1, CreatedAt = DateTime.UtcNow },
                    new PostReaction { Id = 2, PostId = 2, ReactionId = 4, UserId = 2, CreatedAt = DateTime.UtcNow }
                );
        }
    }
}
