using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day06
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            //var input = File.ReadAllLines("test.txt");
            //var input = File.ReadAllLines("test2.txt");

            var solver = new Solution(input);

            Console.WriteLine("Day 06" + Environment.NewLine);
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
            FindMarker(4);

            return 0;
        }

        public int Part2()
        {
            FindMarker(14);

            return 0;
        }

        private void FindMarker(int size)
        {
            foreach (var s in input)
            {
                var c = 0;
                while (s.Substring(c, size).Distinct().Count() != size)
                {
                    c++;
                }
                Console.WriteLine(c + size);
            }
        }
    }
}
