using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProductsApp.WebApi.Util.OperationFilter
{
    public class FileUploadOperation : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.OperationId.ToLower() == "apiimagepost")
            {
                //operation.Parameters.Remove(operation.Parameters.First(p => p.Name == "ContentType"));
                //operation.Parameters.Remove(operation.Parameters.First(p => p.Name == "ContentDisposition"));
                //operation.Parameters.Remove(operation.Parameters.First(p => p.Name == "Headers"));
                //operation.Parameters.Remove(operation.Parameters.First(p => p.Name == "Length"));
                //operation.Parameters.Remove(operation.Parameters.First(p => p.Name == "Name"));
                //operation.Parameters.Remove(operation.Parameters.First(p => p.Name == "FileName"));

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "uploadedFile",
                    In = "formData",
                    Description = "Upload File",
                    Required = true,
                    Type = "file"
                });
                operation.Consumes.Add("multipart/form-data");
            }
        }
    }
}
