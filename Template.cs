using System;


class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var solver = new Solution(input);

        Console.WriteLine("Day 02");
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
        return 0;
    }
    public int Part2()
    {
        return 0;
    }
}
