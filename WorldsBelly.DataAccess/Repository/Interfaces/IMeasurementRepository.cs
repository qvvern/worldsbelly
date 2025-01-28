using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
    public interface IMeasurementRepository
    {
        Task<IQueryable<Measurement>> GetMeasurementsAsync();
        Task<Measurement> GetMeasurementAsync(int id);
        Task UpdateMeasurementAsync(Measurement measurement);
        Task<Measurement> CreateMeasurementAsync(Measurement entity);
    }
}
