using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Repository.Interfaces;

namespace WorldsBelly.DataAccess.Repository
{
    public class ApprovalRepository : IApprovalRepository
    {
        private readonly AppDbContext _dbContext;
        private IRecipeDraftRepository _recipeRepository;

        public ApprovalRepository(AppDbContext dbContext, IRecipeDraftRepository recipeRepository)
        {
            _dbContext = dbContext;
            _recipeRepository = recipeRepository;
        }

        public async Task ApproveApprovalAsync(int id)
        {
            var approvalToRemove = _dbContext.RecipeApprovals.FirstOrDefault(_ => _.Id == id);
            if (approvalToRemove != null)
            {
                await _recipeRepository.ApproveRecipeDraftAsync(approvalToRemove.RecipeTranslationId, approvalToRemove.UnPublishedIngredients > 0);

                if (approvalToRemove.UnPublishedIngredients > 0)
                {
                    var approvals = await _dbContext.RecipeApprovals.Where(_ => _.UnPublishedIngredients > 0 && _.Id != id).ToListAsync();
                    foreach (var approval in approvals)
                    {
                        var unPublishedIngredientIds = approvalToRemove.UnPublishedIngredientIds.Split(";");
                        var unPublishedIngredientNames = approvalToRemove.UnPublishedIngredientNames.Split(";");
                        var counter = 0;
                        foreach (var unPublishedIngredientId in unPublishedIngredientIds)
                        {
                            var isNumeric = int.TryParse(unPublishedIngredientId, out int n);
                            if (isNumeric && approval.UnPublishedIngredientIds.Contains(unPublishedIngredientId + ";"))
                            {
                                approval.UnPublishedIngredientIds = approval.UnPublishedIngredientIds.Replace(unPublishedIngredientId + ";", "");
                                approval.UnPublishedIngredientNames = approval.UnPublishedIngredientNames.Replace(unPublishedIngredientNames[counter] + ";", "");
                                approval.UnPublishedIngredients = approval.UnPublishedIngredients - 1;
                            }
                            counter++;
                        }
                        _dbContext.Update(approval);
                    }
                }
                _dbContext.Remove(approvalToRemove);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Id was not found");
            }
        }

        public async Task<IQueryable<RecipeApproval>> GetApprovalsAsync()
        {
            return _dbContext.RecipeApprovals.OrderByDescending(_ => _.CreatedAt);
        }
    }
}
