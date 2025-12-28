using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static class SyntaxHelper
{
    private static readonly Lazy<TypeSyntax> Var = new(() => ParseTypeName("var"));

    public static VariableDeclarationSyntax Variable(this ExpressionSyntax value, SyntaxToken identifier, TypeSyntax? type = null)
        => VariableDeclaration(type ?? Var.Value)
        .WithVariables(
            VariableDeclarator(identifier.WithLeadingTrivia(Space))
                .WithInitializer(EqualsValueClause(value))
                .AsSingleton());
}
