using Pretendo.Backend.Data.Entities;

namespace Pretendo.Backend.Data.DTOs
{
    /// <summary>
    /// Represents a transferable webhook
    /// </summary>
    public class ConfigurableWebhookDTO
    {
        public string Url { get; set; }
        public string Payload { get; set; }

        public string HttpVerb { get; set; }

        public int Delay { get; set; }
    }

    /// <summary>
    /// Provide transformation ability to Configurable Webhook & DTO
    /// </summary>
    public static class DTOExtensions
    {
        /// <summary>
        /// Captures the HttpVerb value from a string value
        /// </summary>
        /// <param name="verbAsString"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static HttpVerbs ToEnum(this string verbAsString) 
        { 
            if (verbAsString == "GET")
                return HttpVerbs.Get;

            if (verbAsString == "POST")
                return HttpVerbs.Post;

            throw new NotImplementedException();
        }
        /// <summary>
        /// Transform a ConfigurableWebhookDTO to Entity
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static ConfigurableWebhook ToEntity(this ConfigurableWebhookDTO dto)
        {
            return  new ConfigurableWebhook()
            {
                Url = dto.Url,
                Payload = dto.Payload,
                HttpVerb = dto.HttpVerb.ToEnum(),
                Delay = dto.Delay,
            };
        }

        /// <summary>
        /// Transforms a ConfigurableWebhook to DTO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ConfigurableWebhookDTO ToDTO(this ConfigurableWebhook entity)
        {
            return new ConfigurableWebhookDTO()
            {
                Url = entity.Url,
                Payload = entity.Payload,
                HttpVerb = entity.HttpVerb.ToString().ToUpper(),
                Delay = entity.Delay,
            };
        }
    }
}
