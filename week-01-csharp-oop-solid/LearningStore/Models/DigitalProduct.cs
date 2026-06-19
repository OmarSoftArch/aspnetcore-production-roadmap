namespace LearningStore.Models
{
    public class DigitalProduct : Product
    {
        private string _downloadUrl = string.Empty;

        public string DownloadUrl
        {
            get => _downloadUrl;
            set => _downloadUrl = value?.Trim() ?? string.Empty;
        }

        public bool IsInstantDelivery => !string.IsNullOrWhiteSpace(DownloadUrl);

        public override string GetSummary()
        {
            return $"{base.GetSummary()} | Digital | Instant: {IsInstantDelivery}";
        }
    }
}
