using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static partial class Constants
{
    public static readonly LiteralExpressionSyntax True = LiteralExpression(SyntaxKind.TrueLiteralExpression);
    public static readonly LiteralExpressionSyntax False = LiteralExpression(SyntaxKind.FalseLiteralExpression);

    public static readonly SyntaxToken IEndpointRouteBuilderInstanceToken = Identifier("route");
    public static readonly SyntaxToken IServiceCollectionInstanceToken = Identifier("service");
    public static readonly SyntaxToken HttpContextInstanceToken = Identifier("context");
    public static readonly SyntaxToken ThisInstanceToken = Identifier("__this_instance");
    public static readonly SyntaxToken ReturnValueInstanceToken = Identifier("__return_value");
    public static readonly SyntaxToken MethodInfoInstanceToken = Identifier("__method_info");
    public static readonly SyntaxToken OpenApiOperationInstanceToken = Identifier("operation");
    public static readonly SyntaxToken OpenApiOperationTransformerContextInstanceToken = Identifier("context");
    public static readonly SyntaxToken CancellationTokenInstanceToken = Identifier("cancellationToken");

    public static readonly IdentifierNameSyntax IEndpointRouteBuilderInstance = IdentifierName(IEndpointRouteBuilderInstanceToken);
    public static readonly IdentifierNameSyntax IServiceCollectionInstance = IdentifierName(IServiceCollectionInstanceToken);
    public static readonly IdentifierNameSyntax HttpContextInstance = IdentifierName(HttpContextInstanceToken);
    public static readonly IdentifierNameSyntax ThisInstance = IdentifierName(ThisInstanceToken);
    public static readonly IdentifierNameSyntax ReturnValueInstance = IdentifierName(ReturnValueInstanceToken);
    public static readonly IdentifierNameSyntax MethodInfoInstance = IdentifierName(MethodInfoInstanceToken);
    public static readonly IdentifierNameSyntax OpenApiOperationInstance = IdentifierName(OpenApiOperationInstanceToken);
    public static readonly IdentifierNameSyntax OpenApiOperationTransformerContextInstance = IdentifierName(OpenApiOperationTransformerContextInstanceToken);
    public static readonly IdentifierNameSyntax CancellationTokenInstance = IdentifierName(CancellationTokenInstanceToken);

    public static readonly SyntaxToken GeneratedEndpointsToken = Identifier("GeneratedEndpoints");
    public static readonly SyntaxToken AddEndpointsToken = Identifier("AddEndpoints");
    public static readonly SyntaxToken MapEndpointsToken = Identifier("MapEndpoints");

    public static readonly IdentifierNameSyntax NameOf = IdentifierName("nameof");

    public static readonly IdentifierNameSyntax GetMethod = IdentifierName(nameof(GetMethod));
    public static readonly IdentifierNameSyntax SetContext = IdentifierName(nameof(SetContext));
    public static readonly IdentifierNameSyntax ExecuteAsync = IdentifierName(nameof(ExecuteAsync));
    public static readonly IdentifierNameSyntax MapMethods = IdentifierName(nameof(MapMethods));
    public static readonly IdentifierNameSyntax WithMetadata = IdentifierName(nameof(WithMetadata));
    public static readonly IdentifierNameSyntax WithSummary = IdentifierName(nameof(WithSummary));
    public static readonly IdentifierNameSyntax WithDescription = IdentifierName(nameof(WithDescription));
    public static readonly IdentifierNameSyntax RequireAntiforgery = IdentifierName(nameof(RequireAntiforgery));
    public static readonly IdentifierNameSyntax DisableAntiforgery = IdentifierName(nameof(DisableAntiforgery));
    public static readonly IdentifierNameSyntax ExcludeFromDescription = IdentifierName(nameof(ExcludeFromDescription));
    public static readonly IdentifierNameSyntax DisableHttpMetrics = IdentifierName(nameof(DisableHttpMetrics));
    public static readonly IdentifierNameSyntax WithName = IdentifierName(nameof(WithName));
    public static readonly IdentifierNameSyntax WithGroupName = IdentifierName(nameof(WithGroupName));
    public static readonly IdentifierNameSyntax WithTags = IdentifierName(nameof(WithTags));
    public static readonly IdentifierNameSyntax RequireHost = IdentifierName(nameof(RequireHost));
    public static readonly IdentifierNameSyntax RequireAuthorization = IdentifierName(nameof(RequireAuthorization));
    public static readonly IdentifierNameSyntax AllowAnonymous = IdentifierName(nameof(AllowAnonymous));

    public static readonly IdentifierNameSyntax GetOrCreateSchemaAsync = IdentifierName(nameof(GetOrCreateSchemaAsync));
    public static readonly IdentifierNameSyntax Document = IdentifierName(nameof(Document));
    public static readonly IdentifierNameSyntax AddComponent = IdentifierName(nameof(AddComponent));
    public static readonly IdentifierNameSyntax Json = IdentifierName(nameof(Json));
    public static readonly IdentifierNameSyntax AddOpenApiOperationTransformer = IdentifierName(nameof(AddOpenApiOperationTransformer));
    public static readonly IdentifierNameSyntax Deprecated = IdentifierName(nameof(Deprecated));
    public static readonly IdentifierNameSyntax Parameters = IdentifierName(nameof(Parameters));
    public static readonly IdentifierNameSyntax Security = IdentifierName(nameof(Security));
    public static readonly IdentifierNameSyntax RequestBody = IdentifierName(nameof(RequestBody));
    public static readonly IdentifierNameSyntax Add = IdentifierName(nameof(Add));
    public static readonly IdentifierNameSyntax Name = IdentifierName(nameof(Name));
    public static readonly IdentifierNameSyntax Description = IdentifierName(nameof(Description));
    public static readonly IdentifierNameSyntax Explode = IdentifierName(nameof(Explode));
    public static readonly IdentifierNameSyntax AllowEmptyValue = IdentifierName(nameof(AllowEmptyValue));
    public static readonly IdentifierNameSyntax Required = IdentifierName(nameof(Required));
    public static readonly IdentifierNameSyntax Content = IdentifierName(nameof(Content));
    public static readonly IdentifierNameSyntax In = IdentifierName(nameof(In));
    public static readonly IdentifierNameSyntax Style = IdentifierName(nameof(Style));
    public static readonly IdentifierNameSyntax Header = IdentifierName(nameof(Header));
    public static readonly IdentifierNameSyntax Path = IdentifierName(nameof(Path));
    public static readonly IdentifierNameSyntax Simple = IdentifierName(nameof(Simple));
    public static readonly IdentifierNameSyntax Schema = IdentifierName(nameof(Schema));
    public static readonly IdentifierNameSyntax Type = IdentifierName(nameof(Type));
    public static readonly IdentifierNameSyntax Items = IdentifierName(nameof(Items));
    public static readonly IdentifierNameSyntax Array = IdentifierName(nameof(Array));

    public static readonly IdentifierNameSyntax ToArray = IdentifierName(nameof(ToArray));
    public static readonly IdentifierNameSyntax Ok = IdentifierName(nameof(Ok));
    public static readonly IdentifierNameSyntax RouteValues = IdentifierName(nameof(RouteValues));
    public static readonly IdentifierNameSyntax Form = IdentifierName(nameof(Form));
    public static readonly IdentifierNameSyntax Headers = IdentifierName(nameof(Headers));
    public static readonly IdentifierNameSyntax Query = IdentifierName(nameof(Query));
    public static readonly IdentifierNameSyntax Assert = IdentifierName(nameof(Assert));
    public static readonly IdentifierNameSyntax ThrowIfNull = IdentifierName(nameof(ThrowIfNull));
    public static readonly IdentifierNameSyntax RequestAborted = IdentifierName(nameof(RequestAborted));
    public static readonly IdentifierNameSyntax Request = IdentifierName(nameof(Request));
    public static readonly IdentifierNameSyntax Response = IdentifierName(nameof(Response));
    public static readonly IdentifierNameSyntax Connection = IdentifierName(nameof(Connection));
    public static readonly IdentifierNameSyntax WebSockets = IdentifierName(nameof(WebSockets));
    public static readonly IdentifierNameSyntax User = IdentifierName(nameof(User));
    public static readonly IdentifierNameSyntax RequestServices = IdentifierName(nameof(RequestServices));
    public static readonly IdentifierNameSyntax Session = IdentifierName(nameof(Session));

    public static readonly GenericNameSyntax AddScoped = GenericName(nameof(AddScoped));
    public static readonly GenericNameSyntax CreateInstance = GenericName(nameof(CreateInstance));
    public static readonly GenericNameSyntax ConvertEnumerableTo = GenericName(nameof(ConvertEnumerableTo));
    public static readonly GenericNameSyntax ConvertTo = GenericName(nameof(ConvertTo));
    public static readonly GenericNameSyntax ReadFromJsonAsync = GenericName(nameof(ReadFromJsonAsync));
    public static readonly GenericNameSyntax GetRequiredService = GenericName(nameof(GetRequiredService));
    public static readonly GenericNameSyntax GetService = GenericName(nameof(GetService));
    public static readonly GenericNameSyntax GetServices = GenericName(nameof(GetServices));
    public static readonly GenericNameSyntax GetRequiredKeyedService = GenericName(nameof(GetRequiredKeyedService));
    public static readonly GenericNameSyntax GetKeyedService = GenericName(nameof(GetKeyedService));
    public static readonly GenericNameSyntax GetKeyedServices = GenericName(nameof(GetKeyedServices));
    public static readonly GenericNameSyntax GetCustomAttributes = GenericName(nameof(GetCustomAttributes));

    public static readonly TypeSyntax MediaTypeNames_Application = ParseTypeName(KnownTypes.MediaTypeNames_Application);

    public static readonly TypeSyntax HttpMethodsType = ParseTypeName(KnownTypes.HttpMethods);
    public static readonly TypeSyntax Debug = ParseTypeName(KnownTypes.Debug);
    public static readonly TypeSyntax ArgumentNullException = ParseTypeName(KnownTypes.ArgumentNullException);
    public static readonly TypeSyntax TypedResults = ParseTypeName(KnownTypes.TypedResults);
    public static readonly TypeSyntax IEndpointRouteBuilder = ParseTypeName(KnownTypes.IEndpointRouteBuilder);
    public static readonly TypeSyntax EndpointsHelper = ParseTypeName(KnownTypes.ApiEndpointsHelper);
    public static readonly TypeSyntax IServiceCollection = ParseTypeName(KnownTypes.IServiceCollection);
    public static readonly TypeSyntax ActivatorUtilities = ParseTypeName(KnownTypes.ActivatorUtilities);
    public static readonly TypeSyntax AuthorizeAttribute = ParseTypeName(KnownAttributeTypes.Authorize);
    public static readonly TypeSyntax ProducesAttribute = ParseTypeName(KnownAttributeTypes.Produces);
    public static readonly TypeSyntax ProducesResponseTypeAttribute = ParseTypeName(KnownAttributeTypes.ProducesResponseType);
    public static readonly TypeSyntax OpenApiRequestBody = ParseTypeName(KnownTypes.OpenApiRequestBody);
    public static readonly TypeSyntax OpenApiParameter = ParseTypeName(KnownTypes.OpenApiParameter);
    public static readonly TypeSyntax ParameterLocation = ParseTypeName(KnownTypes.ParameterLocation);
    public static readonly TypeSyntax ParameterStyle = ParseTypeName(KnownTypes.ParameterStyle);
    public static readonly TypeSyntax OpenApiSchema = ParseTypeName(KnownTypes.OpenApiSchema);
    public static readonly TypeSyntax OpenApiSchemaReference = ParseTypeName(KnownTypes.OpenApiSchemaReference);
    public static readonly TypeSyntax JsonSchemaType = ParseTypeName(KnownTypes.JsonSchemaType);

    public static readonly TypeSyntax OpenApiMediaType = ParseTypeName(KnownTypes.OpenApiMediaType);
    public static readonly TypeSyntax Dictionary_string_OpenApiMediaType = ParseTypeName($"global::System.Collections.Generic.Dictionary<string, {KnownTypes.OpenApiMediaType}>");
}
