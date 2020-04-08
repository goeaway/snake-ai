using System;
using BenchmarkDotNet.Running;

namespace Snake.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = BenchmarkRunner.Run<AIBenchmarks>();
        }
    }
}
