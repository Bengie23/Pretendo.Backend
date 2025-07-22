using Pretendo.Backend.Data.DataAccess;
using Pretendo.Backend.Handlers.Extensions;
using Pretendo.Backend.Middleware;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Pretendo.Backend
{
    public class Program
    {
        protected static ILogger? Logger;
        public static void Main(string[] args)
        {
            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            Logger = factory.CreateLogger<Program>();
            if (!IsCurrentProcessElevated()) { Logger.LogError("Pretendo.Backend requires elevated access."); }
            var builder = WebApplication.CreateBuilder(args);

            builder.Host
                .UseWindowsService(options =>
                {
                    options.ServiceName = "pretendo-local-mocks";
                });

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<IPretendoRepository, PretendoRepository>();
            builder.Services.AddHandlers(typeof(Program).Assembly);
            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.WriteIndented = true;
                options.SerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            });

            builder.WebHost.UseUrls("http://pretendo.local", "https://pretendo.local");
            Logger.LogWarning(Path.GetDirectoryName(new Uri(Assembly.GetCallingAssembly().Location).LocalPath));
            if (builder.Environment.IsProduction())
            {
                string? path = Environment.GetEnvironmentVariable("PRETENDO_LOCAL_CERT_PATH");
                string? key = Environment.GetEnvironmentVariable("PRETENDO_LOCAL_CERT_KEY");
                if (!string.IsNullOrWhiteSpace(path) && !string.IsNullOrWhiteSpace(key))
                {
                    Logger.LogWarning($"Configuring HTTP Certificate for path: { path} with key {key}");
                    builder.WebHost.ConfigureKestrel(options =>
                    {
                        options.ListenLocalhost(80);
                        options.ListenLocalhost(8080);
                        options.ListenLocalhost(443, listenOptions =>
                        {
                            listenOptions.UseHttps(path, key);
                        });
                    });
                }
            }
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
