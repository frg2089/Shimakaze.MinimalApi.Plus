using Microsoft.CodeAnalysis;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator;

internal static class SourceProductionContextExtensions
{
    extension(SourceProductionContext context)
    {
        public void Report(DiagnosticDescriptor descriptor, ISymbol symbol, params object?[]? messageArgs)
        {
            foreach (Diagnostic? diagnostic in symbol.Locations.Select(location => Diagnostic.Create(descriptor, location, messageArgs)))
                context.ReportDiagnostic(diagnostic);
        }
    }
}