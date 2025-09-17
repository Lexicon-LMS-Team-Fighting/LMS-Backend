using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users");
        b.HasKey(x => x.Id);

        b.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        b.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        b.Property(x => x.UserName).HasMaxLength(60).IsRequired();
        b.Property(x => x.Email).HasMaxLength(254).IsRequired();

        b.HasIndex(x => x.UserName).IsUnique();
        b.HasIndex(x => x.Email).IsUnique();

        /* Un comment when UserCourses and Documents exists
          b.HasMany(x => x.UserCourses)
          .WithOne(uc => uc.User)
          .HasForeignKey(uc => uc.UserId);

         b.HasMany(x => x.Documents)
          .WithOne(d => d.User)
          .HasForeignKey(d => d.UserId);
        */
        // Is this needed? 1:1 to ApplicationUser
        // b.HasOne(x => x.ApplicationUser)
        //  .WithOne(u => u.Profile)
        //  .HasForeignKey<User>(x => x.ApplicationUserId);
    }
}
