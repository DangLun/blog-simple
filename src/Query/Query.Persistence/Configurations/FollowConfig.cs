using Query.Domain.Entities;
using Query.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Query.Persistence.Configurations
{
    public class FollowConfig : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.ToTable(TableName.FollowTable);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("follow_id");
            builder.Property(x => x.FollowedId).HasColumnName("followed_id");
            builder.Property(x => x.FollowerId).HasColumnName("follower_id");
            builder.Property(x => x.FollowedAt).HasColumnName("followed_at");

            builder.HasOne(x => x.Followed).WithMany(x => x.Followers).HasForeignKey(x => x.FollowedId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Follower).WithMany(x => x.Followeds).HasForeignKey(x => x.FollowerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasData(
            new Follow
            {
                Id = 1,
                FollowerId = 1,
                FollowedId = 2,
                FollowedAt = DateTime.UtcNow,
            },
            new Follow{
                Id = 2,
                FollowerId = 2,
                FollowedId = 1,
                FollowedAt = DateTime.UtcNow,
            });
        }
    }
}
