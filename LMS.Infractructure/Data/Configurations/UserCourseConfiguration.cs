using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;

/// <summary>
/// Configures the UserCourse join: composite PK (UserId, CourseId), required FKs with Restrict delete, and seed links.
/// </summary>
public class UserCourseConfiguration : IEntityTypeConfiguration<UserCourse>
{
    public void Configure(EntityTypeBuilder<UserCourse> b)
    {
        
        b.HasKey(x => new { x.UserId, x.CourseId });

        
        b.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne<Course>()
            .WithMany()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        var u1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var u2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var u3 = Guid.Parse("33333333-3333-3333-3333-333333333333");

        var c1 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var c2 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var c3 = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

        b.HasData(
            new UserCourse { UserId = u1, CourseId = c1 },
            new UserCourse { UserId = u1, CourseId = c2 },
            new UserCourse { UserId = u2, CourseId = c1 },
            new UserCourse { UserId = u2, CourseId = c3 },
            new UserCourse { UserId = u3, CourseId = c2 }
        );
    }
}
