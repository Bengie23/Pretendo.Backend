
using Pretendo.Backend.Data.DataAccess;
using System.Web.Http;

namespace Pretendo.Backend.Handlers
{
    ///<inheritdoc cref="IHandler"/>

    public class QueryWebhooks : IHandler
    {
        ///<inheritdoc cref="IHandler.MapHandler(IEndpointRouteBuilder)"/>
        public void MapHandler(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/pretendo/{pretendoId}/webhooks", async ([FromUri] int pretendoId, HttpRequest request, IPretendoRepository repo) =>
            {
                var webhooks = repo.GetWebhooks(pretendoId);
                return webhooks;

            });
        }
    }
}
