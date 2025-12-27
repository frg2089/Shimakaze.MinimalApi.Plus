using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal sealed class ServiceProvider(ExpressionSyntax expr)
{
    private readonly ExpressionSyntax _expr = expr;

    private InvocationExpressionSyntax GetService(TypeSyntax type, SyntaxToken methodName)
        => _expr.InvokeMethod(methodName.AsGeneric(type));
    private InvocationExpressionSyntax GetService(TypeSyntax type, SyntaxToken methodName, ExpressionSyntax key)
        => _expr.InvokeMethod(methodName.AsGeneric(type), Argument(key));

    public ExpressionSyntax GetRequiredService(TypeSyntax type)
        => GetService(type, Identifier("GetRequiredService"));
    public ExpressionSyntax GetService(TypeSyntax type)
        => GetService(type, Identifier("GetService"));
    public ExpressionSyntax GetServices(TypeSyntax type)
        => GetService(type, Identifier("GetServices"));

    public ExpressionSyntax GetRequiredKeyedService(TypeSyntax type, ExpressionSyntax key)
        => GetService(type, Identifier("GetRequiredKeyedService"), key);
    public ExpressionSyntax GetKeyedService(TypeSyntax type, ExpressionSyntax key)
        => GetService(type, Identifier("GetKeyedService"), key);
    public ExpressionSyntax GetKeyedServices(TypeSyntax type, ExpressionSyntax key)
        => GetService(type, Identifier("GetKeyedServices"), key);

    public static implicit operator ExpressionSyntax(ServiceProvider provider) => provider._expr;
}
