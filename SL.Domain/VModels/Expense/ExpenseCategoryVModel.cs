namespace SL.Domain.VModels.Expense
{
    public class ExpenseCategoryVModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public bool IsDefault { get; set; }
        public bool CanDelete { get; set; }
        public int DisplayOrder { get; set; }
    }
}
