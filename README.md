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
|                               Method |       Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------------------------- |-----------:|----------:|----------:|-------:|----------:|
|                 IEnumOnStack0_Normal |  11.916 ns | 0.4977 ns | 0.7295 ns | 0.0064 |      40 B |
|            IEnumOnStack0_Accelerated |  19.812 ns | 0.1493 ns | 0.2141 ns |      - |         - |
|                IEnumOnStack10_Normal |  82.047 ns | 0.3545 ns | 0.5196 ns | 0.0063 |      40 B |
|           IEnumOnStack10_Accelerated | 109.586 ns | 0.3807 ns | 0.5580 ns | 0.0063 |      40 B |
|               IEnumOnStack100_Normal | 633.722 ns | 4.1179 ns | 5.9057 ns | 0.0057 |      40 B |
|          IEnumOnStack100_Accelerated | 753.168 ns | 4.9308 ns | 7.0716 ns | 0.0057 |      40 B |

|                 IEnumOnArray0_Normal |   4.918 ns | 0.0349 ns | 0.0512 ns |      - |         - |
|            IEnumOnArray0_Accelerated |   8.734 ns | 0.0555 ns | 0.0831 ns |      - |         - |
|                IEnumOnArray10_Normal |  40.159 ns | 0.3294 ns | 0.4618 ns | 0.0051 |      32 B |
|           IEnumOnArray10_Accelerated |  21.725 ns | 0.1987 ns | 0.2912 ns |      - |         - |
|               IEnumOnArray100_Normal | 334.424 ns | 2.3286 ns | 3.3396 ns | 0.0048 |      32 B |
|          IEnumOnArray100_Accelerated | 163.400 ns | 0.9046 ns | 1.2974 ns |      - |         - |

|                  IEnumOnList0_Normal |  11.756 ns | 0.0600 ns | 0.0861 ns | 0.0064 |      40 B |
|             IEnumOnList0_Accelerated |  11.899 ns | 0.0348 ns | 0.0511 ns |      - |         - |
|                 IEnumOnList10_Normal |  76.460 ns | 0.2644 ns | 0.3791 ns | 0.0063 |      40 B |
|            IEnumOnList10_Accelerated |  32.540 ns | 0.2343 ns | 0.3434 ns |      - |         - |
|                IEnumOnList100_Normal | 530.900 ns | 3.2422 ns | 4.5452 ns | 0.0057 |      40 B |
|           IEnumOnList100_Accelerated | 231.249 ns | 0.8550 ns | 1.2532 ns |      - |         - |

|                 IListOnArray0_Normal |   4.861 ns | 0.0149 ns | 0.0214 ns |      - |         - |
|            IListOnArray0_Accelerated |   2.583 ns | 0.0215 ns | 0.0315 ns |      - |         - |
|                IListOnArray10_Normal |  39.814 ns | 0.2279 ns | 0.3195 ns | 0.0051 |      32 B |
|           IListOnArray10_Accelerated |  19.772 ns | 0.1441 ns | 0.2156 ns |      - |         - |
|               IListOnArray100_Normal | 337.584 ns | 2.6602 ns | 3.9816 ns | 0.0048 |      32 B |
|          IListOnArray100_Accelerated | 197.713 ns | 0.8613 ns | 1.2624 ns |      - |         - |

|                  IListOnList0_Normal |  11.907 ns | 0.1408 ns | 0.2019 ns | 0.0064 |      40 B |
|             IListOnList0_Accelerated |   2.533 ns | 0.0124 ns | 0.0181 ns |      - |         - |
|                 IListOnList10_Normal |  75.203 ns | 0.4545 ns | 0.6662 ns | 0.0063 |      40 B |
|            IListOnList10_Accelerated |  22.010 ns | 0.1926 ns | 0.2763 ns |      - |         - |
|                IListOnList100_Normal | 530.122 ns | 2.0432 ns | 2.9303 ns | 0.0057 |      40 B |
|           IListOnList100_Accelerated | 201.067 ns | 1.9784 ns | 2.8999 ns |      - |         - |

|          IReadOnlyListOnList0_Normal |  11.504 ns | 0.0649 ns | 0.0951 ns | 0.0064 |      40 B |
|     IReadOnlyListOnList0_Accelerated |   2.352 ns | 0.0180 ns | 0.0253 ns |      - |         - |
|         IReadOnlyListOnList10_Normal |  72.025 ns | 0.4573 ns | 0.6559 ns | 0.0063 |      40 B |
|    IReadOnlyListOnList10_Accelerated |  21.080 ns | 0.2111 ns | 0.3027 ns |      - |         - |
|        IReadOnlyListOnList100_Normal | 509.947 ns | 2.3176 ns | 3.4688 ns | 0.0057 |      40 B |
|   IReadOnlyListOnList100_Accelerated | 199.782 ns | 1.2465 ns | 1.7876 ns |      - |         - |

|                  IDictOnDict0_Normal |  13.454 ns | 0.1009 ns | 0.1479 ns | 0.0076 |      48 B |
|             IDictOnDict0_Accelerated |  10.511 ns | 0.0526 ns | 0.0771 ns |      - |         - |
|                 IDictOnDict10_Normal |  77.683 ns | 0.4120 ns | 0.6039 ns | 0.0076 |      48 B |
|            IDictOnDict10_Accelerated |  38.412 ns | 0.0925 ns | 0.1326 ns |      - |         - |
|                IDictOnDict100_Normal | 540.484 ns | 2.3436 ns | 3.4352 ns | 0.0076 |      48 B |
|           IDictOnDict100_Accelerated | 249.795 ns | 1.4224 ns | 2.1290 ns |      - |         - |

|          IReadOnlyDictOnDict0_Normal |  13.530 ns | 0.0666 ns | 0.0933 ns | 0.0076 |      48 B |
|     IReadOnlyDictOnDict0_Accelerated |  10.512 ns | 0.0649 ns | 0.0951 ns |      - |         - |
|         IReadOnlyDictOnDict10_Normal |  77.828 ns | 0.5313 ns | 0.7619 ns | 0.0076 |      48 B |
|    IReadOnlyDictOnDict10_Accelerated |  38.022 ns | 0.0928 ns | 0.1330 ns |      - |         - |
|        IReadOnlyDictOnDict100_Normal | 555.660 ns | 2.3068 ns | 3.3813 ns | 0.0076 |      48 B |
|   IReadOnlyDictOnDict100_Accelerated | 250.530 ns | 0.9358 ns | 1.3717 ns |      - |         - |

|                ISetOnHashSet0_Normal |  10.897 ns | 0.1553 ns | 0.2228 ns | 0.0064 |      40 B |
|           ISetOnHashSet0_Accelerated |   9.272 ns | 0.0442 ns | 0.0647 ns |      - |         - |
|               ISetOnHashSet10_Normal |  68.202 ns | 0.3388 ns | 0.5071 ns | 0.0063 |      40 B |
|          ISetOnHashSet10_Accelerated |  30.400 ns | 0.2178 ns | 0.3260 ns |      - |         - |
|              ISetOnHashSet100_Normal | 561.097 ns | 1.4157 ns | 1.9847 ns | 0.0057 |      40 B |
|         ISetOnHashSet100_Accelerated | 267.614 ns | 2.4228 ns | 3.5513 ns |      - |         - |

|        IReadOnlySetOnHashSet0_Normal |  10.778 ns | 0.0842 ns | 0.1234 ns | 0.0064 |      40 B |
|   IReadOnlySetOnHashSet0_Accelerated |   9.490 ns | 0.0340 ns | 0.0466 ns |      - |         - |
|       IReadOnlySetOnHashSet10_Normal |  68.022 ns | 0.7982 ns | 1.1448 ns | 0.0063 |      40 B |
|  IReadOnlySetOnHashSet10_Accelerated |  31.568 ns | 0.4505 ns | 0.6603 ns |      - |         - |
|      IReadOnlySetOnHashSet100_Normal | 564.348 ns | 4.3112 ns | 6.3193 ns | 0.0057 |      40 B |
| IReadOnlySetOnHashSet100_Accelerated | 287.325 ns | 1.3242 ns | 1.9410 ns |      - |         - |
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

