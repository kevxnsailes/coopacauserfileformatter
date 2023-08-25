namespace Application
{
    public class ApplicationModel
    {
        public string CUOperation { get; set; } = "CU";
        public string CROperation { get; set; } = "CR";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ExternalId { get; set; }
        public string EmailUsername { get; set; }
        public int ProductId { get; set; } = 1;
        public string AccountNumber { get; set; }
    }
}
