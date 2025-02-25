using Pretendo.Backend.Data.DataAccess;
using Pretendo.Backend.Data.Entities;
using Pretendo.Backend.Handlers.Extensions;
using System.Dynamic;
using System.Text;
using System.Text.Json;

namespace Pretendo.Backend.Handlers
{
    ///<inheritdoc cref="IHandler"/>
    public class GenericEntrypoint : IHandler
    {
        private readonly ILogger<GenericEntrypoint> logger;

        public GenericEntrypoint(ILogger<GenericEntrypoint> logger)
        {
            this.logger = logger;
        }
        ///<inheritdoc cref="IHandler.MapHandler(IEndpointRouteBuilder)"/>
        public void MapHandler(IEndpointRouteBuilder app)
        {
            app.MapGet("/entrypoint", (HttpContext httpContext, IPretendoRepository repository) =>
            {
                logger.LogInformation("Processing Request Started:{host}{path}", httpContext.Request.Host.Host, httpContext.Request.Path.Value);
                JsonSerializerOptions options = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };
                if (!httpContext.RequestContainsSegments())
                {
                    logger.LogWarning("Request does NOT match pretendo structure, ignoring request...");
                    return Results.Json("Listening for pretendos");
                }
                logger.LogInformation("Request DOES HAVE the correct structure, searching for the requested pretendo...");
                var pretendo = FindPretendo(httpContext, repository);
                if (pretendo is null)
                {
                    logger.LogInformation("Pretendo NOT Found");
                    return Results.Json("Pretendo Not Found", statusCode: 404);
                }
                bool somethingWentWrong = false;
                logger.LogInformation("Pretendo Found. Loading & parsing Return Object and other details of this Pretendo.");
                try
                {
                    //If there is return object and is valid json
                    if (pretendo.ReturnObject is not null && pretendo.ReturnObject.IsValidJson(out var isArray))
                    {
                        dynamic? data;
                        if (isArray.HasValue && isArray.Value)
                        {
                            data = JsonSerializer.Deserialize<List<ExpandoObject>>(pretendo.ReturnObject, options);
                        }
                        else
                        {
                            data = JsonSerializer.Deserialize<ExpandoObject>(pretendo.ReturnObject, options);
                        }

                        if (data is null)
                        {
                            logger.LogInformation("There was a problem loading & parsing Return Object");
                            return Results.Json("Pretendo Not Found", statusCode: 404);
                        }

                        // returns json data
                        return Results.Json(data, options, statusCode: pretendo.StatusCode);
                    }
                    //returns text data
                    return Results.Json(pretendo.ReturnObject, statusCode: pretendo.StatusCode);
                }
                catch (Exception)
                {
                    somethingWentWrong = true;
                    logger.LogError("Something went wrong");
                    return Results.Json("Something went wrong", statusCode: 500);
                }
                finally
                {
                    logger.LogInformation("Loading Pretendo's configured Webhook(s)...");
                    if (!somethingWentWrong && pretendo.Webhook is ConfigurableWebhook thisWebhook)
                    {
                        logger.LogInformation("Webhook found.");
                        _ = TriggerWebhook(thisWebhook);
                    }
                }
            });
        }
        private Data.Entities.Pretendo? FindPretendo(HttpContext httpContext, IPretendoRepository repository)
        {
            var domain = httpContext.Request.Host.Host.StartsWith("www.") ? httpContext.Request.Host.Host.Replace("www.","") : httpContext.Request.Host.Host;
            var path = httpContext.PretendoPathFromSegments();
            return repository.FindPretendo(domain, path);
        }
        private async Task TriggerWebhook(ConfigurableWebhook thisWebhook)
        {
            logger.LogInformation("Calling configured webhook");
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(thisWebhook.Payload), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(thisWebhook.Url, content))
                {
                    logger.LogInformation("Configured Webhook responded with status code: {0}", response.StatusCode);
                }
            }
        }
    }
}
