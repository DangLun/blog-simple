using Command.Domain.Entities;
using Command.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Command.Persistence.Configurations
{
    public class PostSavedConfig : IEntityTypeConfiguration<PostSaved>
    {
        public void Configure(EntityTypeBuilder<PostSaved> builder)
        {
            builder.ToTable(TableName.PostSavedTable);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("post_saved_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.PostId).HasColumnName("post_id");
    
            builder.HasOne(x => x.User)
                .WithMany(x => x.PostSaveds)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Post)
                .WithMany(x => x.SavedByUsers)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasData(
                
                new PostSaved
                {
                    Id = 1,
                    PostId = 1,
                    UserId = 1,
                },
                new PostSaved
                {
                    Id = 2,
                    PostId = 2,
                    UserId = 1,
                },
                new PostSaved
                {
                    Id = 3,
                    PostId = 1,
                    UserId = 2,
                }
                );

        }
    }
}
