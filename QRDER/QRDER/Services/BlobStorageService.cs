using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace QRDER.Services
{
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly ILogger<BlobStorageService> _logger;

        public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
        {
            var connectionString = configuration.GetConnectionString("AzureBlobStorage");
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = configuration["AzureBlobStorage:ContainerName"] ?? "qrder";
            _logger = logger;

            // Container'daki tüm dosyaları listele
            ListContainerContents();
        }

        private void ListContainerContents()
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                _logger.LogInformation($"Container adı: {_containerName}");
                
                foreach (var blob in containerClient.GetBlobs())
                {
                    _logger.LogInformation($"Blob adı: {blob.Name}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Container listeleme hatası: {ex.Message}");
            }
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(fileName);

                await blobClient.UploadAsync(fileStream, true);
                _logger.LogInformation($"Dosya başarıyla yüklendi: {fileName}");

                return fileName;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Dosya yükleme hatası: {ex.Message}");
                throw;
            }
        }

        public string GetBlobUrl(string blobName)
        {
            if (string.IsNullOrEmpty(blobName))
                return string.Empty;

            try
            {
                // Eğer blobName zaten tam URL ise, direkt döndür
                if (blobName.StartsWith("http"))
                {
                    _logger.LogInformation($"Blob zaten tam URL: {blobName}");
                    return blobName;
                }

                // Sadece dosya adını al (path varsa temizle)
                var fileName = blobName.Split('/').Last();
                _logger.LogInformation($"İşlenecek dosya adı: {fileName}");

                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(fileName);
                
                // Blob'un var olup olmadığını kontrol et
                if (!blobClient.Exists())
                {
                    _logger.LogWarning($"Blob bulunamadı: {fileName}");
                    return string.Empty;
                }

                var url = blobClient.Uri.ToString();
                _logger.LogInformation($"Blob URL oluşturuldu: {url}");
                return url;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Blob URL oluşturma hatası: {ex.Message}");
                return string.Empty;
            }
        }
    }
} 