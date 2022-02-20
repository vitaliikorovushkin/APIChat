using ChatServerApi.Models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerApi.Services
{
    public class TableService : ITableService
    {
        private readonly CloudTableClient client;
        private CloudTable table;

        public TableService(CloudTableClient client)
        {
            this.client = client;
            table = client.GetTableReference("pv912");
            table.CreateIfNotExistsAsync();

        }

        public async Task AddItem<T>(T item) where T : ITableEntity, new()
        {
            List<T> list = await GetTableItems<T>("User", item);
            if (list.Count == 0)
            {
                TableOperation insert = TableOperation.Insert(item);
                await table.ExecuteAsync(insert);
            }
        }

        public async Task<T> GetTableItem<T>(string name, string password) where T : ITableEntity, new()
        {
            List<T> list = new List<T>();
            TableQuery<T> query = null;
            query = new TableQuery<T>()
            .Where(TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, name),
            TableOperators.And,
            TableQuery.GenerateFilterCondition("Password", QueryComparisons.Equal, password)
            )
            );
            TableQuerySegment<T> segment = null;
            do
            {
                segment = await table.ExecuteQuerySegmentedAsync(query, segment?.ContinuationToken);
                list.AddRange(segment.Results);
            } while (segment.ContinuationToken != null);



            return list[0];
        }

        public async Task<List<T>> GetTableItems<T>(string partitionKey, T item) where T : ITableEntity, new()
        {
            List<T> list = new List<T>();
            TableQuery<T> query = null;
            if (item is User)
            {
                query = new TableQuery<T>()
                .Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, (item as User).Name)
                    )
                );
            }
            else if (item == null)
            {
                query = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            }
            TableQuerySegment<T> segment = null;
            do
            {
                segment = await table.ExecuteQuerySegmentedAsync(query, segment?.ContinuationToken);
                list.AddRange(segment.Results);
            } while (segment.ContinuationToken != null);

            return list;
        }

        public async Task<T> RetrieveItem<T>(T item) where T : ITableEntity, new()
        {
            TableOperation find = TableOperation.Retrieve<T>(item.PartitionKey, item.RowKey);
            TableResult result = await table.ExecuteAsync(find);
            T itemFromDb = (T)result.Result;
            return itemFromDb;
        }

        public Task UpdateTableItem<T>(string pk, string rk, bool flag) where T : ITableEntity, new()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTableItem<T>(T item) where T : ITableEntity, new()
        {
            TableOperation update = TableOperation.Replace(item);
            await table.ExecuteAsync(update);
        }
    }
}
