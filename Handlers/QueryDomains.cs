using Pretendo.Backend.Data.DataAccess;

namespace Pretendo.Backend.Handlers
{
    ///<inheritdoc cref="IHandler"/>
    public class QueryDomains : IHandler
    {
        ///<inheritdoc cref="IHandler.MapHandler(IEndpointRouteBuilder)"/>
        public void MapHandler(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/domain", (HttpContext httpContext, IPretendoRepository repository) =>
            {
                var domains = repository.GetDomainList();
                return domains.Select(x => x.Name).Distinct().ToList();
            });
        }
    }
}
