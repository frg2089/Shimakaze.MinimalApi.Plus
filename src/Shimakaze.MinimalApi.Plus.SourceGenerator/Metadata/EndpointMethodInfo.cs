using System.Xml;

using Microsoft.CodeAnalysis;

using Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Metadata;

internal sealed class EndpointMethodInfo
{
    private readonly Func<string, string> _formatter;

    public bool IsStatic { get; }
    public bool IsAsync { get; }
    public bool IsVoid { get; }
    public bool IsIResult { get; }
    public string MethodName { get; }
    public IReadOnlyList<IParameterSymbol> Parameters { get; }
    public string? Summary { get; }
    public string? Remarks { get; }
    public IReadOnlyDictionary<string, string>? ParameterDescription { get; }

    public string ContainingNamespace { get; }
    public string ContainingTypeName { get; }
    public string? TypeTemplate { get; }
    public bool IsEndpoints { get; }

    public IReadOnlyList<(string Method, string Template)> EndPoints { get; }

    public string? Group { get; }

    private EndpointMethodInfo(IMethodSymbol method, Func<string, string> formatter)
    {
        _formatter = formatter;

        IsStatic = method.IsStatic;
        MethodName = method.Name;
        Parameters = method.Parameters;

        ITypeSymbol? returnType = method.ReturnType;
        if (returnType.IsAwaitable(out ITypeSymbol? trueResultType))
        {
            IsAsync = true;
            returnType = trueResultType;
        }
        if (returnType?.SpecialType is SpecialType.System_Void or null)
        {
            IsVoid = true;
        }
        else
        {
            IsIResult = returnType.AllInterfaces.Any(i => i.IsType(KnownTypes.IResult));
        }

        if (method.GetDocumentationCommentXml(expandIncludes: true) is { Length: not 0 } xml)
        {
            XmlDocument doc = XmlDocument.LoadXml(xml);
            Summary = doc.GetDocumentTagText("summary");
            Remarks = doc.GetDocumentTagText("remarks");
            ParameterDescription = doc.ParseParameterDocuments();
        }

        List<(string Method, string Template)> endpoints = [];
        foreach (AttributeData attribute in method.GetAttributes())
        {
            if (attribute.AttributeClass.IsType(KnownAttributeTypes.HttpDelete)
                || attribute.AttributeClass.IsType(KnownAttributeTypes.HttpGet)
                || attribute.AttributeClass.IsType(KnownAttributeTypes.HttpHead)
                || attribute.AttributeClass.IsType(KnownAttributeTypes.HttpOptions)
                || attribute.AttributeClass.IsType(KnownAttributeTypes.HttpPatch)
                || attribute.AttributeClass.IsType(KnownAttributeTypes.HttpPost)
                || attribute.AttributeClass.IsType(KnownAttributeTypes.HttpPut))
            {
                string httpMethod = attribute.AttributeClass.Name[4..^9];
                if (attribute.ConstructorArguments.FirstOrDefault().Value is not string { Length: not 0 } template)
                    template = string.Empty;

                endpoints.Add((httpMethod, template));
            }
        }

        EndPoints = endpoints;

        INamedTypeSymbol? type = method.ContainingType;
        ContainingNamespace = type.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        ContainingTypeName = type.Name;
        foreach (AttributeData attribute in type.GetAttributes())
        {
            if (attribute.AttributeClass.IsType(KnownAttributeTypes.EndpointGroupName)
                && attribute.ConstructorArguments.Length > 0
                && attribute.ConstructorArguments[0].Value is string group)
                Group = group;
            else if (attribute.AttributeClass.IsType(KnownAttributeTypes.Route)
                && attribute.ConstructorArguments.Length > 0
                && attribute.ConstructorArguments[0].Value is string template)
                TypeTemplate = template;
        }

        while (type is not { SpecialType: SpecialType.System_Object } and not null)
        {
            if (type.IsType(KnownTypes.ApiEndpoints))
            {
                IsEndpoints = true;
                break;
            }

            type = type.BaseType;
        }
    }

    public string CombineTemplate(string template)
    {
        string fullTemplate = GetFullTemplate();
        if (fullTemplate.Contains("[endpoints]"))
        {
            string value = ContainingTypeName;
            if (value.EndsWith("Endpoints", StringComparison.OrdinalIgnoreCase))
                value = value[..^9];

            if (value.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                value = value[..^10];

            fullTemplate = fullTemplate.Replace("[endpoints]", _formatter(value));
        }
        if (fullTemplate.Contains("[controller]"))
        {
            string value = ContainingTypeName;
            if (value.EndsWith("Endpoints", StringComparison.OrdinalIgnoreCase))
                value = value[..^9];

            if (value.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                value = value[..^10];

            fullTemplate = fullTemplate.Replace("[controller]", _formatter(value));
        }
        if (fullTemplate.Contains("[action]"))
        {
            string value = MethodName;
            if (value.EndsWith("Async", StringComparison.OrdinalIgnoreCase))
                value = value[..^5];

            fullTemplate = fullTemplate.Replace("[action]", _formatter(value));
        }

        return fullTemplate.TrimEnd('/');

        string GetFullTemplate()
        {
            if (template.StartsWith("~/"))
                return template[1..];

            if (TypeTemplate is not { Length: not 0 } baseTemplate)
                return template;

            return (baseTemplate.EndsWith('/'), template.StartsWith('/')) switch
            {
                (true, true) => baseTemplate + template[1..],
                (false, false) => baseTemplate + '/' + template,
                _ => baseTemplate + template,
            };
        }
    }

    public static Func<IMethodSymbol, EndpointMethodInfo> Generate(Func<string, string> formatter)
        => method => new(method, formatter);
}
