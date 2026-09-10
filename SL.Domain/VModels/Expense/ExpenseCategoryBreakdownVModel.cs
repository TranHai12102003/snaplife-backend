namespace SL.Domain.VModels.Expense
{
    public class ExpenseCategoryBreakdownVModel
    {
        public long? CategoryId { get; set; }
        public string CategoryName { get; set; } = "Khác";
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public decimal TotalAmount { get; set; }
        public double Percentage { get; set; }
        public int ExpenseCount { get; set; }
    }
}
