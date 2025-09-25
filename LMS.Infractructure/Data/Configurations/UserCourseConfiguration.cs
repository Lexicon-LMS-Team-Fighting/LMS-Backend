using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;

/// <summary>
/// Configuration for the <see cref="UserCourse"/> entity.
/// </summary>
public class UserCourseConfiguration : IEntityTypeConfiguration<UserCourse>
{
    public void Configure(EntityTypeBuilder<UserCourse> builder)
    {
        builder.ToTable("UserCourse");

        builder.HasKey(x => new { x.UserId, x.CourseId });

		builder.HasOne(uc => uc.User)
			.WithMany(u => u.UserCourses)
			.HasForeignKey(uc => uc.UserId)
			.OnDelete(DeleteBehavior.Cascade);
		
		builder.HasOne(uc => uc.Course)
			.WithMany(c => c.UserCourses)
			.HasForeignKey(uc => uc.CourseId)
			.OnDelete(DeleteBehavior.Cascade);

	}
}
