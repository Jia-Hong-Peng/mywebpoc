using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;

namespace SecretManagementPortal.Blob
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobService(string connectionString, string containerName)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = containerName;
        }

        public async Task UploadFileAsync(Stream fileStream, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
        }

        public async Task<IEnumerable<string>> ListJpgFilesAsync()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var imageFiles = new List<string>();

            await foreach (var blobItem in containerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, null))
            {
                // 檢查檔案後綴是否為 .jpg, .png, 或 .gif
                string extension = Path.GetExtension(blobItem.Name).ToLower();
                if (extension == ".jpg" || extension == ".png" || extension == ".gif")
                {
                    var blobClient = containerClient.GetBlobClient(blobItem.Name);
                    imageFiles.Add(blobClient.Uri.AbsoluteUri);
                }
            }

            return imageFiles;
        }
    }
}
