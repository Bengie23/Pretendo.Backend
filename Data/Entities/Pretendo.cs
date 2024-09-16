using System.ComponentModel.DataAnnotations.Schema;

namespace Pretendo.Backend.Data.Entities
{
    /// <summary>
    /// Represents any custom request made into an existing local domain
    /// </summary>
    public class Pretendo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Path { get; set; }
        public string ReturnObject { get; set; }
        public Domain? Domain { get; set; }
        public string Name { get; set; }
        public int StatusCode { get; set; }
        public HttpVerbs HttpVerb { get; set; }
    }
}