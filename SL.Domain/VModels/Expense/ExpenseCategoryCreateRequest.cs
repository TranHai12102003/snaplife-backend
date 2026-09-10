namespace SL.Domain.VModels.Expense
{
    public class ExpenseCategoryCreateRequest
    {
        public string Name { get; set; } = null!;
        public string? Icon { get; set; }
        public string? Color { get; set; }
    }
}
