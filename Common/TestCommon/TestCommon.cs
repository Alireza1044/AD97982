using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TestCommon
{
    public static class TestTools 
    {
        public static readonly char[] IgnoreChars = new char[] { '\n', '\r', ' ' };
        public static readonly char[] NewLineChars = new char[] { '\n', '\r'};

        public static void RunLocalTest(
            string AssignmentName,
            Func<string, string> Processor,
            string TestDataName,
            Action<string, string> Verifier) => 
                            RunLocalTest(
                                    AssignmentName,
                                    Processor,
                                    TestDataName,
                                    false,
                                    null,
                                    int.MaxValue,
                                    Verifier ?? FileVerifier);

        public static void RunLocalTest(
            string AssignmentName, 
            Func<string,string> Processor, 
            string TestDataName=null,
            bool saveMode=false,
            string testDataPathOverride=null,
            int maxTestCases=int.MaxValue,
            Action<string, string> Verifier=null)
        {
            Verifier = Verifier ?? FileVerifier;
            string testDataPath = $"{AssignmentName}_TestData";
            if (!string.IsNullOrEmpty(TestDataName))
                testDataPath = Path.Combine(testDataPath, TestDataName);

            if (!string.IsNullOrEmpty(testDataPathOverride))
                testDataPath = testDataPathOverride;

            Assert.IsTrue(Directory.Exists(testDataPath));
            string[] inFiles = Directory.GetFiles(testDataPath, "*In_*.txt");

            Assert.IsTrue(inFiles.Length > 0);

            int testCaseNumber = 0;
            List<string> failedTests = new List<string>();
            TimeSpan totalTime = new TimeSpan(0);
            foreach (var inFile in inFiles.OrderBy(x => FileNumber(x)))
            {
                if (++testCaseNumber > maxTestCases)
                    break;

                Stopwatch sw;
                string outFile;
                try
                {
                    var lines = File.ReadAllText(inFile);

                    sw = Stopwatch.StartNew();
                    string result = Processor(lines);
                    sw.Stop();
                    totalTime += sw.Elapsed;

                    result = result.Trim(IgnoreChars);
                    if (saveMode)
                    {
                        outFile = inFile.Replace("In_", "Out_");
                        File.WriteAllText(outFile, result);
                        Console.WriteLine($"{Path.GetFileName(Path.GetDirectoryName(inFile))}: {Path.GetFileName(inFile)}=>{Path.GetFileName(outFile)}");
                        continue;
                    }

                    Verifier(inFile, result);

                    Console.WriteLine($"Test Passed ({sw.Elapsed.ToString()}): {inFile}");
                }
                catch (Exception e)
                {
                    failedTests.Add($"Test failed for input {inFile}: {e.Message}");
                    Console.WriteLine($"Test Failed: {inFile}");
                }
            }

            Assert.IsTrue(failedTests.Count == 0,
                $"{failedTests.Count} out of {inFiles.Length} tests failed: " +
                $"{new string(string.Join("\n", failedTests).Take(1000).ToArray())}");

            Console.WriteLine($"All {inFiles.Length} tests passed: {totalTime.ToString()}.");
        }

        private static void FileVerifier(string inputFileName, string testResult)
        {
            string outFile = inputFileName.Replace("In_", "Out_");
            Assert.IsTrue(File.Exists(outFile));
            string expectedResult = string.Join("\n", File.ReadAllLines(outFile)
                .Select(line => line.Trim(IgnoreChars)) // Ignore white spaces 
                .Where(line => !string.IsNullOrWhiteSpace(line))); // Ignore empty lines
            Assert.AreEqual(expectedResult, testResult);
        }

        private static int FileNumber(string fileName)
        {
            int start = fileName.LastIndexOf('_');
            int end = fileName.LastIndexOf('.');
            string fileNumber = fileName.Substring(start + 1, end - start - 1);
            return int.Parse(fileNumber);
        }

        public static string Process(string inStr, Func<long, long[][], long, long, long> processor)
        {
            var lines = inStr.Split(NewLineChars,StringSplitOptions.RemoveEmptyEntries);
            long count = int.Parse(lines.First());
            long[][] data = ReadTree(lines.Skip(1).Take(lines.Length - 2));
            long[] last =lines.Last().Split(IgnoreChars, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(n => long.Parse(n))
                                  .ToArray();

            return string.Join("\n", processor(count, data,last[0],last[1]).ToString());
        }

        public static string Process(string inStr, Func<long, long[][], long> processor)
        {
            var lines = inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
            long count = int.Parse(lines.First()); 
            long[][] data = ReadTree(lines.Skip(1));

            return string.Join("\n", processor(count, data).ToString());
        }

        public static string Process(string inStr, Func<long, long[][], long[]> processor)
        {
            long count;
            long[][] data;
            ParseGraph(inStr, out count, out data);

            return string.Join(" ", processor(count, data));
        }

        public static void ParseGraph(string inStr, out long count, out long[][] data)
        {
            var lines = inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
            count = int.Parse(lines.First());
            data = ReadTree(lines.Skip(1));
        }

        public static string Process(string inStr, Func<long[][], long[][]> processor)
        {
            long[][] data = ReadTree(inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries));

            return string.Join("\n", processor(data)
                .Select(a => string.Join(" ", a)));
        }

        public static string Process(string inStr, Func<string, long[][], string> processor)
        {
            var lines = inStr.Split(NewLineChars, 
                StringSplitOptions.RemoveEmptyEntries);
            string text = lines.First();
            long[][] data = ReadTree(lines.Skip(1));

            return processor(text, data);
        }

        public static string Process(string inStr, Func<long[][], bool> processor)
        {
            long[][] data = ReadTree(inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries));

            return processor(data).ToString();
        }

        private static long[][] ReadTree(IEnumerable<string> lines)
        {
            return lines.Select(line => line.Split(IgnoreChars, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(n => long.Parse(n))
                                     .ToArray()
                 ).ToArray();
        }

        public static string Process(string inStr, Func<string, string, long[]> processor)
        {
            var toks = inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", processor(toks[0], toks[1]));
        }

        public static string Process(string inStr, Func<long, string[], string[]> processor)
        {
            var toks = inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries)
                 .Where(l => !string.IsNullOrWhiteSpace(l));

            long count = long.Parse(toks.First());
            var remainingLines = toks.Skip(1).ToArray();
            return
                string.Join("\n", processor(count, remainingLines));
        }

        public static string Process(string inStr, Func<string[], string[]> processor)
        {
            return
                string.Join("\n",
                processor(inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries)
                 .Where(l => !string.IsNullOrWhiteSpace(l))
                 .ToArray()));
        }

        public static string Process(string inStr, Func<string, string, long> longProcessor)
        {
            var toks = inStr.Split(IgnoreChars, StringSplitOptions.RemoveEmptyEntries);
            return longProcessor(toks[0], toks[1]).ToString();
        }

        public static string Process(string inStr, Func<string, long> longProcessor)
        {            
            return longProcessor(inStr.Trim(IgnoreChars)).ToString();
        }

        public static string Process(string inStr, Func<long[], Tuple<long, long>[]> longProcessor)
        {
            long[] inArray = inStr
                .Split(IgnoreChars, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => long.Parse(s))
                .ToArray();

            return string.Join("\n", longProcessor(inArray).Select(t => $"{t.Item1} {t.Item2}"));
        }

        public static string Process(string inStr, Func<long, long[], Tuple<long, long>[]> longProcessor)
        {
            var allNumbers = inStr
                .Split(IgnoreChars, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => long.Parse(s));

            long firstNumber = allNumbers.First();
            long[] remainingNumbers = allNumbers.Skip(1).ToArray();

            return string.Join("\n", longProcessor(firstNumber, remainingNumbers).Select(t => $"{t.Item1} {t.Item2}"));
        }

        public static string Process(string inStr, 
            Func<long, long> longProcessor)
        {
            long n = long.Parse(inStr);
            return longProcessor(n).ToString();
        }

        public static string Process(string inStr, 
            Func<long, long[]> longProcessor)
        {
            long n = long.Parse(inStr);
            return string.Join("\n", longProcessor(n));
        }

        public static string Process(string inStr, 
            Func<long, long[], string> longProcessor)
        {

            var lines = inStr.Split(IgnoreChars, StringSplitOptions.RemoveEmptyEntries);
            long count = long.Parse(lines.Take(1).First());
            var numbers = lines.Skip(1)
                .Select(n => long.Parse(n))
                .ToArray();

            Assert.AreEqual(count, numbers.Length);

            string result = longProcessor(numbers.Length, numbers);
            Assert.IsTrue(result.All(c => char.IsDigit(c)));
            return result;
        }

        public static string Process(string inStr, 
            Func<long, long, long> longProcessor)
        {
            long a, b;
            ParseTwoNumbers(inStr, out a, out b);
            return longProcessor(a, b).ToString();
        }

        public static string Process<_RetType>(
            string inStr, 
            Func<long, long[], long[], _RetType> longProcessor)
        {
            List<long> list1 = new List<long>(),
                       list2 = new List<long>();

            long firstLine;

            firstLine = ReadParallelArray(inStr, list1, list2);

            return longProcessor(firstLine,
                list1.ToArray(),
                list2.ToArray()).ToString();
        }

        public static string Process(
            string inStr,
            Func<long, long[], long[], long[]> longProcessor)
        {
            List<long> list1 = new List<long>(),
                       list2 = new List<long>();

            long firstLine;

            firstLine = ReadParallelArray(inStr, list1, list2);

            return string.Join("\n", 
                longProcessor(firstLine, list1.ToArray(), list2.ToArray()));
        }

        public static string Process(
            string inStr,
            Func<long[], long[], long[]> longProcessor)
        {
            using (StringReader reader = new StringReader(inStr))
            {
                string[] line = reader.ReadLine().Split(IgnoreChars,
                StringSplitOptions.RemoveEmptyEntries);

                long[] firstLine = new long[line.Length];

                for (int i=0; i < line.Length; i++)
                {
                    firstLine[i] = long.Parse(line[i]);
                }

               line = reader.ReadLine().Split(IgnoreChars,
                StringSplitOptions.RemoveEmptyEntries);

                long[] secondLine = new long[line.Length];
                for (int i = 0; i < line.Length; i++)
                {
                    secondLine[i] = long.Parse(line[i]);
                }

                return string.Join("\n", longProcessor(firstLine, secondLine));
            }
        }

        public static string Process(
            string inStr,
            Func<long, long[], long> longProcessor)
        {
            var lines = inStr.Split(IgnoreChars, StringSplitOptions.RemoveEmptyEntries);
            long count = long.Parse(lines.First());
            var numbers = lines.Skip(1)
                .Select(n => long.Parse(n))
                .ToArray();

            string result = longProcessor(count, numbers).ToString();
            Assert.IsTrue(result.All(c => char.IsDigit(c)));
            return result;
        }

        private static IEnumerable<long[]> ParseInputArrays(string inStr)
        {
            var lines = inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
            foreach(var line in lines)
                yield return 
                    line.Split().Select(n => long.Parse(n)).ToArray();
        }

        public static string Process(
            string inStr,
            Func<long[], long[], long[], long> longProcessor
            )
        {
            var lists = ParseInputArrays(inStr).ToArray();
            return longProcessor(lists[0], lists[1], lists[2]).ToString();
        }

        public static string Process(
            string inStr,
            Func<long[], long[], long> longProcessor
            )
        {
            var lists = ParseInputArrays(inStr).ToArray();
            return longProcessor(lists[0], lists[1]).ToString();
        }

        public static string Process(
            string inStr,
            Func<long, long[], long[]> longProcessor)
        {
            var lines = inStr.Split(IgnoreChars, StringSplitOptions.RemoveEmptyEntries);
            long count = long.Parse(lines.Take(1).First());
            var numbers = lines.Skip(1)
                .Select(n => long.Parse(n))
                .ToArray();

            Assert.AreEqual(count, numbers.Length);

            return string.Join("\n",
                longProcessor(numbers.Length, numbers.ToArray()));
        }

        public static string Process(
            string inStr,
            Func<long[], long[], long[], long[]> longProcessor)
        {
            long[] list1;
            List<long> list2 = new List<long>();
            List<long> list3 = new List<long>();
            string firstLine;

            using (StringReader reader = new StringReader(inStr))
            {
                firstLine = reader.ReadLine();
                string[] toks = firstLine.Split(IgnoreChars, 
                    StringSplitOptions.RemoveEmptyEntries);

                list1 = toks.Select(long.Parse).ToArray();

                string line = null;
                while (null != (line = reader.ReadLine()))
                {
                    long a, b;
                    ParseTwoNumbers(line, out a, out b);
                    list2.Add(a);
                    list3.Add(b);
                }

            }

            return string.Join("\n",
                longProcessor(list1,list2.ToArray(),list3.ToArray()));
        }

        private static long ReadParallelArray(string inStr, 
            List<long> list1, List<long> list2)
        {
            long firstLine;
            using (StringReader reader = new StringReader(inStr))
            {
                firstLine = long.Parse(reader.ReadLine());

                string line = null;
                while (null != (line = reader.ReadLine()))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    long a, b;
                    ParseTwoNumbers(line, out a, out b);
                    list1.Add(a);
                    list2.Add(b);
                }

            }

            return firstLine;
        }

        private static void ParseTwoNumbers(string inStr, 
            out long a, out long b)
        {
            var toks = inStr.Split(IgnoreChars, StringSplitOptions.RemoveEmptyEntries);
            a = long.Parse(toks[0]);
            b = long.Parse(toks[1]);
        }

    }
}
