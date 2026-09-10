namespace SL.Infrastructures.EntityFramework.Entities.SysEntities
{
    public class SysActivityLog : BaseEntity
    {
        public string? UserId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? ActionName { get; set; }
        public string? Path { get; set; }
        public string? Method { get; set; }
        public int? StatusCode { get; set; }
        public long? ExecutionDurationMs { get; set; }
        public string? RequestData { get; set; }
        public string? ResponseData { get; set; }
    }
}

