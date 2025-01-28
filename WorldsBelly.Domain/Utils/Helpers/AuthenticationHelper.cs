using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldsBelly.Domain._SharedModels.Authentication;
using WorldsBelly.Domain.Utils.Constants;

namespace WorldsBelly.Domain.Utils.Helpers
{
    public static class AuthenticationHelper
    {
        public static async Task<AuthInstanceEntity> GetAuthInstanceEntity(string email, string accessToken, string azureStorageAccountConnectionString)
        {
            var table = AuthTable(Storage.UserRefreshTokenTable, azureStorageAccountConnectionString);
            var query = new TableQuery<AuthInstanceEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, email));
            var executedQuery = await table.ExecuteQuerySegmentedAsync(query, null).ConfigureAwait(false);
            return executedQuery.Results.FirstOrDefault(_ => _.AccessToken == accessToken);
        }

        public static Task UpdateAuthInstanceEntity(AuthInstanceEntity authInstanceEntity, string refreshToken, string accessToken, string azureStorageAccountConnectionString)
        {
            var table = AuthTable(Storage.UserRefreshTokenTable, azureStorageAccountConnectionString);
            authInstanceEntity.RefreshToken = refreshToken;
            authInstanceEntity.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            authInstanceEntity.AccessToken = accessToken;
            var insertOperation = TableOperation.Merge(authInstanceEntity);
            return table.ExecuteAsync(insertOperation);
        }

        public static async Task DeleteAuthInstance(string email, string accessToken, string azureStorageAccountConnectionString)
        {
            var authInstanceEntity = await GetAuthInstanceEntity(email, accessToken, azureStorageAccountConnectionString).ConfigureAwait(false);

            if (authInstanceEntity == null)
                throw new Exception("Auth instance was not found");

            var table = AuthTable(Storage.UserRefreshTokenTable, azureStorageAccountConnectionString);
            var insertOperation = TableOperation.Delete(authInstanceEntity);
            await table.ExecuteAsync(insertOperation);
        }

        public static CloudTable AuthTable(string tableName, string azureStorageAccountConnectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(azureStorageAccountConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var tableReference = tableClient.GetTableReference(tableName);

            return tableReference;
        }
    }
}
