using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A7
{
    public class SuffixArray
    {
        public static long[] BuildSuffixArray(string text)
        {
            long[] order = new long[text.Length];
            long[] cls = new long[text.Length];

            order = SortCharachters(text);
            cls = ComputeCharClasses(text,order);

            int l = 1;

            while(l < text.Length)
            {
                order = SortDoubled(text,l,order,cls);
                cls = UpdateClasses(order, cls, l);
                l *= 2;
            }

            return order;
        }

        private static long[] UpdateClasses(long[] newOrder, long[] cls, int l)
        {
            var n = newOrder.Length;
            long[] newCls = new long[n];
            newCls[newOrder[0]] = 0;

            for (int i = 1; i < n - 1; i++)
            {
                var current = newOrder[i];
                var previous = newOrder[i - 1];
                var middle = current + l;
                var middlePrev = (previous + l) % n;
                if (cls[current] != cls[previous] || cls[middle] != cls[middlePrev])
                    newCls[current] = newCls[previous] + 1;
                else
                    newCls[current] = newCls[previous];
            }
            return newCls;
        }

        private static long[] SortDoubled(string text, int l, long[] order, long[] cls)
        {
            long[] count = new long[text.Length];
            long[] newOrder = new long[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                count[cls[i]]++;
            }

            for (int i = 1; i < text.Length; i++)
            {
                count[i] += count[i] + count[i - 1];
            }

            for (int i = text.Length-1; i >= 0; i--)
            {
                var start = (order[i] - l + text.Length) % text.Length;
                var cl = cls[start];
                count[cl]--;
                newOrder[count[cl]] = start;
            }
            return newOrder;
        }

        private static long[] ComputeCharClasses(string text, long[] order)
        {
            long[] cls = new long[text.Length];
            cls[order[0]] = 0;
            for (int i = 1; i < text.Length; i++)
            {
                if (text[(int)order[i]] != text[(int)order[i - 1]])
                    cls[(int)order[i]] = cls[(int)order[i - 1]] + 1;
                else
                    cls[(int)order[i]] = cls[(int)order[i - 1]];
            }
            return cls;
        }

        private static long[] SortCharachters(string text)
        {
            long[] order = new long[text.Length];
            long[] count = new long[5];
            for (int i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case 'A':
                        count[1]++;
                        break;
                    case 'C':
                        count[2]++;
                        break;
                    case 'G':
                        count[3]++;
                        break;
                    case 'T':
                        count[4]++;
                        break;
                    case '$':
                        count[0]++;
                        break;
                }
            }
            for (int i = 1; i < 5; i++)
            {
                count[i] = count[i] + count[i - 1];
            }
            for (int i = text.Length - 1; i >= 0; i--) 
            {
                switch (text[i])
                {
                    case 'A':
                        count[1]--;
                        order[count[1]] = i;
                        break;
                    case 'C':
                        count[2]--;
                        order[count[2]] = i;
                        break;
                    case 'G':
                        count[3]--;
                        order[count[3]] = i;
                        break;
                    case 'T':
                        count[4]--;
                        order[count[4]] = i;
                        break;
                    case '$':
                        count[0]--;
                        order[count[0]] = i;
                        break;
                }
            }

            return order;
        }
    }
}
