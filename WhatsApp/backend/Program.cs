
using backend.Models;
using backend.Repository.VerifyCodeRepository;
using backend.Services;
using WhatsApp.Repository;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserRepository<User>, UserMongoRepository>();
            builder.Services.AddScoped<IVerifyRepository<VerifyCode>, VerifyRepository>();
            builder.Services.AddScoped<IVerifyCodeService<VerifyCode>, VerifyCodeService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMongo();

            builder.Services.AddMyCors();

            builder.Services.AddMyAuthentication(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseHttpsRedirection();
            
            // Authentication = אימות
            app.UseAuthentication();

            // Authorization = הרשאה
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
