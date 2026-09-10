namespace SL.Domain.VModels.Expense
{
    public class ExpenseItemVModel
    {
        public long PostId { get; set; }
        public string? Content { get; set; }
        public string? FoodName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "VND";
        public long? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryIcon { get; set; }
        public string? CategoryColor { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? LocationName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
