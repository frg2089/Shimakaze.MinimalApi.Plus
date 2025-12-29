using System.Diagnostics.CodeAnalysis;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Metadata;

internal sealed class ParameterMetadata
{
    private readonly ParameterFrom _from;

    public SyntaxToken Token { get; }
    public IdentifierNameSyntax Identifier { get; }
    public ExpressionSyntax NameOf { get; }

    public ExpressionSyntax? Description { get; }
    public TypeSyntax Type { get; }
    public ExpressionSyntax NameOfType { get; }
    public string TypeFullName { get; }
    public NullableAnnotation Nullable { get; }
    public bool IsValueType { get; }
    public bool Obsolete { get; }

    [MemberNotNullWhen(true, nameof(ItemType), nameof(NameOfItemType))]
    public bool IsCollection { get; }
    public TypeSyntax? ItemType { get; }
    public ExpressionSyntax? NameOfItemType { get; }

    public bool IsFromBody => _from.HasFlag(ParameterFrom.FromBody);
    public bool IsFromForm => _from.HasFlag(ParameterFrom.FromForm);
    public bool IsFromHeader => _from.HasFlag(ParameterFrom.FromHeader);
    public bool IsFromQuery => _from.HasFlag(ParameterFrom.FromQuery);
    public bool IsFromRoute => _from.HasFlag(ParameterFrom.FromRoute);
    public bool IsFromServices => _from.HasFlag(ParameterFrom.FromServices);
    public bool IsFromKeyedServices => _from.HasFlag(ParameterFrom.FromKeyedServices);

    public ExpressionSyntax? FromForm { get; }
    public ExpressionSyntax? FromHeader { get; }
    public ExpressionSyntax? FromQuery { get; }
    public ExpressionSyntax? FromRoute { get; }
    public ExpressionSyntax? FromKeyedServices { get; }

    public ParameterMetadata(IParameterSymbol parameter, string? desc)
    {
        Token = SyntaxFactory.Identifier($"__parameter_{parameter.Name}");
        Identifier = SyntaxFactory.IdentifierName(Token);
        NameOf = SyntaxFactory.Literal(parameter.Name).AsString();
        if (desc is { Length: not 0 } && !string.IsNullOrWhiteSpace(desc))
            Description = SyntaxFactory.Literal(desc).AsString();

        TypeFullName = parameter.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        Type = SyntaxFactory.ParseTypeName(TypeFullName);
        NameOfType = SyntaxFactory.Literal(parameter.Type.Name).AsString();
        Nullable = parameter.NullableAnnotation;
        IsValueType = parameter.Type.BaseType?.SpecialType is SpecialType.System_ValueType;
        if (parameter.Type is IArrayTypeSymbol arrayTypeSymbol)
        {
            IsCollection = true;
            ItemType = SyntaxFactory.ParseTypeName(arrayTypeSymbol.ElementType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
            NameOfItemType = SyntaxFactory.Literal(arrayTypeSymbol.ElementType.Name).AsString();
        }
        else if (parameter.Type.AllInterfaces.FirstOrDefault(i => i.SpecialType is SpecialType.System_Collections_Generic_IEnumerator_T) is { } enumerable)
        {
            IsCollection = true;
            var itemType = enumerable.TypeArguments.First();
            ItemType = SyntaxFactory.ParseTypeName(itemType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
            NameOfItemType = SyntaxFactory.Literal(itemType.Name).AsString();
        }

        foreach (var attribute in parameter.GetAttributes())
        {
            if (attribute.AttributeClass is null)
                continue;

            string attributeType = attribute.AttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            switch (attributeType)
            {
                case KnownAttributeTypes.Description:
                    Description = SyntaxFactory.ParseExpression(attribute.ConstructorArguments.FirstOrDefault().ToCSharpString());
                    break;
                case KnownAttributeTypes.FromKeyedServices:
                    FromKeyedServices = SyntaxFactory.ParseExpression(attribute.ConstructorArguments.FirstOrDefault().ToCSharpString());
                    _from |= ParameterFrom.FromKeyedServices;
                    break;
                case KnownAttributeTypes.FromServices:
                    _from |= ParameterFrom.FromServices;
                    break;
                case KnownAttributeTypes.FromBody:
                    _from |= ParameterFrom.FromBody;
                    break;
                case KnownAttributeTypes.FromForm:
                    _from |= ParameterFrom.FromForm;
                    FromForm = SyntaxFactory.ParseExpression(attribute.NamedArguments.FirstOrDefault(i => i.Key is "Name").Value.ToCSharpString());
                    break;
                case KnownAttributeTypes.FromHeader:
                    _from |= ParameterFrom.FromHeader;
                    FromHeader = SyntaxFactory.ParseExpression(attribute.NamedArguments.FirstOrDefault(i => i.Key is "Name").Value.ToCSharpString());
                    break;
                case KnownAttributeTypes.FromQuery:
                    _from |= ParameterFrom.FromQuery;
                    FromQuery = SyntaxFactory.ParseExpression(attribute.NamedArguments.FirstOrDefault(i => i.Key is "Name").Value.ToCSharpString());
                    break;
                case KnownAttributeTypes.FromRoute:
                    _from |= ParameterFrom.FromRoute;
                    FromRoute = SyntaxFactory.ParseExpression(attribute.NamedArguments.FirstOrDefault(i => i.Key is "Name").Value.ToCSharpString());
                    break;
                case KnownAttributeTypes.Obsolete:
                    Obsolete = true;
                    break;
            }
        }
    }
}
