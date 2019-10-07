using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Bash.PublicApiAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ClassAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = "CLASS0001";
        private static readonly LocalizableString Title = "Public classes should be sealed";
        private static readonly LocalizableString MessageFormat = "Public classes should be sealed";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            "Style",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            if (!IsClassSealed(classDeclaration) && IsClassPublic(classDeclaration))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation()));
            }
        }

        private static bool IsClassPublic(ClassDeclarationSyntax classDeclaration)
        {
            var isPublic = classDeclaration.Modifiers.Any(IsPublicModifier);

            Console.WriteLine(classDeclaration);
            Console.WriteLine(isPublic);

            if (classDeclaration.Parent is ClassDeclarationSyntax parent)
            {
                return isPublic && IsClassPublic(parent);
            }

            return isPublic;
        }

        private static bool IsClassSealed(MemberDeclarationSyntax classDeclaration) =>
            classDeclaration.Modifiers.Any(IsSealedModifier);

        private static bool IsSealedModifier(SyntaxToken modifier) =>
            modifier.IsKind(SyntaxKind.SealedKeyword);

        private static bool IsPublicModifier(SyntaxToken modifier) =>
            modifier.IsKind(SyntaxKind.PublicKeyword);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(Rule);
    }
}
