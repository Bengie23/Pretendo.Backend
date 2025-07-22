using Microsoft.AspNetCore.Mvc;
using Pretendo.Backend.Data.DataAccess;
using Pretendo.Backend.Handlers.Extensions;
using Pretendo.Backend.Scripting;

namespace Pretendo.Backend.Handlers
{
    ///<inheritdoc cref="IHandler"/>
    public class CreatePretendo : IHandler
    {
        private const string localhost = "127.0.0.1";
        private readonly ILogger<CreatePretendo> logger;

        public CreatePretendo(ILogger<CreatePretendo> logger)
        {
            this.logger = logger;
        }
        ///<inheritdoc cref="IHandler.MapHandler(IEndpointRouteBuilder)"/>
        public void MapHandler(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/domain/{domainName}/pretendos", async ([FromRoute] string domainName, HttpRequest request, IPretendoRepository repo) =>
            {
                Data.Entities.Pretendo? pret = await request.ReadFromJsonAsync<Data.Entities.Pretendo>();
                if (pret is not null)
                {
                    var parsedJson = pret.ReturnObject.FromPretendoString();
                    if (parsedJson.IsValidJson(out _))
                    {
                        pret.ReturnObject = parsedJson;
                    }

                    var path_pieces = pret.Path.Split('?');
                    if (path_pieces.Length == 2)
                    {
                        pret.Args = path_pieces[1];
                        pret.Path = path_pieces[0];
                    }

                    DomainCreator.CreateDomain(domainName);
                    repo.AddPretendo(domainName, pret);
                }
                var checker = IsDnsWorking(domainName);
                if (!checker)
                {
                    logger.LogError("Unable to register local mock in DNS.");
                }
                return StatusCodes.Status200OK;
            });
        }

        private bool IsDnsWorking(string domain)
        {
            try
            {
                return System.Net.Dns.GetHostEntry(domain).AddressList
                    .Any(ip => ip.ToString() == localhost);
            }
            catch
            {
                return false;
            }
        }
    }
}
