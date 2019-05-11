using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A8
{
    public class Q1Evaquating : Processor
    {
        public Q1Evaquating(string testDataName) : base(testDataName)
        {
            //this.ExcludeTestCaseRangeInclusive(1, 1);
            //this.ExcludeTestCaseRangeInclusive(11, 100);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long>)Solve);

        public virtual long Solve(long nodeCount, long edgeCount, long[][] edges)
        {
            //Graph[] graph = Graph.BuildGraph(nodeCount,edgeCount,edges);
            //Graph[] rGraph = Graph.BuildGraph(nodeCount, edgeCount, edges);

            int[,] graph = new int[nodeCount + 1, nodeCount+ 1];
            int[,] residualGraph = new int[nodeCount + 1, nodeCount + 1];

            BuildGraph(graph, edges);
            BuildGraph(residualGraph, edges);
            
            return FindMaxFlow(graph, residualGraph, nodeCount);
        }

        private long FindMaxFlow(int[,] graph, int[,] residualGraph, long nodeCount)
        {
            int source = 1;
            int sink = (int)nodeCount;
            int[] parent = new int[nodeCount + 1];
            long maxFlow = 0;

            while (BFS(residualGraph, source, sink, parent,nodeCount))
            {
                long pathFlow = int.MaxValue;
                int i = sink;

                while (i != source && i != 0)
                {
                    pathFlow = Math.Min(pathFlow, residualGraph[parent[i], i]);
                    i = parent[i];
                }

                i = sink;

                while (i != source && i != 0)
                {
                    residualGraph[parent[i], i] -= (int)pathFlow;
                    residualGraph[i, parent[i]] += (int)pathFlow;
                    i = parent[i];
                }

                maxFlow += pathFlow;
                if (pathFlow == 0) break;
            }

            return maxFlow;
        }

        private bool BFS(int[,] residualGraph, int source, int sink, int[] parent,long nodeCount)
        {
            bool[] isVisited = new bool[nodeCount + 1];

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(source);
            isVisited[source] = true;
            parent[source] = -1;

            while(queue.Count != 0)
            {
                int temp = queue.Dequeue();
                for (int i = 1; i <= nodeCount; i++)
                {
                    if(isVisited[i] == false && residualGraph[temp,i] > 0)
                    {
                        queue.Enqueue(i);
                        parent[i] = temp;
                        isVisited[i] = true;
                    }
                }
            }

            return isVisited[source] == true;
        }

        private void BuildGraph(int[,] graph, long[][] edges)
        {
            for (int i = 0; i < edges.Length; i++)
            {
                graph[(int)edges[i][0], (int)edges[i][1]] += (int)edges[i][2];
            }
        }
    }
}
