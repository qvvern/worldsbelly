using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Repository.Interfaces;

namespace WorldsBelly.DataAccess.Repository
{
    public class ImageRepository : IImageRepository
    {
        private const string StorageConnectionString = "";
        private const string AccountName = "worldsbellystorage";
        private const string AccountKey = "";
        private const string BlobContainerNameRecipe = "recipe-images";
        private readonly AppDbContext _dbContext;

        public ImageRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<string> StoreImageAsync(string imageData, string path, string name = null, int? maxWidth = null, int? maxHeight = null, int? maxBoth = null, int? cropMax = null, string blobContainerName = null)
        {
            if (String.IsNullOrEmpty(blobContainerName))
            {
                throw new Exception("blobContainerName not specified");
            }
            string mimeType = imageData.Split(":")[0]?.Split(";")[0];
            if (String.IsNullOrWhiteSpace(mimeType) || !mimeType.StartsWith("image"))
            {
                mimeType = "image/jpeg";
            }
            string strImage = Regex.Replace(imageData, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

            byte[] data = Convert.FromBase64String(strImage);
            byte[] resizedDate = ResizeByteImage(data, maxWidth: maxWidth, maxHeight: maxHeight, maxBoth: maxBoth, cropMax: cropMax);
            if (String.IsNullOrEmpty(name))
            {
                Guid g = Guid.NewGuid();
                var pathName = path + "/" + g;
                await UploadBlobAsync(pathName, blobContainerName, resizedDate, mimeType);
                return $"/{blobContainerName}/{pathName}";
            }
            else
            {
                var pathName = path + "/" + name;
                await UploadBlobAsync(pathName, blobContainerName, resizedDate, mimeType);
                return $"/{blobContainerName}/{pathName}";
            }
        }
        public async Task<bool> RemoveImageAsync(string path, string blobContainerName = null)
        {
            if (String.IsNullOrEmpty(blobContainerName))
            {
                throw new Exception("blobContainerName not specified");
            }
            return await RemoveBlobAsync(path, blobContainerName);
        }
        public async Task<bool> RemoveImageFolderAsync(string path, string blobContainerName = null)
        {
            if (String.IsNullOrEmpty(blobContainerName))
            {
                throw new Exception("blobContainerName not specified");
            }
            return await RemoveBlobFolderAsync(path, blobContainerName);
        }
        public static byte[] ResizeByteImage(byte[] PassedImage, int? maxWidth = null, int? maxHeight = null, int? maxBoth = null, int? cropMax = null)
        {
            if (maxWidth == null && maxHeight == null && maxBoth == null)
            {
                return PassedImage;
            }
            byte[] ReturnedThumbnail;

            using (MemoryStream StartMemoryStream = new MemoryStream(),
                                NewMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                StartMemoryStream.Write(PassedImage, 0, PassedImage.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                Bitmap startBitmap = new Bitmap(StartMemoryStream);

                // set thumbnail height and width proservingal to the original image.  
                int newHeight = startBitmap.Height;
                int newWidth = startBitmap.Width;
                double HW_ratio;


                if (maxBoth != null)
                {
                    if (startBitmap.Height < startBitmap.Width)
                    {
                        newHeight = maxBoth.GetValueOrDefault();
                        HW_ratio = (double)((double)maxBoth / (double)startBitmap.Height);
                        newWidth = (int)(HW_ratio * (double)startBitmap.Width);
                    }
                    else
                    {
                        newWidth = maxBoth.GetValueOrDefault();
                        HW_ratio = (double)((double)maxBoth / (double)startBitmap.Width);
                        newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                    }
                }
                else
                {
                    if (maxWidth != null)
                    {
                        newWidth = maxWidth.GetValueOrDefault();
                        HW_ratio = (double)((double)newWidth / (double)startBitmap.Width);
                        newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                    }
                    else if (maxHeight != null)
                    {
                        newHeight = maxHeight.GetValueOrDefault();
                        HW_ratio = (double)((double)newHeight / (double)startBitmap.Height);
                        newWidth = (int)(HW_ratio * (double)startBitmap.Width);
                    }
                }

                // create a new Bitmap with dimensions for the thumbnail.  
                Bitmap newBitmap = new Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeBitmapImage(startBitmap, newWidth, newHeight);

                // Crop Image
                int maxSize = (cropMax ?? maxWidth ?? maxHeight ?? maxBoth).GetValueOrDefault();
                Bitmap cropBitmap = cropAtRect(newBitmap, maxSize);

                // Save this image to the specified stream in the specified format.  
                cropBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                ReturnedThumbnail = NewMemoryStream.ToArray();
            }

            // return the resized image as a string of bytes.  
            return ReturnedThumbnail;
        }

        // Resize a Bitmap  
        private static Bitmap ResizeBitmapImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return resizedImage;
        }
        // Crop a Bitmap  
        private static Bitmap cropAtRect(Bitmap b, int maxSize)
        {
            int width = b.Width;
            int height = b.Height;

            if (b.Width > maxSize) width = maxSize;
            if (b.Height > maxSize) height = maxSize;
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(b, 0, 0);
            }
            return resizedImage;
        }

        private static async Task<bool> RemoveBlobAsync(string name, string containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
            return await blockBlob.DeleteIfExistsAsync();
        }
        private static async Task<bool> RemoveBlobFolderAsync(string name, string containerName)
        {
            var blobServiceClient = new BlobServiceClient(StorageConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobItems = blobContainerClient.GetBlobsAsync(prefix: name);
            await foreach (BlobItem blobItem in blobItems)
            {
                BlobClient blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                await blobClient.DeleteIfExistsAsync();
            }
            return true;
        }

        private static async Task UploadBlobAsync(string name, string containerName, byte[] data, string contentType)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
                blockBlob.Properties.ContentType = contentType;
                await blockBlob.UploadFromByteArrayAsync(data, 0, data.Length);
            }
            catch (Exception e)
            {
                throw new Exception("cannot upload blob", e);
            }
        }
        private static string GeneratePreSignedUrlAzure(string blobContainerName, string blobName, BlobContainerSasPermissions permission)
        {
            string blobEndpoint = $"https://{AccountName}.blob.core.windows.net";

            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(AccountName, AccountKey);
            BlobSasBuilder sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobContainerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(100)
            };

            sasBuilder.SetPermissions(permission);

            string sasToken = sasBuilder.ToSasQueryParameters(sharedKeyCredential).ToString();
            return $"{blobEndpoint}/{blobContainerName}/{blobName}?{sasToken}";
        }

    }
}
