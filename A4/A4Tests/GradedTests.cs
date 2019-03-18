using Microsoft.VisualStudio.TestTools.UnitTesting;
using A4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A4.Tests
{
    [TestClass()]
    public class GradedTests
    {
        [TestMethod(), Timeout(20000)]
        [DeploymentItem("TestData", "A4_TestData")]
        public void SolveTest()
        {
            Processor[] problems = new Processor[] {
               new Q1BuildingRoads("TD1"),
               new Q2Clustering("TD2"),
               new Q3ComputeDistance("TD3")
            };

            foreach (var p in problems)
            {
                TestTools.RunLocalTest("A4", p.Process, p.TestDataName, p.Verifier);
            }
        }
    }
}