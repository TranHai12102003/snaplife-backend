using SL.Infrastructures.EntityFramework.Entities;

namespace SL.Infrastructures.EntityFramework.SeedData
{
    public static class ExpenseCategorySeedData
    {
        private static readonly DateTime SeedDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static readonly ExpenseCategory Breakfast = new()
        {
            Id = 1,
            Name = "Ăn sáng",
            Icon = "🍳",
            Color = "#FF9800",
            IsDefault = true,
            DisplayOrder = 1,
            CreatedDate = SeedDate,
            IsActive = true
        };

        public static readonly ExpenseCategory Lunch = new()
        {
            Id = 2,
            Name = "Ăn trưa",
            Icon = "🍱",
            Color = "#4CAF50",
            IsDefault = true,
            DisplayOrder = 2,
            CreatedDate = SeedDate,
            IsActive = true
        };

        public static readonly ExpenseCategory Dinner = new()
        {
            Id = 3,
            Name = "Ăn tối",
            Icon = "🍲",
            Color = "#E91E63",
            IsDefault = true,
            DisplayOrder = 3,
            CreatedDate = SeedDate,
            IsActive = true
        };

        public static readonly ExpenseCategory Coffee = new()
        {
            Id = 4,
            Name = "Cà phê & Trà sữa",
            Icon = "☕",
            Color = "#795548",
            IsDefault = true,
            DisplayOrder = 4,
            CreatedDate = SeedDate,
            IsActive = true
        };

        public static readonly ExpenseCategory Groceries = new()
        {
            Id = 5,
            Name = "Đi chợ & Siêu thị",
            Icon = "🛒",
            Color = "#00BCD4",
            IsDefault = true,
            DisplayOrder = 5,
            CreatedDate = SeedDate,
            IsActive = true
        };

        public static readonly ExpenseCategory Transport = new()
        {
            Id = 6,
            Name = "Di chuyển & Xăng xe",
            Icon = "🚗",
            Color = "#607D8B",
            IsDefault = true,
            DisplayOrder = 6,
            CreatedDate = SeedDate,
            IsActive = true
        };

        public static readonly ExpenseCategory Shopping = new()
        {
            Id = 7,
            Name = "Mua sắm & Khác",
            Icon = "🛍️",
            Color = "#9C27B0",
            IsDefault = true,
            DisplayOrder = 7,
            CreatedDate = SeedDate,
            IsActive = true
        };

        public static ExpenseCategory[] GetAll() =>
        [
            Breakfast,
            Lunch,
            Dinner,
            Coffee,
            Groceries,
            Transport,
            Shopping
        ];
    }
}
