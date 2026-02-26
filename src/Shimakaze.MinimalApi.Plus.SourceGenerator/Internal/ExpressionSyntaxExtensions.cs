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

    extension(TypedConstant constant)
    {
        /// <summary>
        /// 辅助方法：将 TypedConstant (来自 AttributeData) 转换为 ExpressionSyntax
        /// </summary>
        public ExpressionSyntax CreateExpressionFromTypedConstant()
        {
            if (constant.IsNull)
                return LiteralExpression(SyntaxKind.NullLiteralExpression);

            switch (constant.Kind)
            {
                case TypedConstantKind.Primitive:
                case TypedConstantKind.Enum:
                    // 枚举通常作为 Primitive 处理，但为了安全显式处理
                    // 如果 constant.Value 是 int/long 等，直接转字面量即可，编译器会推断枚举类型
                    // 如果需要显式_cast_，逻辑会更复杂，通常直接输出值即可
                    return CreateLiteralExpression(constant.Value);

                case TypedConstantKind.Type when constant.Value is ITypeSymbol typeSymbol:
                    // 对应 typeof(T)
                    return TypeOfExpression(ParseTypeName(typeSymbol.ToDisplayString()));
                case TypedConstantKind.Type:
                    throw new InvalidOperationException("Invalid type in TypedConstant.");

                case TypedConstantKind.Array:
                    // 处理数组初始化 new[] { ... }
                    var elements = constant.Values.Select(v => v.CreateExpressionFromTypedConstant());

                    // 尝试推断数组元素类型以生成 new type[] { ... } 或 new[] { ... }
                    // 简单起见，这里生成 new[] { ... } (隐式类型数组)，或者你可以解析 constant.Type
                    TypeSyntax arrayType = ParseTypeName(constant.Type?.ToDisplayString() ?? "var");

                    return ArrayCreationExpression(
                        ArrayType(arrayType)
                            .AddRankSpecifiers(ArrayRankSpecifier()), // 空秩表示隐式大小
                        InitializerExpression(SyntaxKind.ArrayInitializerExpression, elements.AsSeparatedList())
                    );

                default:
                    throw new NotSupportedException($"Unsupported TypedConstant kind: {constant.Kind}");
            }
        }
    }

    extension(object? value)
    {
        /// <summary>
        /// 辅助方法：将 object 值转换为 LiteralExpressionSyntax
        /// </summary>
        public LiteralExpressionSyntax CreateLiteralExpression()
        {
            return value switch
            {
                null => LiteralExpression(SyntaxKind.NullLiteralExpression),
                true => LiteralExpression(SyntaxKind.TrueLiteralExpression),
                false => LiteralExpression(SyntaxKind.FalseLiteralExpression),
                int i => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(i)),
                long l => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(l)),
                double d => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(d)),
                float f => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(f)),
                decimal m => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(m)),
                string s => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(s)),// 处理字符串中的特殊字符和转义
                char c => LiteralExpression(SyntaxKind.CharacterLiteralExpression, Literal(c)),
                _ => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value.ToString())),//  fallback 到 ToString()，可能不是有效的 C# 代码，视具体情况处理
            };
        }
    }
}
