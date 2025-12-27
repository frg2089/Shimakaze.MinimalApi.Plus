using Microsoft.CodeAnalysis;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static class SeparatedSyntaxListHelper
{
    public static SeparatedSyntaxList<TNode> AsSingleton<TNode>(this TNode node)
        where TNode : SyntaxNode
        => SingletonSeparatedList(node);

    public static SeparatedSyntaxList<TNode> AsSeparatedList<TNode>(this IEnumerable<TNode> nodes)
        where TNode : SyntaxNode
        => SeparatedList(nodes);
}
