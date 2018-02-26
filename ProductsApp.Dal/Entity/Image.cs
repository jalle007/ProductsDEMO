using System;
using System.Collections.Generic;

namespace ProductsApp.Dal.Entity
{
    public class Image
    {
        public long Id { get; set; }
        public DeviceType DeviceType { get; set; }
        public Platform Platform { get; set; }
        public string DeviceToken { get; set; }
        public string ImageUrl { get; set; }
        public long UserId { get; set; }
        public long? EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Sku { get; set; }
        public string Username { get; set; }
        public string ProfileUrl { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public DateTimeOffset Created { get; set; }
    }

    public enum DeviceType
    {
        Invalid = 0,
        None = 999,
        Ios = 1,
        Android = 2
    }

    public enum Platform
    {
        Invalid = 0,
        KicksOnFire = 1,
        ProductsApp = 2
    }


}
