using System.Text;

namespace Pretendo.Backend.Middleware
{
    /// <summary>
    /// Middleware to rewrite requests from all the other possible domains into a unique endpoint called 'entrypoint'
    /// </summary>
    public class RewriteMiddleware
    {
        private readonly RequestDelegate _next;

        public RewriteMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/domain", StringComparison.OrdinalIgnoreCase) || context.Request.Path.StartsWithSegments("/entrypoint", StringComparison.OrdinalIgnoreCase) || context.Request.Path.StartsWithSegments("/favicon.ico", StringComparison.OrdinalIgnoreCase))
            {
                //ignore request
                await _next(context);
                return;
            }
            var url = BuildEntrypointUrl(context);
            context.Response.Redirect(url);
            return;
        }

        private bool ShouldIgnoreRequest(HttpContext context)
        {
            return (context.Request.Path.StartsWithSegments("/api/domain", StringComparison.OrdinalIgnoreCase) ||
                    context.Request.Path.StartsWithSegments("/entrypoint", StringComparison.OrdinalIgnoreCase) ||
                    context.Request.Path.StartsWithSegments("/favicon.ico", StringComparison.OrdinalIgnoreCase));
        }

        private string BuildEntrypointUrl(HttpContext context)
        {
            return string.Concat("/entrypoint", context.Request.Path.HydrateEntrypointSegments());
        }
    }
}
