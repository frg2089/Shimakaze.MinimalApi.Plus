using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

using Microsoft.CodeAnalysis;

using Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Metadata;

internal sealed class ActionMetadata : CommonMetadata
{
    private readonly List<(string Method, string? Template)> _endpoints = [];
    public ControllerMetadata Controller { get; }
    public string Name { get; }
    [AllowNull]
    public string ActionName { get => field ?? Name; private set; }
    public bool IsStatic { get; }
    public bool IsAsync { get; }
    public bool IsVoid { get; }
    public bool IsIResult { get; }
    public string? Summary { get; }
    public string? Remarks { get; }
    public ImmutableArray<ParameterMetadata> Parameters { get; }

    [NotNull]
    public ImmutableDictionary<string, ImmutableArray<string>>? EndPoints => field ??= _endpoints
        .Select(i => (i.Method, Template: i.Template ?? Template ?? string.Empty))
        .GroupBy(i => i.Template, i => i.Method)
        .ToImmutableDictionary(i => i.Key, i => i.ToImmutableArray());

    public ActionMetadata(ControllerMetadata controller, IMethodSymbol method)
    {
        Controller = controller;
        Name = method.Name;
        IsStatic = method.IsStatic;
        IsAsync = method.IsAsync;

        var returnType = method.ReturnType;
        if (returnType.IsAwaitable(out returnType))
            IsAsync = true;

        IsVoid = returnType is { SpecialType: SpecialType.System_Void } or null;
        IsIResult = returnType?.AllInterfaces.Any(i => i.IsType(KnownTypes.IResult)) ?? false;

        IReadOnlyDictionary<string, string>? parameterDescription = null;
        if (method.GetDocumentationCommentXml(expandIncludes: true) is { Length: not 0 } xml)
        {
            var doc = XmlDocument.LoadXml(xml);
            Summary = doc.GetDocumentTagText("summary");
            Remarks = doc.GetDocumentTagText("remarks");
            parameterDescription = doc.ParseParameterDocuments();
        }

        foreach (var attribute in method.GetAttributes())
            ParseAttribute(attribute);

        Parameters = [.. method.Parameters.Select(i =>
        {
            if (parameterDescription?.TryGetValue(i.Name, out string? desc) is not true)
                desc = null;

            return new ParameterMetadata(i, desc);
        })];
    }

    protected override void ParseAttribute(AttributeData attribute)
    {
        switch (attribute.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
        {
            case KnownAttributeTypes.HttpDelete:
            case KnownAttributeTypes.HttpGet:
            case KnownAttributeTypes.HttpHead:
            case KnownAttributeTypes.HttpOptions:
            case KnownAttributeTypes.HttpPatch:
            case KnownAttributeTypes.HttpPost:
            case KnownAttributeTypes.HttpPut:
                {
                    string? template = null;
                    if (attribute.ConstructorArguments.Length is not 0)
                        template = attribute.ConstructorArguments[0].Value as string;

                    _endpoints.Add(($"{KnownTypes.HttpMethods}.{attribute.AttributeClass.Name[4..^9]}", template));
                    break;
                }
            case KnownAttributeTypes.AcceptVerbs:
                var tmp = attribute.ConstructorArguments.First();
                if (tmp.Kind is TypedConstantKind.Array)
                {
                    foreach (string httpMethod in tmp.Values.Select(i => i.Value).OfType<string>())
                        _endpoints.Add((httpMethod, null));
                }
                else if (tmp.Value is string template)
                {
                    _endpoints.Add((template, null));
                }
                break;
        }
        base.ParseAttribute(attribute);
    }
}
