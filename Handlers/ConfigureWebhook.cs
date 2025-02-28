using Microsoft.AspNetCore.Mvc;
using Pretendo.Backend.Data.DataAccess;
using Pretendo.Backend.Data.DTOs;
using Pretendo.Backend.Data.Entities;
using Pretendo.Backend.Handlers.Extensions;

namespace Pretendo.Backend.Handlers
{
    ///<inheritdoc cref="IHandler"/>
    public class ConfigureWebhook : IHandler
    {
        ///<inheritdoc cref="IHandler.MapHandler(IEndpointRouteBuilder)"/>
        public void MapHandler(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/pretendo/{pretendoId}/webhooks", async ([FromRoute] int pretendoId, HttpRequest request, IPretendoRepository repo) =>
            {
                ConfigurableWebhookDTO? dto = await request.ReadFromJsonAsync<ConfigurableWebhookDTO>();
                if (dto is not null)
                {
                    ConfigurableWebhook webhook = dto.ToEntity();
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
