namespace Pretendo.Backend.Handlers.Extensions
{
    /// <summary>
    /// Extends WebApp to attach any implementation of IHandler into webApp
    /// </summary>
    public static class WebAppExtensions
    {
        /// <summary>
        /// Gathers all 'IHandler' implementations and attach them into the webApp
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder MapHandlers(this WebApplication app)
        {
            IEnumerable<IHandler> handlers = app.Services.GetRequiredService<IEnumerable<IHandler>>();

            foreach (var handler in handlers)
            {
                handler.MapHandler(app);
            }

            return app;
        }
    }
}
