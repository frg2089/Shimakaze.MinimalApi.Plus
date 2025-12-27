using System.ComponentModel;

using Microsoft.AspNetCore.Http;

namespace Shimakaze.MinimalApi.Plus;

public static class ApiEndpointsExtensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static TEndpoints SetContext<TEndpoints>(this TEndpoints endpoints, HttpContext context)
        where TEndpoints : ApiEndpoints
    {
        endpoints.Context = context;
        return endpoints;
    }
}