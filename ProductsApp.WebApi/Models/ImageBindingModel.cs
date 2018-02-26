using ProductsApp.Dal.Entity;
using Microsoft.AspNetCore.Http;

namespace ProductsApp.WebApi.Models
{
    public class ImageBindingModel
    {
        public DeviceType DeviceType { get; set; }
        public Platform Platform { get; set; }
        public string DeviceToken { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Sku { get; set; }
        public string Username { get; set; }
        public string ProfileUrl { get; set; }
    }
}
