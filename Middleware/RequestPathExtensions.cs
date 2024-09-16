using System.Text;

namespace Pretendo.Backend.Middleware
{
    /// <summary>
    /// Extension methods for the Request Path object
    /// </summary>
    public static class RequestPathExtensions
    {
        /// <summary>
        /// Hydrates a segments query string based on the given path string
        /// </summary>
        /// <param name="pathString"></param>
        /// <returns></returns>
        public static String HydrateEntrypointSegments(this PathString pathString)
        {
            var segments = pathString.Value.Split("/");
            var query_string = new StringBuilder("?segments=");
            foreach (var segment in segments)
            {
                query_string.Append(segment);
                if (segment != segments.Last())
                {
                    query_string.Append("&segments=");
                }
            }
            return query_string.ToString();
        }
    }
}
