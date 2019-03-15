using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A4
{
    public class Q1BuildingRoads : Processor
    {
        public const double Max = 2_000_000_000;
        public struct Edge
        {
            public int Key { get; set; }
            public Coordinates Destination { get; set; }
            public double Weight { get; set; }
            public Edge(int key, Coordinates destintation, double weight)
            {
                Key = key;
                Destination = destintation;
                Weight = weight;
            }
        }

        public struct Coordinates
        {
            public double X;
            public double Y;
            public Coordinates(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        public struct Node
        {
            public int Key { get; set; }
            public List<Edge> Children { get; set; }
            public double Distance { get; set; }
            public Coordinates CoOrds { get; set; }
            public bool IsInMST { get; set; }
            public Node(int key, long x = 0, long y = 0)
            {
                Key = key;
                Children = new List<Edge>();
                Distance = Max;
                IsInMST = false;
                CoOrds = new Coordinates(x, y);
            }
        }
        public Q1BuildingRoads(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], double>)Solve);


        public double Solve(long pointCount, long[][] points)
        {
            Node[] graph = new Node[pointCount];
            BuildGraph(graph, points);
            return BuildMST(graph);
        }

        public static void BuildGraph(Node[] graph, long[][] points)
        {
            for (int i = 0; i < graph.Length; i++)
            {
                graph[i] = new Node(i, points[i][0], points[i][1]);
            }
            for (int i = 0; i < graph.Length; i++)
            {
                for (int j = i + 1; j < graph.Length; j++)
                {
                    double distance = CalculateDistance(graph[i].CoOrds, graph[j].CoOrds);
                    graph[i].Children.Add(new Edge(j, graph[j].CoOrds, distance));
                    graph[j].Children.Add(new Edge(i, graph[i].CoOrds, distance));
                }
            }
        }

        public static double CalculateDistance(Coordinates startNode, Coordinates endNode)
        {
            var x2 = Math.Pow(startNode.X - endNode.X, 2);
            var y2 = Math.Pow(startNode.Y - endNode.Y, 2);
            var result = Math.Pow(x2 + y2, 0.5);
            return Math.Abs(result);
        }

        private static double BuildMST(Node[] graph)
        {
            graph[0].Distance = 0;
            List<Node> mst = new List<Node>();
            double distance = 0;
            while (mst.Count() < graph.Length)
            {
                var minDist = FindMinDistance(graph);
                mst.Add(graph[minDist]);
                graph[minDist].IsInMST = true;
                distance += graph[minDist].Distance;
            }
            return Math.Round(distance, 6);
        }

        public static int FindMinDistance(Node[] graph)
        {
            double dist = Max;
            int index = -1;
            for (int i = 0; i < graph.Length; i++)
            {
                if (graph[i].Distance < dist && !graph[i].IsInMST)
                {
                    dist = graph[i].Distance;
                    index = i;
                }
            }
            Relax(graph, graph[index]);
            return index;
        }

        public static void Relax(Node[] graph, Node node)
        {
            foreach (var child in node.Children)
            {
                if (child.Weight < graph[child.Key].Distance && !graph[child.Key].IsInMST)
                    graph[child.Key].Distance = child.Weight;
            }
        }
    }
}
