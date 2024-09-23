using Pretendo.Backend.Data.DataAccess;
using Pretendo.Backend.Handlers.Extensions;
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
                Data.Entities.Pretendo? pret = await request.ReadFromJsonAsync<Data.Entities.Pretendo>();
                if (pret is not null)
                {
                    var parsedJson = pret.ReturnObject.FromPretendoString();
                    if (parsedJson.IsValidJson())
                    {
                        pret.ReturnObject = parsedJson;
                    }
                }
                DomainCreator.CreateDomain(domainName);
                repo.AddPretendo(domainName, pret);
                return StatusCodes.Status200OK;
            });
        }
    }
}
