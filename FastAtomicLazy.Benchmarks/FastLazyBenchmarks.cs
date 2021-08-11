// Copyright (c) Aaron Stannard 2016. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using BenchmarkDotNet.Attributes;
using FastAtomicLazy.Benchmarks.Configs;

namespace FastAtomicLazy.Benchmarks
{
    [Config(typeof(MicroBenchmarkConfig))]
    public class FastLazyBenchmarks
    {
        private FastLazy<int> _fastLazy;
        private Lazy<int> _unsafeLazy;
        private Lazy<int> _publishOnlySafeLazy;
        private Lazy<int> _readAndExecuteSafeLazy;


        [GlobalSetup]
        public void Setup()
        {
            _fastLazy = new FastLazy<int>(() => new Random().Next());
            _unsafeLazy = new Lazy<int>(() => new Random().Next());
            _publishOnlySafeLazy = new Lazy<int>(() => new Random().Next(), LazyThreadSafetyMode.PublicationOnly);
            _readAndExecuteSafeLazy = new Lazy<int>(() => new Random().Next(), LazyThreadSafetyMode.ExecutionAndPublication);
        }

        [Benchmark]
        public int ReadFastLazy()
        {
            return _fastLazy.Value;
        }

        [Benchmark]
        public int ReadLazyNotThreadSafe()
        {
            return _unsafeLazy.Value;
        }

        [Benchmark]
        public int ReadLazyPublishOnlyThreadSafe()
        {
            return _publishOnlySafeLazy.Value;
        }

        [Benchmark]
        public int ReadLazyReadAndExecuteThreadSafe()
        {
            return _readAndExecuteSafeLazy.Value;
        }
    }
}
