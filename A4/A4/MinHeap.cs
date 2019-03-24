using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static A4.Q1BuildingRoads;

namespace A4
{
    public class MinHeap
    {
        public static int[] Heap { get; set; }
        private static int[] heapIndex { get; set; }
        public static double[] Priorities { get; set; }
        private static int size { get; set; }
        public MinHeap(int capacity)
        {
            Heap = new int[capacity];
            heapIndex = new int[capacity+1];
            Priorities = new double[capacity];
            size = 0;
        }

        public int ExtractMin()
        {
            var min = Heap[0];
            Swap(0, size - 1);
            size--;
            SiftDown(0);
            return min;
        }

        public void BuildHeap(Node[] graph,int startNode)
        {
            for (int i = 1; i < graph.Length; i++)
            {
                Priorities[i - 1] = graph[i].Potential;
                Heap[i - 1] = graph[i].Key;
                heapIndex[i] = i - 1;
            }
            SiftUp(startNode-1);
            size = graph.Length-1;
        }

        private static void SiftUp(int idx)
        {
            while(idx > 0 && Priorities[idx] < Priorities[Parent(idx)])
            {
                Swap(idx, Parent(idx));
                idx = Parent(idx);
            }
        }

        private static void SiftDown(int idx)
        {
            var left = LeftChild(idx);
            var right = RightChild(idx);
            int max = 0;
            if (left < size && Priorities[left] < Priorities[idx])
                max = left;
            else max = idx;
            if (right < size && Priorities[right] < Priorities[max])
                max = right;
            if(max != idx)
            {
                Swap(idx, max);
                SiftDown(max);
            }
        }

        public void ChangePriority(int key,double newPriority)
        {
            int idx = heapIndex[key];
            Priorities[idx] = newPriority;
            SiftUp(idx);
        }

        private static void Swap(int idx1, int idx2)
        {
            heapIndex[Heap[idx1]] = idx2;
            heapIndex[Heap[idx2]] = idx1;

            var temp = Heap[idx1];
            Heap[idx1] = Heap[idx2];
            Heap[idx2] = temp;

            var temp2 = Priorities[idx1];
            Priorities[idx1] = Priorities[idx2];
            Priorities[idx2] = temp2;
        }

        public static int Parent(int idx) => (idx - 1) / 2;
        public static int RightChild(int idx) => idx * 2 + 2;
        public static int LeftChild(int idx) => idx * 2 + 1;
    }
}
