using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductsApp.Dashboard.Data;
using ProductsApp.Dashboard.Models;
using ProductsApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsApp.Service.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProductsApp.Dashboard.Util;
using System.IO;
using ProductsApp.Dal.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProductsApp.Dashboard.Controllers {
  [Authorize]
  public class ImagesController : Controller {
    private readonly ApplicationDbContext _context;
    private IHostingEnvironment _env;
    private readonly ImageService _imageService;
    private readonly SkuService _skuService;
    public const string sAll = "-- ALL --";
    List<SelectListItem> skus;

    public ImagesController (ApplicationDbContext context, IHostingEnvironment env, ImageService imageService, SkuService skuService) {
      _context = context;
      _imageService = imageService;
      _skuService = skuService;
      _env = env;
      }

    [HttpPost]
    public async Task<JsonResult> Autocomplete (string Prefix) {
      if (skus.IsNull()) skus = await GetSKUsAsync(false);

      var res = (from s in skus
                 where s.Value.ToLower().StartsWith(Prefix.ToLower()) || s.Text.ToLower().Contains(Prefix.ToLower())
                 select s).ToList();

      return Json(res);
      }

    [HttpGet]
    public async Task<IActionResult> Create () {
      return View();
      }

    [HttpGet]
    public async Task<IActionResult> Upload (IList<IFormFile> files) {
        if (skus==null) {
          skus = await GetSKUsAsync(false);
          ViewBag.SKU = skus;
          }

        return View();
      }

    [HttpPost]
    public async Task<IActionResult> Upload ([FromForm]ImageBindingModel model) {
      if (!ModelState.IsValid) {
        return Ok(new ApiResponse() {
          Success = false,
          Message = "Invalid data",
          Data = ModelState
          });
        }

      var sku1 = (string)Request.Form["sku1"];
      if (sku1 != "") model.Sku = sku1;

      model.Sku = model.Sku.ToAlphaNumericOnly();

      var productDetails = _skuService.GetProductDetailsBySku(model.Sku);
      SkuServiceProduct skuServiceProduct = null;

      if (!productDetails.Success || productDetails.Data == null || !productDetails.Data.Any()) {
        skuServiceProduct = null;
        } else {
        skuServiceProduct = productDetails.Data.First();

        if (skuServiceProduct.Sku.ToLower() != model.Sku.ToAlphaNumericOnly().ToLower()) {
          skuServiceProduct = null;
          }
        }

      if (Request.Form.Files == null || !Request.Form.Files.Any()) {
        return Ok(new ApiResponse() {
          Success = false,
          Message = "No files in your request"
          });
        }

      var file = Request.Form.Files[0];

      var uploadedPath = await _imageService.UplaodImage(Path.GetExtension(file.FileName), file.ContentType, file.OpenReadStream());

      var image = await _imageService.AddImage(new Image() {
        Created = DateTimeOffset.UtcNow,
        Sku = model.Sku,
        EventId = skuServiceProduct?.Id,
        Platform = model.Platform,
        Title = skuServiceProduct == null ? model.Title : skuServiceProduct.Title,
        DeviceToken = model.DeviceToken,
        DeviceType = model.DeviceType,
        ProfileUrl = model.ProfileUrl,
        UserId = model.UserId,
        ImageUrl = uploadedPath,
        Username = model.Username,
        });

      return RedirectToAction("Index", "Images");
      }

    // GET: Images
    [HttpGet]
    public async Task<IActionResult> Index () {
      return View(await GetImages());
      }

    [HttpPost]
    public async Task<IActionResult> Index (string sku) {
      var sku1 = (string)Request.Form["sku1"];
      if (sku.Contains(sAll) && sku1 == "") return RedirectToAction("Index", "Images");

      return View(await GetImages(sku1 != "" ? sku1 : sku));
      }

    private async Task<List<ImagesViewModel>> GetImages (string sku = null) {
      if (skus == null) {
        skus = (await GetSKUsAsync(true));
        ViewBag.SKUs = skus;
        }

      var imageResponse = await _imageService.GetImages("chronological", 0, 1, 100, sku);

      var imagesView = new List<ImagesViewModel>() { };
      foreach (var img in imageResponse.Images) {
        imagesView.Add(new ImagesViewModel { Image = img, Checked = false });
        }

      ViewBag.Count = imageResponse.Count;
      ViewBag.TotalPages = imageResponse.TotalPages;

      return imagesView;
      }

    // POST: Images/Delete/5
    [HttpGet]
    [Route("images/delete/{id}")]
    public async Task<IActionResult> Delete (long id) {
      var response = await _imageService.DeleteImage(id);

      return RedirectToAction("Index");
      }

    // POST: Images/Delete/selected  
    [HttpPost]
    public async Task<IActionResult> DeleteSelected (List<ImagesViewModel> model) {

      var itemId = Request.Form["item.Image.Id"];
      var itemChecked = Request.Form["item.Checked"];

      for (int i = 0; i < itemChecked.Count; i++) {
        if (Convert.ToBoolean(itemChecked[i])) {
          // call del api
          var response = await _imageService.DeleteImage(Convert.ToInt32(itemId[i]));
          }
        }

      return RedirectToAction("Index");
      }

    public async Task<List<SelectListItem>> GetSKUsAsync (bool all = true) {
      var images = await _imageService.GetImages("chronological", 0, 1, int.MaxValue, null);

      var SKUs = images.Images
                                  .Select(s => new SelectListItem { Value = s.Sku, Text = $"{s.Sku} - ({s.Title})" })
                                  .Distinct().ToList();

      if (all) SKUs.Insert(0, new SelectListItem { Value = sAll, Text = sAll });
      return SKUs;
      }

    public List<string> JoinSKUandTitle (List<AutocompleteViewModel> SKUs) {
      var result = new List<string>();

      foreach (var sku in SKUs) {
        result.Add($"{sku.Value} ({sku.Text})");
        }
      return result;
      }
    }
  }
