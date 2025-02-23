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
        ///<inheritdoc cref="IHandler.MapHandler(IEndpointRouteBuilder)"/>
        public void MapHandler(IEndpointRouteBuilder app)
        {
            app.MapGet("/entrypoint", (HttpContext httpContext, IPretendoRepository repository) =>
            {
                if (!httpContext.RequestContainsSegments())
                {
                    return Results.Json("Listening for pretendos");
                }
                var pretendo = FindPretendo(httpContext, repository);
                if (pretendo is null)
                {
                    return Results.Json("Pretendo Not Found", statusCode: 404);
                }
                bool somethingWentWrong = false;
                Console.WriteLine("Pretendo Found");
                try
                {
                    //If there is return object and is valid json
                    if (pretendo.ReturnObject is not null && pretendo.ReturnObject.IsValidJson(out var isArray))
                    {
                        dynamic data = null;
                        if (isArray.HasValue && isArray.Value)
                        {
                            data = JsonSerializer.Deserialize<List<ExpandoObject>>(pretendo.ReturnObject);
                        }
                        else
                        {
                            data = JsonSerializer.Deserialize<ExpandoObject>(pretendo.ReturnObject);
                        }

                        if (data is null)
                        {
                            return Results.Json("Pretendo Not Found", statusCode: 404);
                        }
                        // returns json data
                        return Results.Json(data, statusCode: pretendo.StatusCode);
                    }
                    //returns text data
                    return Results.Json(pretendo.ReturnObject, statusCode: pretendo.StatusCode);
                }
                catch (Exception)
                {
                    somethingWentWrong = true;
                    return Results.Json("Something went wrong", statusCode: 500);
                }
                finally
                {
                    if (pretendo.Webhook is ConfigurableWebhook thisWebhook)
                    {
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
            Console.WriteLine("Calling configured webhook");
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(thisWebhook.Payload), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(thisWebhook.Url, content))
                {
                    Console.WriteLine("Configured Webhook responded with status code: {0}", response.StatusCode);
                }
            }
        }
    }
}
