using Microsoft.VisualStudio.TestTools.UnitTesting;
using A12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A12.Tests
{
    [TestClass()]
    public class GradedTests
    {
        [TestMethod(), Timeout(30000)]
        [DeploymentItem("TestData", "A1_TestData")]
        public void SolveTest()
        {
            Processor[] problems = new Processor[] {
                new Q1MazeExit("TD1"),
                new Q2AddExitToMaze("TD2"),
                new Q3Acyclic("TD3"),
                new Q4OrderOfCourse("TD4"),
                new Q5StronglyConnected("TD5")
            };

            foreach (var p in problems)
            {
                TestTools.RunLocalTest("A1", p.Process, p.TestDataName, p.Verifier);
            }
        }
    }
}