using Microsoft.VisualStudio.TestTools.UnitTesting;
using A5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5.Tests
{
    [TestClass()]
    public class GradedTests
    {
        [TestMethod(), Timeout(5000)]
        [DeploymentItem("TestData", "A5_TestData")]
        public void SolveTest()
        {
            Processor[] problems = new Processor[] {
                new Q1ConstructTrie("TD1"),
                new Q2MultiplePatternMatching("TD2"),
                new Q3GeneralizedMPM("TD3"),
                new Q4SuffixTree("TD4"),
                new Q5ShortestNonSharedSubstring("TD5")
            };

            foreach (var p in problems)
            {
                TestTools.RunLocalTest(
                    "A5",
                    p.Process,
                    p.TestDataName,
                    Verifier: p.Verifier,
                    VerifyResultWithoutOrder: p.VerifyResultWithoutOrder,
                    excludedTestCases: p.ExcludedTestCases);
            }
        }
    }
}