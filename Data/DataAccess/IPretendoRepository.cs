using Pretendo.Backend.Data.DTOs;
using Pretendo.Backend.Data.Entities;

namespace Pretendo.Backend.Data.DataAccess
{
    /// <summary>
    /// Wrapper for Pretendo Data Actions
    /// </summary>
    public interface IPretendoRepository
    {
        /// <summary>
        /// Returns any match for given domain and path
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public Entities.Pretendo? FindPretendo(string domain, string path);
        /// <summary>
        /// Returns a list of domains
        /// </summary>
        /// <returns></returns>
        public List<Domain> GetDomainList();

        /// <summary>
        /// Returns a list of Pretendo(s) for given domain
        /// </summary>
        /// <param name="domain">Domain name</param>
        /// <returns></returns>
        public List<PretendoDTO> GetPretendos(string domain);

        /// <summary>
        /// Creates a new Pretendo instance
        /// </summary>
        /// <param name="domain">Domain name</param>
        /// <param name="pretendo">Pretendo object</param>
        public void AddPretendo(string domain, Entities.Pretendo pretendo);

        /// <summary>
        /// Performs Initial Data Seed
        /// </summary>
        public void Seed();
    }
}
