using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DnDSharp.Analyzer
{
    public class ChoicesAttributeFieldOrPropertyExists : ISubAnalyzer
    {
        public ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(new DiagnosticDescriptor[]{
        Rule_Exists,
        Rule_PropertyIsReadable,
        Rule_IsPublic,
        Rule_TypeMatches,
    });
        private static readonly DiagnosticDescriptor Rule_Exists = new DiagnosticDescriptor("DNDSHARP3001", "ChoicesAttribute has no matching backing field", "No field or property with name '{0}' found. Choice attributes always require a public static array field or property of the same type as the parameter they decorate.", "", DiagnosticSeverity.Error, true);
        private static readonly DiagnosticDescriptor Rule_IsPublic = new DiagnosticDescriptor("DNDSHARP3002", "ChoicesAttribute has no matching backing field", "Property or field referenced by a ChoicesAttribute must have public read access", "", DiagnosticSeverity.Error, true);
        private static readonly DiagnosticDescriptor Rule_PropertyIsReadable = new DiagnosticDescriptor("DNDSHARP3003", "ChoicesAttribute has no matching backing field", "A property referenced by a ChoicesAttribute must always have a public read accessor with no parameters", "", DiagnosticSeverity.Error, true);
        private static readonly DiagnosticDescriptor Rule_TypeMatches = new DiagnosticDescriptor("DNDSHARP3004", "ChoicesAttribute has no matching backing field", "ChoicesAttribute requires a field or property of type '{0}' but found a field or property of type '{1}'", "", DiagnosticSeverity.Error, true);
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
                    foreach (var attr in param.GetAttributes().Where(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, AttributeType)))
                    {
                        var fieldReferenceArgument = attr.ConstructorArguments.Last();
                        if (!SymbolEqualityComparer.Default.Equals(fieldReferenceArgument.Type, context.Compilation.GetSpecialType(SpecialType.System_String))) continue;
                        var members = namedType.GetMembers();
                        var requiredMemberName = fieldReferenceArgument.Value as string;
                        bool IsValidMember(ISymbol member)
                        {
                            if (member.Name != requiredMemberName) return false;
                            if (!member.IsStatic) return false;
                            if (!((member is IFieldSymbol) || (member is IPropertySymbol))) return false;
                            return true;
                        }
                        var referencedMember = members.FirstOrDefault(IsValidMember);
                        if (referencedMember == null)
                        {
                            context.ReportDiagnostic(Diagnostic.Create(Rule_Exists, param.Locations[0], additionalLocations: param.Locations, messageArgs: requiredMemberName));
                            continue;
                        }

                        if (referencedMember.DeclaredAccessibility != Accessibility.Public)
                        {
                            context.ReportDiagnostic(Diagnostic.Create(Rule_IsPublic, referencedMember.Locations[0], additionalLocations: param.Locations));
                            continue;
                        }

                        if (referencedMember is IPropertySymbol property && (property.GetMethod?.DeclaredAccessibility != Accessibility.Public || property.GetMethod.Parameters.Length != 0))
                        {
                            context.ReportDiagnostic(Diagnostic.Create(Rule_PropertyIsReadable, property.GetMethod?.Locations[0] ?? property.Locations[0], param.Locations));
                            continue;
                        }
                        var memberType = referencedMember is IFieldSymbol symbol ? symbol.Type : ((IPropertySymbol)referencedMember).Type;
                        var choiceAmountArguments = attr.ConstructorArguments.Take(attr.ConstructorArguments.Length - 1);
                        var minPicksArg = choiceAmountArguments.First();
                        var maxPicksArg = choiceAmountArguments.Last();
                        var attributeMinCount = (int)minPicksArg.Value;
                        var attributeMaxCount = (int)maxPicksArg.Value;
                        if (attributeMinCount < 0 || attributeMaxCount < attributeMinCount) continue; //dont know what type to check for, since the attribute args are invalid

                        var requiresArrayType = !(attributeMaxCount == 1 && attributeMinCount == 1);
                        if (!(param.Type is IArrayTypeSymbol) && requiresArrayType) continue; //~~can't~~ shouldn't evaluate type for field since the parameter type is already wrong

                        var paramIsNotArrayType = choiceAmountArguments.Select(arg => arg.Value as int?).All(v => v == 1);
                        var requiredType = paramIsNotArrayType ? context.Compilation.CreateArrayTypeSymbol(param.Type, 1) : param.Type;
                        if (!SymbolEqualityComparer.Default.Equals(memberType, requiredType))
                        {
                            context.ReportDiagnostic(Diagnostic.Create(Rule_TypeMatches, referencedMember.Locations[0], param.Locations, messageArgs: new object[] { requiredType, memberType }));
                            continue;
                        }
                    }
                }
            }
        }
    }
}