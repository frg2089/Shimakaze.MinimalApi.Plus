using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Metadata;

internal sealed class AuthorizeData
{
    public ExpressionSyntax? Policy { get; }
    public ExpressionSyntax? Roles { get; }
    public ExpressionSyntax? AuthenticationSchemes { get; }

    public AuthorizeData(AttributeData attribute)
    {
        if (attribute.ConstructorArguments.Length is not 0)
            Policy = ParseExpression(attribute.ConstructorArguments[0].ToCSharpString());

        foreach (var item in attribute.NamedArguments)
        {
            switch (item.Key)
            {
                case nameof(Policy):
                    Policy = ParseExpression(item.Value.ToCSharpString());
                    break;
                case nameof(Roles):
                    Roles = ParseExpression(item.Value.ToCSharpString());
                    break;
                case nameof(AuthenticationSchemes):
                    AuthenticationSchemes = ParseExpression(item.Value.ToCSharpString());
                    break;
            }
        }
    }
}
