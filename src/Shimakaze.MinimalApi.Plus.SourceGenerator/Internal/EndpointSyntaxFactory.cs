using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;
using Shimakaze.MinimalApi.Plus.SourceGenerator.Metadata;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static class EndpointSyntaxFactory
{
    public static IEnumerable<StatementSyntax> GenerateCode(this EndpointMethodInfo endpointInfo, ExpressionSyntax route)
    {
        SyntaxTrivia comment = Comment($"// {endpointInfo.ContainingNamespace}.{endpointInfo.ContainingTypeName}.{endpointInfo.MethodName}");

        IdentifierNameSyntax method = IdentifierName(endpointInfo.MethodName);

        TypeSyntax thisType = ParseTypeName($"{endpointInfo.ContainingNamespace}.{endpointInfo.ContainingTypeName}");
        TypeOfExpressionSyntax typeOfThisType = TypeOfExpression(thisType);

        List<StatementSyntax> debugChecks = new(endpointInfo.Parameters.Count);
        List<StatementSyntax> nullableChecks = new(endpointInfo.Parameters.Count);

        SyntaxToken[] parameterTokens = new SyntaxToken[endpointInfo.Parameters.Count];
        IdentifierNameSyntax[] parameters = new IdentifierNameSyntax[endpointInfo.Parameters.Count];
        ExpressionSyntax[] parameterNames = new ExpressionSyntax[endpointInfo.Parameters.Count];
        TypeSyntax[] parameterTypes = new TypeSyntax[endpointInfo.Parameters.Count];
        StatementSyntax[] defines = new StatementSyntax[endpointInfo.Parameters.Count];

        SyntaxToken thisInstanceToken = Identifier("__this_instance");
        IdentifierNameSyntax thisInstance = IdentifierName(thisInstanceToken);

        SyntaxToken returnValueToken = Identifier("__return_value");
        IdentifierNameSyntax returnValue = IdentifierName(returnValueToken);

        for (int i = 0; i < endpointInfo.Parameters.Count; i++)
        {
            IParameterSymbol symbol = endpointInfo.Parameters[i];
            ITypeSymbol typeSymbol = symbol.Type;
            parameterTokens[i] = Identifier($"__parameter_{symbol.Name}");
            parameters[i] = IdentifierName(parameterTokens[i]);
            parameterNames[i] = Literal(symbol.Name).AsString();
            parameterTypes[i] = ParseTypeName(typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

            TypeSyntax type = parameterTypes[i];

            if (symbol.NullableAnnotation is NullableAnnotation.NotAnnotated && symbol.Type.BaseType?.SpecialType is not SpecialType.System_ValueType)
            {
                debugChecks.Add(DebugAssertIsNotNull(parameters[i]));
                nullableChecks.Add(RuntimeAssertIsNotNull(parameters[i], parameterNames[i]));
            }

            if (symbol.NullableAnnotation is NullableAnnotation.Annotated)
                type = type.Nullable();

            defines[i] = symbol
                .GetInstance(type, parameterNames[i])
                .Variable(parameterTokens[i], type)
                .AsStatement();
        }

        ExpressionSyntax getMethodInfo = typeOfThisType.InvokeMethod(
            IdentifierName("GetMethod"),
            GetMethodArguments(
                Literal(endpointInfo.MethodName).AsString(),
                parameterTypes.Select(TypeOfExpression)))
            .NotNullAssert();

        foreach (IGrouping<string, string>? endpoint in endpointInfo.EndPoints.GroupBy(i => i.Template, i => i.Method))
        {
            List<StatementSyntax> body = [
                ..defines,
                ..debugChecks,
                ..nullableChecks,
            ];

            // global::Microsoft.Extensions.DependencyInjection.ActivatorUtilities.CreateInstance<T>(, context);
            if (!endpointInfo.IsStatic)
            {
                ExpressionSyntax createInstanceExpr = TypeSyntaxes
                    .ActivatorUtilities
                    .InvokeMethod(
                        Identifier("CreateInstance").AsGeneric(thisType),
                        Argument(HttpContext.RequestServices));

                if (endpointInfo.IsEndpoints)
                    createInstanceExpr = createInstanceExpr
                        .InvokeMethod(IdentifierName("SetContext"), Argument(HttpContext.Name));

                body.Add(createInstanceExpr
                    .Variable(thisInstanceToken)
                    .AsStatement());
            }

            ExpressionSyntax call = endpointInfo.IsStatic
                ? thisType
                : thisInstance;

            call = call.InvokeMethod(method, parameters.Select(Argument));
            if (endpointInfo.IsAsync)
                call = call.Await();

            StatementSyntax node = endpointInfo.IsVoid
                ? call.AsStatement()
                : call.Variable(returnValueToken).AsStatement();

            body.Add(node);

            if (!endpointInfo.IsVoid)
                body.Add(GetResult(
                    returnValue,
                    endpointInfo.IsIResult)
                    .InvokeMethod(
                        IdentifierName("ExecuteAsync"),
                        Argument(HttpContext.Name))
                    .Await()
                    .AsStatement());

            LambdaExpressionSyntax lambda =
                SimpleLambdaExpression(
                    Parameter(HttpContext.Token),
                    Block(body.AsSeparatedList()))
                .WithAsync();

            InvocationExpressionSyntax invokeChain = route
                .InvokeMethod(
                    IdentifierName("MapMethods"),
                    Argument(
                        Literal(endpointInfo.CombineTemplate(endpoint.Key)).AsString())!,
                    Argument(
                        CollectionExpression(
                            endpoint
                                .Select(IdentifierName)
                                .Select(TypeSyntaxes.HttpMethodsType.GetMember)
                                .Select(ExpressionElement)
                                .AsSeparatedList<CollectionElementSyntax>()))!,
                    Argument(lambda)!)
                .InvokeMethod(
                    IdentifierName("WithMetadata"),
                    Argument(getMethodInfo));

            if (endpointInfo.Summary is { Length: not 0 })
            {
                invokeChain = invokeChain
                    .InvokeMethod(
                        IdentifierName("WithSummary"),
                        Argument(
                            Literal(endpointInfo.Summary)
                                .AsString()));
            }
            if (endpointInfo.Remarks is { Length: not 0 })
            {
                invokeChain = invokeChain
                    .InvokeMethod(
                        IdentifierName("WithDescription"),
                        Argument(
                            Literal(endpointInfo.Remarks)
                                .AsString()));
            }

            yield return
               ExpressionStatement(invokeChain)
               .WithLeadingTrivia(comment);
        }
    }

    private static IEnumerable<ArgumentSyntax> GetMethodArguments(ExpressionSyntax methodName, IEnumerable<ExpressionSyntax> parameterTypes)
    {
        yield return Argument(methodName);
        if (parameterTypes.Any())
            yield return Argument(
                CollectionExpression(
                    parameterTypes
                        .Select(ExpressionElement)
                        .AsSeparatedList<CollectionElementSyntax>()));
    }

    private static ExpressionSyntax GetInstance(this IParameterSymbol parameter, TypeSyntax type, ExpressionSyntax name)
    {
        ITypeSymbol parameterType = parameter.Type;
        IEnumerable<ExpressionSyntax> fromCodes = ParseFromRequest(parameter, type, name);

        if (fromCodes.Any())
            return fromCodes.Aggregate((a, b) => a.Coalesce(b));
        if (parameterType.IsType(KnownTypes.HttpContext))
            return HttpContext.Name;
        if (parameterType.IsType(KnownTypes.CancellationToken))
            return HttpContext.RequestAborted;
        if (parameterType.IsType(KnownTypes.HttpRequest))
            return HttpContext.Request;
        if (parameterType.IsType(KnownTypes.HttpResponse))
            return HttpContext.Response;
        if (parameterType.IsType(KnownTypes.ConnectionInfo))
            return HttpContext.Connection;
        if (parameterType.IsType(KnownTypes.WebSocketManager))
            return HttpContext.WebSockets;
        if (parameterType.IsType(KnownTypes.ClaimsPrincipal))
            return HttpContext.User;
        if (parameterType.IsType(KnownTypes.IServiceProvider))
            return HttpContext.RequestServices;
        if (parameterType.IsType(KnownTypes.ISession))
            return HttpContext.Session;

        TypeSyntax? itemType = null;
        if (parameterType.AllInterfaces.FirstOrDefault(i => i.SpecialType is SpecialType.System_Collections_Generic_IEnumerator_T) is { } enumerable)
            itemType = ParseTypeName(enumerable.TypeArguments.First().ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

        if (parameter.GetAttribute(KnownAttributeTypes.FromKeyedServices) is { } fromKeyedServices)
        {
            ExpressionSyntax key = ParseExpression(fromKeyedServices.ConstructorArguments.FirstOrDefault().ToCSharpString());

            if (itemType is not null)
                return HttpContext.RequestServices.GetKeyedServices(itemType, key);

            if (type is NullableTypeSyntax)
                return HttpContext.RequestServices.GetKeyedService(type, key);

            return HttpContext.RequestServices.GetRequiredKeyedService(type, key);
        }
        // if (parameter.GetAttribute(Attributes.FromServices) is not { } fromServices)
        if (itemType is not null)
            return HttpContext.RequestServices.GetServices(itemType);

        if (type is NullableTypeSyntax)
            return HttpContext.RequestServices.GetService(type);

        return HttpContext.RequestServices.GetRequiredService(type);
    }

    private static ExpressionSyntax FromRequestSimple(SimpleNameSyntax member, AttributeData attribute, TypeSyntax parameterType, bool isCollection, ExpressionSyntax name)
    {
        string parameterName = attribute.NamedArguments.FirstOrDefault(i => i.Key is "Name").Value.ToCSharpString();
        if (parameterName is not "null")
            name = ParseExpression(parameterName);

        ExpressionSyntax value = HttpContext.Request.GetMember(member);
        value =
            ElementAccessExpression(value)
            .WithArgumentList(
                BracketedArgumentList(
                    SingletonSeparatedList(
                        Argument(name))));

        GenericNameSyntax method = isCollection
            ? GenericName("ConvertEnumerableTo")
            : GenericName("ConvertTo");

        method = method
            .WithTypeArgumentList(
                TypeArgumentList(
                    SingletonSeparatedList(parameterType)));

        ExpressionSyntax expr = TypeSyntaxes.EndpointsHelper
            .GetMember(method);

        expr = expr
            .Invoke()
            .WithArgumentList(
                ArgumentList(
                    SingletonSeparatedList(
                        Argument(value))));

        if (isCollection)
            expr = CollectionExpression(
                SingletonSeparatedList<CollectionElementSyntax>(
                    SpreadElement(expr)));

        return expr;
    }

    private static IEnumerable<ExpressionSyntax> ParseFromRequest(IParameterSymbol parameterSymbol, TypeSyntax type, ExpressionSyntax name)
    {
        if (parameterSymbol.GetAttribute(KnownAttributeTypes.FromBody) is { } fromBody)
        {
            ExpressionSyntax expr = HttpContext.Request
                .InvokeMethod(
                    Identifier("ReadFromJsonAsync")
                        .AsGeneric(type),
                    Argument(
                        HttpContext.RequestAborted))
                .Await()
                .Parenthesized();
            if (type is not NullableTypeSyntax)
                expr = expr.NotNullAssert();

            yield return expr;
        }
        if (parameterSymbol.GetAttribute(KnownAttributeTypes.FromRoute) is { } fromRoute)
        {
            yield return FromRequestSimple(
                IdentifierName("RouteValues"),
                fromRoute,
                type,
                false,
                name);
        }
        if (parameterSymbol.GetAttribute(KnownAttributeTypes.FromForm) is { } fromForm)
        {
            TypeSyntax? itemType = GetCollectionItemType(parameterSymbol);
            yield return FromRequestSimple(
                IdentifierName("Form"),
                fromForm,
                itemType ?? type,
                itemType is not null,
                name);
        }
        if (parameterSymbol.GetAttribute(KnownAttributeTypes.FromHeader) is { } fromHeader)
        {
            TypeSyntax? itemType = GetCollectionItemType(parameterSymbol);
            yield return FromRequestSimple(
                IdentifierName("Headers"),
                fromHeader,
                itemType ?? type,
                itemType is not null,
                name);
        }
        if (parameterSymbol.GetAttribute(KnownAttributeTypes.FromQuery) is { } fromQuery)
        {
            TypeSyntax? itemType = GetCollectionItemType(parameterSymbol);
            yield return FromRequestSimple(
                IdentifierName("Query"),
                fromQuery,
                itemType ?? type,
                itemType is not null,
                name);
        }
    }

    private static TypeSyntax? GetCollectionItemType(IParameterSymbol parameter)
    {
        if (parameter.Type is not INamedTypeSymbol { SpecialType: SpecialType.System_Collections_Generic_IEnumerable_T } symbol)
            return null;

        ITypeSymbol typeSymbol = symbol.TypeArguments.First();

        TypeSyntax type = ParseTypeName(typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
        if (type is not NullableTypeSyntax)
            type = NullableType(type);

        return type;
    }

    public static ExpressionSyntax GetResult(ExpressionSyntax result, bool isIResult)
    {
        if (isIResult)
            return result;

        return TypeSyntaxes
            .TypedResults
            .InvokeMethod(
                IdentifierName("Ok"),
                Argument(result));
    }

    public static StatementSyntax DebugAssertIsNotNull(ExpressionSyntax obj)
    {
        ExpressionSyntax expr = LiteralExpression(SyntaxKind.NullLiteralExpression);
        expr = IsPatternExpression(
            obj.WithTrailingTrivia(Space),
            UnaryPattern(
                ConstantPattern(expr)
                .WithLeadingTrivia(Space))
            .WithLeadingTrivia(Space));

        expr = TypeSyntaxes.Debug
            .InvokeMethod(
                IdentifierName("Assert"),
                Argument(expr));

        return ExpressionStatement(expr);
    }

    public static StatementSyntax RuntimeAssertIsNotNull(ExpressionSyntax parameter, ExpressionSyntax parameterName)
    {
        InvocationExpressionSyntax expr = TypeSyntaxes
            .ArgumentNullException
            .InvokeMethod(
                IdentifierName("ThrowIfNull"),
                Argument(parameter)!,
                Argument(parameterName)!);

        return ExpressionStatement(expr);
    }
}