using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;

/// <summary>
/// Configuration for the <see cref="LMSActivity"/> entity.
/// </summary>
public class LMSActivityConfiguration : IEntityTypeConfiguration<LMSActivity>
{
    public void Configure(EntityTypeBuilder<LMSActivity> builder)
    {
        builder.ToTable("Activity");

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.ActivityType)
             .WithMany(e => e.Activities)
             .HasForeignKey(e => e.ActivityTypeId)
             .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(e => e.Module)
             .WithMany(e => e.Activities)
             .HasForeignKey(e => e.ModuleId)
             .OnDelete(DeleteBehavior.Cascade);
    }
}