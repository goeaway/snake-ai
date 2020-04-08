using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BenchmarkDotNet.Attributes;
using Snake.Abstractions;
using Snake.AI;
using Snake.Factories;

namespace Snake.Benchmarks
{
    [MemoryDiagnoser]
    public class AIBenchmarks
    {
        private IController _controller;

        [Params(10)]
        public int Bounds { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _controller = new AIController(AIOptions.GetDefault(0), new CountdownGameFactory(Bounds, Bounds), new Router());
        }

        [Benchmark]
        public bool Completes() 
        {
            while (!_controller.Act())
            {
                Thread.Sleep(1);
            }

            return true;
        }

    }
}
