namespace SL.Infrastructures.EntityFramework.Entities
{
    public class PostHashtag
    {
        public long PostId { get; set; }
        public long HashtagId { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual Hashtag Hashtag { get; set; } = null!;
    }
}

