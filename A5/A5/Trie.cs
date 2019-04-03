using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5
{
    public class Trie
    {
        List<Trie> Children { get; set; }
        public int Key { get; set; }
        public char Charachter { get; set; }
        public Trie Parent { get; set; }
        public int Position { get; set; }
        public bool IsLeaf { get; set; }
        public bool InText { get; private set; }

        public Trie(int key, char charachter, Trie parent = null)
        {
            this.Key = key;
            Charachter = charachter;
            Parent = parent;
            Position = -1;
            IsLeaf = false;
            InText = false;
            Children = new List<Trie>();
        }
        public static Trie ConstructTrie(long n, string[] patterns)
        {
            int key = 1;
            Trie root = new Trie(0, '-');
            foreach (var pattern in patterns)
            {
                Insert(root, pattern, ref key);
            }
            return root;
        }

        public static Trie ConstructSuffixTree(string text)
        {
            int key = 1;
            int position = 0;
            Trie root = new Trie(0, '-');

            while (text.Any())
            {
                Insert(root, text,ref key, position);
                text = text.Remove(0,1);
                position++;
            }

            return root;
        }

        public static long[] Search(Trie root, string text,string[] patterns)
        {
            List<long> result = new List<long>();
            int i = 0;

            while (i <= text.Length)
            {
                var res = Has(root, text, i);
                if (res)
                    result.Add(i);
                i++;
            }

            if (!result.Any())
                return new long[] { -1 };

            return result.ToArray();
        }

        private static bool Has(Trie root, string text, int i)
        {
            Trie temp = root;

            while (i < text.Length)
            {
                if (temp.Children.Exists(x => x.Charachter == text[i]))
                {
                    temp = temp.Children.Find(x => x.Charachter == text[i]);
                    i++;
                    if (temp.IsLeaf)
                    {
                        return true;
                    }
                    continue;
                }
                break;
            }
            return false;
        }

        internal static long[] FindPatterns(Trie root, string[] patterns)
        {
            List<long> result = new List<long>();
            foreach (var pattern in patterns)
            {
                DFS(root,pattern,ref result);
            }
            result.Reverse();
            return result.ToArray();
        }

        private static void DFS(Trie root, string pattern, ref List<long> result)
        {
            Stack<Trie> stack = new Stack<Trie>();
            Trie temp = root;
            stack.Push(temp);
            int i = 0;
            bool flag = false;
            while (stack.Any())
            {
                temp = stack.Peek();
                if (temp.Charachter != '-')
                {
                    if (i == pattern.Length - 1 && pattern[i] == temp.Charachter)
                    {
                        FindPosition(temp, ref result);
                        flag = true;
                        i = 0;
                        stack.Pop();
                    }
                    else if (i < pattern.Length && pattern[i] == temp.Charachter)
                    {
                        stack.Pop();
                        i++;
                    }
                    else if (pattern[i] != temp.Charachter)
                    {
                        i = 0;
                        stack.Pop();
                    }
                }
                else
                    stack.Pop();
                for (int j = 0; j < temp.Children.Count; j++)
                {
                    stack.Push(temp.Children[j]);
                }
            }
            if(!flag)
                result.Add(-1);
        }

        //private static int Contains(Trie root, string pattern)
        //{
        //    Trie temp = root;

        //    for (int i = 0; i < pattern.Length; i++)
        //    {
        //        if (i == pattern.Length - 1)
        //        {
        //            if (temp.Children.Exists(x => x.Charachter == pattern[i]))
        //            {
        //                temp = temp.Children.Find(x => x.Charachter == pattern[i]);
        //                return FindPosition(temp);
        //            }
        //            break;
        //        }
        //        else
        //        {
        //            if (temp.Children.Exists(x => x.Charachter == pattern[i]))
        //            {
        //                temp = temp.Children.Find(x => x.Charachter == pattern[i]);
        //                continue;
        //            }
        //            break;
        //        }
        //    }
        //    return -1;
        //}

        private static void FindPosition(Trie root, ref List<long> result)
        {
            Trie temp = root;
            while (true)
            {
                if (temp.Position != -1)
                    result.Add(temp.Position);
                if (!temp.Children.Any())
                    break;
                temp = temp.Children.First();
            }
        }

        private static void Insert(Trie root, string pattern, ref int key, int position = -1)
        {
            var word = pattern.ToCharArray();
            Trie temp = root;

            for (int i = 0; i < word.Length; i++)
            {
                if (temp.Children.Exists(x => x.Charachter == word[i]))
                {
                    temp = temp.Children.Find(x => x.Charachter == word[i]);
                    continue;
                }
                temp.Children.Add(new Trie(key, word[i], temp));
                key++;
                temp = temp.Children[temp.Children.Count - 1];
            }
            temp.Position = position;
            temp.IsLeaf = true;
        }

        public static string[] Print(Trie root)
        {
            List<string> result = new List<string>();

            Stack<Trie> stack = new Stack<Trie>();
            stack.Push(root);

            while (stack.Any())
            {
                Trie temp = stack.Pop();
                if(temp.Parent != null)
                {
                    result.Add(temp.Parent.Key.ToString() + "->" + temp.Key.ToString() + ":" + temp.Charachter.ToString());
                }
                for (int i = 0; i < temp.Children.Count; i++)
                {
                    stack.Push(temp.Children[i]);
                }
            }

            return result.ToArray();
        }
    }
}
