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
        public const long Max = 2_000_000_000;
        public struct Node
        {
            public List<(Node, long)> Children { get; set; }
            public List<(Node, long)> Parent { get; set; }
            public long Weight { get; set; }
            public int Key { get; set; }
            public bool IsChecked { get; set; }
            public Node(int key=0)
            {
                Children = new List<(Node, long)>();
                Parent = new List<(Node, long)>();
                Weight = Max;
                Key = key;
                IsChecked = false;
            }
            public Node(int key, List<(Node, long)> children, List<(Node, long)> parent,long weight,bool isChecked)
            {
                Children = children;
                Parent = parent;
                Weight = weight;
                Key = key;
                IsChecked = isChecked;
            }
        }

        public long[] Solve(long NodeCount, long EdgeCount,
                              long[][] edges, long QueriesCount,
                              long[][] Queries)
        {

            Node[] graph = new Node[NodeCount + 1];
            Node[] graphR = new Node[NodeCount + 1];
            List<long> result = new List<long>();

            BuildGraph(edges,ref graph);
            BuildReverseGraph(edges,ref graphR);

            

            for (int i = 0; i < QueriesCount; i++)
            {
                //for (int j = 1; j < graph.Count(); j++)
                //{
                //    graph[j].IsChecked = false;
                //    graphR[j].IsChecked = false;
                //    graph[j].Weight = Max;
                //    graphR[j].Weight = Max;
                //}

                //List<Node> graphList = graph.ToList();
                //List<Node> graphListR = graphR.ToList();              

                result.Add(BiDirectionalDijkstra(graph,graphR, Queries[i][0], Queries[i][1]));
            }
            return result.ToArray();
        }
        public static void BuildGraph(long[][] edges,ref Node[] graph)
        {
            for (int i = 1; i < graph.Length; i++)
            {
                graph[i] = new Node(i);
            }
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                graph[edges[i][0]].Children.Add((graph[edges[i][1]], edges[i][2]));
            }
        }
        public static void BuildReverseGraph(long[][] edges,ref Node[] graph)
        {
            for (int i = 1; i < graph.Length; i++)
            {
                graph[i] = new Node(i);
            }
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                graph[edges[i][1]].Parent.Add((graph[edges[i][0]], edges[i][2]));
            }
        }

        public long BiDirectionalDijkstra(Node[] Graph,Node[] GraphR ,long startNode, long endNode)
        {
            List<int> proc = new List<int>();
            List<int> procR = new List<int>();

            var graphDict = Graph.ToDictionary(x => x.Key);
            var reverseGraphDict = GraphR.ToDictionary(x => x.Key);

            graphDict.Remove(0);
            reverseGraphDict.Remove(0);

            graphDict[(int)startNode] = new Node(Graph[startNode].Key, Graph[startNode].Children,
                Graph[startNode].Parent,0, Graph[startNode].IsChecked);
            reverseGraphDict[(int)endNode] = new Node(GraphR[endNode].Key, GraphR[endNode].Children,
                GraphR[endNode].Parent,0, GraphR[endNode].IsChecked);

            while (true)
            {
                Node temp = new Node(0);
                Node tempR = new Node(0);

                if (Graph.Length != 0)
                    graphDict = graphDict.OrderBy(x => x.Value.Weight).ToDictionary(x => x.Key, x => x.Value);

                //for (int j = 0; j < Graph.Length; j++)
                //{
                //    if (isChecked[Graph[j].Key].Item2 == false)
                //    {
                //        temp = Graph[j];
                //        break;
                //    }
                //}
                int tempKey = graphDict.First(x => !x.Value.IsChecked).Key;
                temp = Graph[tempKey];
                //Graph[tempKey].IsChecked = true;
                graphDict[tempKey] = 
                    new Node(graphDict[tempKey].Key, graphDict[tempKey].Children, graphDict[tempKey].Parent, graphDict[tempKey].Weight,true);
                
                for (int j = 0; j < temp.Children.Count(); j++)//process
                {
                    if (graphDict[temp.Key].Weight + graphDict[temp.Key].Children[j].Item2 < graphDict[temp.Children[j].Item1.Key].Weight)//relax
                    {
                        //temp.Children[j].Item1.Weight = temp.Weight + temp.Children[j].Item2;
                        //Graph[temp.Children[j].Item1.Key].Weight = 
                        //    graphDict[temp.Key].Weight + graphDict[temp.Children[j].Item1.Key].Weight;
                        graphDict[temp.Children[j].Item1.Key] = 
                            new Node(graphDict[temp.Children[j].Item1.Key].Key, graphDict[temp.Children[j].Item1.Key].Children,
                            graphDict[temp.Children[j].Item1.Key].Parent,
                            graphDict[temp.Key].Weight + graphDict[temp.Key].Children[j].Item2,
                            graphDict[temp.Children[j].Item1.Key].IsChecked);
                        //Graph[temp.Children[j].Item1.Key];
                    }
                }
                proc.Add(temp.Key);
                if (procR.Contains(temp.Key))
                {
                    return ShortestPath(startNode, graphDict, proc, endNode, reverseGraphDict, procR);
                }

                //do the same for Reverse

                if (Graph.Length != 0)
                    reverseGraphDict = reverseGraphDict.OrderBy(x => x.Value.Weight).ToDictionary(x => x.Key, x => x.Value);

                //for (int j = 0; j < Graph.Length; j++)
                //{
                //    if (isCheckedR[Graph[j].Key].Item2 == false)
                //    {
                //        tempR = Graph[j];
                //        break;
                //    }
                //}

                int tempRKey = reverseGraphDict.First(x => !x.Value.IsChecked).Key;
                tempR = GraphR[tempRKey];
                //GraphR[tempRKey].IsChecked = true;
                reverseGraphDict[tempRKey] = 
                    new Node(reverseGraphDict[tempRKey].Key, reverseGraphDict[tempRKey].Children, 
                    reverseGraphDict[tempRKey].Parent, reverseGraphDict[tempRKey].Weight,true);

                for (int j = 0; j < tempR.Parent.Count(); j++)//process
                {
                    if (reverseGraphDict[tempR.Key].Weight + reverseGraphDict[tempR.Key].Parent[j].Item2 < reverseGraphDict[tempR.Parent[j].Item1.Key].Weight)//relax
                    {
                        //tempR.Parent[j].Item1.Weight = tempR.Weight + tempR.Parent[j].Item2;
                        //GraphR[tempR.Children[j].Item1.Key].Weight =
                        //    reverseGraphDict[tempR.Key].Weight + reverseGraphDict[tempR.Children[j].Item1.Key].Weight;
                        reverseGraphDict[tempR.Parent[j].Item1.Key] = new Node(reverseGraphDict[tempR.Parent[j].Item1.Key].Key,
                            reverseGraphDict[tempR.Parent[j].Item1.Key].Children, reverseGraphDict[tempR.Parent[j].Item1.Key].Parent,
                            reverseGraphDict[tempR.Key].Weight + reverseGraphDict[tempR.Key].Parent[j].Item2,
                            reverseGraphDict[tempR.Parent[j].Item1.Key].IsChecked);
                    }
                }
                procR.Add(tempR.Key);

                if (proc.Contains(tempR.Key))
                {
                    return ShortestPath(startNode, graphDict, proc, endNode, reverseGraphDict, procR);
                }

            }
        }


        public long ShortestPath(long startNode, Dictionary<int, Node> graphDict, List<int> proc
            , long endNode, Dictionary<int, Node> reverseGraphDict, List<int> procR)
        {
            long distance = Max;
            List<int> process = proc.Concat(procR).ToList();
            //isChecked = isChecked.OrderBy(x => x.Key).ToDictionary(x=>x.Key,x=>x.Value);
            //isCheckedR = isCheckedR.OrderBy(x => x.Key).ToDictionary(x=>x.Key,x=>x.Value);

            foreach (var key in process)
            {
                if (graphDict[key].Weight + reverseGraphDict[key].Weight < distance)
                {
                    distance = graphDict[key].Weight + reverseGraphDict[key].Weight;
                }
            }

            if (distance == Max)
                return -1;
            else
                return distance;
        }
    }
}
