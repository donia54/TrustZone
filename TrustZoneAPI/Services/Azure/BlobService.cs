using Azure.Storage.Blobs;
using Azure.Storage;
using Azure.Storage.Sas;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Azure
{

    public interface IBlobService
    {
        string ExtractBlobName(string url);
        Task<bool> FileExistsAsync(string containerName, string fileName);

        Task<string> GenerateUploadSasUrlAsync(string containerName, string fileName);
        Task<string> GeneratePictureLoadSasUrlAsync(string containerName, string blobPath);
    }
    public class BlobService :IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
       // private readonly BlobContainerClient _blobContainerClient;
        // private readonly ILogService _LogService;
        private readonly string _storageAccountName = "trustzone";
        private readonly string _storageAccountKey = "1A3ImHdE0ddzyoRanGXFqVzrK/J8AgEjY3VaApibvY8TJNlnEzExGr5s3IU1vKaMRyJFtOJymlxs+AStUiKFvg==";


        private readonly IUserRepository _userRepository;
      


        public BlobService(IUserRepository userRepository)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=trustzone;AccountKey=1A3ImHdE0ddzyoRanGXFqVzrK/J8AgEjY3VaApibvY8TJNlnEzExGr5s3IU1vKaMRyJFtOJymlxs+AStUiKFvg==;EndpointSuffix=core.windows.net";
            _blobServiceClient = new BlobServiceClient(connectionString);
 
            _userRepository = userRepository;
           
        }


        private BlobContainerClient GetContainerClient(string containerName)
        {
            return _blobServiceClient.GetBlobContainerClient(containerName);
        }


        public async Task<string> GenerateUploadSasUrlAsync(string containerName, string fileName)
        {
            // Ensure file name follows your preferred naming conventions
            var containerClient = GetContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

          

            // Set SAS Token permissions: Allow write and create
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = fileName,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1) // Set expiration
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);
            // Build the SAS Token
            var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_storageAccountName, _storageAccountKey)).ToString();

            // Generate the full URI with the SAS token
            var sasUri = $"{blobClient.Uri}?{sasToken}";

            return sasUri;
        }


        public async Task<string> GeneratePictureLoadSasUrlAsync(string containerName, string PicturePath)
        {
            if (PicturePath == null)
                throw new Exception("Picture path is null");


            var containerClient = GetContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(PicturePath);


            var exists = await blobClient.ExistsAsync();
            if (!exists)
            {
                //await _LogService.LogInfo($"The specified file does not exist in Azure Blob Storage. FilePath: {filePath}.");
                throw new FileNotFoundException("The specified Picture does not exist in Azure Blob Storage.", PicturePath);
            }

         
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = PicturePath,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(24) // temp
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_storageAccountName, _storageAccountKey)).ToString();

            var sasUri = $"{blobClient.Uri}?{sasToken}";

            return sasUri;
        }


        public async Task<bool> FileExistsAsync(string containerName, string fileName)
        {
            var containerClient = GetContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            return await blobClient.ExistsAsync();
        }


        public string ExtractBlobName(string url)
        {
            try
            {
                var uri = new Uri(url);
                var segments = uri.Segments;

                if (segments.Length >= 2)
                {
                   
                    return segments.Last().Split('?')[0]; 
                }

                return  null;
            }
            catch
            {
                return null;
            }
        }


    }
}
