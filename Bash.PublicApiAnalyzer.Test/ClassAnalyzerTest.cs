using Xunit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Bash.PublicApiAnalyzer.Test
{
    public class ClassAnalyzerTest : DiagnosticVerifier
    {
        [Fact]
        public void ErrorsWhenNonNestedPublicClassIsNotSealed()
        {
            var expected = new DiagnosticResult
            {
                Severity = DiagnosticSeverity.Error,
                Id = "CLASS0001",
                Message = "Public classes should be sealed",
                Locations = new[]
                {
                    new DiagnosticResultLocation("Test0.cs", 1, 14)
                }
            };

            const string code = "public class Foo" +
                                "{" +
                                "}";

            VerifyCSharpDiagnostic(code, expected);
        }

        [Fact]
        public void PassesWhenPublicNonNestedClassIsSealed()
        {
            const string code = "public sealed class Foo" +
                                "{" +
                                "}";

            VerifyCSharpDiagnostic(code);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new ClassAnalyzer();
        }
    }
}
