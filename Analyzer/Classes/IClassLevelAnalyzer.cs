using System.Collections.Immutable;
using System.Linq;
using DnDSharp.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class IClassLevelAnalyzer : DiagnosticAnalyzer
{
    readonly ISubAnalyzer[] Analyzers = new ISubAnalyzer[]{
        new IClassLevelHasDeclaringClass(),
        new IClassLevelDeclaringClassHasClassID(),
        new IClassLevelHasClassLevelAttribute(),
        new NonAbstractIClassLevelCtorHasValidChoicesAttribute(),
        new ChoicesAttributeFieldOrPropertyExists(),
};
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Analyzers.SelectMany(a => a.SupportedDiagnostics).ToArray());
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();
        foreach (var analyzer in Analyzers)
            analyzer.Initialize(context);
    }
}