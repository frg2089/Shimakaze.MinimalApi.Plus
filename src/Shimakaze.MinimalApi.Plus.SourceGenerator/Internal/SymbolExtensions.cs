using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Microsoft.CodeAnalysis;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static class SymbolExtensions
{
    public static bool IsType([NotNullWhen(true)] this ISymbol? symbol, string fullyQualifiedTypeName)
    {
        if (symbol.IsGenericType(out _))
            return symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith(fullyQualifiedTypeName);

        return symbol?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == fullyQualifiedTypeName;
    }

    public static bool IsGenericType([NotNullWhen(true)] this ISymbol? symbol, [NotNullWhen(true)] out INamedTypeSymbol? namedType)
    {
        namedType = null;
        if (symbol is INamedTypeSymbol { IsGenericType: true } type)
        {
            namedType = type;
            return true;
        }

        return false;
    }

    public static string ToCode(this Accessibility accessibility) => accessibility switch
    {
        Accessibility.NotApplicable => string.Empty,
        Accessibility.Private => "private",
        Accessibility.ProtectedAndInternal => "private internal",
        Accessibility.Protected => "protected",
        Accessibility.Internal => "internal",
        Accessibility.ProtectedOrInternal => "protected internal",
        Accessibility.Public => "public",
        _ => throw new NotSupportedException(),
    };

    public static bool IsAwaitable(this ITypeSymbol type, out ITypeSymbol? resultType)
    {
        resultType = null;

        if (type.IsType(KnownTypes.Task) || type.IsType(KnownTypes.ValueTask))
        {
            if (type.IsGenericType(out INamedTypeSymbol? namedType))
                resultType = namedType.TypeArguments.First();

            return true;
        }

        return false;
    }

    public static AttributeData? GetAttribute(this ISymbol symbol, string fullyQualifiedTypeName)
    {
        ImmutableArray<AttributeData> attributes = symbol.GetAttributes();
        AttributeData? attribute = attributes.FirstOrDefault(i => i.AttributeClass.IsType(fullyQualifiedTypeName));
        return attribute;
    }
}
