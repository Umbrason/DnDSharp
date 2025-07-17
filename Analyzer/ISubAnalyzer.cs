using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

public interface ISubAnalyzer
{
    ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
    void Initialize(AnalysisContext context);
}