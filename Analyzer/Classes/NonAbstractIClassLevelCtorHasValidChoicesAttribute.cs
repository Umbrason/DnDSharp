using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DnDSharp.Analyzer
{

    public class NonAbstractIClassLevelCtorHasValidChoicesAttribute : ISubAnalyzer
    {
        public ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(new DiagnosticDescriptor[]{
        Rule_Exists,
        Rule_ExactlyOne,
        Rule_MinPicksIsGreaterEqualZero,
        Rule_MaxPicksIsGreaterEqualMinPicks,
        Rule_ParamIsArrayTypeWhenChoicesAreGreaterOne,

    });
        private static readonly DiagnosticDescriptor Rule_Exists = new DiagnosticDescriptor("DNDSHARP2001", "Constructor parameter for IClassLevel implementations require exactly one valid 'Choices' attribute", "The parameter '{0}' is missing the 'Choices' attribute", "", DiagnosticSeverity.Error, true);
        private static readonly DiagnosticDescriptor Rule_ExactlyOne = new DiagnosticDescriptor("DNDSHARP2002", "Constructor parameter for IClassLevel implementations require exactly one valid 'Choices' attribute", "The parameter '{0}' has too many 'Choices' attributes", "", DiagnosticSeverity.Error, true);

        private static readonly DiagnosticDescriptor Rule_MinPicksIsGreaterEqualZero = new DiagnosticDescriptor("DNDSHARP2003", "Constructor parameter for IClassLevel implementations require exactly one valid 'Choices' attribute", "The minPicks parameter of the 'Choices' attribute must be greater than or equal to zero but is '{0}'", "", DiagnosticSeverity.Error, true);
        private static readonly DiagnosticDescriptor Rule_MaxPicksIsGreaterEqualMinPicks = new DiagnosticDescriptor("DNDSHARP2004", "Constructor parameter for IClassLevel implementations require exactly one valid 'Choices' attribute", "The maxPicks parameter of the 'Choices' attribute must be greater than or equal to minPicks ('{0}') but is '{1}'", "", DiagnosticSeverity.Error, true);

        private static readonly DiagnosticDescriptor Rule_ParamIsArrayTypeWhenChoicesAreGreaterOne = new DiagnosticDescriptor("DNDSHARP2005", "Constructor parameter for IClassLevel implementations require exactly one valid 'Choices' attribute", "The parameter '{0}' must be an array type because multiple or zero choices are allowed by the 'Choices' attributes, but is of type '{1}'", "", DiagnosticSeverity.Error, true);


        public void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(EnsureDeclaringType, SymbolKind.NamedType);
        }

        private void EnsureDeclaringType(SymbolAnalysisContext context)
        {
            var namedType = context.Symbol as INamedTypeSymbol;
            if (namedType.IsAbstract) return; //abstract types dont need to adhere to this
            var IClassLevelType = context.Compilation.GetTypeByMetadataName("DnDSharp.Core.IClassLevel");
            if (!namedType.AllInterfaces.Contains(IClassLevelType)) return;
            if (namedType.InstanceConstructors.Length == 0) return; //has no parameters, can skip
            var AttributeType = context.Compilation.GetTypeByMetadataName("DnDSharp.Core.ChoicesAttribute");
            foreach (var ctor in namedType.InstanceConstructors)
            {
                foreach (var param in ctor.Parameters)
                {
                    var attributes = param.GetAttributes().Where(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, AttributeType));
                    if (!attributes.Any())
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule_Exists, param.Locations[0], additionalLocations: param.Locations));
                        continue;
                    }
                    if (attributes.Count() > 1)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule_ExactlyOne, param.Locations[0], additionalLocations: param.Locations));
                        continue;
                    }
                    var attribute = attributes.First();
                    var choiceCountParams = attribute.ConstructorArguments.Take(attribute.ConstructorArguments.Length - 1);
                    var minPicksArg = choiceCountParams.First();
                    var maxPicksArg = choiceCountParams.Last();
                    var attributeMinCount = (int)minPicksArg.Value;
                    var attributeMaxCount = (int)maxPicksArg.Value;
                    if (attributeMinCount < 0)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule_MinPicksIsGreaterEqualZero, param.Locations[0], additionalLocations: param.Locations, messageArgs: attributeMinCount));
                        continue;
                    }
                    if (attributeMaxCount < attributeMinCount)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule_MaxPicksIsGreaterEqualMinPicks, param.Locations[0], additionalLocations: param.Locations, messageArgs: new object[] { attributeMinCount, attributeMaxCount }));
                        continue;
                    }
                    var requiresArrayType = !(attributeMaxCount == 1 && attributeMinCount == 1);
                    if (!(param.Type is IArrayTypeSymbol) && requiresArrayType)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule_ParamIsArrayTypeWhenChoicesAreGreaterOne, param.Locations[0], additionalLocations: param.Locations, messageArgs: new object[] { param.Name, param.Type }));
                        continue;
                    }
                }
            }
        }
    }
}