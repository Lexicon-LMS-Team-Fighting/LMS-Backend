using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;

/// <summary>
/// Configuration for the <see cref="ApplicationUser"/> entity.
/// </summary>
public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("ApplicationUser");

        builder.HasMany(e => e.UserCourses)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        var u1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var u2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var u3 = Guid.Parse("33333333-3333-3333-3333-333333333333");

        builder.HasData(
            new ApplicationUser { Id = u1.ToString(), FirstName = "Pelle", LastName = "Larsson", UserName = "Pelle123", Email = "pelle@mail.com" },
            new ApplicationUser { Id = u2.ToString(), FirstName = "Anna", LastName = "Svensson", UserName = "Anna1337", Email = "anna@mail.com" },
            new ApplicationUser { Id = u3.ToString(), FirstName = "Håkan", LastName = "Karlsson", UserName = "Tech-hakan", Email = "hakan_it@mail.com" }
        );
    }
}
