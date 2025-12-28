using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal sealed class ServiceProvider(ExpressionSyntax expr)
{
    private readonly ExpressionSyntax _expr = expr;

    private InvocationExpressionSyntax GetService(TypeSyntax type, GenericNameSyntax methodName)
        => _expr.InvokeMethod(methodName.WithType(type));
    private InvocationExpressionSyntax GetService(TypeSyntax type, GenericNameSyntax methodName, ExpressionSyntax key)
        => _expr.InvokeMethod(methodName.WithType(type), Argument(key));

    public ExpressionSyntax GetRequiredService(TypeSyntax type)
        => GetService(type, Constants.GetRequiredService);
    public ExpressionSyntax GetService(TypeSyntax type)
        => GetService(type, Constants.GetService);
    public ExpressionSyntax GetServices(TypeSyntax type)
        => GetService(type, Constants.GetServices);

    public ExpressionSyntax GetRequiredKeyedService(TypeSyntax type, ExpressionSyntax? key)
        => GetService(type, Constants.GetRequiredKeyedService, key ?? LiteralExpression(SyntaxKind.NullKeyword));
    public ExpressionSyntax GetKeyedService(TypeSyntax type, ExpressionSyntax? key)
        => GetService(type, Constants.GetKeyedService, key ?? LiteralExpression(SyntaxKind.NullKeyword));
    public ExpressionSyntax GetKeyedServices(TypeSyntax type, ExpressionSyntax? key)
        => GetService(type, Constants.GetKeyedServices, key ?? LiteralExpression(SyntaxKind.NullKeyword));

    public static implicit operator ExpressionSyntax(ServiceProvider provider) => provider._expr;
}
