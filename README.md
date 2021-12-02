# Enumerable Acceleration

Enumerating collections with 'foreach' is an extremely common pattern in C# code.
The C# compiler tries its best to implement this feature efficiently, but there are
limits to what it can do.

C# does a great job when enumerating an array or a List<T>. Unfortunately, if you program
with interfaces, such as IEnumerable<T>, IList<T>, or IDictionary<K,V>, then enumeration using
foreach is not as efficient. In particular, when you enumerate using interface types, you end
up causing a memory allocation for every enumeration which in the long run hurts your program's
efficiency. In addition, normal enumeration ends up causing two or more interface-based method
calls per item being enumerated (and interface calls are slower than normal method calls).

This project shows a simple remedy which delivers good performance improvements over the common
case. In some cases, performance is improved, in other cases allocations are reduced. In 
the common case of enumerating an IList or IReadOnlyList, both are improved.

Usage is pretty simple:

```csharp
	// normal
	foreach (var item in mylist)
	{ ... }

	// accelerated
	foreach (var item in mylist.AcceleratedEnum())
	{ ... }
```

## Benchmarks

The table below highlights the performance benefits. *_Normal benchmarks are using classic foreach
loops, while the *_Accelerated benchmarks are using slighty modified loops which optimize behavior.

The benchmarks are grouped as follows:

* IEnumOnStack* represent iteration of an IEnumerable<int> which is implemented via a Stack<int>
* IEnumOnArray* represent iteration of an IEnumerable<int> which is implemented via an int[].
* IEnumOnList* represent iteration of an IEnumerable<int> which is implemented via a List<int>.
* IListOnArray* represent iteration of an IList<int> which is implemented via an int[].
* IListOnList* represent iteration of an IList<int> which is implemented via a List<int>.
* IReadOnlyListOnList* represent iteration of an IReadOnlyList<int> which is implemented via a List<int>.
* IDictOnDict* represent iteration of an IDictionary<int, int> which is implemented via a Dictionary<int, int>.
* IReadOnlyDictOnDict* represent iteration of an IReadOnlyDictionary<int, int> which is implemented via a Dictionary<int, int>.
* ISetOnHashSet* represent iteration of an ISet<int> which is implemented via a HashSet<int>.
* IReadOnlySetOnHashSet* represent iteration of an IReadOnlySet<int> which is implemented via a HashSet<int>.

The number in the names indicating the number of iterations in the benchmark.


```
|                               Method |       Mean |     Error |     StdDev |     Median |  Gen 0 | Allocated |
|------------------------------------- |-----------:|----------:|-----------:|-----------:|-------:|----------:|
|                 IEnumOnStack0_Normal |   7.835 ns | 0.0487 ns |  0.0713 ns |   7.818 ns | 0.0064 |      40 B |
|            IEnumOnStack0_Accelerated |  21.756 ns | 0.1273 ns |  0.1866 ns |  21.732 ns |      - |         - |
|                IEnumOnStack10_Normal |  51.225 ns | 0.1772 ns |  0.2484 ns |  51.255 ns | 0.0063 |      40 B |
|           IEnumOnStack10_Accelerated | 104.523 ns | 0.6646 ns |  0.9531 ns | 104.198 ns | 0.0063 |      40 B |
|               IEnumOnStack100_Normal | 441.694 ns | 1.5558 ns |  2.1810 ns | 441.157 ns | 0.0062 |      40 B |
|          IEnumOnStack100_Accelerated | 673.767 ns | 7.4510 ns | 11.1523 ns | 669.868 ns | 0.0057 |      40 B |

|                 IEnumOnArray0_Normal |   5.042 ns | 0.0149 ns |  0.0209 ns |   5.042 ns |      - |         - |
|            IEnumOnArray0_Accelerated |   8.718 ns | 0.0495 ns |  0.0710 ns |   8.682 ns |      - |         - |
|                IEnumOnArray10_Normal |  43.264 ns | 0.1577 ns |  0.2261 ns |  43.216 ns | 0.0051 |      32 B |
|           IEnumOnArray10_Accelerated |  21.593 ns | 0.3019 ns |  0.4426 ns |  21.377 ns |      - |         - |
|               IEnumOnArray100_Normal | 361.073 ns | 2.7242 ns |  3.7289 ns | 360.711 ns | 0.0048 |      32 B |
|          IEnumOnArray100_Accelerated | 159.083 ns | 0.4205 ns |  0.6294 ns | 159.296 ns |      - |         - |

|                  IEnumOnList0_Normal |   7.832 ns | 0.0989 ns |  0.1450 ns |   7.786 ns | 0.0064 |      40 B |
|             IEnumOnList0_Accelerated |  12.086 ns | 0.0575 ns |  0.0843 ns |  12.061 ns |      - |         - |
|                 IEnumOnList10_Normal |  50.759 ns | 0.8968 ns |  1.3145 ns |  50.214 ns | 0.0063 |      40 B |
|            IEnumOnList10_Accelerated |  34.795 ns | 0.1183 ns |  0.1697 ns |  34.731 ns |      - |         - |
|                IEnumOnList100_Normal | 441.623 ns | 2.6154 ns |  3.7509 ns | 440.653 ns | 0.0062 |      40 B |
|           IEnumOnList100_Accelerated | 250.984 ns | 1.5284 ns |  2.2876 ns | 250.508 ns |      - |         - |

|                 IListOnArray0_Normal |   5.336 ns | 0.0250 ns |  0.0367 ns |   5.327 ns |      - |         - |
|            IListOnArray0_Accelerated |   2.245 ns | 0.0194 ns |  0.0284 ns |   2.235 ns |      - |         - |
|                IListOnArray10_Normal |  42.823 ns | 0.2089 ns |  0.3062 ns |  42.781 ns | 0.0051 |      32 B |
|           IListOnArray10_Accelerated |  22.436 ns | 0.0965 ns |  0.1415 ns |  22.439 ns |      - |         - |
|               IListOnArray100_Normal | 355.346 ns | 1.5737 ns |  2.2569 ns | 354.495 ns | 0.0048 |      32 B |
|          IListOnArray100_Accelerated | 185.222 ns | 6.1391 ns |  8.9986 ns | 178.228 ns |      - |         - |

|                  IListOnList0_Normal |   8.520 ns | 0.0241 ns |  0.0353 ns |   8.518 ns | 0.0064 |      40 B |
|             IListOnList0_Accelerated |   1.966 ns | 0.0103 ns |  0.0144 ns |   1.967 ns |      - |         - |
|                 IListOnList10_Normal |  50.042 ns | 0.2441 ns |  0.3578 ns |  49.970 ns | 0.0063 |      40 B |
|            IListOnList10_Accelerated |  21.418 ns | 0.0782 ns |  0.1121 ns |  21.396 ns |      - |         - |
|                IListOnList100_Normal | 438.398 ns | 2.5678 ns |  3.8433 ns | 437.097 ns | 0.0062 |      40 B |
|           IListOnList100_Accelerated | 198.318 ns | 0.4904 ns |  0.7033 ns | 198.447 ns |      - |         - |

|          IReadOnlyListOnList0_Normal |   8.385 ns | 0.2052 ns |  0.3071 ns |   8.243 ns | 0.0064 |      40 B |
|     IReadOnlyListOnList0_Accelerated |   2.242 ns | 0.0195 ns |  0.0279 ns |   2.239 ns |      - |         - |
|         IReadOnlyListOnList10_Normal |  49.844 ns | 0.1899 ns |  0.2600 ns |  49.808 ns | 0.0063 |      40 B |
|    IReadOnlyListOnList10_Accelerated |  21.827 ns | 0.1025 ns |  0.1503 ns |  21.810 ns |      - |         - |
|        IReadOnlyListOnList100_Normal | 439.944 ns | 1.9713 ns |  2.8895 ns | 439.524 ns | 0.0062 |      40 B |
|   IReadOnlyListOnList100_Accelerated | 198.190 ns | 0.3359 ns |  0.4598 ns | 198.173 ns |      - |         - |

|                  IDictOnDict0_Normal |  11.179 ns | 0.0475 ns |  0.0696 ns |  11.197 ns | 0.0076 |      48 B |
|             IDictOnDict0_Accelerated |  10.591 ns | 0.0316 ns |  0.0473 ns |  10.591 ns |      - |         - |
|                 IDictOnDict10_Normal |  55.168 ns | 0.6465 ns |  0.9063 ns |  54.906 ns | 0.0076 |      48 B |
|            IDictOnDict10_Accelerated |  37.919 ns | 0.1078 ns |  0.1579 ns |  37.924 ns |      - |         - |
|                IDictOnDict100_Normal | 456.278 ns | 2.0765 ns |  3.0437 ns | 456.889 ns | 0.0076 |      48 B |
|           IDictOnDict100_Accelerated | 248.180 ns | 1.1518 ns |  1.6882 ns | 247.690 ns |      - |         - |

|          IReadOnlyDictOnDict0_Normal |  11.317 ns | 0.1696 ns |  0.2433 ns |  11.200 ns | 0.0076 |      48 B |
|     IReadOnlyDictOnDict0_Accelerated |  10.998 ns | 0.3312 ns |  0.4854 ns |  10.910 ns |      - |         - |
|         IReadOnlyDictOnDict10_Normal |  55.157 ns | 0.4493 ns |  0.6585 ns |  55.085 ns | 0.0076 |      48 B |
|    IReadOnlyDictOnDict10_Accelerated |  37.957 ns | 0.2014 ns |  0.2823 ns |  37.867 ns |      - |         - |
|        IReadOnlyDictOnDict100_Normal | 456.584 ns | 2.9986 ns |  4.3953 ns | 454.774 ns | 0.0076 |      48 B |
|   IReadOnlyDictOnDict100_Accelerated | 248.004 ns | 0.7440 ns |  1.0670 ns | 248.081 ns |      - |         - |

|                ISetOnHashSet0_Normal |   7.287 ns | 0.0485 ns |  0.0663 ns |   7.272 ns | 0.0064 |      40 B |
|           ISetOnHashSet0_Accelerated |   9.600 ns | 0.0706 ns |  0.0989 ns |   9.586 ns |      - |         - |
|               ISetOnHashSet10_Normal |  47.307 ns | 0.2490 ns |  0.3728 ns |  47.201 ns | 0.0063 |      40 B |
|          ISetOnHashSet10_Accelerated |  31.516 ns | 0.1497 ns |  0.2099 ns |  31.444 ns |      - |         - |
|              ISetOnHashSet100_Normal | 461.619 ns | 1.7594 ns |  2.5233 ns | 461.005 ns | 0.0062 |      40 B |
|         ISetOnHashSet100_Accelerated | 248.588 ns | 0.6216 ns |  0.8915 ns | 248.260 ns |      - |         - |

|        IReadOnlySetOnHashSet0_Normal |   7.319 ns | 0.0443 ns |  0.0663 ns |   7.315 ns | 0.0064 |      40 B |
|   IReadOnlySetOnHashSet0_Accelerated |   9.450 ns | 0.0370 ns |  0.0531 ns |   9.439 ns |      - |         - |
|       IReadOnlySetOnHashSet10_Normal |  46.799 ns | 0.1203 ns |  0.1647 ns |  46.774 ns | 0.0063 |      40 B |
|  IReadOnlySetOnHashSet10_Accelerated |  30.685 ns | 0.1540 ns |  0.2305 ns |  30.614 ns |      - |         - |
|      IReadOnlySetOnHashSet100_Normal | 460.292 ns | 1.7197 ns |  2.5207 ns | 459.824 ns | 0.0062 |      40 B |
| IReadOnlySetOnHashSet100_Accelerated | 256.467 ns | 1.3636 ns |  1.9556 ns | 256.575 ns |      - |         - |
```

## How This Works

Acceleration is achieved by providing custom enumerator implementations for specific cases:

* When collections are empty, a fast non-allocating enumerator is used which reduces the overhead of enumerating empty collections
(which actually happens a lot in the real world)

* When collections are indexable, a fast non-allocating enumerator is used that eliminates an allocation and cuts in half the number
of interface method calls needed to enumerate the collection.

## Language Support?

It would be mighty handy if a future version of the C# compiler automatically leverage this kind of optimization
to magically accelerate foreach usage in common code.

