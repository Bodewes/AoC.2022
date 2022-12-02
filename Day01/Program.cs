using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var solver = new Solution(input);

            Console.WriteLine("Day 01");
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

        private List<int> elves = new List<int>();

        public int Part1()
        {
            CountCalories();
            return elves.Max();
        }

        public int Part2()
        {
            return elves.OrderByDescending(i => i).Take(3).Sum();
        }

        private void CountCalories()
        {
            int current = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == string.Empty)
                {
                    elves.Add(current);
                    current = 0;
                }
                else
                {
                    current += int.Parse(input[i]);
                }

            }
        }
    }
}
