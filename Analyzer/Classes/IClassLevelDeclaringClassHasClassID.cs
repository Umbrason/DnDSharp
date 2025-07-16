using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DnDSharp.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IClassLevelDeclaringClassHasClassID : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(new DiagnosticDescriptor[]{
        Rule
    });
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor("DNDSHARP1003", "IClassLevel enclosing class is missing a public static ClassID field", "IClassLevel implementations must always be inside of an enclosing class with a public static ClassID field", "", DiagnosticSeverity.Error, true);
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(EnsureDeclaringType, SymbolKind.NamedType);
        }

        private void EnsureDeclaringType(SymbolAnalysisContext context)
        {
            var namedType = context.Symbol as INamedTypeSymbol;
            var containingType = namedType.ContainingType;
            if (containingType == null) return;
            var IClassLevelType = context.Compilation.GetTypeByMetadataName("DnDSharp.Core.IClassLevel");
            if (!namedType.Interfaces.Contains(IClassLevelType)) return; //Only *direct* implementations of IClassLevel need the ClassID in the containing class. Inherited implementations will just inherit the ClassID reference for free I think

            var ClassIDType = context.Compilation.GetTypeByMetadataName("DnDSharp.Core.ClassID");
            if (containingType.GetMembers().Any(symbol => IsPublicStaticClassIDField(symbol, ClassIDType))) return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, namedType.Locations[0], additionalLocations: namedType.Locations));

        }

        private bool IsPublicStaticClassIDField(ISymbol symbol, INamedTypeSymbol ClassIDType)
        {
            if (symbol is IFieldSymbol fieldSymbol && fieldSymbol.DeclaredAccessibility == Accessibility.Public && fieldSymbol.Name == "ClassID" && SymbolEqualityComparer.Default.Equals(fieldSymbol.Type, ClassIDType)) return true;
            if (symbol is IPropertySymbol propertySymbol && propertySymbol.GetMethod.DeclaredAccessibility == Accessibility.Public && propertySymbol.Name == "ClassID" && SymbolEqualityComparer.Default.Equals(propertySymbol.Type, ClassIDType)) return true;
            return false;
        }
    }
}