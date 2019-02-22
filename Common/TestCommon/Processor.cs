using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCommon
{
    public abstract class Processor
    {
        public abstract string Process(string inStr);
        public string TestDataName { get; private set; }

        public virtual Action<string, string> Verifier { get; set; } = null;

        public Processor(string testDataName) =>
            this.TestDataName = testDataName;
    }
}
