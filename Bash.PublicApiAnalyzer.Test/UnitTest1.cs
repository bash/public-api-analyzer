using Xunit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Formatting;

namespace Bash.PublicApiAnalyzer.Test
{
    public class UnitTest1 : DiagnosticVerifier
    {
        [Fact]
        public void Test1()
        {
            var expected = new DiagnosticResult();
        }
    }
}
