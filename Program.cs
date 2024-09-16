using Pretendo.Backend.Data.DataAccess;
using Pretendo.Backend.Handlers.Extensions;
using Pretendo.Backend.Middleware;

namespace Pretendo.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<IPretendoRepository, PretendoRepository>();
            builder.Services.AddHandlers(typeof(Program).Assembly);
            PretendoDBSeed.Initialize();
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();
            app.MapHandlers();
            app.UseRewriteMiddleware();
            app.Run();

        }
    }
}
