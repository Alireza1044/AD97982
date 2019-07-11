using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestCommon;

namespace Exam2.Tests
{
    [DeploymentItem("TestData", "Exam2_TestData")]
    [TestClass()]
    public class GradedTests
    {
        [TestMethod(), Timeout(20000)]
        public void SolveTest_Q1LatinSquareSAT()
        {
            RunTest(new Q1LatinSquareSAT("TD1"));
        }

        [TestMethod(), Timeout(1000)]
        public void SolveTest_Q2LatinSquareBT()
        {
            RunTest(new Q2LatinSquareBT("TD1"));
        }


        public static void RunTest(Processor p)
        {
            TestTools.RunLocalTest("Exam2", p.Process, p.TestDataName, p.Verifier, VerifyResultWithoutOrder: p.VerifyResultWithoutOrder,
                excludedTestCases: p.ExcludedTestCases);
        }

    }
}