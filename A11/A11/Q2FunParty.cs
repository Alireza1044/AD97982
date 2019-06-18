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
        public List<long> Children { get; set; } = new List<long>();
        public long Parent { get; set; }
        public long FunFactor { get; set; }
        public Node(long parent = -1, long funFactor = -1)
        {
            this.Parent = parent;
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

            for (int i = 1; i < graph.Length; i++)
            {
                if (graph[i].Parent == -1)
                {
                    root = i;
                    break;
                }
            }

            long result = int.MaxValue;

            result = FunParty(graph,root,result);

            return result;
        }

        private long FunParty(Node[] graph, long root, long result)
        {
            if(result == int.MaxValue)
            {
                if (graph[root].Children.Count == 0)
                    result = graph[root].FunFactor;
                else
                {
                    long fTemp = graph[root].FunFactor;
                    for (int i = 0; i < graph[root].Children.Count; i++)
                    {
                        for (int j = 0; j < graph[graph[root].Children[i]].Children.Count; j++)
                        {
                            fTemp += FunParty(graph, graph[graph[root].Children[i]].Children[j],result);
                        }
                    }
                    long sTemp = 0;

                    for (int i = 0; i < graph[root].Children.Count; i++)
                    {
                        sTemp += FunParty(graph,graph[root].Children[i],result);
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
                graph[i] = new Node();
            }

            for (int i = 0; i < hierarchy.Length; i++)
            {
                if (graph[hierarchy[i][0]].FunFactor == -1)
                {
                    if (graph[hierarchy[i][1]].FunFactor == -1)
                    {
                        graph[hierarchy[i][0]] = new Node(funFactor: funFactors[hierarchy[i][0] - 1]);
                        graph[hierarchy[i][0]].Children.Add(hierarchy[i][1]);
                        graph[hierarchy[i][1]] = new Node(funFactor: funFactors[hierarchy[i][1] - 1], parent: hierarchy[i][0]);
                    }
                    else
                    {
                        graph[hierarchy[i][1]].Children.Add(hierarchy[i][0]);
                        graph[hierarchy[i][0]] = new Node(funFactor:funFactors[hierarchy[i][0] - 1],parent: hierarchy[i][1]);
                    }
                }
                else
                {
                    if (graph[hierarchy[i][1]].FunFactor == -1)
                    {
                        graph[hierarchy[i][0]].Children.Add(hierarchy[i][1]);
                        graph[hierarchy[i][1]] = new Node(funFactor: funFactors[hierarchy[i][1] - 1], parent: hierarchy[i][0]);
                    }
                    else
                    {
                        long temp = hierarchy[i][1];
                        while (graph[temp].Parent != -1)
                            temp = graph[temp].Parent;
                        graph[hierarchy[i][0]].Children.Add(temp);
                        graph[temp].Parent = hierarchy[i][0];
                    }
                }
            }

            return graph;
        }
    }
}
