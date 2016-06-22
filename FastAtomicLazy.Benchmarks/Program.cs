// Copyright (c) Aaron Stannard 2016. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Running;

namespace FastAtomicLazy.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<FastLazyBenchmarks>();
        }
    }
}
