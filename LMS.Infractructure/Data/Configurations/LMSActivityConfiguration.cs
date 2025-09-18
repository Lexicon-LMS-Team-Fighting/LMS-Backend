using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;

public class LMSActivityConfiguration : IEntityTypeConfiguration<LMSActivity>
{
    public void Configure(EntityTypeBuilder<LMSActivity> builder)
    {
        builder.ToTable("LMSActivity");
        
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.ActivityType)
             .WithMany(e => e.LMSActivities)
             .HasForeignKey(e => e.ActivityTypeId)
             .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Module)
             .WithMany(e => e.Activities)
             .HasForeignKey(e => e.ModuleId)
             .OnDelete(DeleteBehavior.Cascade);
    }
}