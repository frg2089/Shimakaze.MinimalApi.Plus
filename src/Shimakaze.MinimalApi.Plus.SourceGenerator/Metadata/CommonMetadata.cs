using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Metadata;

internal abstract class CommonMetadata
{
    public ExpressionSyntax? EndpointName { get; private set; }
    public ExpressionSyntax? Group { get; private set; }
    public string? Template { get; private set; }
    public ImmutableArray<ExpressionSyntax>? Tags { get; private set; }
    public ImmutableArray<ExpressionSyntax>? Hosts { get; private set; }

    public bool? Authorize { get; private set; }
    public bool? ValidateAntiForgeryToken { get; private set; }
    public bool? IgnoreAntiforgeryToken { get; private set; }
    public bool? Hidden { get; private set; }
    public bool? AllowAnonymous { get; private set; }
    public bool? DisableHttpMetrics { get; private set; }

    protected virtual void ParseAttribute(AttributeData attribute)
    {
        switch (attribute.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
        {
            case KnownAttributeTypes.ApiExplorerSettings:
                foreach (var item in attribute.NamedArguments)
                {
                    switch (item.Key)
                    {
                        case "GroupName":
                            Group = ParseExpression(item.Value.ToCSharpString());
                            break;
                        case "IgnoreApi":
                            Hidden = item.Value.Value is true;
                            break;
                    }
                }
                break;
            case KnownAttributeTypes.ExcludeFromDescription:
                Hidden = true;
                break;
            case KnownAttributeTypes.EndpointName:
                EndpointName = ParseExpression(attribute.ConstructorArguments.First().ToCSharpString());
                break;
            case KnownAttributeTypes.EndpointGroupName:
                Group = ParseExpression(attribute.ConstructorArguments.First().ToCSharpString());
                break;
            case KnownAttributeTypes.Tags:
                Tags = [.. attribute.ConstructorArguments.First().Values.Select(i => ParseExpression(i.ToCSharpString()))];
                break;
            case KnownAttributeTypes.Host:
                {
                    var tmp = attribute.ConstructorArguments.First();
                    if (tmp.Kind is TypedConstantKind.Array)
                        Hosts = [.. tmp.Values.Select(i => ParseExpression(i.ToCSharpString()))];
                    else if (tmp.Value is string host)
                        Hosts = [ParseExpression(host)];
                }
                break;
            case KnownAttributeTypes.Authorize:
                Authorize = true;
                break;
            case KnownAttributeTypes.ValidateAntiForgeryToken:
                ValidateAntiForgeryToken = true;
                break;
            case KnownAttributeTypes.IgnoreAntiforgeryToken:
                IgnoreAntiforgeryToken = true;
                break;
            case KnownAttributeTypes.AllowAnonymous:
                AllowAnonymous = true;
                break;
            case KnownAttributeTypes.DisableHttpMetrics:
                DisableHttpMetrics = true;
                break;
            case KnownAttributeTypes.Route:
                // 它不该是表达式
                Template = attribute.ConstructorArguments.First().Value as string;
                break;
            case KnownAttributeTypes.Produces:
                // TODO: Produces
                // [Produces("application/json")]
                // [Produces("application/xml")]
                // ---
                // endpoint.Produces("application/json", "application/xml");
                break;
            case KnownAttributeTypes.ProducesResponseType:
                // TODO: ProducesResponseType
                // [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
                // ---
                // endpoint.Produces<User>(StatusCodes.Status200OK);

                // [ProducesResponseType(StatusCodes.Status404NotFound)]
                // ---
                // endpoint.Produces(StatusCodes.Status404NotFound);
                break;
        }
    }
}