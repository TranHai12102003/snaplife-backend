using SL.Domain.Common.Models;

namespace SL.Domain.VModels.Post
{
    public class PostFilterVModel : BaseFilterParams
    {
        public bool? IsExpenseOnly { get; set; }
        public long? GroupId { get; set; }
        public long? ExpenseCategoryId { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}


