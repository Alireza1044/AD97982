using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Node
    {
        long Max = 2_000_000_000;
        public List<(Node, long)> Children { get; set; }
        public List<(Node, long)> Parent { get; set; }
        public Node Prev { get; set; }
        public long Weight { get; set; }
        public int Key { get; set; }
        public bool IsChecked { get; set; }
        public Node(int key)
        {
            Children = new List<(Node, long)>();
            Parent = new List<(Node, long)>();
            Weight = Max;
            Key = key;
            IsChecked = false;
        }
    }

    public class Q1MinCost : Processor
    {
        public Q1MinCost(string testDataName) : base(testDataName) { }

        public static long Max = 2_000_000_000;
        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, long, long>)Solve);


        public long Solve(long nodeCount, long[][] edges, long startNode, long endNode)
        {
            //Write Your Code Here
            Node[] graph = new Node[nodeCount + 1];
            BuildGraph(edges, graph);
            return FindMinimumCost(graph, startNode, endNode);
        }

        public static long FindMinimumCost(Node[] graph, long startNode, long endNode)
        {
            List<Node> nodes = new List<Node>();
            graph[startNode].Weight = 0;
            var graphList = graph.ToList();
            graphList.RemoveAt(0);

            while (true)
            {
                Node temp = new Node(0);
                if (graphList.Count != 0)
                    graphList = graphList.OrderBy(x => x.Weight).ToList();


                for (int i = 0; i < graphList.Count; i++)
                {
                    if (graphList[i].IsChecked == false)
                    {
                        temp = graphList[i];
                        nodes.Add(temp);
                        break;
                    }
                }

                temp.IsChecked = true;
                if (temp.Key == endNode)
                {
                    if (temp.Weight == Max)
                        break;
                    else
                        return temp.Weight;
                }

                for (int i = 0; i < temp.Children.Count(); i++)
                {
                    if (temp.Weight + temp.Children[i].Item2 < temp.Children[i].Item1.Weight)
                        temp.Children[i].Item1.Weight = temp.Weight + temp.Children[i].Item2;

                }
            }
            return -1;
        }

        public static Node[] BuildGraph(long[][] edges, Node[] graph)
        {
            for (int i = 1; i < graph.Length; i++)
            {
                graph[i] = new Node(i);
            }
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                graph[edges[i][0]].Children.Add((graph[edges[i][1]], edges[i][2]));
                graph[edges[i][1]].Parent.Add((graph[edges[i][0]], edges[i][2]));
            }
            return graph;
        }
    }
}
