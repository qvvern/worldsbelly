using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
    public interface IImageRepository
    {
        //Task<ICollection<Recipe>> GetRecipesAsync();
        Task<string> StoreImageAsync(string imageData, string path, string name = null, int? maxWidth = null, int? maxHeight = null, int? maxBoth = null, int? cropMax = null, string blobContainerName = null);
        Task<bool> RemoveImageAsync(string path, string blobContainerName = null);
        Task<bool> RemoveImageFolderAsync(string path, string blobContainerName = null);
    }
}
