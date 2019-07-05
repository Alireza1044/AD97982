using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A11
{
    public class Node
    {
        public List<long> Connected { get; set; } = new List<long>();
        public long FunFactor { get; set; }
        public bool IsChecked { get; set; }
        public Node(long funFactor = -1)
        {
            IsChecked = false;
            this.FunFactor = funFactor;
        }
    }
    public class Q2FunParty : Processor
    {
        public Q2FunParty(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[], long[][], long>)Solve);

        public virtual long Solve(long n, long[] funFactors, long[][] hierarchy)
        {
            Node[] graph = BuildGraph(n, funFactors, hierarchy);
            long root = -1;

            bool[] isRoot = new bool[n + 1];

            for (int i = 0; i < hierarchy.Length; i++)
            {
                isRoot[hierarchy[i][1]] = true;
            }

            for (int i = 1; i < isRoot.Length; i++)
            {
                if (!isRoot[i])
                {
                    root = i;
                    break;
                }
            }

            long result = int.MaxValue;

            result = FunParty(graph, root, result);

            return result;
        }

        private long FunParty(Node[] graph, long root, long result)
        {
            if (result == int.MaxValue)
            {
                if (graph[root].Connected.Count == 0)
                    result = graph[root].FunFactor;
                else
                {
                    long fTemp = graph[root].FunFactor;
                    for (int i = 0; i < graph[root].Connected.Count; i++)
                    {
                        graph[graph[root].Connected[i]].Connected.Remove(root);
                    }
                    for (int i = 0; i < graph[root].Connected.Count; i++)
                    {
                        for (int j = 0; j < graph[graph[root].Connected[i]].Connected.Count; j++)
                        {
                            graph[graph[graph[root].Connected[i]].Connected[j]].Connected.Remove(graph[root].Connected[i]);
                            fTemp += FunParty(graph, graph[graph[root].Connected[i]].Connected[j], result);
                        }
                    }
                    long sTemp = 0;
                    for (int i = 0; i < graph[root].Connected.Count; i++)
                    {
                        graph[graph[root].Connected[i]].Connected.Remove(root);
                        sTemp += FunParty(graph, graph[root].Connected[i], result);
                    }
                    result = Math.Max(sTemp, fTemp);
                }
            }
            return result;
        }

        private Node[] BuildGraph(long n, long[] funFactors, long[][] hierarchy)
        {
            Node[] graph = new Node[n + 1];

            for (int i = 1; i < graph.Length; i++)
            {
                graph[i] = new Node(funFactors[i - 1]);
            }

            for (int i = 0; i < hierarchy.Length; i++)
            {
                graph[hierarchy[i][0]].Connected.Add(hierarchy[i][1]);
                graph[hierarchy[i][1]].Connected.Add(hierarchy[i][0]);
            }
            return graph;
        }
    }
}
