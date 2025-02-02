
using Pretendo.Backend.Data.DataAccess;

namespace Pretendo.Backend.Handlers
{
    ///<inheritdoc cref="IHandler"/>
    public class PingPong : IHandler
    {
        public void MapHandler(IEndpointRouteBuilder app)
        {
            app.MapGet("/pretendo/ping", (HttpContext httpContext, IPretendoRepository repository) =>
            {
                return "pong";
            });
        }
    }
}
