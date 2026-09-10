namespace SL.Infrastructures.EntityFramework.Entities.SysEntities
{
    public abstract class BaseEntity : AuditEntity
    {
        public long Id { get; set; }
    }
}
