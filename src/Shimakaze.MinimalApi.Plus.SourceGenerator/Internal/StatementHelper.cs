using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static class StatementHelper
{
    public static StatementSyntax AsStatement(this VariableDeclarationSyntax variableDeclaration)
        => LocalDeclarationStatement(variableDeclaration);
    public static StatementSyntax AsStatement(this ExpressionSyntax expression)
        => ExpressionStatement(expression);
}
