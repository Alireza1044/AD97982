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
    [TestClass()]
    public class GradedTests
    {
        [TestMethod(), Timeout(20000)]
        [DeploymentItem("TestData", "A3_TestData")]
        public void SolveTest()
        {
            Processor[] problems = new Processor[] {
               new Q1MinCost("TD1"),
               new Q2DetectingAnomalies("TD2"),
               new Q3ExchangingMoney("TD3"),
               new Q4FriendSuggestion("TD4")
            };

            foreach (var p in problems)
            {
                TestTools.RunLocalTest("A3", p.Process, p.TestDataName,p.Verifier);
            }
        }
    }
}