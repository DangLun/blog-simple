using Command.Domain.Entities;
using Command.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Command.Persistence.Configurations
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable(TableName.RefreshTokenTable);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("refresh_token_id");
            builder.Property(x => x.ExpirationDate).HasColumnName("expiration_date");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.Token).HasColumnName("token");
            builder.Property(x => x.IsRevoked).HasColumnName("is_revoked");

            
            builder.HasIndex(x => x.Token).IsUnique();
            builder.HasOne(x => x.User).WithMany(x => x.RefreshTokens).HasForeignKey(x => x.UserId);
        }
    }
}
