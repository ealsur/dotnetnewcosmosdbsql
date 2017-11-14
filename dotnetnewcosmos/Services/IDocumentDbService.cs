namespace dotnetnewcosmos.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using dotnetnewcosmos.Models;

    public interface IDocumentDbService
    {
        Task<IEnumerable<Item>> GetItemsAsync(Expression<Func<Item, bool>> predicate);
        Task<Item> GetItemAsync(string id);
        Task AddItemAsync(Item item);
        Task UpdateItemAsync(string id, Item item);
        Task DeleteItemAsync(string id);
    }
}
