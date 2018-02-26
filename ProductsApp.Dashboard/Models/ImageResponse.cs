using System;
using System.Collections.Generic;
using ProductsApp.Service.Models;

namespace ProductsApp.Dashboard.Models {

  public class ImageResponse {
    public long Count { get; set; }
    public long TotalPages { get; set; }
    public List<ImageItem> Images { get; set; }

    }

  public class ImageItem {
    public long Id { get; set; }
    public DeviceType DeviceType { get; set; }
    public Platform Platform { get; set; }
    public string ImageUrl { get; set; }
    public long UserId { get; set; }
    public string Title { get; set; }
    public string Sku { get; set; }
    public string Username { get; set; }
    public string ProfileUrl { get; set; }
    public long Likes { get; set; }
    public DateTimeOffset Created { get; set; }

  //  public bool? Checked{ get; set; }
    }

  public enum DeviceType {
    Invalid = 0,
    None = 999,
    Ios = 1,
    Android = 2
    }

  public enum Platform {
    Invalid = 0,
    KicksOnFire = 1,
    ProductsApp = 2
    }
  }
