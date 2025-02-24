using Microsoft.AspNetCore.Mvc;
using Pretendo.Backend.Data.DataAccess;

namespace Pretendo.Backend.Handlers
{
    ///<inheritdoc cref="IHandler"/>
    public class QueryPretendos : IHandler
    {
        ///<inheritdoc cref="IHandler.MapHandler(IEndpointRouteBuilder)"/>
        public void MapHandler(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/domain/{domainName}/pretendos", ([FromRoute] string domainName, HttpRequest request, IPretendoRepository repo) =>
            {
                var pretendos = repo.GetPretendos(domainName);
                return pretendos;

            });
        }
    }
}
