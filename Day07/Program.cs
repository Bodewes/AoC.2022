using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day07
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = File.ReadAllLines("test.txt");
            var input = File.ReadAllLines("input.txt");
            //var input = File.ReadAllLines("inputFrank.txt");

            var solver = new Solution(input);

            Console.WriteLine("Day 07" + Environment.NewLine);
            TimeAction<int>(solver.Part1);
            TimeAction<int>(solver.Part2);

            Console.ReadKey();
        }

        private static void TimeAction<T>(Func<T> action)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var result = action();

            Console.WriteLine($"Answer = {result}");

            sw.Stop();
            Console.WriteLine($"Took: {sw.Elapsed}" + Environment.NewLine);
        }
    }

    public class Solution
    {
        private readonly string[] input;

        private int totalSize = 70_000_000;
        private int required =  30_000_000;

        public Solution(string[] input)
        {
            this.input = input;
        }

        public int Part1()
        {
            Item root = new Item("/", null);

            ParseInput(root);

            root.Print();

            // visit all folders and sum if TotalSize <= 100000;
            var total = VisitAllSum(root, (item) => (item.TotalSize <= 100000 ? item.TotalSize : 0));

            return total;
        }

        public int Part2()
        {
            Item root = new Item("/", null);

            ParseInput(root);

            var totalUsed = root.TotalSize;

            var free = totalSize - totalUsed;
            var deleteAmount = required - free;

            var smallest = VisitAllSearch(root, deleteAmount);

            return smallest;
        }

        private void ParseInput(Item root)
        {
            Item current = root;
            foreach (var line in this.input)
            {
                if (line.StartsWith('$')) // command
                {
                    if (line[2] == 'c') //cd
                    {
                        if (line[5] == '/') // select root
                        {
                            current = root;
                        }
                        else if (line[5] == '.')//move folder up
                        {
                            current = current.Parent;
                        }
                        else // change into folder
                        {
                            var name = line.Substring(5);
                            var sub = GetOrAddSub(current, name);
                            current = sub;
                        }
                    }
                    else // ls
                    {
                        // noop
                    }
                }
                else // output
                {
                    var tokens = line.Split(' ');
                    if (tokens[0] == "dir")
                    {
                        var name = tokens[1];
                        GetOrAddSub(current, name);
                    }
                    else
                    {
                        var size = int.Parse(tokens[0]);
                        var name = tokens[1];
                        var file = new Item(name, size, current);
                        current.Children.Add(file);
                    }

                }
            }
        }

        private int VisitAllSum(Item current, Func<Item, int> func)
        {
            var thisSize = func(current);
            Console.WriteLine(current.Name + "  " + thisSize);

            var childSize = current.Children.Where(f => f.Size == 0).Sum(f => VisitAllSum(f, func));

            return thisSize + childSize;
        }

        private int VisitAllSearch(Item current, int deleteAmount) // find smallest dir above deleteAmount
        {
            if (current.TotalSize >= deleteAmount) // valid candidate;
            {
                
                var subFolders = current.Children.Where(f => f.Size == 0).ToList();

                if (subFolders.Any())
                {
                    var minC = subFolders.Min(f => VisitAllSearch(f, deleteAmount));
                    return Math.Min(minC, current.TotalSize);
                }
                else
                {
                    return current.TotalSize;
                }

            }
            else
            {
                // too small, thus all children also too small.
                return int.MaxValue;
            }
        }


        private Item GetOrAddSub(Item current, string name)
        {
            var sub = current.Children.SingleOrDefault(x => x.Name == name);
            if (sub == null)
            {
                sub = new Item(name, current);
                current.Children.Add(sub);
            }

            return sub;
        }


    }

    public class Item
    {
        public Item(string name, Item parent) : this(name, 0, parent)// folder
        {
        }

        public Item(string name, int size, Item parent) // file
        {
            Name = name;
            Size = size;
            Parent = parent;
        }

        public string Name { get; set; }
        public int Size { get; set; }
        public List<Item> Children { get; } = new List<Item>();
        public Item Parent = null;

        public void Print(string indent = "")
        {
            Console.WriteLine($"{indent} {Name} {(Size == 0 ? "(dir)" : $"(file, size={Size})")}");
            foreach (var c in Children)
            {
                c.Print(indent + "  ");
            }
        }

        public int TotalSize => Children.Sum(x => x.TotalSize) + Size;

        public string Fullname => 
    }


}
