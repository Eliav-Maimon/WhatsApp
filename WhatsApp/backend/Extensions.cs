using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace backend
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var mongoDbConnectionString = configuration.GetConnectionString("MongoDBConnection");
                var databaseName = configuration["DatabaseName"];
                var mongoClient = new MongoClient(mongoDbConnectionString);
                return mongoClient.GetDatabase(databaseName);
            });
            return services;
        }
    }
}