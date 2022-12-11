using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = File.ReadAllLines("test.txt");
            var input = File.ReadAllLines("input.txt");

            var solver = new Solution(input);

            Console.WriteLine("Day 11" + Environment.NewLine);
            TimeAction<long>(() => solver.Part1(20, true));
            TimeAction<long>(() => solver.Part1(10_000, false));


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

        public long Part1(int rounds, bool divideWorry = true)
        {
            var monkeys = ParseInput(input).ToArray();

            var maxDiviser = 1;
            for (int mi = 0; mi < monkeys.Length; mi++)
            {
                maxDiviser *= monkeys[mi].Test;
            }
            Console.WriteLine($"MaxDiviser: {maxDiviser}");

            for (int r = 0; r < rounds; r++)
            {
                for(int mi =0; mi < monkeys.Length; mi++)
                {
                    var m = monkeys[mi];
                    while(m.Items.Count > 0)
                    {
                        m.InspectionCount++;
                        var worry = m.Items.Dequeue();
                        worry = m.Operation(worry);

                        if (divideWorry)
                        {
                            worry /= 3;
                        }
                        else
                        {
                            worry = worry % maxDiviser;
                        }
                            

                        if (worry % m.Test == 0)
                        {
                            monkeys[m.TrueMonkey].Items.Enqueue(worry);
                        }
                        else
                        {
                            monkeys[m.FalseMonkey].Items.Enqueue(worry);
                        }
                    }
                }
                // Print 
                //Console.WriteLine($"After Round {r+1}");
                //for (int mi = 0; mi < monkeys.Length; mi++)
                //{
                //    Console.WriteLine($"Monkey {mi}: " + string.Join(", ", monkeys[mi].Items));
                //}
                //Console.WriteLine();
            }

            Console.WriteLine($"Inspection Counts");
            for (int mi = 0; mi < monkeys.Length; mi++)
            {
                Console.WriteLine($"Monkey {mi}: {monkeys[mi].InspectionCount}");
            }
            Console.WriteLine();

            var top2 = monkeys.Select(m => m.InspectionCount).OrderByDescending(m => m).Take(2).ToArray();

            return top2[0]*top2[1];
        }

        public int Part2()
        {
            return 0;
        }

        private List<Monkey> ParseInput(string[] input)
        {
            var i = 0;
            var Monkey = new List<Monkey>();
            while (i < input.Length) {

                if (input[i].StartsWith("Monkey")){
                    var id = input[i][7] - '0';
                    var m = new Monkey(id);
                    i++;

                    m.Items = new Queue<long>(input[i].Substring(18).Split(",").Select(x => long.Parse(x)));
                    i++;

                    var op = input[i][23];
                    var op2 = input[i].Substring(25);
                    if (op == '+')
                    {
                        if (op2 == "old")
                        {
                            m.Operation = (old) => old + old;
                        }
                        else
                        {
                            m.Operation = (old) => old + int.Parse(op2);
                        }
                    }
                    else //'*'
                    {
                        if (op2 == "old")
                        {
                            m.Operation = (old) => old * old;
                        }
                        else
                        {
                            m.Operation = (old) => old * int.Parse(op2);
                        }
                    }
                    i++;

                    m.Test = int.Parse(input[i].Substring(21));
                    i++;

                    m.TrueMonkey = int.Parse(input[i].Substring(29));
                    i++;

                    m.FalseMonkey = int.Parse(input[i].Substring(30));
                    i++;

                    // newline
                    i++;

                    Monkey.Add(m);
                }
            }

            return Monkey;
        }
    }

    public class Monkey
    {
        private readonly int id;

        public Monkey(int id )
        {
            this.id = id;
            this.InspectionCount = 0;
        }

        public Queue<long> Items { get; set; }
        public Func<long, long> Operation { get; set; }
        public int Test { get; set; }
        public int TrueMonkey { get; set; }
        public int FalseMonkey { get; set; }

        public long InspectionCount { get; set; }
    }
}
