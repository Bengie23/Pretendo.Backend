using System.ComponentModel.DataAnnotations.Schema;

namespace Pretendo.Backend.Data.Entities
{
    /// <summary>
    /// A domain represents any existing local domain for example 'pretendo.local'
    /// </summary>
    public class Domain
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Pretendo> Pretendos { get; set; }
    }
}