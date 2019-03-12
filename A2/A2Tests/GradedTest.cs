using Microsoft.VisualStudio.TestTools.UnitTesting;
using A2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A2.Tests
{
    [TestClass()]
    public class GradedTests
    {
        [TestMethod(), Timeout(4000)]
        [DeploymentItem("TestData", "A2_TestData")]
        public void SolveTest()
        {
            Processor[] problems = new Processor[] {
                new Q1ShortestPath("TD1"),
                new Q2BipartiteGraph("TD2")
            };

            foreach (var p in problems)
            {
                TestTools.RunLocalTest("A2", p.Process, p.TestDataName, p.Verifier);
            }
        }
    }
}
