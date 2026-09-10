namespace SL.Infrastructures.EntityFramework.Entities.SysEntities
{
    public abstract class AuditEntity
    {
        public bool? IsActive { get; set; } = true;
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

