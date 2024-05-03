using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IImageRepository imageRepository;

    public ImagesController(IImageRepository imageRepository)
    {
        this.imageRepository = imageRepository;
    }


    // POST: /api/Images/Upload
    [HttpPost]
    [Route("Upload")]
    public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
    {
        ValidateFileUpload(request);
        if (ModelState.IsValid)
        {
            // use repository to upload the file
            var imageDomain = new Image()
            {
                File = request.File,
                FileName = request.FileName,
                FileDescription = request.FileDescription,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length
            };

            await imageRepository.UploadAsync(imageDomain);
            return Ok(imageDomain);


        }
        return BadRequest(ModelState);
    }



    private void ValidateFileUpload(ImageUploadRequestDto request)
    {
        const int MAX_FILE_SIZE = 10485760;
        var validFileExtensions = new string[] { ".jpg", ".jpeg", ".png" };

        if (!validFileExtensions.Contains(Path.GetExtension(request.File.FileName)))
        {
            ModelState.AddModelError("file", "Unsupported file extension");
        }

        if (request.File.Length > MAX_FILE_SIZE)
        {
            ModelState.AddModelError("file", "File size exceeded 10MB.");
        }


    }
}