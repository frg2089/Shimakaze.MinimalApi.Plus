using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static class ExpressionSyntaxExtensions
{
    extension(ExpressionSyntax method)
    {
        public InvocationExpressionSyntax Invoke()
            => InvocationExpression(method);

        public InvocationExpressionSyntax Invoke(SeparatedSyntaxList<ArgumentSyntax> arguments)
            => method.Invoke().WithArgumentList(ArgumentList(arguments));

        public InvocationExpressionSyntax Invoke(params IEnumerable<ArgumentSyntax>? arguments)
        {
            var expr = method.Invoke();

            if (arguments is not null)
                expr = expr.WithArgumentList(ArgumentList(arguments.OfType<ArgumentSyntax>().AsSeparatedList()));

            return expr;
        }

        public InvocationExpressionSyntax Invoke(ArgumentSyntax argument)
            => method.Invoke(argument.AsSingleton());

        public ExpressionSyntax InvokeAsync()
            => method.Invoke().Await();

        public ExpressionSyntax InvokeAsync(SeparatedSyntaxList<ArgumentSyntax> arguments)
            => method.Invoke(arguments).Await();

        public ExpressionSyntax InvokeAsync(params IEnumerable<ArgumentSyntax>? arguments)
            => method.Invoke(arguments).Await();

        public ExpressionSyntax InvokeAsync(ArgumentSyntax argument)
            => method.Invoke(argument).Await();
    }

    extension(ExpressionSyntax expr)
    {
        public ExpressionSyntax Await()
            => AwaitExpression(expr.WithLeadingTrivia(Space));

        public ExpressionSyntax Parenthesized()
            => ParenthesizedExpression(expr);

        public ExpressionSyntax NotNullAssert()
            => PostfixUnaryExpression(SyntaxKind.SuppressNullableWarningExpression, expr);

        public ExpressionSyntax Coalesce(ExpressionSyntax next)
            => BinaryExpression(
                SyntaxKind.CollectionExpression,
                expr,
                next);
    }

    extension(ExpressionSyntax type)
    {
        public ExpressionSyntax GetMember(SimpleNameSyntax member)
            => MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                type,
                member);

        public InvocationExpressionSyntax InvokeMethod(SimpleNameSyntax methodName, SeparatedSyntaxList<ArgumentSyntax> arguments)
            => type
                .GetMember(methodName)
                .Invoke(arguments);

        public InvocationExpressionSyntax InvokeMethod(SimpleNameSyntax methodName, params IEnumerable<ArgumentSyntax>? arguments)
            => type
                .GetMember(methodName)
                .Invoke(arguments);

        public InvocationExpressionSyntax InvokeMethod(SimpleNameSyntax methodName, ArgumentSyntax argument)
            => type
                .GetMember(methodName)
                .Invoke(argument);
    }

    extension(LambdaExpressionSyntax lambda)
    {
        public LambdaExpressionSyntax WithAsync()
            => lambda.WithAsyncKeyword(SyntaxKind.AsyncKeyword.Token);
    }

    extension(SyntaxKind kind)
    {
        public SyntaxToken Token => Token(kind);
    }

    extension(SyntaxToken token)
    {
        public ExpressionSyntax AsString()
            => LiteralExpression(SyntaxKind.StringLiteralExpression, token);
    }

    extension(GenericNameSyntax generic)
    {
        public GenericNameSyntax WithType(SeparatedSyntaxList<TypeSyntax> types)
            => generic.WithTypeArgumentList(TypeArgumentList(types));

        public GenericNameSyntax WithType(params IEnumerable<TypeSyntax> types)
            => generic.WithType(types.AsSeparatedList());

        public GenericNameSyntax WithType(TypeSyntax type)
            => generic.WithType(type.AsSingleton());
    }

    extension(TypeSyntax type)
    {
        public TypeSyntax Nullable()
        {
            if (type is not NullableTypeSyntax)
                type = NullableType(type);

            return type;
        }
    }
}
