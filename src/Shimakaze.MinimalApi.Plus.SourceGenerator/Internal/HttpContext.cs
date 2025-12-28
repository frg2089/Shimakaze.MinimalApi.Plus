using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static class HttpContext
{
    public static ExpressionSyntax RequestAborted { get; } = Constants.HttpContextInstance.GetMember(Constants.RequestAborted);
    public static ExpressionSyntax Request { get; } = Constants.HttpContextInstance.GetMember(Constants.Request);
    public static ExpressionSyntax Response { get; } = Constants.HttpContextInstance.GetMember(Constants.Response);
    public static ExpressionSyntax Connection { get; } = Constants.HttpContextInstance.GetMember(Constants.Connection);
    public static ExpressionSyntax WebSockets { get; } = Constants.HttpContextInstance.GetMember(Constants.WebSockets);
    public static ExpressionSyntax User { get; } = Constants.HttpContextInstance.GetMember(Constants.User);
    public static ServiceProvider RequestServices { get; } = new(Constants.HttpContextInstance.GetMember(Constants.RequestServices));
    public static ExpressionSyntax Session { get; } = Constants.HttpContextInstance.GetMember(Constants.Session);
}
