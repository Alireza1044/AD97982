using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using Exam1;

namespace Exam1.Tests
{
    [TestClass()]
    public class GradedTests
    {
        [TestMethod()]
        [DeploymentItem("TestData", "Exam1_TestData")]
        public void SolveQ1BetweennessTest()
        {
            Processor p = new Q1Betweenness("TD1");
            TestTools.RunLocalTest("Exam1",
                p.Process,
                p.TestDataName,
                p.Verifier,
                false,
                excludedTestCases: p.ExcludedTestCases
                );
        }

        [TestMethod()]
        [DeploymentItem("TestData", "Exam1_TestData")]
        public void SolveQ2CryptanalystTest()
        {
            Processor p = new Q2Cryptanalyst("TD2");
            TestTools.RunLocalTest("Exam1",
                p.Process,
                p.TestDataName,
                p.Verifier,
                false,
                excludedTestCases:p.ExcludedTestCases);
        }
    }
}
