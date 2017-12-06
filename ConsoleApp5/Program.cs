using System;
using BenchmarkDotNet.Running;

namespace ConsoleApp5
{
    internal static class Program
    {
        public static void Main()
        {
            var summary = BenchmarkRunner.Run<AsChunkBenchmark>();
            Console.WriteLine(summary);
        }
    }
}
