namespace LearningStore.Models
{
    public class Customer
    {
        private string _companyName = string.Empty;
        private string _contactEmail = string.Empty;

        public int Id { get; set; }

        public string CompanyName
        {
            get => _companyName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Company name is required.", nameof(value));

                _companyName = value.Trim();
            }
        }

        public string ContactEmail
        {
            get => _contactEmail;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
                    throw new ArgumentException("A valid contact email is required.", nameof(value));

                _contactEmail = value.Trim();
            }
        }

        public bool IsActive { get; set; } = true;

        public string GetDisplayName()
        {
            return $"{CompanyName} <{ContactEmail}>";
        }
    }
}
