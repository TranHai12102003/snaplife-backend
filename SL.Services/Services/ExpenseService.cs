using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SL.Domain.Common.Constants;
using SL.Domain.Common.Models;
using SL.Domain.IServices;
using SL.Domain.VModels.Expense;
using SL.Infrastructures.EntityFramework;
using SL.Infrastructures.EntityFramework.Entities;
using SL.Services.Mappings;

namespace SL.Services.Services
{
    public class ExpenseService : Globals, IExpenseService
    {
        private readonly SnapLifeContext _context;

        public ExpenseService(
            SnapLifeContext context,
            IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _context = context;
        }

        public async Task<ResponseResult> GetCategoriesAsync()
        {
            var currentUserId = GlobalUserId;

            var categories = await _context.ExpenseCategories
                .Where(c => c.IsActive == true && (c.IsDefault || c.UserId == currentUserId))
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();

            var vmodels = categories
                .Select(c => ExpenseMappings.CategoryEntityToVModel(c, currentUserId))
                .ToList();

            return new SuccessResponseResult(vmodels);
        }

        public async Task<ResponseResult> CreateCategoryAsync(ExpenseCategoryCreateRequest request)
        {
            var currentUserId = GlobalUserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ErrorResponseResult(Strings.Messages.UserNotAuthenticated);
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new ErrorResponseResult("Tên danh mục không được để trống.");
            }

            var trimmedName = request.Name.Trim();
            var exists = await _context.ExpenseCategories
                .AnyAsync(c => c.IsActive == true
                            && (c.IsDefault || c.UserId == currentUserId)
                            && c.Name.ToLower() == trimmedName.ToLower());

            if (exists)
            {
                return new ErrorResponseResult("Danh mục này đã tồn tại.");
            }

            var category = new ExpenseCategory
            {
                Name = trimmedName,
                Icon = string.IsNullOrWhiteSpace(request.Icon) ? "🍴" : request.Icon.Trim(),
                Color = string.IsNullOrWhiteSpace(request.Color) ? "#607D8B" : request.Color.Trim(),
                IsDefault = false,
                UserId = currentUserId,
                DisplayOrder = 100,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = currentUserId,
                IsActive = true
            };

            _context.ExpenseCategories.Add(category);
            await _context.SaveChangesAsync();

            var vmodel = ExpenseMappings.CategoryEntityToVModel(category, currentUserId);
            return new SuccessResponseResult(vmodel, "Tạo danh mục chi tiêu thành công.");
        }

        public async Task<ResponseResult> DeleteCategoryAsync(long categoryId)
        {
            var currentUserId = GlobalUserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ErrorResponseResult(Strings.Messages.UserNotAuthenticated);
            }

            var category = await _context.ExpenseCategories.FindAsync(categoryId);
            if (category == null || category.IsActive != true)
            {
                return new ErrorResponseResult("Danh mục không tồn tại.");
            }

            if (category.IsDefault)
            {
                return new ErrorResponseResult("Không thể xóa danh mục mặc định của hệ thống.");
            }

            if (category.UserId != currentUserId)
            {
                return new ErrorResponseResult("Bạn không có quyền xóa danh mục này.");
            }

            category.IsActive = false;
            category.UpdatedDate = DateTime.UtcNow;
            category.UpdatedBy = currentUserId;

            await _context.SaveChangesAsync();
            return new SuccessResponseResult("Xóa danh mục chi tiêu thành công.");
        }

        public async Task<PaginationModel<ExpenseItemVModel>> GetExpenseHistoryAsync(ExpenseFilterParams filter)
        {
            var currentUserId = GlobalUserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new PaginationModel<ExpenseItemVModel> { TotalRecords = 0, Records = new List<ExpenseItemVModel>() };
            }

            // 1. Áp dụng BuildQueryable với AsNoTracking()
            var baseQuery = _context.Posts
                .AsNoTracking()
                .Where(p => p.UserId == currentUserId && p.IsExpense == true)
                .Where(BuildQueryable(filter));

            // 2. CountAsync() trước khi Include
            var totalRecords = await baseQuery.CountAsync();

            // 3. Include và phân trang
            var posts = await baseQuery
                .Include(p => p.ExpenseCategory)
                .Include(p => p.PostMedias.Where(pm => pm.IsActive == true).OrderBy(pm => pm.DisplayOrder))
                    .ThenInclude(pm => pm.File)
                .OrderByDescending(p => p.CreatedDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var records = posts.Select(ExpenseMappings.PostToExpenseItemVModel).ToList();

            return new PaginationModel<ExpenseItemVModel>
            {
                TotalRecords = totalRecords,
                Records = records
            };
        }

        public async Task<ExpenseSummaryVModel> GetExpenseSummaryAsync(DateTime? fromDate, DateTime? toDate)
        {
            var currentUserId = GlobalUserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ExpenseSummaryVModel();
            }

            var start = fromDate ?? DateTime.UtcNow.Date.AddDays(-30);
            var end = toDate ?? DateTime.UtcNow;

            var posts = await _context.Posts
                .AsNoTracking()
                .Include(p => p.ExpenseCategory)
                .Include(p => p.PostMedias.Where(pm => pm.IsActive == true).OrderBy(pm => pm.DisplayOrder))
                    .ThenInclude(pm => pm.File)
                .Where(p => p.UserId == currentUserId
                         && p.IsExpense == true
                         && p.IsActive == true
                         && p.CreatedDate >= start
                         && p.CreatedDate <= end)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            var totalAmount = posts.Sum(p => p.Amount ?? 0);
            var totalExpenses = posts.Count;

            var daysCount = Math.Max(1, (int)(end.Date - start.Date).TotalDays + 1);
            var dailyAverage = totalExpenses > 0 ? Math.Round(totalAmount / daysCount, 2) : 0;

            // Phân bổ theo danh mục
            var categoryGroups = posts
                .GroupBy(p => new
                {
                    CategoryId = p.ExpenseCategoryId,
                    Name = p.ExpenseCategory != null ? p.ExpenseCategory.Name : "Khác",
                    Icon = p.ExpenseCategory != null ? p.ExpenseCategory.Icon : "🍴",
                    Color = p.ExpenseCategory != null ? p.ExpenseCategory.Color : "#9E9E9E"
                })
                .Select(g =>
                {
                    var catTotal = g.Sum(p => p.Amount ?? 0);
                    return new ExpenseCategoryBreakdownVModel
                    {
                        CategoryId = g.Key.CategoryId,
                        CategoryName = g.Key.Name,
                        Icon = g.Key.Icon,
                        Color = g.Key.Color,
                        TotalAmount = catTotal,
                        Percentage = totalAmount > 0 ? Math.Round((double)(catTotal / totalAmount) * 100, 2) : 0,
                        ExpenseCount = g.Count()
                    };
                })
                .OrderByDescending(c => c.TotalAmount)
                .ToList();

            var recentExpenses = posts.Take(5).Select(ExpenseMappings.PostToExpenseItemVModel).ToList();

            return new ExpenseSummaryVModel
            {
                Period = $"{start:dd/MM/yyyy} - {end:dd/MM/yyyy}",
                TotalAmount = totalAmount,
                Currency = "VND",
                TotalExpenses = totalExpenses,
                DailyAverage = dailyAverage,
                Categories = categoryGroups,
                RecentExpenses = recentExpenses
            };
        }

        public async Task<List<FoodCalendarDayVModel>> GetFoodCalendarAsync(int year, int month)
        {
            var currentUserId = GlobalUserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new List<FoodCalendarDayVModel>();
            }

            var startOfMonth = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);

            var posts = await _context.Posts
                .AsNoTracking()
                .Include(p => p.PostMedias.Where(pm => pm.IsActive == true).OrderBy(pm => pm.DisplayOrder))
                    .ThenInclude(pm => pm.File)
                .Where(p => p.UserId == currentUserId
                         && p.IsExpense == true
                         && p.IsActive == true
                         && p.CreatedDate >= startOfMonth
                         && p.CreatedDate <= endOfMonth)
                .OrderBy(p => p.CreatedDate)
                .ToListAsync();

            var groupedByDay = posts
                .GroupBy(p => DateOnly.FromDateTime(p.CreatedDate!.Value))
                .Select(g =>
                {
                    var firstPostWithMedia = g.FirstOrDefault(p => p.PostMedias.Any(pm => pm.File != null));
                    var thumbUrl = firstPostWithMedia?.PostMedias.FirstOrDefault()?.File?.FilePath;

                    var foodNames = g
                        .Where(p => !string.IsNullOrEmpty(p.FoodName))
                        .Select(p => p.FoodName!)
                        .Distinct()
                        .ToList();

                    return new FoodCalendarDayVModel
                    {
                        Date = g.Key,
                        TotalAmount = g.Sum(p => p.Amount ?? 0),
                        Currency = "VND",
                        Count = g.Count(),
                        ThumbnailUrl = thumbUrl,
                        FoodNames = foodNames
                    };
                })
                .OrderBy(d => d.Date)
                .ToList();

            return groupedByDay;
        }

        #region Private Helpers

        private static Expression<Func<Post, bool>> BuildQueryable(ExpenseFilterParams fParams)
        {
            return p =>
                (fParams.IsActive == null || p.IsActive == fParams.IsActive) &&
                (!fParams.CategoryId.HasValue || p.ExpenseCategoryId == fParams.CategoryId.Value) &&
                (!fParams.MinAmount.HasValue || p.Amount >= fParams.MinAmount.Value) &&
                (!fParams.MaxAmount.HasValue || p.Amount <= fParams.MaxAmount.Value) &&
                (string.IsNullOrEmpty(fParams.SearchString) ||
                    (p.FoodName != null && p.FoodName.Contains(fParams.SearchString)) ||
                    (p.Content != null && p.Content.Contains(fParams.SearchString))) &&
                (!fParams.FromDate.HasValue || p.CreatedDate >= fParams.FromDate.Value) &&
                (!fParams.ToDate.HasValue || p.CreatedDate <= fParams.ToDate.Value);
        }

        #endregion
    }
}
