using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.DataAccess.Repository.Interfaces
{
    public interface IApprovalRepository
    {
        // Portal
        Task<IQueryable<RecipeApproval>> GetApprovalsAsync();
        Task ApproveApprovalAsync(int id);
    }
}
