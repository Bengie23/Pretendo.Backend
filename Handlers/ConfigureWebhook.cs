
using Microsoft.Extensions.ObjectPool;
using Pretendo.Backend.Data.DataAccess;
using Pretendo.Backend.Handlers.Extensions;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Web.Http;

namespace Pretendo.Backend.Handlers
{
    ///<inheritdoc cref="IHandler"/>
    public class ConfigureWebhook : IHandler
    {
        ///<inheritdoc cref="IHandler.MapHandler(IEndpointRouteBuilder)"/>
        public void MapHandler(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/pretendo/{pretendoId}/webhooks", async ([FromUri] int pretendoId, HttpRequest request, IPretendoRepository repo) =>
            {
                Data.Entities.ConfigurableWebhook? webhook = await request.ReadFromJsonAsync<Data.Entities.ConfigurableWebhook>();
                if (webhook is not null)
                {
                    var parsedJson = webhook.Payload.FromPretendoString();
                    if (parsedJson.IsValidJson(out _))
                    {
                        webhook.Payload = parsedJson;
                    }

                    repo.ConfigureWebhook(pretendoId, webhook);

                }
                return StatusCodes.Status200OK;
            });
        }
    }
}
