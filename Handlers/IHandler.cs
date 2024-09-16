namespace Pretendo.Backend.Handlers
{
    /// <summary>
    /// Represents an API action that replaces classic Controller objects in favor of minimal APIs.
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// Register new API action for minimal APIs
        /// </summary>
        /// <param name="app"></param>
        void MapHandler(IEndpointRouteBuilder app);
    }
}
