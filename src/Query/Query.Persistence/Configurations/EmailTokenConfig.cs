using Query.Domain.Entities;
using Query.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Query.Persistence.Configurations
{
    public class EmailTokenConfig : IEntityTypeConfiguration<EmailToken>
    {
        public void Configure(EntityTypeBuilder<EmailToken> builder)
        {
            builder.ToTable(TableName.EmailTokenTable);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("email_token_id");
            builder.Property(x => x.Token).HasColumnName("email_token");
            builder.Property(x => x.ExpiredAt).HasColumnName("expired_at");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.IsUsed).HasColumnName("is_used");


            builder.HasOne(x => x.User).WithMany(x => x.EmailTokens).HasForeignKey(
                x => x.UserId);
        }
    }
}
