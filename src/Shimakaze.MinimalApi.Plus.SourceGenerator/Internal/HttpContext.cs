using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static class HttpContext
{
    public static SyntaxToken Token { get; } = Identifier("context");
    public static IdentifierNameSyntax Name { get; } = IdentifierName(Token);
    public static ExpressionSyntax RequestAborted { get; } = Name.GetMember(IdentifierName(nameof(RequestAborted)));
    public static ExpressionSyntax Request { get; } = Name.GetMember(IdentifierName(nameof(Request)));
    public static ExpressionSyntax Response { get; } = Name.GetMember(IdentifierName(nameof(Response)));
    public static ExpressionSyntax Connection { get; } = Name.GetMember(IdentifierName(nameof(Connection)));
    public static ExpressionSyntax WebSockets { get; } = Name.GetMember(IdentifierName(nameof(WebSockets)));
    public static ExpressionSyntax User { get; } = Name.GetMember(IdentifierName(nameof(User)));
    public static ServiceProvider RequestServices { get; } = new(Name.GetMember(IdentifierName(nameof(RequestServices))));
    public static ExpressionSyntax Session { get; } = Name.GetMember(IdentifierName(nameof(Session)));
}
