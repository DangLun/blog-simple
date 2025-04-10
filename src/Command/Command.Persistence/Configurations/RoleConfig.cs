using Command.Domain.Entities;
using Command.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Command.Persistence.Configurations
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(TableName.RoleTable);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("role_id");
            builder.Property(x => x.RoleName).HasColumnName("role_name");

            builder.HasData(
                
                new Role
                {
                    Id = 1,
                    RoleName = "ADMIN"
                },
                new Role
                {
                    Id = 2,
                    RoleName = "USER"
                }
                );
        }
    }
}
