// Copyright (c) Aaron Stannard 2016. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;

namespace FastAtomicLazy.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<FastLazyBenchmarks>(
                DefaultConfig.Instance
                    .With(Job.Clr.WithId("Desktop"))
                    .With(Job.Core.With(CsProjCoreToolchain.NetCoreApp20).WithId("Core 2.0"))
                    .With(Job.Core.With(CsProjCoreToolchain.NetCoreApp21).WithId("Core 2.1")));
        }
    }
}
