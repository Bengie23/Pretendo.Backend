using Pretendo.Backend.Data.DataAccess;
using System.Web.Http;

namespace Pretendo.Backend.Handlers
{
    ///<inheritdoc cref="IHandler"/>
    public class QueryPretendos : IHandler
    {
        ///<inheritdoc cref="IHandler.MapHandler(IEndpointRouteBuilder)"/>
        public void MapHandler(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/domain/{domainName}/pretendos", async ([FromUri] string domainName, HttpRequest request, IPretendoRepository repo) =>
            {
                var pretendos = repo.GetPretendos(domainName);
                return pretendos;

            });
        }
    }
}
