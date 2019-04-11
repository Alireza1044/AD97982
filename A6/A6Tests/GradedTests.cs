using Microsoft.VisualStudio.TestTools.UnitTesting;
using A6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A6.Tests
{
    [TestClass()]
    public class GradedTests
    {
        [TestMethod(), Timeout(9000)]
        [DeploymentItem("TestData", "A6_TestData")]
        public void SolveTest()
        {
            Processor[] problems = new Processor[] {
                new Q1ConstructBWT("TD1"),
                new Q2ReconstructStringFromBWT("TD2"),
                new Q3MatchingAgainCompressedString("TD3"),
                new Q4ConstructSuffixArray("TD4")
            };

            foreach (var p in problems)
            {
                TestTools.RunLocalTest("A6", p.Process, p.TestDataName, p.Verifier, VerifyResultWithoutOrder: p.VerifyResultWithoutOrder,
                    excludedTestCases: p.ExcludedTestCases);
            }
        }
    }
}