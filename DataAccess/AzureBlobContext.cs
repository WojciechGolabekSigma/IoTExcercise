using Azure.Storage.Blobs;

namespace IoTExcercise.DataAccess
{
    public class AzureBlobContext
    {
        public string ContainerName { get; set; }
        public BlobContainerClient BlobContainterClient { get; set; }

        public AzureBlobContext(BlobServiceClient blobServiceClient, string containerName)
        {
            ContainerName = containerName;
            BlobContainterClient = blobServiceClient.GetBlobContainerClient(containerName);
        }

    }
}
