using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("Module");
        
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.Course)
             .WithMany(e => e.Modules)
             .HasForeignKey(e => e.CourseId)
             .OnDelete(DeleteBehavior.Cascade);
    }
}