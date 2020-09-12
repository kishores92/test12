using GreenPOS.Common;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace GreenPOS.Service
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly AppSettings _appSettings;

        public AzureStorageService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task UploadDocAsync(string source, string filename)
        {
            var strorageconn = _appSettings.AzureDataStorageConnection;
            CloudStorageAccount storageacc = CloudStorageAccount.Parse(strorageconn);

            //Create Reference to Azure Blob
            CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();

            //The next 2 lines create if not exists a container named "democontainer"
            CloudBlobContainer container = blobClient.GetContainerReference("quotes");

            await container.CreateIfNotExistsAsync();

            //The next 7 lines upload the file test.txt with the name DemoBlob on the container "democontainer"
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.Properties.ContentType = "application/pdf";
            using (var stream = System.IO.File.OpenRead(source))
            {
                await blockBlob.UploadFromStreamAsync(stream);
            }


        }

        public async Task DeleteDocAsync(string filename)
        {
            var strorageconn = _appSettings.AzureDataStorageConnection;
            CloudStorageAccount storageacc = CloudStorageAccount.Parse(strorageconn);

            //Create Reference to Azure Blob
            CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();

            //The next 2 lines create if not exists a container named "democontainer"
            CloudBlobContainer container = blobClient.GetContainerReference("quotes");

            await container.CreateIfNotExistsAsync();

            //The next 7 lines upload the file test.txt with the name DemoBlob on the container "democontainer"
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
           
            await blockBlob.DeleteAsync();
        }
        public async Task UploadDocAsync(System.IO.Stream stream, string filename)
        {
            var strorageconn = _appSettings.AzureDataStorageConnection;
            CloudStorageAccount storageacc = CloudStorageAccount.Parse(strorageconn);

            //Create Reference to Azure Blob
            CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();

            //The next 2 lines create if not exists a container named "democontainer"
            CloudBlobContainer container = blobClient.GetContainerReference("quotes");

            await container.CreateIfNotExistsAsync();

            //The next 7 lines upload the file test.txt with the name DemoBlob on the container "democontainer"
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.Properties.ContentType = "application/pdf";
          //  using (var stream = System.IO.File.OpenRead(source))
            {
                await blockBlob.UploadFromStreamAsync(stream);
            }


        }
    }

    public interface IAzureStorageService
    {

        Task UploadDocAsync(string source,string filename);

        Task UploadDocAsync(System.IO.Stream stream, string filename);
        Task DeleteDocAsync(string filename);
    }
}
