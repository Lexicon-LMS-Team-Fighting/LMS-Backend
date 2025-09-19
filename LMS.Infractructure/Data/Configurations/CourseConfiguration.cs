using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;
/// <summary>
/// Configures the Course entity: primary key, maps StartDate/EndDate to SQL 'date', and seeds sample rows.
/// </summary>
public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> b)
    {
        b.HasKey(x => x.CourseId);

        var c1 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var c2 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var c3 = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

        b.Property(x => x.StartDate).HasColumnType("date");
        b.Property(x => x.EndDate).HasColumnType("date");

        b.HasData(
            new Course { CourseId = c1, Name = "Machine Learning 1", Description = "Basics in Machine Learning", StartDate = new DateTime(2025, 9, 1), EndDate = new DateTime(2025, 12, 15) },
            new Course { CourseId = c2, Name = "Python 1", Description = "Basics in Python.", StartDate = new DateTime(2025, 9, 15), EndDate = new DateTime(2025, 12, 20) },
            new Course { CourseId = c3, Name = "Github", Description = "Basics in Github", StartDate = new DateTime(2025, 10, 1), EndDate = new DateTime(2026, 1, 15) }
        );
    }
}
