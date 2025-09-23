using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;

/// <summary>
/// Configuration for the <see cref="ActivityType"/> entity.
/// </summary>
public class ActivityTypeConfiguration : IEntityTypeConfiguration<ActivityType>
{
    public void Configure(EntityTypeBuilder<ActivityType> builder)
    {
        builder.ToTable("ActivityType");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.Name)
            .IsUnique();

        builder.HasMany(e => e.LMSActivities)
            .WithOne(e => e.ActivityType)
            .HasForeignKey(e => e.ActivityTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}