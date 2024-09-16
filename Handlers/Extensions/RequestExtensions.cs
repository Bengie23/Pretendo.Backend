namespace Pretendo.Backend.Handlers.Extensions
{
    /// <summary>
    /// Extends HttpContext to grab and evaluate its content
    /// </summary>
    public static class RequestExtensions
    {
        private const string segmentsKey = "segments";

        /// <summary>
        /// Checks if there are any segments in the Request.Query object
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool RequestContainsSegments(this HttpContext context)
        {
            var segments = context.Request.Query[segmentsKey];
            return segments.Any();
        }

        /// <summary>
        /// Returns segments in the Request.Query object
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string PretendoPathFromSegments(this HttpContext context)
        {
            var segments = context.Request.Query["segments"];
            return string.Join("/", segments);
        }
    }
}
