using Pretendo.Backend.Data.DataAccess;
using Pretendo.Backend.Handlers.Extensions;
using Pretendo.Backend.Middleware;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Pretendo.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!IsCurrentProcessElevated()) { Console.WriteLine("Pretendo.Backend requires elevated access."); }
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

        [DllImport("libc")]
        private static extern uint geteuid();

        public static bool IsCurrentProcessElevated()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // https://github.com/dotnet/sdk/blob/v6.0.100/src/Cli/dotnet/Installer/Windows/WindowsUtils.cs#L38
                using var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            // https://github.com/dotnet/maintenance-packages/blob/62823150914410d43a3fd9de246d882f2a21d5ef/src/Common/tests/TestUtilities/System/PlatformDetection.Unix.cs#L58
            // 0 is the ID of the root user
            return geteuid() == 0;
        }

    }
}
