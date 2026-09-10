using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Infrastructures.EntityFramework.Entities
{
    public class ExpenseCategory : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public bool IsDefault { get; set; } = false;
        public string? UserId { get; set; }
        public int DisplayOrder { get; set; } = 0;

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
