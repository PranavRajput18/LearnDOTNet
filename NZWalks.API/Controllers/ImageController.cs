using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm]ImageUploadRequestDto imageUploadRequestDto)
        {
            ValidateFileUpload(imageUploadRequestDto);
            if (ModelState.IsValid)
            {
                //Map DTO to Domain Model
                var imageDomainModel = new Image
                {
                    File = imageUploadRequestDto.File,
                    FileExtention = Path.GetExtension(imageUploadRequestDto.File.FileName),
                    FileSizeInBytes = imageUploadRequestDto.File.Length,
                    FileName = imageUploadRequestDto.FileName, 
                    FileDescription = imageUploadRequestDto.FileDescription
                };
                //User repository to upload image
                await imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);

        }

        private void ValidateFileUpload(ImageUploadRequestDto imageUploadRequestDto)
        {
            if (imageUploadRequestDto == null)
            {
                var allowedExtentions = new string[] { ".jpg", ".jpeg", ".png" };
                if (!allowedExtentions.Contains(Path.GetExtension(imageUploadRequestDto.File.FileName)))
                {
                    ModelState.AddModelError("file", "Unsupported File Extention");
                    
                }
                if (imageUploadRequestDto.File.Length > 10485760)
                {
                    ModelState.AddModelError("file","File Size more than 10MB ,please upload a smaller size file");  
                }
            }
        }
    }
}
