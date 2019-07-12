using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A11
{
    public class Edge
    {
        public List<long> Connected { get; set; }
        public List<long> ConnectedReverse { get; set; }
        public long Key { get; set; }
        public long InDegree { get; set; }
        public bool IsVisited { get; set; }
        public Edge(long key)
        {
            Key = key;
            IsVisited = false;
            InDegree = 0;
            Connected = new List<long>();
            ConnectedReverse = new List<long>(); ;
        }
    }
}
