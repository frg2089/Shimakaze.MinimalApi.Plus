using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Metadata;

internal sealed class EndpointsMetadata
{
    public static EndpointsMetadata Create(ActionMetadata action, Func<string, string> formatter) => new(action.Controller, action, formatter);

    private readonly string _controllerName;
    private readonly string _actionName;

    public SyntaxTrivia Comment { get; }
    public bool TypeIsEndpoints { get; }
    public TypeSyntax Type { get; }
    public TypeOfExpressionSyntax TypeOfType { get; }
    public IdentifierNameSyntax Name { get; }
    public ExpressionSyntax NameOfMethod { get; }
    public bool IsStaticType { get; }
    public bool IsStatic { get; }
    public bool IsAsync { get; }
    public bool IsVoid { get; }
    public bool IsIResult { get; }
    public ExpressionSyntax? Summary { get; }
    public ExpressionSyntax? Remarks { get; }
    public ExpressionSyntax? EndpointName { get; }
    public ImmutableArray<ParameterMetadata> Parameters { get; }
    public ImmutableDictionary<LiteralExpressionSyntax, ImmutableArray<IdentifierNameSyntax>> EndPoints { get; }
    public ExpressionSyntax? GroupName { get; }
    public ImmutableArray<ExpressionSyntax>? Tags { get; }
    public ImmutableArray<ExpressionSyntax>? Hosts { get; }
    public ImmutableArray<AuthorizeData> Authorize { get; }
    public bool? ValidateAntiForgeryToken { get; }
    public bool? IgnoreAntiforgeryToken { get; }
    public bool? ExcludeFromDescription { get; }
    public bool? AllowAnonymous { get; }
    public bool? DisableHttpMetrics { get; }
    public bool? Obsolete { get; }

    public EndpointsMetadata(ControllerMetadata controller, ActionMetadata action, Func<string, string> formatter)
    {
        TypeIsEndpoints = controller.IsEndpoints;
        Comment = Comment($"// {controller.Namespace}.{controller.Name}.{action.Name}");
        Type = controller.Type;
        TypeOfType = TypeOfExpression(Type);
        Name = IdentifierName(action.Name);
        NameOfMethod = Literal(action.Name).AsString();
        _controllerName = controller.ControllerName;
        _actionName = action.ActionName;
        IsStaticType = controller.IsStatic;
        IsStatic = action.IsStatic;
        IsAsync = action.IsAsync;
        IsVoid = action.IsVoid;
        IsIResult = action.IsIResult;
        if (action.Summary is { Length: not 0 } summary && !string.IsNullOrWhiteSpace(summary))
            Summary = Literal(summary).AsString();
        if (action.Remarks is { Length: not 0 } remarks && !string.IsNullOrWhiteSpace(remarks))
            Remarks = Literal(remarks).AsString();
        EndpointName = action.EndpointName;
        Parameters = action.Parameters;
        EndPoints = action.EndPoints.ToImmutableDictionary(
            i => Literal(CombineTemplate(controller.Template, i.Key, formatter)).AsString(),
            i => i.Value.Select(IdentifierName).ToImmutableArray());
        GroupName = action.Group ?? controller.Group;
        Tags = action.Tags ?? controller.Tags;
        Hosts = action.Hosts ?? controller.Hosts;
        Authorize = [.. action.Authorize ?? controller.Authorize];
        ValidateAntiForgeryToken = action.ValidateAntiForgeryToken ?? controller.ValidateAntiForgeryToken;
        IgnoreAntiforgeryToken = action.IgnoreAntiforgeryToken ?? controller.IgnoreAntiforgeryToken;
        ExcludeFromDescription = action.Hidden ?? controller.Hidden;
        AllowAnonymous = action.AllowAnonymous ?? controller.AllowAnonymous;
        DisableHttpMetrics = action.DisableHttpMetrics ?? controller.DisableHttpMetrics;
        Obsolete = action.Obsolete ?? controller.Obsolete;
    }

    private string CombineTemplate(string? controllerTemplate, string template, Func<string, string> formatter)
    {
        string fullTemplate = GetFullTemplate();
        if (fullTemplate.Contains("[endpoints]"))
        {
            string value = _controllerName;
            if (value.EndsWith("Endpoints", StringComparison.OrdinalIgnoreCase))
                value = value[..^9];

            if (value.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                value = value[..^10];

            fullTemplate = fullTemplate.Replace("[endpoints]", formatter(value));
        }
        if (fullTemplate.Contains("[controller]"))
        {
            string value = _controllerName;
            if (value.EndsWith("Endpoints", StringComparison.OrdinalIgnoreCase))
                value = value[..^9];

            if (value.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                value = value[..^10];

            fullTemplate = fullTemplate.Replace("[controller]", formatter(value));
        }
        if (fullTemplate.Contains("[action]"))
        {
            string value = _actionName;
            if (value.EndsWith("Async", StringComparison.OrdinalIgnoreCase))
                value = value[..^5];

            fullTemplate = fullTemplate.Replace("[action]", formatter(value));
        }

        return fullTemplate.TrimEnd('/');

        string GetFullTemplate()
        {
            if (template.StartsWith("~/"))
                return template[1..];

            if (controllerTemplate is not { Length: not 0 })
                return template;

            return (controllerTemplate.EndsWith('/'), template.StartsWith('/')) switch
            {
                (true, true) => controllerTemplate + template[1..],
                (false, false) => controllerTemplate + '/' + template,
                _ => controllerTemplate + template,
            };
        }
    }
}