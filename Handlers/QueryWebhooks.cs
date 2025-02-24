
using Microsoft.AspNetCore.Mvc;
using Pretendo.Backend.Data.DataAccess;

namespace Pretendo.Backend.Handlers
{
    ///<inheritdoc cref="IHandler"/>

    public class QueryWebhooks : IHandler
    {
        ///<inheritdoc cref="IHandler.MapHandler(IEndpointRouteBuilder)"/>
        public void MapHandler(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/pretendo/{pretendoId}/webhooks", ([FromRoute] int pretendoId, HttpRequest request, IPretendoRepository repo) =>
            {
                var webhooks = repo.GetWebhooks(pretendoId);
                return webhooks;

            });
        }
    }
}
