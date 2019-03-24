using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using static A4.Q1BuildingRoads;

namespace A4
{
    public class Q2Clustering : Processor
    {
        public Q2Clustering(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, double>)Solve);


        public double Solve(long pointCount, long[][] points, long clusterCount)
        {
            Node[] graph = new Node[pointCount];
            BuildGraph(graph, points);
            return BuildMST(graph,clusterCount);
        }

        private static double BuildMST(Node[] graph,long clusterCount)
        {
            graph[0].Distance = 0;
            List<Node> mst = new List<Node>();
            while (mst.Count() < graph.Length)
            {
                var minDist = FindMinDistance(graph);
                mst.Add(graph[minDist]);
                graph[minDist].IsInMST = true;
            }
            mst = mst.OrderByDescending(x => x.Distance).ToList();
            return Math.Round(mst[(int)(clusterCount)-2].Distance, 6);
        }
    }
}
