using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infractructure.Data.Configurations;

/// <summary>
/// Configuration for the <see cref="Document"/> entity.
/// </summary>
public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Document");
        
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.Path)
            .IsUnique();

        builder.HasOne(e => e.User)
            .WithMany(e => e.Documents)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Course)
            .WithMany(e => e.Documents)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(e => e.Module)
            .WithMany(e => e.Documents)
            .HasForeignKey(e => e.ModuleId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(e => e.Activity)
            .WithMany(e => e.Documents)
            .HasForeignKey(e => e.ActivityId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}