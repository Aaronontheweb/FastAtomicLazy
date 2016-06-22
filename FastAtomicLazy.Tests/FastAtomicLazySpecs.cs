// Copyright (c) Aaron Stannard 2016. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FastAtomicLazy.Tests
{
    public class FastAtomicLazySpecs
    {
        [Fact]
        public void FastAtomicLazy_should_indicate_no_value_has_been_produced()
        {
            var fal = new FastLazy<int>(() => 2);
            Assert.False(fal.IsValueCreated);
        }

        [Fact]
        public void FastAtomicLazy_should_produce_value()
        {
            var fal = new FastLazy<int>(() => 2);
            var value = fal.Value;
            Assert.Equal(2, value);
            Assert.True(fal.IsValueCreated);
        }

        [Fact]
        public void FastAtomicLazy_must_be_threadsafe()
        {
            for (var c = 0; c < 100000; c++) // try this 100000 times
            {
                var values = new ConcurrentBag<int>();
                var fal = new FastLazy<int>(() => new Random().Next(1, Int32.MaxValue));
                var result = Parallel.For(0, 1000, i => values.Add(fal.Value)); // 1000 concurrent operations
                SpinWait.SpinUntil(() => result.IsCompleted);
                var value = values.First();
                Assert.NotEqual(0, value);
                Assert.True(values.All(x => x.Equals(value)));
            }
        }

        [Fact]
        public void FastAtomicLazy_only_single_value_creation_attempt()
        {
            int attempts = 0;
            Func<int> slowValueFactory = () =>
            {
                attempts++;
                Thread.Sleep(100);
                return new Random().Next(1, Int32.MaxValue);
            };

            var values = new ConcurrentBag<int>();
            var fal = new FastLazy<int>(slowValueFactory);
            var result = Parallel.For(0, 1000, i => values.Add(fal.Value)); // 1000 concurrent operations
            SpinWait.SpinUntil(() => result.IsCompleted);
            var value = values.First();
            Assert.NotEqual(0, value);
            Assert.True(values.All(x => x.Equals(value)));
            Assert.Equal(1000, values.Count);
            Assert.Equal(1, attempts);
        }
    }
}
