using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Metadata;

internal sealed class ControllerMetadata : CommonMetadata
{
    public string Name { get; }
    [AllowNull]
    public string ControllerName { get => field ?? Name; private set; }
    public TypeSyntax Type { get; }

    public bool IsStatic { get; }
    public string Namespace { get; }
    public ImmutableArray<ActionMetadata> Actions { get; }
    public bool IsEndpoints { get; }

    public ControllerMetadata(INamedTypeSymbol type)
    {
        Name = type.Name;
        Namespace = type.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        Type = SyntaxFactory.ParseTypeName($"{Namespace}.{Name}");
        IsStatic = type.IsStatic;

        foreach (var attribute in type.GetAttributes())
            ParseAttribute(attribute);

        Actions = [.. type.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(i => i.GetAttribute(KnownAttributeTypes.NonAction) is null)
            .Select(i => new ActionMetadata(this, i))];

        var current = type;
        while (current is not { SpecialType: SpecialType.System_Object } and not null)
        {
            if (current.IsType(KnownTypes.ApiEndpoints))
            {
                IsEndpoints = true;
                break;
            }

            current = type.BaseType;
        }
    }
}
