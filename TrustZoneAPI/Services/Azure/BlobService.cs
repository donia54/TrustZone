using Azure.Storage.Blobs;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using TrustZoneAPI.Services.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Azure
{

    public interface IBlobService
    {
        Task<string> GenerateUploadSasUrlAsync(string fileName);
        Task<string> GeneratePictureLoadSasUrlAsync(string PicturePath);
    }
    public class BlobService :IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _blobContainerClient;
        // private readonly ILogService _LogService;
        private readonly string _storageAccountName;
        private readonly string _storageAccountKey;
        private readonly IUserRepository _userRepository;


        public BlobService(IUserRepository userRepository, IConfiguration configuration)
        {
            string connectionString = configuration["AzureBlob:ConnectionString"];
            string containerName = configuration["AzureBlob:ContainerName"];
            _storageAccountName = configuration["AzureBlob:AccountName"];
            _storageAccountKey = configuration["AzureBlob:AccountKey"];


            _blobServiceClient = new BlobServiceClient(connectionString);
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            _userRepository = userRepository;
        }


        public async Task<string> GenerateUploadSasUrlAsync(string fileName)
        {
            // Ensure file name follows your preferred naming conventions

            var blobClient = _blobContainerClient.GetBlobClient(fileName);

            // Set SAS Token permissions: Allow write and create
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _blobContainerClient.Name,
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


        public async Task<string> GeneratePictureLoadSasUrlAsync(string PicturePath)
        {
            if (PicturePath == null)
                throw new Exception("Picture path is null");

            
            var blobClient = _blobContainerClient.GetBlobClient(PicturePath);

            
            var exists = await blobClient.ExistsAsync();
            if (!exists)
            {
                //await _LogService.LogInfo($"The specified file does not exist in Azure Blob Storage. FilePath: {filePath}.");
                throw new FileNotFoundException("The specified Picture does not exist in Azure Blob Storage.", PicturePath);
            }

         
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _blobContainerClient.Name,
                BlobName = PicturePath,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(24) // temp
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_storageAccountName, _storageAccountKey)).ToString();

            var sasUri = $"{blobClient.Uri}?{sasToken}";

            return sasUri;
        }

    }
}
