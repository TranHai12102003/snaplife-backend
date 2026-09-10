namespace SL.Domain.VModels.Post
{
    public class HashtagVModel
    {
        public long Id { get; set; }
        public string Tag { get; set; } = null!;
        public int UsageCount { get; set; }
    }
}

