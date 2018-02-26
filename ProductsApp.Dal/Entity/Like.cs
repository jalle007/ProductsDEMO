using System;

namespace ProductsApp.Dal.Entity
{
    public class Like
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ImageId { get; set; }
        public virtual Image Image { get; set; }
        public Platform Platform { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
