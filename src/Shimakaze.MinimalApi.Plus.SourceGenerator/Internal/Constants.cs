using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static class Constants
{
    public static readonly SyntaxToken IEndpointRouteBuilderInstanceToken = Identifier("route");
    public static readonly SyntaxToken IServiceCollectionInstanceToken = Identifier("service");
    public static readonly SyntaxToken HttpContextInstanceToken = Identifier("context");
    public static readonly SyntaxToken ThisInstanceToken = Identifier("__this_instance");
    public static readonly SyntaxToken ReturnValueInstanceToken = Identifier("__return_value");
    public static readonly SyntaxToken MethodInfoInstanceToken = Identifier("__method_info");

    public static readonly IdentifierNameSyntax IEndpointRouteBuilderInstance = IdentifierName(IEndpointRouteBuilderInstanceToken);
    public static readonly IdentifierNameSyntax IServiceCollectionInstance = IdentifierName(IServiceCollectionInstanceToken);
    public static readonly IdentifierNameSyntax HttpContextInstance = IdentifierName(HttpContextInstanceToken);
    public static readonly IdentifierNameSyntax ThisInstance = IdentifierName(ThisInstanceToken);
    public static readonly IdentifierNameSyntax ReturnValueInstance = IdentifierName(ReturnValueInstanceToken);
    public static readonly IdentifierNameSyntax MethodInfoInstance = IdentifierName(MethodInfoInstanceToken);

    public static readonly SyntaxToken GeneratedEndpointsToken = Identifier("GeneratedEndpoints");
    public static readonly SyntaxToken AddEndpointsToken = Identifier("AddEndpoints");
    public static readonly SyntaxToken MapEndpointsToken = Identifier("MapEndpoints");

    public static readonly IdentifierNameSyntax GetMethod = IdentifierName("GetMethod");
    public static readonly IdentifierNameSyntax SetContext = IdentifierName("SetContext");
    public static readonly IdentifierNameSyntax ExecuteAsync = IdentifierName("ExecuteAsync");
    public static readonly IdentifierNameSyntax MapMethods = IdentifierName("MapMethods");
    public static readonly IdentifierNameSyntax WithMetadata = IdentifierName("WithMetadata");
    public static readonly IdentifierNameSyntax WithSummary = IdentifierName("WithSummary");
    public static readonly IdentifierNameSyntax WithDescription = IdentifierName("WithDescription");

    public static readonly IdentifierNameSyntax RequireAntiforgery = IdentifierName("RequireAntiforgery");
    public static readonly IdentifierNameSyntax DisableAntiforgery = IdentifierName("DisableAntiforgery");
    public static readonly IdentifierNameSyntax ExcludeFromDescription = IdentifierName("ExcludeFromDescription");
    public static readonly IdentifierNameSyntax DisableHttpMetrics = IdentifierName("DisableHttpMetrics");
    public static readonly IdentifierNameSyntax WithName = IdentifierName("WithName");
    public static readonly IdentifierNameSyntax WithGroupName = IdentifierName("WithGroupName");
    public static readonly IdentifierNameSyntax WithTags = IdentifierName("WithTags");
    public static readonly IdentifierNameSyntax RequireHost = IdentifierName("RequireHost");
    public static readonly IdentifierNameSyntax RequireAuthorization = IdentifierName("RequireAuthorization");
    public static readonly IdentifierNameSyntax AllowAnonymous = IdentifierName("AllowAnonymous");

    public static readonly IdentifierNameSyntax ToArray = IdentifierName("ToArray");
    public static readonly IdentifierNameSyntax Ok = IdentifierName("Ok");
    public static readonly IdentifierNameSyntax RouteValues = IdentifierName("RouteValues");
    public static readonly IdentifierNameSyntax Form = IdentifierName("Form");
    public static readonly IdentifierNameSyntax Headers = IdentifierName("Headers");
    public static readonly IdentifierNameSyntax Query = IdentifierName("Query");
    public static readonly IdentifierNameSyntax Assert = IdentifierName("Assert");
    public static readonly IdentifierNameSyntax ThrowIfNull = IdentifierName("ThrowIfNull");
    public static readonly IdentifierNameSyntax RequestAborted = IdentifierName("RequestAborted");
    public static readonly IdentifierNameSyntax Request = IdentifierName("Request");
    public static readonly IdentifierNameSyntax Response = IdentifierName("Response");
    public static readonly IdentifierNameSyntax Connection = IdentifierName("Connection");
    public static readonly IdentifierNameSyntax WebSockets = IdentifierName("WebSockets");
    public static readonly IdentifierNameSyntax User = IdentifierName("User");
    public static readonly IdentifierNameSyntax RequestServices = IdentifierName("RequestServices");
    public static readonly IdentifierNameSyntax Session = IdentifierName("Session");

    public static readonly GenericNameSyntax AddScoped = GenericName("AddScoped");
    public static readonly GenericNameSyntax CreateInstance = GenericName("CreateInstance");
    public static readonly GenericNameSyntax ConvertEnumerableTo = GenericName("ConvertEnumerableTo");
    public static readonly GenericNameSyntax ConvertTo = GenericName("ConvertTo");
    public static readonly GenericNameSyntax ReadFromJsonAsync = GenericName("ReadFromJsonAsync");
    public static readonly GenericNameSyntax GetRequiredService = GenericName("GetRequiredService");
    public static readonly GenericNameSyntax GetService = GenericName("GetService");
    public static readonly GenericNameSyntax GetServices = GenericName("GetServices");
    public static readonly GenericNameSyntax GetRequiredKeyedService = GenericName("GetRequiredKeyedService");
    public static readonly GenericNameSyntax GetKeyedService = GenericName("GetKeyedService");
    public static readonly GenericNameSyntax GetKeyedServices = GenericName("GetKeyedServices");
    public static readonly GenericNameSyntax GetCustomAttributes = GenericName("GetCustomAttributes");
}
