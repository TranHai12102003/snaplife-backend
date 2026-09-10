namespace SL.Domain.VModels.Expense
{
    public class FoodCalendarDayVModel
    {
        public DateOnly Date { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "VND";
        public int Count { get; set; }
        public string? ThumbnailUrl { get; set; }
        public List<string> FoodNames { get; set; } = new List<string>();
    }
}
