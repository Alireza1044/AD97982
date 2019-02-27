using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A2
{
    public class Q2BipartiteGraph : Processor
    {
        public Q2BipartiteGraph(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);


        public long Solve(long NodeCount, long[][] edges)
        {
            //Write your code here]
            Q1ShortestPath q1 = new Q1ShortestPath("A1");
            Node[] Graph = new Node[NodeCount + 1];
            q1.BuildTree(Graph, edges);

            return CheckBipartite(Graph);
        }

        private long CheckBipartite(Node[] graph)
        {
            graph[1].Color = "Red";
            Queue<Node> nodes = new Queue<Node>();
            nodes.Enqueue(graph[1]);

            while (nodes.Count != 0)
            {
                Node temp = nodes.Dequeue();
                if (temp.IsChecked == true)
                    continue;
                temp.IsChecked = true;
                foreach (var node in temp.Connected)
                {
                    if (node.Color == temp.Color)
                        return 0;
                    else
                    {
                        switch(temp.Color)
                        {
                            case "Red":
                                node.Color = "Blue";
                                break;
                            case "Blue":
                                node.Color = "Red";
                                break;
                        }
                    }
                    if (node.IsChecked == false)
                        nodes.Enqueue(node);
                }
            }
            return 1;
        }
    }

}
