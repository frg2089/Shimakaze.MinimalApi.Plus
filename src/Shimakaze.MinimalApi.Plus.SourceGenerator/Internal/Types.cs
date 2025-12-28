using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static class Types
{
    public static TypeSyntax HttpMethodsType { get; } = SyntaxFactory.ParseTypeName(KnownTypes.HttpMethods);
    public static TypeSyntax Debug { get; } = SyntaxFactory.ParseTypeName(KnownTypes.Debug);
    public static TypeSyntax ArgumentNullException { get; } = SyntaxFactory.ParseTypeName(KnownTypes.ArgumentNullException);
    public static TypeSyntax TypedResults { get; } = SyntaxFactory.ParseTypeName(KnownTypes.TypedResults);
    public static TypeSyntax IEndpointRouteBuilder { get; } = SyntaxFactory.ParseTypeName(KnownTypes.IEndpointRouteBuilder);
    public static TypeSyntax EndpointsHelper { get; } = SyntaxFactory.ParseTypeName(KnownTypes.ApiEndpointsHelper);
    public static TypeSyntax IServiceCollection { get; } = SyntaxFactory.ParseTypeName(KnownTypes.IServiceCollection);
    public static TypeSyntax ActivatorUtilities { get; } = SyntaxFactory.ParseTypeName(KnownTypes.ActivatorUtilities);
    public static TypeSyntax Authorize { get; } = SyntaxFactory.ParseTypeName(KnownAttributeTypes.Authorize);
    public static TypeSyntax Produces { get; } = SyntaxFactory.ParseTypeName(KnownAttributeTypes.Produces);
    public static TypeSyntax ProducesResponseType { get; } = SyntaxFactory.ParseTypeName(KnownAttributeTypes.ProducesResponseType);
}
