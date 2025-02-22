using System.ComponentModel.DataAnnotations.Schema;

namespace Pretendo.Backend.Data.Entities
{
    public class ConfigurableWebhook
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Url { get; set; }
        public string Payload { get; set; }
    }
}
