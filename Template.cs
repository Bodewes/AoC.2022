using System;


class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("test.txt");
        //var input = File.ReadAllLines("input.txt");

        var solver = new Solution(input);

        Console.WriteLine("Day 02" + Environment.NewLine);
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
        return 0;
    }

    public int Part2()
    {
        return 0;
    }
}
