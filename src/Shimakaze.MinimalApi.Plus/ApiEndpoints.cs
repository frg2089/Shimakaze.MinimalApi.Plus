using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Shimakaze.MinimalApi.Plus;

public abstract class ApiEndpoints
{
    protected internal HttpContext Context { get; set; } = default!;

    protected IFeatureCollection Features => Context.Features;
    protected HttpRequest Request => Context.Request;
    protected HttpResponse Response => Context.Response;
    protected ConnectionInfo Connection => Context.Connection;
    protected WebSocketManager WebSockets => Context.WebSockets;
    protected ClaimsPrincipal User => Context.User;
    protected IServiceProvider RequestServices => Context.RequestServices;
    protected CancellationToken RequestAborted => Context.RequestAborted;
    protected string TraceIdentifier => Context.TraceIdentifier;
    protected ISession Session => Context.Session;
}
