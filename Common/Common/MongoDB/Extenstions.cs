using Common.Settings;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Common.Repository;
using MassTransit;

namespace Common.Extenstions
{
    public static class Extenstions
    {
        public static void AddMondoDb(this IServiceCollection services, IConfiguration configuration)
        {
            //To serialize GUID & DateTime
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            //DB Setting & Configuration
            services.AddSingleton(sp =>
            {
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var mongoClient = new MongoClient(mongoDbSettings?.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings?.ServiceName);
            });
        }

        public static void AddRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            //Add General Repository
            services.AddTransient<IRepository<T>>(sp =>
            {
                var db = sp.GetService<IMongoDatabase>();
                return new Repository<T>(db, collectionName);
            });
        }


    }
}
