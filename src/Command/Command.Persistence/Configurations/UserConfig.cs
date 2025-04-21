using Contract.Extensions;
using Command.Domain.Entities;
using Command.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Command.Persistence.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(TableName.UserTable);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("user_id");
            builder.Property(x => x.PasswordHash).HasColumnName("password_hash");
            builder.Property(x => x.FullName).HasColumnName("full_name");
            builder.Property(x => x.Email).HasColumnName("email");
            builder.Property(x => x.Bio).HasColumnName("bio");
            builder.Property(x => x.Avatar).HasColumnName("avatar");
            builder.Property(x => x.RoleId).HasColumnName("role_id");
            builder.Property(x => x.LastLoginAt).HasColumnName("last_login_at");
            builder.Property(x => x.IsLoginWithGoogle).HasColumnName("is_login_google");
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            builder.Property(x => x.IsActived).HasColumnName("is_actived");
            builder.Property(x => x.IsEmailVerified).HasColumnName("is_email_verified");


            builder.HasOne(x => x.Role).WithMany(x => x.Users).HasForeignKey(x => x.RoleId);

            builder.HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@gmail.com",
                    FullName = "admin",
                    IsLoginWithGoogle = false,
                    Bio = "bio-admin",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    Avatar = "default-avatar.png",
                    //PasswordHash = _passwordHasher.HashPassword("admin"),
                    PasswordHash = PasswordExtensions.HashPassword("admin"),
                    RoleId = 1,
                    IsActived = true,
                    IsEmailVerified = true,
                },
                new User
                {
                    Id = 2,
                    Email = "user@gmail.com",
                    FullName = "user",
                    IsLoginWithGoogle = false,
                    Bio = "bio-user",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    Avatar = "default-avatar.png",
                    //PasswordHash = _passwordHasher.HashPassword("user"),
                    PasswordHash = PasswordExtensions.HashPassword("user"),
                    RoleId = 2,
                    IsActived = true,
                    IsEmailVerified = true,
                }
                );
        }
    }
}
