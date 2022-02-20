using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerApi.Services
{
    public interface ITableService
    {
        Task AddItem<T>(T item) where T : ITableEntity, new();
        Task<List<T>> GetTableItems<T>(string partitionKey, T item) where T : ITableEntity, new();
        Task<T> GetTableItem<T>(string name, string password) where T : ITableEntity, new();
        Task UpdateTableItem<T>(string pk, string rk, bool flag) where T : ITableEntity, new();
        Task UpdateTableItem<T>(T item) where T : ITableEntity, new();
        Task<T> RetrieveItem<T>(T item) where T : ITableEntity, new();
    }
}
