using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Storage.Models
{
    public class Logical
    {
        private string containerName;
        private readonly BlobServiceClient blobService;
        public Logical(BlobServiceClient client)
        {
            containerName = "home";
            blobService = client;
        }

        public async Task<bool> AddImage(string filename, byte[] data)
        {
            BlobContainerClient container = blobService.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);
            BlobClient blob = container.GetBlobClient(filename);
            var res = await blob.UploadAsync(BinaryData.FromBytes(data));
            if (res.GetRawResponse().Status == 201)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Tuple<string, string>>> GetImages()
        {
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            BlobContainerClient container = blobService.GetBlobContainerClient(containerName);
            foreach (var item in container.GetBlobs())
            {
                BlobClient blob = container.GetBlobClient(item.Name);
                list.Add(new Tuple<string, string>(blob.Name, blob.Uri.AbsoluteUri));
            }
            return list;
        }
    }
}
