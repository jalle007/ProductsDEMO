using System;
using System.Collections.Generic;
using ProductsApp.Dal.Entity;

namespace ProductsApp.Service.Models
{
    public class ImageResponse
    {
        public long Count { get; set; }
        public long TotalPages { get; set; }
        public List<ImageItemResponse> Images{ get; set; }
    }

    public class ImageItemResponse
    {
        public long Id { get; set; }
        public DeviceType DeviceType { get; set; }
        public Platform Platform { get; set; }
        public string ImageUrl { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Sku { get; set; }
        public long? EventId { get; set; }
        public string Username { get; set; }
        public string ProfileUrl { get; set; }
        public long Likes { get; set; }
        public DateTimeOffset Created { get; set; }
        public bool UserLike { get; set; }
    }
}
