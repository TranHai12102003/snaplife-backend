using SL.Domain.Common.Models;
using SL.Domain.VModels.Expense;

namespace SL.Domain.IServices
{
    public interface IExpenseService
    {
        Task<ResponseResult> GetCategoriesAsync();
        Task<ResponseResult> CreateCategoryAsync(ExpenseCategoryCreateRequest request);
        Task<ResponseResult> DeleteCategoryAsync(long categoryId);
        Task<PaginationModel<ExpenseItemVModel>> GetExpenseHistoryAsync(ExpenseFilterParams filter);
        Task<ExpenseSummaryVModel> GetExpenseSummaryAsync(DateTime? fromDate, DateTime? toDate);
        Task<List<FoodCalendarDayVModel>> GetFoodCalendarAsync(int year, int month);
    }
}
