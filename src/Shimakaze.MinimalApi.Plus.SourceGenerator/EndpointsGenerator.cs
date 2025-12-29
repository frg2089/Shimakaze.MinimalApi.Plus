using System.Collections.Immutable;

using Humanizer;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

using Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;
using Shimakaze.MinimalApi.Plus.SourceGenerator.Metadata;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public sealed class EndpointsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterImplementationSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName(
                KnownTypes.ApiEndpointsAttribute[8..],
                static (node, _) => node is not null,
                static (context, _) => context.TargetSymbol as INamedTypeSymbol)
            .Collect()
            .Combine(context
                .AnalyzerConfigOptionsProvider
                .Select(static (c, _) => c.GlobalOptions)
                .Combine(context
                    .CompilationProvider
                    .Select(static (c, _) => c.ReferencedAssemblyNames))),
            Generate);
    }
    private static void Generate(SourceProductionContext context, (ImmutableArray<INamedTypeSymbol?> Symbols, (AnalyzerConfigOptions Options, IEnumerable<AssemblyIdentity> References)) data)
    {
        var options = data.Item2.Options;

        var openApi = data.Item2.References.FirstOrDefault(i => i.Name is "Microsoft.AspNetCore.OpenApi")?.Version;

        if (!options.TryGetValue("build_property.RootNamespace", out string? @namespace))
            @namespace = KnownTypes.CommonNamespace[8..];

        var controllers = data
            .Symbols
            .OfType<INamedTypeSymbol>()
            .Where(i =>
            {
                switch (i)
                {
                    case { TypeKind: TypeKind.Class, IsAbstract: false, IsGenericType: false }:
                        if (i.GetAttribute(KnownAttributeTypes.NonController) is not null)
                            return false;
                        return true;
                    case { IsGenericType: true }:
                        context.Report(DiagnosticDescriptors.API0002, i, i!);
                        return false;
                    case { IsAbstract: true }:
                        context.Report(DiagnosticDescriptors.API0003, i, i!);
                        return false;
                    default:
                        context.Report(DiagnosticDescriptors.API0001, i, i!, i.TypeKind);
                        return false;
                }
            })
            .Select(i => new ControllerMetadata(i));

        var code = CompilationUnit()
            .WithUsings(
                List([
                    UsingDirective(IdentifierName("System")),
                    UsingDirective(IdentifierName("System.Linq")),
                    UsingDirective(IdentifierName("System.Reflection")),
                    UsingDirective(IdentifierName("Microsoft.AspNetCore.Builder")),
                    UsingDirective(IdentifierName("Microsoft.AspNetCore.Hosting")),
                    UsingDirective(IdentifierName("Microsoft.AspNetCore.Http")),
                    UsingDirective(IdentifierName("Microsoft.AspNetCore.Routing")),
                    UsingDirective(IdentifierName("Microsoft.Extensions.DependencyInjection")),
                    UsingDirective(IdentifierName(KnownTypes.CommonNamespace[8..])),
                    ]))
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    FileScopedNamespaceDeclaration(IdentifierName(@namespace))
                    .WithMembers(
                        List<MemberDeclarationSyntax>([
                            ClassDeclaration(Constants.GeneratedEndpointsToken)
                                .WithMembers(
                                    List<MemberDeclarationSyntax>([
                                        MethodDeclaration(
                                            Constants.IServiceCollection,
                                            Constants.AddEndpointsToken)
                                            .WithParameterList(
                                            ParameterList(
                                                Parameter(Constants.IServiceCollectionInstanceToken)
                                                    .WithType(Constants.IServiceCollection)
                                                    .WithModifiers(TokenList(SyntaxKind.ThisKeyword.Token))
                                                    .AsSingleton()))
                                            .WithBody(
                                                Block(controllers
                                                    .Where(i => !i.IsStatic)
                                                    .Select(type => Constants.IServiceCollectionInstance.InvokeMethod(Constants.AddScoped.WithType(type.Type)).AsStatement())
                                                    .Concat([ReturnStatement(Constants.IServiceCollectionInstance)])
                                                    .AsSeparatedList()))
                                            .WithModifiers(
                                                TokenList(
                                                    SyntaxKind.PublicKeyword.Token,
                                                    SyntaxKind.StaticKeyword.Token)),
                                        MethodDeclaration(
                                            PredefinedType(SyntaxKind.VoidKeyword.Token),
                                            Constants.MapEndpointsToken)
                                            .WithParameterList(
                                                ParameterList(Parameter(Constants.IEndpointRouteBuilderInstanceToken)
                                                    .WithType(Constants.IEndpointRouteBuilder)
                                                    .WithModifiers(TokenList(SyntaxKind.ThisKeyword.Token))
                                                    .AsSingleton()))
                                            .WithBody(
                                                Block(controllers
                                                    .SelectMany(i => i.Actions)
                                                    .Select(i => EndpointsMetadata.Create(i, i => i.Kebaberize()))
                                                    .SelectMany(i => i.GenerateCode(Constants.IEndpointRouteBuilderInstance, openApi))
                                                    .AsSeparatedList()))
                                            .WithModifiers(
                                                TokenList(
                                                    SyntaxKind.PublicKeyword.Token,
                                                    SyntaxKind.StaticKeyword.Token)),
                                    ]))
                                .WithModifiers(
                                    TokenList(
                                        SyntaxKind.InternalKeyword.Token,
                                        SyntaxKind.StaticKeyword.Token,
                                        SyntaxKind.PartialKeyword.Token))
                                    ]))))
            .WithLeadingTrivia(
                TriviaList(
                    Comment($"// Generated by {typeof(EndpointsGenerator).FullName}"),
                    Comment($"//      ❤️ from frg2089"),
                    Trivia(
                        NullableDirectiveTrivia(SyntaxKind.EnableKeyword.Token, true))
                    ));

        context.AddSource("GeneratedEndpoints.g.cs", code.NormalizeWhitespace().ToFullString());
    }
}

