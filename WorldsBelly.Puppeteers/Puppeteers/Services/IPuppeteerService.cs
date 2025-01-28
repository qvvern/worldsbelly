using System.Collections.Generic;
using System.Threading.Tasks;
using WorldsBelly.Puppeteers.Models;

namespace WorldsBelly.Puppeteers.Service.Interfaces
{
    public interface IPuppeteerService
    {
        Task<List<Translation>> TranslateAsync(List<string> translation);
        Task<FoundNutrient> FindNutrientsAsync(string url);
    }
}
