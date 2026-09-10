using SL.Domain.VModels.Expense;
using SL.Infrastructures.EntityFramework.Entities;

namespace SL.Services.Mappings
{
    public static class ExpenseMappings
    {
        public static ExpenseCategoryVModel CategoryEntityToVModel(ExpenseCategory category, string? currentUserId)
        {
            return new ExpenseCategoryVModel
            {
                Id = category.Id,
                Name = category.Name,
                Icon = category.Icon,
                Color = category.Color,
                IsDefault = category.IsDefault,
                CanDelete = !category.IsDefault && !string.IsNullOrEmpty(currentUserId) && category.UserId == currentUserId,
                DisplayOrder = category.DisplayOrder
            };
        }

        public static ExpenseItemVModel PostToExpenseItemVModel(Post post)
        {
            var firstMedia = post.PostMedias
                .Where(pm => pm.IsActive == true)
                .OrderBy(pm => pm.DisplayOrder)
                .FirstOrDefault();

            return new ExpenseItemVModel
            {
                PostId = post.Id,
                Content = post.Content,
                FoodName = post.FoodName,
                Amount = post.Amount ?? 0,
                Currency = post.Currency ?? "VND",
                CategoryId = post.ExpenseCategoryId,
                CategoryName = post.ExpenseCategory?.Name ?? "Khác",
                CategoryIcon = post.ExpenseCategory?.Icon ?? "🍴",
                CategoryColor = post.ExpenseCategory?.Color ?? "#9E9E9E",
                ThumbnailUrl = firstMedia?.File?.FilePath,
                LocationName = post.LocationName,
                CreatedDate = post.CreatedDate ?? DateTime.UtcNow
            };
        }
    }
}
