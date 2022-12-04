using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day03
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = File.ReadAllLines("test.txt");
            var input = File.ReadAllLines("input.txt");
            var solver = new Solution(input);

            Console.WriteLine("Day 03");
            TimeAction<int>(solver.Part1);
            TimeAction<int>(solver.Part2);

            Console.ReadKey();
        }

        private static void TimeAction<T>(Func<T> action)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Console.WriteLine(action());

            sw.Stop();
            Console.WriteLine($"Took: {sw.Elapsed}");
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
            return this.input.Select(x => getPrio(x)).Sum();
        }

        private int getPrio(string x)
        {
            var l = x.Length;
            var m = l / 2;
            var s1 = x.Substring(0, m);
            var s2 = x.Substring(m);
            var twice = s1.Intersect(s2).Distinct().Single(); ;
            var value = twice >= 'a' && twice <= 'z' ? twice - 'a' + 1 : twice - 'A' + 27;
            // Console.WriteLine($"{twice} - {value}");
            return value;
        }

        public int Part2()
        {
            var total = 0;
            for(var i = 0; i < input.Length; i += 3)
            {
                total += getBadge(input[i], input[i + 1], input[i + 2]);
            }
            return total;
        }

        private int getBadge(string v1, string v2, string v3)
        {
            var badge = v1.Intersect(v2.Intersect(v3)).Distinct().Single();
            var value = badge >= 'a' && badge <= 'z' ? badge - 'a' + 1 : badge - 'A' + 27;
            // Console.WriteLine($"{badge} - {value}");
            return value;
        }
    }

}
