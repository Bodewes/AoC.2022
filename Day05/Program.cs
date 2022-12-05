using System;
using System.Diagnostics;
using System.IO;

namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var start = new string[] { "MSJLVFNR", "HWJFZDNP", "GDCRW", "SBN", "NFBCPWZM", "WMRP", "WSLGNTR", "VBNFHTQ", "FNZHML" }; // voor aan is top van stack.

            //var input = File.ReadAllLines("test.txt");
            //var start = new string[] { "NZ", "DCM", "P" }; // voor aan is top van stack.

            var solver = new Solution(input);

            Console.WriteLine("Day 05" + Environment.NewLine);
            TimeAction<string>(() => solver.MoveCrates((string[])start.Clone(), 9000));
            TimeAction<string>(() => solver.MoveCrates((string[])start.Clone(), 9001));

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
        private string[] stacks;


        public Solution(string[] input)
        {
            this.input = input;
        }

        public string MoveCrates(string[] initialPositions, int moverType)
        {
            this.stacks = initialPositions;
            foreach (var task in input)
            {
                //format "move 1 from 7 to 6"
                var tokens = task.Split(' ');
                var count = int.Parse(tokens[1]);
                var from = int.Parse(tokens[3]) - 1;
                var to = int.Parse(tokens[5]) - 1;

                if (moverType == 9000)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var c = stacks[from][0];
                        stacks[from] = stacks[from].Substring(1);
                        stacks[to] = c + stacks[to];
                    }
                }
                else
                {
                    var c = stacks[from].Substring(0, count);
                    stacks[from] = stacks[from].Substring(count);
                    stacks[to] = c + stacks[to];
                }
            }
            var answer = "";
            foreach (var s in stacks)
            {
                answer += s.Length > 0 ? s[0] : ' ';
                //Console.WriteLine(s);
            }

            return answer;
        }
    }
}
