using ProductsApp.Dal.Entity;

namespace ProductsApp.WebApi.Models
{
    public class LikeBindingModel
    {
        public long ImageId { get; set; }
        public long UserId { get; set; }
        public Platform Platform { get; set; }

        public bool Like { get; set; }
    }
}
