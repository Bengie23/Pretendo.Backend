
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Pretendo.Backend.Data.DTOs;
using Pretendo.Backend.Data.Entities;
using Pretendo.Backend.Handlers.Extensions;
using Pretendo.Backend.Scripting;
using System.Collections;

namespace Pretendo.Backend.Data.DataAccess
{
    ///<inheritdoc cref="IPretendoRepository"/>
    public class PretendoRepository : IPretendoRepository
    {
        ///<inheritdoc cref="IPretendoRepository.AddPretendo(string, Pretendo)"/>
        public void AddPretendo(string domain, Entities.Pretendo pretendo)
        {
            var currentDomain = GetOrCreate(domain);

            if (currentDomain != null)
            {
                using (var context = new PretendoDbContext())
                {
                    var domain_to_update = context.Domains.Single(x => x.Id == currentDomain.Id);
                    if (domain_to_update.Pretendos == null)
                    {
                        domain_to_update.Pretendos = new List<Entities.Pretendo>();
                    }
                    domain_to_update.Pretendos.Add(pretendo);
                    context.SaveChanges();
                }
            }

        }

        ///<inheritdoc cref="IPretendoRepository.ConfigureWebhook(int, ConfigurableWebhook)"/>
        public void ConfigureWebhook(int pretendoId, ConfigurableWebhook webhook)
        {
            using (var context = new PretendoDbContext())
            {
                var pretendo = context.Pretendos.Single(x => x.Id == pretendoId);
                if (pretendo is null)
                {
                    throw new Exception("Unable to configure webhook. Pretendo Not Found");
                }
                pretendo.Webhook = webhook;
                context.SaveChanges();
            }
        }

        ///<inheritdoc cref="IPretendoRepository.FindPretendo(string, string)"/>
        public Entities.Pretendo? FindPretendo(string domain, string path)
        {
            List<Entities.Pretendo> pretendos = null;
            using (var context = new PretendoDbContext())
            {
                pretendos = context.Pretendos
                   .Include(x => x.Domain)
                   .Include(x=>x.Webhook)
                   .Where(x => x.Domain != null)
                   .Where(x => x.Path.Trim('/') == path.Trim('/'))
                   .Where(x => x.Domain.Name == domain)
                   .ToList();
            }
            if (pretendos.Count > 1)
            {
                throw new Exception("Unable to find Pretendo. There are more than one result for this query");
            }
            return pretendos.FirstOrDefault();
        }

        ///<inheritdoc cref="IPretendoRepository.GetDomainList"/>
        public List<Domain> GetDomainList()
        {
            using (var context = new PretendoDbContext())
            {
                return context.Domains
                    .Include(x => x.Pretendos)
                    .ToList();
            }
        }

        ///<inheritdoc cref="IPretendoRepository.GetPretendos(string)"/>
        public List<PretendoDTO> GetPretendos(string domain)
        {
            var pretendos = new List<Entities.Pretendo>();
            using (var context = new PretendoDbContext())
            {
                var domainObject = context.Domains.Include(x => x.Pretendos).Where(x => x.Name == domain).FirstOrDefault();
                if (domainObject != null)
                {
                    pretendos = domainObject.Pretendos;
                }
            }
            return pretendos.Select(x => new PretendoDTO
            {
                Id = x.Id,
                Name = x.Name,
                Path = x.Path,
                ReturnObject = x.ReturnObject,
                StatusCode = x.StatusCode
            }).ToList();
        }

        ///<inheritdoc cref="IPretendoRepository.GetWebhooks(int pretendoId)"/>
        public List<ConfigurableWebhook> GetWebhooks(int pretendoId)
        {
           using (var context = new PretendoDbContext())
            {
                var pretendo = context.Pretendos.Include(x => x.Webhook).Where(x => x.Id == pretendoId).FirstOrDefault();
                if (pretendo is null)
                {
                    throw new Exception("Pretendo Not Found");
                }
                if (pretendo.Webhook is null)
                {
                    return new List<ConfigurableWebhook>();
                }
                return new List<ConfigurableWebhook> { pretendo.Webhook };
            }
        }

        ///<inheritdoc cref="IPretendoRepository.Seed"/>
        public void Seed()
        {
            using (var context = new PretendoDbContext())
            {
                if (context.Domains.Count() == 0)
                {
                    var dummy = new List<Domain>
                    {
                        new Domain {
                            Name = "pretendo.local",
                            Pretendos = new List<Entities.Pretendo> {
                                new Entities.Pretendo { 
                                    Path = "/a/b/c", 
                                    ReturnObject = "Hello World", 
                                    StatusCode = 200, 
                                    Name = "Test1", 
                                    //Webhook = new ConfigurableWebhook {
                                    //    Url = "https://localhost:7296/api/webhook",
                                    //    Payload = @"{ 'Message': 'TEST' }".FromPretendoString(),
                                    //}
                                }
                            }
                        }
                    };
                    context.AddRange(dummy);
                    context.SaveChanges();
                }
            }
            using (var context = new PretendoDbContext())
            {
                foreach (var item in context.Domains)
                {
                    DomainCreator.CreateDomain(item.Name);
                }
            }
        }

        private Domain GetOrCreate(string domainName)
        {
            Domain? domain = null;
            using (var context = new PretendoDbContext())
            {
                domain = context.Domains.Where(x => x.Name == domainName).SingleOrDefault();
                if (domain is null)
                {
                    domain = new Domain
                    {
                        Name = domainName,
                    };
                    context.Domains.Add(domain);
                    context.SaveChanges();
                }
            }
            return domain;
        }
    }

    public static class PretendoDBSeed
    {
        public static void Initialize()
        {
            var repo = new PretendoRepository();
            repo.Seed();
        }
    }
}
