using SL.Domain.Common.Constants;

namespace SL.Domain.Common.Models
{
    public class BaseFilterParams
    {
        public string? SearchString { get; set; }
        public int PageNumber { get; set; } = Numbers.Pagination.DefaultPageNumber;
        public int PageSize { get; set; } = Numbers.Pagination.DefaultMobilePageSize;
        public bool? IsActive { get; set; } = true;
    }
}

