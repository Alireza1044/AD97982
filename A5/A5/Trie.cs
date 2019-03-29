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
        public Trie(int key, char charachter, Trie parent = null)
        {
            this.Key = key;
            Charachter = charachter;
            Parent = parent;
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

        private static void Insert(Trie root, string pattern, ref int key)
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
