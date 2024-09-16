using Pretendo.Backend.Data.DataAccess;
using Pretendo.Backend.Scripting;
using System.Web.Http;

namespace Pretendo.Backend.Handlers
{
    ///<inheritdoc cref="IHandler"/>
    public class CreatePretendo : IHandler
    {
        ///<inheritdoc cref="IHandler.MapHandler(IEndpointRouteBuilder)"/>
        public void MapHandler(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/domain/{domainName}/pretendos", async ([FromUri] string domainName, HttpRequest request, IPretendoRepository repo) =>
            {
                var jsonObj = await request.ReadFromJsonAsync<Data.Entities.Pretendo>();

                DomainCreator.CreateDomain(domainName);
                repo.AddPretendo(domainName, jsonObj);
                return StatusCodes.Status200OK;
            });
        }
    }
}
