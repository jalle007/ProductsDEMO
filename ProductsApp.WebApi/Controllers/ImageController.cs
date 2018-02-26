using System;
using System.IO;
using System.Linq;
using ProductsApp.Dal.Entity;
using ProductsApp.Service;
using ProductsApp.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ProductsApp.WebApi.Util;

namespace ProductsApp.WebApi.Controllers
{

    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        private readonly ImageService _imageService;
        private readonly SkuService _skuService;

        public ImageController(ImageService imageService, SkuService skuService)
        {
            _imageService = imageService;
            _skuService = skuService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string order, long userId, string sku = null, int page = 1, int pageSize = 100)
        {
            var itemSku = string.IsNullOrEmpty(sku) ? sku : sku.ToAlphaNumericOnly();
            var images = await _imageService.GetImages(order, userId, page, pageSize, itemSku);
            return Ok(new ApiResponse()
            {
                Success = true,
                Data = images
            });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete( long Id)
        {
            var deleted = await _imageService.DeleteImage(Id);
            return Ok(new ApiResponse()
            {
                Success = deleted,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]ImageBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse()
                {
                    Success = false,
                    Message = "Invalid data",
                    Data = ModelState
                });
            }

            model.Sku = model.Sku.ToAlphaNumericOnly();

            var productDetails = _skuService.GetProductDetailsBySku(model.Sku);

            SkuServiceProduct skuServiceProduct = null;

            if (!productDetails.Success || productDetails.Data == null || !productDetails.Data.Any())
            {
                skuServiceProduct = null;
            }
            else
            {
                skuServiceProduct = productDetails.Data.First();

                if (skuServiceProduct.Sku.ToLower() != model.Sku.ToAlphaNumericOnly().ToLower())
                {
                    skuServiceProduct = null;
                }
            }

            if (Request.Form.Files == null || !Request.Form.Files.Any())
            {
                return Ok(new ApiResponse()
                {
                    Success = false,
                    Message = "No files in your request"
                });
            }

            var file = Request.Form.Files[0];

            var uploadedPath = await _imageService.UplaodImage(Path.GetExtension(file.FileName), file.ContentType, file.OpenReadStream());

            var image = await _imageService.AddImage(new Image()
            {
                Created = DateTimeOffset.UtcNow,
                Sku = model.Sku,
                EventId = skuServiceProduct?.Id,
                Platform = model.Platform,
                Title = skuServiceProduct == null? model.Title : skuServiceProduct.Title,
                DeviceToken = model.DeviceToken,
                DeviceType = model.DeviceType,
                ProfileUrl = model.ProfileUrl,
                UserId = model.UserId,
                ImageUrl = uploadedPath,
                Username = model.Username,
            });

            return Ok(new ApiResponse()
            {
                Success = true,
                Data = image
            });

        }

        [HttpPost]
        [Route("Like")]
        public async Task<IActionResult> Like([FromBody]LikeBindingModel model)
        {
            await _imageService.Like(model.ImageId, model.UserId, model.Platform, model.Like);
            return Ok(new ApiResponse()
            {
                Success = true
            });
        }
    }
}