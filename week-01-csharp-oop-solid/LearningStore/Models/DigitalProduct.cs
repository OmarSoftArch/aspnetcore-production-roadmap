using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningStore.Models
{
    class DigitalProduct:Product
    {
        public string DownloadUrl { get; set; } = string.Empty;
        public bool IsInstantDelivery => !string.IsNullOrWhiteSpace(DownloadUrl);
        public override string GetSummary()
        {
            return $"{base.GetSummary()} | Digital | Instant: {IsInstantDelivery}";
        }
    }
}
