using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace ProductsApp.Service
{
    public class SkuService
    {
        private const string ProductsAppUtilServiceUrl = "https://ProductsApp-util-services.azurewebsites.net/api/product/getbysku";
        public ProductDetailsResponse GetProductDetailsBySku(string sku)
        {
            var client = new RestClient(ProductsAppUtilServiceUrl);
            var request = new RestRequest(Method.POST);

            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Basic dXRpbHNlcnZpY2U6dXRpbGF1dGg3Nzc=");

            request.AddJsonBody(new
            {
                Sku = sku
            });

            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ProductDetailsResponse>(response.Content);
        }

    }

    public class ProductDetailsResponse
    {
        public bool Success { get; set; }
        public List<SkuServiceProduct> Data { get; set; }

    }

    public class SkuServiceProduct
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ColorWay { get; set; }
        public string MainImageUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Sku { get; set; }
        public string[] BrandCollection { get; set; }
        public string ThumbUrl { get; set; }
        public long EventId { get; set; }
    }
}
