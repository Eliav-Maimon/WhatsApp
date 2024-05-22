using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

        public static void AddMyAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("JwtSettings");
            services.AddAuthentication(x =>
                      {
                          x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                          x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                          x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                          // what we want to validate:
                      }).AddJwtBearer(x =>
                      {
                          x.TokenValidationParameters = new TokenValidationParameters
                          {
                              ValidIssuer = jwtConfig["Issuer"],
                              ValidAudience = jwtConfig["Audience"],
                              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"])),
                              ValidateIssuer = true,
                              ValidateAudience = true,
                              ValidateLifetime = true,
                              ValidateIssuerSigningKey = true
                          };
                      }
            );
            services.AddAuthorization();
        }

        public static void AddMyCors(this IServiceCollection services)
        {
            services.AddCors(x => x.AddDefaultPolicy(
              opts => opts
                  .WithOrigins("http://127.0.0.1:4200", "http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  ));
        }
    }
}