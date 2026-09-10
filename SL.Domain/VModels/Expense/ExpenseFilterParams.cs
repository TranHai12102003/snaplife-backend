using SL.Domain.Common.Models;

namespace SL.Domain.VModels.Expense
{
    public class ExpenseFilterParams : BaseFilterParams
    {
        public long? CategoryId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}
