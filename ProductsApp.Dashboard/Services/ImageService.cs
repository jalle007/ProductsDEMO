using System.Collections.Generic;
using System.Linq;
using ProductsApp.Dashboard.Models;
using ProductsApp.Service.Models;
using Newtonsoft.Json;
using RestSharp;

namespace ProductsApp.Dashboard.Services {
  public class ImagesService {
    private const string APIUrl = "http://ProductsApp-on-feet-dev.azurewebsites.net/api/";

    public ApiResponse GetImages  (string sku = null, int page = 1, int pageSize = 100) {
      string order = "chronological";
      string url = APIUrl + $"Image?order={order}&sku={sku}&page={page}&pageSize={pageSize}";
    
      var client = new RestClient(url);
      var request = new RestRequest(Method.GET);
      IRestResponse response = client.Execute(request);

      return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
      }

    public List<string> GetSKUs () {
        var allImages = GetImages(null,1,int.MaxValue);
        var allSKUs = allImages.Data.Images.Select(l => l.Sku).Distinct().ToList();
        return allSKUs;
      }

    public ApiResponse DeleteImage  (long Id) {
      
      string url = APIUrl + $"Image/{Id}";
    
      var client = new RestClient(url);
      var request = new RestRequest(Method.DELETE);
      IRestResponse response = client.Execute(request);

      return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
      }

    }

  

  //public class ApiResponse {
  //  public bool Success { get; set; }
  //  public string Message { get; set; }
  //  public ImageResponse Data { get; set; }
  //  }


  }
