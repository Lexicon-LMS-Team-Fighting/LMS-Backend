using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Infractructure.Data.Configurations
{
    /// <summary>
    /// Configuration for the <see cref="LMSActivityFeedback"/> entity.
    /// </summary>
    public class LMSActivityFeedbackConfiguration : IEntityTypeConfiguration<LMSActivityFeedback>
    {
        public void Configure(EntityTypeBuilder<LMSActivityFeedback> builder)
        {
            builder.ToTable("LMSActivityFeedback");

            builder.HasKey(x => new { x.UserId, x.LMSActivityId });

            builder.HasOne(f => f.User)
                .WithMany(u => u.LMSActivityFeedbacks)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.LMSActivity)
                .WithMany(f => f.LMSActivityFeedbacks)
                .HasForeignKey(f => f.LMSActivityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
