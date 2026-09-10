namespace SL.Domain.VModels.Expense
{
    public class ExpenseSummaryVModel
    {
        public string Period { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "VND";
        public int TotalExpenses { get; set; }
        public decimal DailyAverage { get; set; }
        public List<ExpenseCategoryBreakdownVModel> Categories { get; set; } = new List<ExpenseCategoryBreakdownVModel>();
        public List<ExpenseItemVModel> RecentExpenses { get; set; } = new List<ExpenseItemVModel>();
    }
}
