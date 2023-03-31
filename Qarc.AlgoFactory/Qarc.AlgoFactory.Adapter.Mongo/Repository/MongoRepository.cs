using MongoDB.Driver;
using Qarc.AlgoFactory.Adapter.Mongo.Indexes;
using Qarc.AlgoFactory.Core.Application.SharedKernel;
using Qarc.Algos.SharedKernel.InputModel;
using System.Linq.Expressions;

namespace Qarc.AlgoFactory.Adapter.Mongo.Repository
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _collection;
        private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            this._collection = database.GetCollection<T>(collectionName);
            CollectionIndex.CreateIndexIfRequired(this._collection);
        }

        public async Task CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _collection.InsertOneAsync(entity);
        }

        public async Task CreateManyAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await _collection.InsertManyAsync(entities);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await _collection.Find(_filterBuilder.Empty).ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            var filter = _filterBuilder.Eq(e => e.Id, id);

            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(string id)
        {
            FilterDefinition<T> filter = _filterBuilder.Eq(e => e.Id, id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            FilterDefinition<T> filter = _filterBuilder.Eq(e => e.Id, entity.Id);
            await _collection.ReplaceOneAsync(filter, entity);
        }
    }
}
