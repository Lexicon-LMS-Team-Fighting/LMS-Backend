using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class LMSActivityFeedback
    {
        public string UserId { get; set; } = null!;
        public Guid LMSActivityId { get; set; }
        public string? Feedback { get; set; }
        public string Status { get; set; } = string.Empty;

        // Foreign Keys / Navigation Properties
        public ApplicationUser User { get; set; } = null!;
        public LMSActivity LMSActivity { get; set; } = null!;
    }
}
