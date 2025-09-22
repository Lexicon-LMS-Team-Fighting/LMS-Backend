using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;

/// <summary>
/// </summary>
public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Course");

        builder.HasKey(e => e.Id);


        builder.HasIndex(e => e.Name)
            .IsUnique();

        builder.HasMany(e => e.UserCourses)
            .WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        var c1 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var c2 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var c3 = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

        builder.Property(x => x.StartDate).HasColumnType("date");
        builder.Property(x => x.EndDate).HasColumnType("date");

        builder.HasData(
            new Course { Id = c1, Name = "Machine Learning 1", Description = "Basics in Machine Learning", StartDate = new DateTime(2025, 9, 1), EndDate = new DateTime(2025, 12, 15) },
            new Course { Id = c2, Name = "Python 1", Description = "Basics in Python.", StartDate = new DateTime(2025, 9, 15), EndDate = new DateTime(2025, 12, 20) },
            new Course { Id = c3, Name = "Github", Description = "Basics in Github", StartDate = new DateTime(2025, 10, 1), EndDate = new DateTime(2026, 1, 15) }
        );
    }
}
