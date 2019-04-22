using TestCommon;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Exam1
{
    public class Q2Cryptanalyst : Processor
    {
        public static string[] dict { get; set; }
        public Q2Cryptanalyst(string testDataName) : base(testDataName)
        {
            //this.ExcludeTestCaseRangeInclusive(10, 37);
            this.ExcludeTestCaseRangeInclusive(31, 37);
            //dict = File.ReadAllLines(@"../../../Exam1Tests/TestData/TD2/dictionary.txt");
        }

        public override string Process(string inStr) => Solve(inStr);

        public HashSet<string> Vocab = new HashSet<string>();

        public string Solve(string cipher)
        {
            dict = File.ReadAllLines(@"../../../Exam1Tests/TestData/TD2/dictionary.txt");

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i < dict.Length; i++)
            {
                dictionary.Add(dict[i], dict[i]);
            }
            string guesspass = null;
            for (int i = 0; i <= 99999; i++)
            {
                guesspass = i.ToString();
                Encryption enc = new Encryption(guesspass, (char)32, (char)122, false);
                string guesstext = enc.Decrypt(cipher);
                var textSplit = guesstext.Split(' ');
                int containing = 0;
                double accuracy = 0;
                for (int j = 0; j < textSplit.Length; j++)
                {
                    if (dictionary.ContainsKey(textSplit[j]))
                    {
                        containing++;
                        accuracy = ((containing * 100)/ textSplit.Length);
                    }
                    if (accuracy >= 10)
                        return guesstext.GetHashCode().ToString();
                }
            }
            //Cryptanalysis c = new Cryptanalysis(
            //    @"Exam1_TestData\TD2\dictionary.txt",
            //    '0', '9');
            //return c.Decipher(
            //    cipher, 3, ' ', 'z',
            //    Cryptanalysis.IsDeciphered(1).GetHashCode().ToString());
            return "x";
        }
    }
}