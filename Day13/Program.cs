using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = File.ReadAllLines("test.txt");
            var input = File.ReadAllLines("input.txt");

            var solver = new Solution(input);

            Console.WriteLine("Day 13" + Environment.NewLine);
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

        public Solution(string[] input)
        {
            this.input = input;
        }

        public int Part1()
        {
            var pairIndex = 0;
            var i = 0;
            var result = 0;
            while( i< input.Length)
            {
                pairIndex++;
                var row_a = Frame.Parse(input[i]);
                var row_b = Frame.Parse(input[i+1]);

                Console.WriteLine("A: " + row_a);
                Console.WriteLine("B: " + row_b);
                
                i += 3;

                try
                {
                    row_a.CheckOrder(row_b);
                }catch(OrderException e)
                {
                    if (e.RightOrder)
                    {
                        Console.WriteLine("in right order");
                        result += pairIndex;
                    }
                    else
                    {
                        Console.WriteLine("NOT in right order");
                    }
                }

                Console.WriteLine();
            }

            return result;
        }

        public int Part2()
        {
            var frames = new List<Frame>();
            foreach(var line in input)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    frames.Add(Frame.Parse(line));
                }
            }
            var twoFrame = Frame.Parse("[[2]]");
            var sixFrame = Frame.Parse("[[6]]");

            frames.Add(twoFrame);
            frames.Add(sixFrame);

            frames.Sort(new FrameComparer());

            foreach(var f in frames)
            {
                Console.WriteLine(f);
            }

            return (frames.IndexOf(twoFrame)+1)*(frames.IndexOf(sixFrame)+1);
        }
    }

    public class FrameComparer : IComparer<Frame>
    {
        public int Compare([AllowNull] Frame x, [AllowNull] Frame y)
        {
            try
            {
                x.CheckOrder(y);
            }
            catch(OrderException e)
            {
                if (e.RightOrder) return -1;
                else return 1;
            }
            return 0;
        }
    }

    public class Frame
    {
        public int Value { get; set; }
        public List<Frame> Values { get; set; }

        public bool HasValue => Values == null;

        public Frame()
        {
            this.Value = -1;
            this.Values = new List<Frame>();
        }

        public Frame(int value)
        {
            this.Value = value;
            this.Values = null;
        }

        public void CheckOrder(Frame other)
        {
            if (this.HasValue && other.HasValue)
            {
                if (this.Value != other.Value)
                {
                    throw new OrderException(this.Value < other.Value);
                }
                // same, and thus no conclusion
            }
            else if (this.HasValue || other.HasValue)
            {

                if (this.HasValue)
                {
                    var f = new Frame();
                    f.Values.Add(this);
                    f.CheckOrder(other);
                }
                else
                {
                    var of = new Frame();
                    of.Values.Add(other);
                    this.CheckOrder(of);
                }
            }
            else // both lists.
            {
                var isShorter = this.Values.Count < other.Values.Count;
                var isLonger = this.Values.Count > other.Values.Count;
                var checkSize = Math.Min(this.Values.Count, other.Values.Count);
                for (int i = 0; i< checkSize; i++)
                {
                    this.Values[i].CheckOrder(other.Values[i]);
                }
                if (isShorter) throw new OrderException(true);
                if (isLonger) throw new OrderException(false);
            }
        }

        public static Frame Parse(string input)
        {
            //Console.WriteLine("Parsing: " + input);
            if (input[0] != '[') // digit input
            {
                return new Frame(int.Parse(input));
            }
            else // list [ ... ]
            {
                var f = new Frame();
                var tokens = GetFrameTokens(input.Substring(1, input.Length-2));
                foreach(var t in tokens)
                {
                    f.Values.Add(Parse(t));
                }
                return f;
            }
        }

        public static IEnumerable<string> GetFrameTokens(string input)
        {
            //Console.WriteLine("Getting tokens in "+input);
            var s = 0;
            var d = 0;
            for(int i = 0; i < input.Length; i++)
            {
                if (input[i] == '[') // start sub range keep track of depth
                {
                    if (d== 0)
                    {
                        s = i; // sub token starts here
                    }
                    d++;
                }
                else if (input[i] == ']')
                {
                    d--;
                    if (d == 0) // sub ended
                    {
                        yield return input.Substring(s, i - s+1);
                    }
                }
                else if (input[i] == ',')
                {
                    // noop, skip
                }
                else //digit
                {
                    if (d == 0) { // skip digit in subs
                   
                        // find whole digit
                        var j = i;
                        while (j < input.Length && input[j] != ']' && input[j] != ',' && input[j] != '[')
                        {
                            j++;
                        }
                        //var digit = int.Parse(input.Substring(i, j - i));
                        yield return input.Substring(i, j - i);
                        i = j;
                    }
                }
            }
        }


        public override string ToString()
        {
            if (this.HasValue){
                return this.Value.ToString();
            }
            else
            {
                var s = string.Join(",", this.Values.Select(x => x.ToString()));
                return "[" + s + "]";
            }
        }
    }

    public class OrderException : Exception {
        public OrderException(bool rightOrder)
        {
            RightOrder = rightOrder;
        }

        public bool RightOrder { get; }
    }
}
