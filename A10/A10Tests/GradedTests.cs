using Microsoft.VisualStudio.TestTools.UnitTesting;
using A3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3.Tests
{
    [DeploymentItem("TestData", "A10_TestData")]
    [TestClass()]
    public class GradedTests
    {
        [TestMethod()/*, Timeout(1000)*/]
        public void SolveTest_Q1FrequencyAssignment()
        {
            RunTest(new Q1FrequencyAssignment("TD1"));
        }

        [TestMethod(), Timeout(5000)]
        public void SolveTest_Q2CleaningApartment()
        {
            RunTest(new Q2CleaningApartment("TD2"));
        }

        [TestMethod(), Timeout(1000)]
        public void SolveTest_Q3AdBudgetAllocation()
        {
            RunTest(new Q3AdBudgetAllocation("TD3"));
        }

        public static void RunTest(Processor p)
        {
            TestTools.RunLocalTest("A10", p.Process, p.TestDataName, p.Verifier,
                VerifyResultWithoutOrder: p.VerifyResultWithoutOrder,
                excludedTestCases: p.ExcludedTestCases);
        }
    }
}