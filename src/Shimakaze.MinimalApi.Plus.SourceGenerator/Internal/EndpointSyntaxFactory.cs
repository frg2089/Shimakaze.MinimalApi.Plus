using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Shimakaze.MinimalApi.Plus.SourceGenerator.Metadata;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Shimakaze.MinimalApi.Plus.SourceGenerator.Internal.Constants;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static class EndpointSyntaxFactory
{
    public static IEnumerable<StatementSyntax> GenerateCode(this EndpointsMetadata metadata, ExpressionSyntax route)
    {
        List<StatementSyntax> debugChecks = new(metadata.Parameters.Length);
        List<StatementSyntax> nullableChecks = new(metadata.Parameters.Length);

        var defines = new StatementSyntax[metadata.Parameters.Length];

        for (int i = 0; i < metadata.Parameters.Length; i++)
        {
            var parameter = metadata.Parameters[i];

            var type = parameter.Type;

            if (parameter.Nullable is NullableAnnotation.NotAnnotated && !parameter.IsValueType)
            {
                debugChecks.Add(DebugAssertIsNotNull(parameter.Identifier));
                nullableChecks.Add(RuntimeAssertIsNotNull(parameter.Identifier, parameter.NameOf));
            }

            if (parameter.Nullable is NullableAnnotation.Annotated)
                type = type.Nullable();

            defines[i] = GetInstance(parameter, type, parameter.NameOf)
                .Variable(parameter.Token, type)
                .AsStatement();
        }

        var getMethodInfo = metadata.TypeOfType.InvokeMethod(
            GetMethod,
            GetMethodArguments(
                metadata.NameOfMethod,
                metadata.Parameters.Select(i => i.Type).Select(TypeOfExpression)))
            .NotNullAssert()
            .Variable(MethodInfoInstanceToken)
            .AsStatement()
            .WithLeadingTrivia(metadata.Comment);

        foreach (var endpoint in metadata.EndPoints)
        {
            List<StatementSyntax> body = [
                ..defines,
                ..debugChecks,
                ..nullableChecks,
            ];

            // global::Microsoft.Extensions.DependencyInjection.ActivatorUtilities.CreateInstance<T>(, context);
            if (!metadata.IsStatic)
            {
                ExpressionSyntax createInstanceExpr = Types
                    .ActivatorUtilities
                    .InvokeMethod(
                        CreateInstance.WithType(metadata.Type),
                        Argument(HttpContext.RequestServices));

                if (metadata.TypeIsEndpoints)
                    createInstanceExpr = createInstanceExpr
                        .InvokeMethod(SetContext, Argument(HttpContextInstance));

                body.Add(createInstanceExpr
                    .Variable(ThisInstanceToken)
                    .AsStatement());
            }

            ExpressionSyntax call = metadata.IsStatic
                ? metadata.Type
                : ThisInstance;

            call = call.InvokeMethod(metadata.Name, metadata.Parameters.Select(i => i.Identifier).Select(Argument));
            if (metadata.IsAsync)
                call = call.Await();

            var node = metadata.IsVoid
                ? call.AsStatement()
                : call.Variable(ReturnValueInstanceToken).AsStatement();

            body.Add(node);

            if (!metadata.IsVoid)
                body.Add(GetResult(
                    ReturnValueInstance,
                    metadata.IsIResult)
                    .InvokeMethod(
                        ExecuteAsync,
                        Argument(HttpContextInstance))
                    .Await()
                    .AsStatement());

            var lambda =
                SimpleLambdaExpression(
                    Parameter(HttpContextInstanceToken),
                    Block(body.AsSeparatedList()))
                .WithAsync();

            var invokeChain = route
                .InvokeMethod(
                    MapMethods,
                    [
                        Argument(endpoint.Key),
                        Argument(
                            CollectionExpression(
                                endpoint
                                    .Value
                                    .Select(ExpressionElement)
                                    .AsSeparatedList<CollectionElementSyntax>())),
                        Argument(lambda),
                    ])
                .InvokeMethod(
                    WithMetadata,
                    Argument(MethodInfoInstance));

            if (metadata.Summary is not null)
                invokeChain = invokeChain.InvokeMethod(WithSummary, Argument(metadata.Summary));

            if (metadata.Remarks is not null)
                invokeChain = invokeChain.InvokeMethod(WithDescription, Argument(metadata.Remarks));

            if (metadata.ValidateAntiForgeryToken is true)
                invokeChain = invokeChain.InvokeMethod(RequireAntiforgery);

            if (metadata.IgnoreAntiforgeryToken is true)
                invokeChain = invokeChain.InvokeMethod(DisableAntiforgery);

            if (metadata.ExcludeFromDescription is true)
                invokeChain = invokeChain.InvokeMethod(ExcludeFromDescription);

            if (metadata.DisableHttpMetrics is true)
                invokeChain = invokeChain.InvokeMethod(DisableHttpMetrics);

            if (metadata.EndpointName is not null)
                invokeChain = invokeChain.InvokeMethod(WithName, Argument(metadata.EndpointName));

            if (metadata.GroupName is not null)
                invokeChain = invokeChain.InvokeMethod(WithGroupName, Argument(metadata.GroupName));

            if (metadata.Tags is not null)
                invokeChain = invokeChain.InvokeMethod(WithTags, metadata.Tags.Value.Select(Argument));

            if (metadata.Hosts is not null)
                invokeChain = invokeChain.InvokeMethod(RequireHost, metadata.Hosts.Value.Select(Argument));

            if (metadata.Authorize is true)
                invokeChain = invokeChain.InvokeMethod(
                    RequireAuthorization,
                    Argument(
                        MethodInfoInstance
                            .InvokeMethod(GetCustomAttributes.WithType(Types.Authorize))
                            .InvokeMethod(ToArray)));

            if (metadata.AllowAnonymous is true)
                invokeChain = invokeChain.InvokeMethod(AllowAnonymous);

            yield return
               Block(
                   List([
                       getMethodInfo,
                       ExpressionStatement(invokeChain),
                   ]));
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

    public static ExpressionSyntax GetResult(ExpressionSyntax result, bool isIResult)
    {
        if (isIResult)
            return result;

        return Types
            .TypedResults
            .InvokeMethod(
                Ok,
                Argument(result));
    }

    private static ExpressionSyntax FromRequestSimple(SimpleNameSyntax member, TypeSyntax parameterType, bool isCollection, ExpressionSyntax name)
    {
        var value = HttpContext.Request.GetMember(member);
        value =
            ElementAccessExpression(value)
            .WithArgumentList(
                BracketedArgumentList(
                    SingletonSeparatedList(
                        Argument(name))));

        var method = isCollection
            ? ConvertEnumerableTo
            : ConvertTo;

        method = method
            .WithTypeArgumentList(
                TypeArgumentList(
                    SingletonSeparatedList(parameterType)));

        var expr = Types.EndpointsHelper
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

    private static IEnumerable<ExpressionSyntax> ParseFromRequest(ParameterMetadata parameter, TypeSyntax type, ExpressionSyntax name)
    {
        if (parameter.IsFromBody)
        {
            var expr = HttpContext.Request
                .InvokeMethod(
                   ReadFromJsonAsync
                        .WithType(type),
                    Argument(
                        HttpContext.RequestAborted))
                .Await()
                .Parenthesized();
            if (type is not NullableTypeSyntax)
                expr = expr.NotNullAssert();

            yield return expr;
        }
        if (parameter.IsFromRoute)
            yield return FromRequestSimple(
                RouteValues,
                type,
                false,
                parameter.FromRoute ?? name);

        if (parameter.IsFromForm)
            yield return FromRequestSimple(
                Form,
                parameter.ItemType ?? type,
                parameter.IsCollection,
                parameter.FromRoute ?? name);

        if (parameter.IsFromHeader)
            yield return FromRequestSimple(
                Headers,
                parameter.ItemType ?? type,
                parameter.IsCollection,
                parameter.FromRoute ?? name);

        if (parameter.IsFromQuery)
            yield return FromRequestSimple(
                Query,
                parameter.ItemType ?? type,
                parameter.IsCollection,
                parameter.FromRoute ?? name);
    }

    private static ExpressionSyntax GetInstance(ParameterMetadata parameter, TypeSyntax type, ExpressionSyntax name)
    {
        var fromCodes = ParseFromRequest(parameter, type, name);

        if (fromCodes.Any())
            return fromCodes.Aggregate((a, b) => a.Coalesce(b));

        switch (parameter.TypeFullName)
        {
            case KnownTypes.HttpContext:
                return HttpContextInstance;
            case KnownTypes.CancellationToken:
                return HttpContext.RequestAborted;
            case KnownTypes.HttpRequest:
                return HttpContext.Request;
            case KnownTypes.HttpResponse:
                return HttpContext.Response;
            case KnownTypes.ConnectionInfo:
                return HttpContext.Connection;
            case KnownTypes.WebSocketManager:
                return HttpContext.WebSockets;
            case KnownTypes.ClaimsPrincipal:
                return HttpContext.User;
            case KnownTypes.IServiceProvider:
                return HttpContext.RequestServices;
            case KnownTypes.ISession:
                return HttpContext.Session;
        }

        if (parameter.IsFromKeyedServices)
        {
            if (parameter.IsCollection)
                return HttpContext.RequestServices.GetKeyedServices(parameter.ItemType, parameter.FromKeyedServices);

            if (type is NullableTypeSyntax)
                return HttpContext.RequestServices.GetKeyedService(type, parameter.FromKeyedServices);

            return HttpContext.RequestServices.GetRequiredKeyedService(type, parameter.FromKeyedServices);
        }
        // if (parameter.GetAttribute(Attributes.FromServices) is not { } fromServices)
        if (parameter.IsCollection)
            return HttpContext.RequestServices.GetServices(parameter.ItemType);

        if (type is NullableTypeSyntax)
            return HttpContext.RequestServices.GetService(type);

        return HttpContext.RequestServices.GetRequiredService(type);
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

        expr = Types.Debug
            .InvokeMethod(
                Assert,
                Argument(expr));

        return ExpressionStatement(expr);
    }

    public static StatementSyntax RuntimeAssertIsNotNull(ExpressionSyntax parameter, ExpressionSyntax parameterName)
    {
        var expr = Types
            .ArgumentNullException
            .InvokeMethod(
                ThrowIfNull,
                [
                    Argument(parameter),
                    Argument(parameterName),
                ]);

        return ExpressionStatement(expr);
    }
}