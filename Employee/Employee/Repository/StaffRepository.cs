using Employee.Model;
using MongoDB.Driver;
using SharpCompress.Common;

namespace Employee.Repository
{
    public class StaffRepository
    {
        private const string collectionName = "staffs";
        private readonly IMongoCollection<Staff> dbCollection;
        private readonly FilterDefinitionBuilder<Staff> filterBuilder = Builders<Staff>.Filter;

        public StaffRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Employee");
            dbCollection = database.GetCollection<Staff>(collectionName);
        }

        public async Task<IReadOnlyCollection<Staff>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<Staff> GetByIdAsync(Guid id)
        {
            FilterDefinition<Staff> filter = filterBuilder.Eq(entity => entity.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Staff entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Staff entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<Staff> filter = filterBuilder.Eq(existing => existing.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<Staff> filter = filterBuilder.Eq(existing => existing.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }

    }
}
