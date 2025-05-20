using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly NZWalksDbContext dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor,NZWalksDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            HttpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",$"{image.FileName}{image.File}");
            //Upload image to local path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{HttpContextAccessor.HttpContext.Request.Scheme}://{HttpContextAccessor.HttpContext.Request.Host}{HttpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtention}";
            
            image.FilePath = urlFilePath;

            //Add Image to Images table
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }
    }
}
