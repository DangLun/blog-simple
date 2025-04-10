using Command.Domain.Entities;
using Command.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Command.Persistence.Configurations
{
    public class BlackListTokenConfig : IEntityTypeConfiguration<BlackListToken>
    {
        public void Configure(EntityTypeBuilder<BlackListToken> builder)
        {
            builder.ToTable(TableName.BlackListToken);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TokenRevoked).HasColumnName("token_revoked");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.Reason).HasColumnName("reason");

            builder.HasOne(x => x.User).WithMany(x => x.BlackListTokens).HasForeignKey(x => x.UserId);
        }
    }
}
