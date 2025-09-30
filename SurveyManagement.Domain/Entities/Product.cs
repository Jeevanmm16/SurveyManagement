namespace SurveyManagement.Domain.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; } // GUID ID  
        public string ProductName { get; set; } = default!;
        public ICollection<Survey> Surveys { get; set; } = new List<Survey>();
    }
}