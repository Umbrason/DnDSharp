using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DnDSharp.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IClassLevelHasDeclaringClass : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(new DiagnosticDescriptor[]{
        Rule
    });
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor("DNDSHARP1001", "IClassLevel implementation not in enclosing class", "IClassLevel implementations must always be inside of an enclosing class with a static ClassID field", "", DiagnosticSeverity.Error, true);
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(EnsureDeclaringType, SymbolKind.NamedType);
        }

        private void EnsureDeclaringType(SymbolAnalysisContext context)
        {
            var namedType = context.Symbol as INamedTypeSymbol;
            var IClassLevelType = context.Compilation.GetTypeByMetadataName("DnDSharp.Core.IClassLevel");
            if (!namedType.AllInterfaces.Contains(IClassLevelType)) return;
            if (namedType.ContainingType != null) return;
            context.ReportDiagnostic(Diagnostic.Create(Rule, namedType.Locations[0], additionalLocations: namedType.Locations));

        }
    }
}