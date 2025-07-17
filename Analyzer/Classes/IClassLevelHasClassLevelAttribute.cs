using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DnDSharp.Analyzer
{

    public class IClassLevelHasClassLevelAttribute : ISubAnalyzer
    {
        public ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(new DiagnosticDescriptor[]{
        Rule
    });
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor("DNDSHARP1002", "IClassLevel is missing Attribute 'ClassLevelAttribute'", "IClassLevel implementations must always be annotated with a 'ClassLevel' attribute", "", DiagnosticSeverity.Error, true);
        public void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(EnsureDeclaringType, SymbolKind.NamedType);
        }

        private void EnsureDeclaringType(SymbolAnalysisContext context)
        {
            var namedType = context.Symbol as INamedTypeSymbol;

            var IClassLevelType = context.Compilation.GetTypeByMetadataName("DnDSharp.Core.IClassLevel");
            if (!namedType.Interfaces.Contains(IClassLevelType)) return; //Only *direct* implementations of IClassLevel need Class Level attribute

            var AttributeType = context.Compilation.GetTypeByMetadataName("DnDSharp.Core.ClassLevelAttribute");
            if (namedType.GetAttributes().Any(attribute => SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, AttributeType)))
                return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, namedType.Locations[0], additionalLocations: namedType.Locations));

        }
    }
}