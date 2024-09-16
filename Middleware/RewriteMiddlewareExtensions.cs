namespace Pretendo.Backend.Middleware
{
    /// <summary>
    /// Static helper for registering custom middleware
    /// </summary>
    public static class RewriteMiddlewareExtensions
    {
        /// <summary>
        /// Register custom Middleware into DI
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRewriteMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RewriteMiddleware>();
        }
    }
}
