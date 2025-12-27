namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static class KnownTypes
{
    public const string CommonNamespace = "global::Shimakaze.MinimalApi.Plus";
    public const string ApiEndpointsHelper = $"{CommonNamespace}.ApiEndpointsHelper";
    public const string ApiEndpoints = $"{CommonNamespace}.ApiEndpoints";
    public const string ApiEndpointsAttribute = $"{CommonNamespace}.ApiEndpointsAttribute";

    public const string HttpContext = "global::Microsoft.AspNetCore.Http.HttpContext";
    public const string CancellationToken = "global::System.Threading.CancellationToken";
    public const string HttpRequest = "global::Microsoft.AspNetCore.Http.HttpRequest";
    public const string HttpResponse = "global::Microsoft.AspNetCore.Http.HttpResponse";
    public const string ConnectionInfo = "global::Microsoft.AspNetCore.Http.ConnectionInfo";
    public const string WebSocketManager = "global::Microsoft.AspNetCore.Http.WebSocketManager";
    public const string ClaimsPrincipal = "global::Microsoft.AspNetCore.Http.ClaimsPrincipal";
    public const string ISession = "global::Microsoft.AspNetCore.Http.ISession";

    public const string IServiceProvider = "global::System.IServiceProvider";
    public const string IServiceCollection = "global::Microsoft.Extensions.DependencyInjection.IServiceCollection";
    public const string ActivatorUtilities = "global::Microsoft.Extensions.DependencyInjection.ActivatorUtilities";

    public const string HttpMethods = "global::Microsoft.AspNetCore.Http.HttpMethods";
    public const string IResult = "global::Microsoft.AspNetCore.Http.IResult";
    public const string TypedResults = "global::Microsoft.AspNetCore.Http.TypedResults";

    public const string IOptions = "global::Microsoft.Extensions.Options.IOptions";

    public const string JsonSerializer = "global::System.Text.Json.JsonSerializer";
    public const string JsonSerializerOptions = "global::System.Text.Json.JsonSerializerOptions";

    public const string Task = "global::System.Threading.Tasks.Task";
    public const string ValueTask = "global::System.Threading.Tasks.ValueTask";

    public const string MethodInfo = "global::System.Reflection.MethodInfo";
    public const string Debug = "global::System.Diagnostics.Debug";
    public const string ArgumentNullException = "global::System.ArgumentNullException";

    public const string IEndpointRouteBuilder = "global::Microsoft.AspNetCore.Routing.IEndpointRouteBuilder";

}
