using Microsoft.CodeAnalysis;

using Shimakaze.MinimalApi.Plus.SourceGenerator.Resources;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator;

internal static class DiagnosticDescriptors
{
    private const string Category = "Shimakaze.MinimalApi.Plus";

    public static readonly DiagnosticDescriptor API0001 = new(nameof(API0001), Resource.API0001, Resource.API0001_Message, Category, DiagnosticSeverity.Error, true);
    public static readonly DiagnosticDescriptor API0002 = new(nameof(API0002), Resource.API0002, Resource.API0002_Message, Category, DiagnosticSeverity.Error, true);
    public static readonly DiagnosticDescriptor API0003 = new(nameof(API0003), Resource.API0003, Resource.API0003_Message, Category, DiagnosticSeverity.Error, true);
}
