using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;
/// <summary>
/// Configures the User entity: primary key and deterministic development seed data.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.HasKey(x => x.Id);

        var u1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var u2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var u3 = Guid.Parse("33333333-3333-3333-3333-333333333333");

        b.HasData(
            new User { Id = u1, FirstName = "Pelle", LastName = "Larsson", UserName = "Pelle123", Email = "pelle@mail.com" },
            new User { Id = u2, FirstName = "Anna", LastName = "Svensson", UserName = "Anna1337", Email = "anna@mail.com" },
            new User { Id = u3, FirstName = "Håkan", LastName = "Karlsson", UserName = "Tech-hakan", Email = "hakan_it@mail.com" }
        );
    }
}
