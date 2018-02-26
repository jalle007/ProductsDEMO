using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ProductsApp.Dal;
using ProductsApp.Dal.Entity;
using ProductsApp.Service.Exception;
using ProductsApp.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ProductsApp.Service
{
    public class ImageService
    {
        private readonly OnFeetContext _context;
        private readonly CloudBlobContainer _container;

        public ImageService(OnFeetContext context, string storageConnectionString, string imageContainer)
        {
            _context = context;
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();

            _container = cloudBlobClient.GetContainerReference(imageContainer);
            _container.CreateIfNotExistsAsync().Wait();

            _container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Container
            }).Wait();
        }

        public async Task<string> UplaodImage(string extenstion, string contentType, Stream fileStream)
        {
            var filename = $"{Guid.NewGuid()}{extenstion}";
            var blobReference = _container.GetBlockBlobReference(filename);
            await blobReference.UploadFromStreamAsync(fileStream);

            if (!string.IsNullOrEmpty(contentType))
            {
                blobReference.Properties.ContentType = contentType;
                blobReference.SetPropertiesAsync().Wait();
            }

            return $"https://{blobReference.Uri.DnsSafeHost}{blobReference.Uri.AbsolutePath}";
        }

        public async Task<Image> AddImage(Image image)
        {
            await _context.AddAsync(image);
            await _context.SaveChangesAsync();
            return image;
        }
        
        public async Task<bool> DeleteImage( long Id)
        {   
            var image = _context.FindAsync<Image>(Id).Result;
            if (image != null) 
            {
              _context.Remove(image);
              await _context.SaveChangesAsync();
              return true;
            }
            return false;
        }

        public async Task<ImageResponse> GetImages(string order, long userId, int page = 1, int pageSize = 100, string sku = null)
        {

            Expression<Func<Image, bool>> condition;

            if (!string.IsNullOrEmpty(sku))
            {
                condition = (image) => image.Sku.ToLower() == sku.ToLower();
            }
            else
            {
                condition = (image) => true;
            }

            if (order.ToLower() == "popular")
            {
                var count = _context.Images.Where(condition).Count();
                var imageItems = await _context.Images
                    .Where(condition)
                    .OrderByDescending(img => img.Likes.Count)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(img => new ImageItemResponse()
                    {
                        Id = img.Id,
                        Platform = img.Platform,
                        Sku = img.Sku,
                        EventId = img.EventId,
                        ImageUrl = img.ImageUrl,
                        Likes = img.Likes.LongCount(),
                        Title = img.Title,
                        UserId = img.UserId,
                        DeviceType = img.DeviceType,
                        ProfileUrl = img.ProfileUrl,
                        Username = img.Username,
                        Created = img.Created,
                        UserLike = img.Likes.Any(like => like.UserId == userId)
                    })
                    .ToListAsync();

                return new ImageResponse()
                {
                    Count = count,
                    Images = imageItems,
                    TotalPages = (long)decimal.Ceiling((decimal)count/pageSize)
                };
            }

            if (order.ToLower() == "chronological")
            {
                var count = _context.Images.Where(condition).Count();
                var imageItems = await _context.Images
                    .Where(condition)
                    .OrderByDescending(img => img.Created)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(img => new ImageItemResponse()
                    {
                        Id = img.Id,
                        Platform = img.Platform,
                        EventId = img.EventId,
                        Sku = img.Sku,
                        ImageUrl = img.ImageUrl,
                        Likes = img.Likes.LongCount(),
                        Title = img.Title,
                        UserId = img.UserId,
                        DeviceType = img.DeviceType,
                        ProfileUrl = img.ProfileUrl,
                        Username = img.Username,
                        Created = img.Created,
                        UserLike = img.Likes.Any(like => like.UserId == userId)
                    })
                    .ToListAsync();

                return new ImageResponse()
                {
                    Count = count,
                    Images = imageItems,
                    TotalPages = (long)decimal.Ceiling((decimal)count / pageSize)
                };
            }

            throw new ServiceException("Invalid order type");
}
        public async Task Like(long imageId, long userid, Platform platform, bool like)
        {
            var image = await _context.Images.Where(img => img.Id == imageId).FirstOrDefaultAsync();

            if (image == null)
            {
                throw new ServiceException("Unable to find image to like");
            }

            var likeEntry = await _context.Likes.Where(likeEntity => likeEntity.ImageId == imageId && likeEntity.UserId == userid &&
                                                                 likeEntity.Platform == platform)
                .FirstOrDefaultAsync();

            if (like && likeEntry == null)
            {
                await _context.Likes.AddAsync(new Like()
                {
                    ImageId = imageId,
                    Platform = platform,
                    UserId = userid,
                    Timestamp = DateTimeOffset.UtcNow
                });
                await _context.SaveChangesAsync();
            }

            if (!like && likeEntry != null)
            {
                _context.Likes.Remove(likeEntry);
                await _context.SaveChangesAsync();
            }
        }


    }
}
