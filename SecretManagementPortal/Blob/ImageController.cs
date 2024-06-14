using Microsoft.AspNetCore.Mvc;

namespace SecretManagementPortal.Blob
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly BlobService _blobService;

        public ImageController(BlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpGet("images")]
        public async Task<IActionResult> GetImages()
        {
            var images = await _blobService.ListJpgFilesAsync();
            return Ok(images);
        }
    }

}
