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

        public InvocationExpressionSyntax Invoke(IEnumerable<ArgumentSyntax> arguments)
        {
            var expr = method.Invoke();

            if (arguments is not null)
                expr = expr.WithArgumentList(ArgumentList(arguments.OfType<ArgumentSyntax>().AsSeparatedList()));

            return expr;
        }

        public InvocationExpressionSyntax Invoke(ArgumentSyntax argument)
            => method.Invoke(argument.AsSingleton());

        public AwaitExpressionSyntax InvokeAsync()
            => method.Invoke().Await();

        public AwaitExpressionSyntax InvokeAsync(SeparatedSyntaxList<ArgumentSyntax> arguments)
            => method.Invoke(arguments).Await();

        public AwaitExpressionSyntax InvokeAsync(IEnumerable<ArgumentSyntax> arguments)
            => method.Invoke(arguments).Await();

        public AwaitExpressionSyntax InvokeAsync(ArgumentSyntax argument)
            => method.Invoke(argument).Await();
    }

    extension(ExpressionSyntax expr)
    {
        public AwaitExpressionSyntax Await()
            => AwaitExpression(expr.WithLeadingTrivia(Space));

        public ParenthesizedExpressionSyntax Parenthesized()
            => ParenthesizedExpression(expr);

        public PostfixUnaryExpressionSyntax NotNullAssert()
            => PostfixUnaryExpression(SyntaxKind.SuppressNullableWarningExpression, expr);

        public BinaryExpressionSyntax Coalesce(ExpressionSyntax next)
            => BinaryExpression(
                SyntaxKind.CollectionExpression,
                expr,
                next);

        public AssignmentExpressionSyntax Assignment(ExpressionSyntax right)
            => AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                expr,
                right);

        public AssignmentExpressionSyntax CoalesceAssignment(ExpressionSyntax right)
            => AssignmentExpression(
                SyntaxKind.CoalesceAssignmentExpression,
                expr,
                right);
    }

    extension(ExpressionSyntax type)
    {
        public MemberAccessExpressionSyntax GetMember(SimpleNameSyntax member)
            => MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                type,
                member);

        public InvocationExpressionSyntax InvokeMethod(SimpleNameSyntax methodName, SeparatedSyntaxList<ArgumentSyntax> arguments)
            => type
                .GetMember(methodName)
                .Invoke(arguments);

        public InvocationExpressionSyntax InvokeMethod(SimpleNameSyntax methodName, IEnumerable<ArgumentSyntax> arguments)
            => type
                .GetMember(methodName)
                .Invoke(arguments);

        public InvocationExpressionSyntax InvokeMethod(SimpleNameSyntax methodName, ArgumentSyntax argument)
            => type
                .GetMember(methodName)
                .Invoke(argument);

        public InvocationExpressionSyntax InvokeMethod(SimpleNameSyntax methodName)
            => type
                .GetMember(methodName)
                .Invoke();
    }

    extension(LambdaExpressionSyntax lambda)
    {
        public LambdaExpressionSyntax WithAsync()
            => lambda.WithAsyncKeyword(SyntaxKind.AsyncKeyword.Token);

        public LambdaExpressionSyntax WithStatic()
            => lambda.AddModifiers(SyntaxKind.StaticKeyword.Token);
    }

    extension(SyntaxKind kind)
    {
        public SyntaxToken Token => Token(kind);
    }

    extension(SyntaxToken token)
    {
        public LiteralExpressionSyntax AsString()
            => LiteralExpression(SyntaxKind.StringLiteralExpression, token);
    }

    extension(GenericNameSyntax generic)
    {
        public GenericNameSyntax WithType(SeparatedSyntaxList<TypeSyntax> types)
            => generic.WithTypeArgumentList(TypeArgumentList(types));

        public GenericNameSyntax WithType(IEnumerable<TypeSyntax> types)
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
