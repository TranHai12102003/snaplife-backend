namespace SL.Infrastructures.EntityFramework.Entities.SysEntities
{
    public class SysConfiguration : BaseEntity
    {
        public string ConfigKey { get; set; } = null!;
        public string? ConfigValue { get; set; }
        public string? Description { get; set; }
        public string? Group { get; set; }
    }
}

