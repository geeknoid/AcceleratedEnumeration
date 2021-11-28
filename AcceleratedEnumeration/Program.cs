using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace Bench;

[MemoryDiagnoser]
public class Program
{
    private static readonly IEnumerable<int> _ienumOnStack0 = new Stack<int>();
    private static readonly IEnumerable<int> _ienumOnStack10 = new Stack<int>(new int[10]);
    private static readonly IEnumerable<int> _ienumOnStack100 = new Stack<int>(new int[100]);

    private static readonly IEnumerable<int> _ienumOnArray0 = new int[0];
    private static readonly IEnumerable<int> _ienumOnArray10 = new int[10];
    private static readonly IEnumerable<int> _ienumOnArray100 = new int[100];

    private static readonly IEnumerable<int> _ienumOnList0 = new List<int>(new int[0]);
    private static readonly IEnumerable<int> _ienumOnList10 = new List<int>(new int[10]);
    private static readonly IEnumerable<int> _ienumOnList100 = new List<int>(new int[100]);

    private static readonly IList<int> _ilistOnArray0 = new int[0];
    private static readonly IList<int> _ilistOnArray10 = new int[10];
    private static readonly IList<int> _ilistOnArray100 = new int[100];

    private static readonly IList<int> _ilistOnList0 = new List<int>(new int[0]);
    private static readonly IList<int> _ilistOnList10 = new List<int>(new int[10]);
    private static readonly IList<int> _ilistOnList100 = new List<int>(new int[100]);

    private static readonly IDictionary<int, int> _idictOnDict0 = new Dictionary<int, int>();
    private static readonly IDictionary<int, int> _idictOnDict10 = new Dictionary<int, int>();
    private static readonly IDictionary<int, int> _idictOnDict100 = new Dictionary<int, int>();

    static Program()
    {
        for (int i = 0; i < 10; i++)
        {
            _idictOnDict10[i] = i;
        }

        for (int i = 0; i < 100; i++)
        {
            _idictOnDict100[i] = i;
        }
    }

    public static void Main(string[] args)
    {
#if false
        var dontRequireSlnToRunBenchmarks = ManualConfig
            .Create(DefaultConfig.Instance)
            .AddJob(Job.MediumRun.WithToolchain(InProcessEmitToolchain.Instance));

        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, dontRequireSlnToRunBenchmarks);
#endif
        var e1 = _idictOnDict100.GetEnumerator();
        var e2 = _idictOnDict100.AcceleratedEnum().GetEnumerator();

        var p = new Program();
        p.IDictOnDict100_Accelerated();
    }

    [Benchmark]
    public void IEnumOnStack0_Normal()
    {
        foreach (var _ in _ienumOnStack0)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnStack0_Accelerated()
    {
        foreach (var _ in _ienumOnStack0.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnStack10_Normal()
    {
        foreach (var _ in _ienumOnStack10)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnStack10_Accelerated()
    {
        foreach (var _ in _ienumOnStack10.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnStack100_Normal()
    {
        foreach (var _ in _ienumOnStack100)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnStack100_Accelerated()
    {
        foreach (var _ in _ienumOnStack100.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnArray0_Normal()
    {
        foreach (var _ in _ienumOnArray0)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnArray0_Accelerated()
    {
        foreach (var _ in _ienumOnArray0.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnArray10_Normal()
    {
        foreach (var _ in _ienumOnArray10)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnArray10_Accelerated()
    {
        foreach (var _ in _ienumOnArray10.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnArray100_Normal()
    {
        foreach (var _ in _ienumOnArray100)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnArray100_Accelerated()
    {
        foreach (var _ in _ienumOnArray100.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnList0_Normal()
    {
        foreach (var _ in _ienumOnList0)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnList0_Accelerated()
    {
        foreach (var _ in _ienumOnList0.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnList10_Normal()
    {
        foreach (var _ in _ienumOnList10)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnList10_Accelerated()
    {
        foreach (var _ in _ienumOnList10.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnList100_Normal()
    {
        foreach (var _ in _ienumOnList100)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IEnumOnList100_Accelerated()
    {
        foreach (var _ in _ienumOnList100.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IListOnArray0_Normal()
    {
        foreach (var _ in _ilistOnArray0)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IListOnArray0_Accelerated()
    {
        foreach (var _ in _ilistOnArray0.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IListOnArray10_Normal()
    {
        foreach (var _ in _ilistOnArray10)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IListOnArray10_Accelerated()
    {
        foreach (var _ in _ilistOnArray10.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IListOnArray100_Normal()
    {
        foreach (var _ in _ilistOnArray100)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IListOnArray100_Accelerated()
    {
        foreach (var _ in _ilistOnArray100.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IListOnList0_Normal()
    {
        foreach (var _ in _ilistOnList0)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IListOnList0_Accelerated()
    {
        foreach (var _ in _ilistOnList0.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IListOnList10_Normal()
    {
        foreach (var _ in _ilistOnList10)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IListOnList10_Accelerated()
    {
        foreach (var _ in _ilistOnList10.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IListOnList100_Normal()
    {
        foreach (var _ in _ilistOnList100)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IListOnList100_Accelerated()
    {
        foreach (var _ in _ilistOnList100.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IDictOnDict0_Normal()
    {
        foreach (var _ in _idictOnDict0)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IDictOnDict0_Accelerated()
    {
        foreach (var _ in _idictOnDict0.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IDictOnDict10_Normal()
    {
        foreach (var _ in _idictOnDict10)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IDictOnDict10_Accelerated()
    {
        foreach (var _ in _idictOnDict10.AcceleratedEnum())
        {
            // nothing
        }
    }

    [Benchmark]
    public void IDictOnDict100_Normal()
    {
        foreach (var _ in _idictOnDict100)
        {
            // nothing
        }
    }

    [Benchmark]
    public void IDictOnDict100_Accelerated()
    {
        foreach (var _ in _idictOnDict100.AcceleratedEnum())
        {
            // nothing
        }
    }
}
