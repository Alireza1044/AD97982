using TestCommon;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Exam1
{
    public class Q2Cryptanalyst : Processor
    {
        public Q2Cryptanalyst(string testDataName) : base(testDataName)
        {
            //this.ExcludeTestCaseRangeInclusive(1, 25);
            //this.ExcludeTestCaseRangeInclusive(27, 37);
        }

        public override string Process(string inStr) => Solve(inStr);

        public HashSet<string> Vocab = new HashSet<string>();

        public string Solve(string cipher)
        {
            Cryptanalysis c = new Cryptanalysis(
                @"Exam1_TestData\TD2\dictionary.txt", 
                '0', '9');
            return c.Decipher(
                cipher, 3, ' ', 'z', 
                Cryptanalysis.IsDecipheredI1).GetHashCode().ToString();
        }
    }
}