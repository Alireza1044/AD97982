using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Q4FriendSuggestion : Processor
    {
        public Q4FriendSuggestion(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long, long[][], long[]>)Solve);

        public int Max = 2_000_000_000;
        public long[] Solve(long NodeCount, long EdgeCount,
                              long[][] edges, long QueriesCount,
                              long[][] Queries)
        {

            Node[] graph = new Node[NodeCount + 1];
            Node[] graphR = new Node[NodeCount + 1];
            List<long> result = new List<long>();
            for (int i = 0; i < QueriesCount; i++)
            {
                Q1MinCost.BuildGraph(edges, graph);
                BuildGraphR(edges, graphR);
                List<Node> graphList = graph.ToList();
                List<Node> graphListR = graphR.ToList();

                graphList = graph.ToList();
                graphListR = graph.ToList();
                graphList[(int)Queries[i][0]].Weight = 0;
                graphListR[(int)Queries[i][1]].Weight = 0;
                graphList.RemoveAt(0);
                graphListR.RemoveAt(0);
                result.Add(BiDirectionalDijkstra(graph, Queries[i][0], Queries[i][1], graphList,graphListR));
            }
            return result.ToArray();
        }

        public static Node[] BuildGraphR(long[][] edges, Node[] graph)
        {
            for (int i = 1; i < graph.Length; i++)
            {
                graph[i] = new Node(i);
            }
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                //graph[edges[i][0]].Children.Add((graph[edges[i][1]], edges[i][2]));
                graph[edges[i][1]].Parent.Add((graph[edges[i][0]], edges[i][2]));
            }
            return graph;
        }

        public long BiDirectionalDijkstra(Node[] Graph, long startNode, long endNode,
            List<Node> graphList, List<Node> graphListR)
        {
            List<int> proc = new List<int>();
            List<int> procR = new List<int>();
            graphListR[(int)endNode-1].Weight = 0;
            graphList[(int)startNode-1].Weight = 0;

            while(true)
            {
                Node temp = new Node(0);
                Node tempR = new Node(0);

                if (graphList.Count != 0)
                    graphList = graphList.OrderBy(x => x.Weight).ToList();

                for (int j = 0; j < graphList.Count; j++)
                {
                    if (graphList[j].IsChecked == false)
                    {
                        temp = graphList[j];
                        break;
                    }
                }

                temp.IsChecked = true;

                for (int j = 0; j < temp.Children.Count(); j++)//process
                {
                    if (temp.Weight + temp.Children[j].Item2 < temp.Children[j].Item1.Weight)//relax
                    {
                        temp.Children[j].Item1.Weight = temp.Weight + temp.Children[j].Item2;
                        proc.Add(temp.Key);
                    }
                }
                if (procR.Contains(temp.Key))
                {
                    return ShortestPath(startNode, graphList, proc, endNode, graphListR, procR);
                }
                //do the same for Reverse
                if (graphListR.Count != 0)
                    graphListR = graphListR.OrderBy(x => x.Weight).ToList();

                for (int j = 0; j < graphListR.Count; j++)
                {
                    if (graphListR[j].IsChecked == false)
                    {
                        tempR = graphListR[j];
                        break;
                    }
                }

                tempR.IsChecked = true;

                for (int j = 0; j < tempR.Parent.Count(); j++)//process
                {
                    if (tempR.Weight + tempR.Parent[j].Item2 < tempR.Parent[j].Item1.Weight)//relax
                    {
                        tempR.Parent[j].Item1.Weight = tempR.Weight + tempR.Parent[j].Item2;
                        procR.Add(tempR.Key);
                    }
                }

                if (proc.Contains(tempR.Key))
                {
                    return ShortestPath(startNode,graphList,proc,endNode,graphListR,procR);
                }

            }
        }


        public long ShortestPath(long startNode, List<Node> graphList, List<int> proc
            , long endNode, List<Node> graphListR, List<int> procR)
        {
            long Max = 2_000_000_000;
            long distance = Max;
            List<int> process = proc.Concat(procR).ToList();
            graphList = graphList.OrderBy(x => x.Key).ToList();
            graphListR = graphListR.OrderBy(x => x.Key).ToList();

            foreach (var key in process)
            {
                if (graphList[key - 1].Weight + graphListR[key - 1].Weight < distance)
                {
                    distance = graphList[key - 1].Weight + graphListR[key - 1].Weight;
                }
            }

            if (distance == Max)
                return -1;
            else
                return distance;
        }
    }
}
