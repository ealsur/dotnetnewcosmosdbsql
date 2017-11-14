namespace dotnetnewcosmos.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using dotnetnewcosmos.Models;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Microsoft.Extensions.Configuration;

    public class DocumentDbService : IDocumentDbService
    {
        private readonly CosmosDbSettings _settings;
        private readonly Uri _collectionUri;
        private DocumentClient _dbClient;

        public DocumentDbService(IConfigurationSection configuration)
        {
            _settings = new CosmosDbSettings(configuration);
            _collectionUri = GetCollectionLink();
            //See https://azure.microsoft.com/documentation/articles/documentdb-performance-tips/ for performance tips
            _dbClient = new DocumentClient(_settings.DatabaseUri, _settings.DatabaseKey, new ConnectionPolicy());

            
        }

        public async Task InitializeAsync()
        {
            Database database = await _dbClient.CreateDatabaseIfNotExistsAsync(
                new Database() {
                    Id = _settings.DatabaseName
                });

            await _dbClient.CreateDocumentCollectionIfNotExistsAsync(database.SelfLink, 
                new DocumentCollection() {
                    Id = _settings.CollectionName
                }, 
                new RequestOptions()
                {
                    OfferThroughput = 400
                });
        }
        
        public async Task AddItemAsync(Item item)
        {
            await _dbClient.CreateDocumentAsync(_collectionUri, item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await _dbClient.DeleteDocumentAsync(GetDocumentLink(id));
        }

        public async Task<Item> GetItemAsync(string id)
        {
            try
            {
                return await _dbClient.ReadDocumentAsync<Item>(GetDocumentLink(id));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(Expression<Func<Item, bool>> predicate)
        {
            IDocumentQuery<Item> query = _dbClient.CreateDocumentQuery<Item>(_collectionUri, new FeedOptions { MaxItemCount = -1 }).Where(predicate).AsDocumentQuery();
            List<Item> results = new List<Item>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<Item>());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, Item item)
        {
            await _dbClient.ReplaceDocumentAsync(GetDocumentLink(id), item);
        }

        private Uri GetCollectionLink()
        {
            return UriFactory.CreateDocumentCollectionUri(_settings.DatabaseName, _settings.CollectionName);
        }

        private Uri GetDocumentLink(string id)
        {
            return UriFactory.CreateDocumentUri(_settings.DatabaseName, _settings.CollectionName, id);
        }
    }
}
