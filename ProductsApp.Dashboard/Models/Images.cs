using System;
using System.Collections.Generic;

namespace ProductsApp.Dashboard.Models {
  public partial class Images {
    public long Id { get; set; }
    public DateTimeOffset Created { get; set; }
    public string Description { get; set; }
    public string DeviceToken { get; set; }
    public int DeviceType { get; set; }
    public int Platform { get; set; }
    public string ProfileUrl { get; set; }
    public string Sku { get; set; }
    public string Title { get; set; }
    public long UserId { get; set; }
    public string Username { get; set; }
    public string ImageUrl { get; set; }
    }

 
  }
