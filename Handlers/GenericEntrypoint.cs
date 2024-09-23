using Pretendo.Backend.Data.DataAccess;
using Pretendo.Backend.Handlers.Extensions;
using System.Dynamic;
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
                var domain = httpContext.Request.Host.Host;
                var path = httpContext.PretendoPathFromSegments();
                Data.Entities.Pretendo? pretendo = repository.FindPretendo(domain, path);
                if (pretendo is null)
                {
                    return Results.Json("Pretendo Not Found", statusCode: 404);
                }
                if (pretendo.ReturnObject is not null && pretendo.ReturnObject.IsValidJson())
                {
                    dynamic data = JsonSerializer.Deserialize<ExpandoObject>(pretendo.ReturnObject);

                    if (data is not null)
                    {
                        return Results.Json(data, statusCode: pretendo.StatusCode);
                    }
                }

                return Results.Json(pretendo.ReturnObject, statusCode: pretendo.StatusCode);
            });
        }
    }
}
