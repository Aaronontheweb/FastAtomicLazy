# FastAtomicLazy

Faster, less leakier thread-safe Lazy&lt;T&gt; implementation in C#.

## Rationale
[`Lazy<T>`](https://msdn.microsoft.com/en-us/library/dd642331.aspx) is an excellent general purpose implementation for lazily instantiated types. However, it is inappropriate for the scenarios where both of the following are true:

1. You are using `Lazy<T>` in a concurrent context where multiple publish events may occur simultaneously and type `T` is an `IDisposable` resource. Using either [`LazyThreadSafetyMode.None` or `LazyThreadSafetyMode.PublicationOnly`](https://msdn.microsoft.com/en-us/library/system.threading.lazythreadsafetymode.aspx) will result in a resource leak during this scenario.
2. You are using `Lazy<T>` in a performance-sensitive context where the cost of using a `lock` operation isn't acceptable, hence you cannot use `Lazy<T>` with `LazyThreadSafetyMode.ExecutionAndPublication`.

If both of the above are true, then you should use `FastLazy<T>` in this library. 

### Guarantees
`FastLazy<T>` guarantees the following:

1. The lazy value of type `T` will be instantiated *exactly* once, even if many concurrent instantiation calls are happening simultaneously (threadsafe, singleton instantiation.) [Spec to verify](https://github.com/Aaronontheweb/FastAtomicLazy/blob/master/FastAtomicLazy.Tests/FastAtomicLazySpecs.cs#L49).
2. The same instance of `T` will be served on every `FastLazy<T>.Value` call (threadsafe reads.) [Spec to verify](https://github.com/Aaronontheweb/FastAtomicLazy/blob/master/FastAtomicLazy.Tests/FastAtomicLazySpecs.cs#L34).
3. `FastLazy<T>.Value` calls will significantly outperform the equivalent `Lazy<T>.Value` call (lock-free, threadsafe reads.) [Benchmark to verify](https://github.com/Aaronontheweb/FastAtomicLazy/blob/master/FastAtomicLazy.Benchmarks/FastLazyBenchmarks.cs).

`FastLazy<T>` will not cache exceptions thrown by the factory method, nor will it perform any of the other niceties included inside `System.Lazy<T>`. It relies on the user to do the right thing.


## Benchmarks
`FastLazy<T>` is up to twice as fast as the non-threadsafe verison of [`Lazy<T>`](https://msdn.microsoft.com/en-us/library/dd642331.aspx).

```ini

BenchmarkDotNet=v0.9.7.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i7-3630QM CPU 2.40GHz, ProcessorCount=8
Frequency=2338442 ticks, Resolution=427.6352 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=32-bit RELEASE [AttachedDebugger]
JitModules=clrjit-v4.6.1080.0

Type=FastLazyBenchmarks  Mode=Throughput  

```
|                           Method |    Median |    StdDev |
|--------------------------------- |---------- |---------- |
|                     ReadFastLazy | 1.2866 ns | 0.1015 ns |
|            ReadLazyNotThreadSafe | 2.8733 ns | 0.0448 ns |
|    ReadLazyPublishOnlyThreadSafe | 2.8824 ns | 0.0993 ns |
| ReadLazyReadAndExecuteThreadSafe | 2.8793 ns | 0.0524 ns |
