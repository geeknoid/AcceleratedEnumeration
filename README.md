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
* IDictOnDict* represent iteration of an IDictionary<int, int> which is implemented via a Dictionary<int, int>.

The number in the names indicating the number of iterations in the benchmark.

```
|                      Method |       Mean |     Error |     StdDev |  Gen 0 | Allocated |
|---------------------------- |-----------:|----------:|-----------:|-------:|----------:|
|        IListOnArray0_Normal |   5.926 ns | 0.0635 ns |  0.0930 ns |      - |         - |
|   IListOnArray0_Accelerated |   2.136 ns | 0.0266 ns |  0.0390 ns |      - |         - |
|       IListOnArray10_Normal |  39.764 ns | 0.1083 ns |  0.1518 ns | 0.0051 |      32 B |
|  IListOnArray10_Accelerated |  24.637 ns | 0.1374 ns |  0.2014 ns |      - |         - |
|      IListOnArray100_Normal | 320.780 ns | 2.5178 ns |  3.7685 ns | 0.0048 |      32 B |
| IListOnArray100_Accelerated | 198.725 ns | 0.6077 ns |  0.7902 ns |      - |         - |

|         IListOnList0_Normal |   9.369 ns | 0.3310 ns |  0.4747 ns | 0.0064 |      40 B |
|    IListOnList0_Accelerated |   2.198 ns | 0.0262 ns |  0.0367 ns |      - |         - |
|        IListOnList10_Normal |  45.096 ns | 0.2318 ns |  0.3250 ns | 0.0063 |      40 B |
|   IListOnList10_Accelerated |  19.821 ns | 0.1801 ns |  0.2695 ns |      - |         - |
|       IListOnList100_Normal | 420.507 ns | 1.1876 ns |  1.6648 ns | 0.0062 |      40 B |
|  IListOnList100_Accelerated | 196.444 ns | 0.4998 ns |  0.7168 ns |      - |         - |

|         IDictOnDict0_Normal |  10.727 ns | 0.0591 ns |  0.0866 ns | 0.0076 |      48 B |
|    IDictOnDict0_Accelerated |   6.232 ns | 0.1342 ns |  0.1967 ns |      - |         - |
|        IDictOnDict10_Normal |  55.055 ns | 0.3224 ns |  0.4825 ns | 0.0076 |      48 B |
|   IDictOnDict10_Accelerated |  79.291 ns | 0.3768 ns |  0.5639 ns | 0.0076 |      48 B |
|       IDictOnDict100_Normal | 455.573 ns | 1.9593 ns |  2.8100 ns | 0.0076 |      48 B |
|  IDictOnDict100_Accelerated | 542.763 ns | 3.2932 ns |  4.7230 ns | 0.0076 |      48 B |

|        IEnumOnArray0_Normal |   5.336 ns | 0.0295 ns |  0.0404 ns |      - |         - |
|   IEnumOnArray0_Accelerated |   8.958 ns | 0.0769 ns |  0.1127 ns |      - |         - |
|       IEnumOnArray10_Normal |  39.912 ns | 0.2146 ns |  0.3146 ns | 0.0051 |      32 B |
|  IEnumOnArray10_Accelerated |  22.489 ns | 0.1937 ns |  0.2778 ns |      - |         - |
|      IEnumOnArray100_Normal | 317.651 ns | 1.7742 ns |  2.6006 ns | 0.0048 |      32 B |
| IEnumOnArray100_Accelerated | 164.408 ns | 0.7622 ns |  1.1409 ns |      - |         - |

|         IEnumOnList0_Normal |   8.495 ns | 0.2161 ns |  0.3168 ns | 0.0064 |      40 B |
|    IEnumOnList0_Accelerated |  12.843 ns | 0.1593 ns |  0.2384 ns |      - |         - |
|        IEnumOnList10_Normal |  45.361 ns | 0.2171 ns |  0.3043 ns | 0.0063 |      40 B |
|   IEnumOnList10_Accelerated |  38.079 ns | 0.1196 ns |  0.1596 ns |      - |         - |
|       IEnumOnList100_Normal | 421.577 ns | 1.9884 ns |  2.9145 ns | 0.0062 |      40 B |
|  IEnumOnList100_Accelerated | 272.963 ns | 1.6057 ns |  2.2510 ns |      - |         - |

|        IEnumOnStack0_Normal |   8.683 ns | 0.1161 ns |  0.1664 ns | 0.0064 |      40 B |
|   IEnumOnStack0_Accelerated |  19.399 ns | 0.1143 ns |  0.1676 ns |      - |         - |
|       IEnumOnStack10_Normal |  50.389 ns | 0.1712 ns |  0.2343 ns | 0.0063 |      40 B |
|  IEnumOnStack10_Accelerated | 102.005 ns | 0.9595 ns |  1.4361 ns | 0.0063 |      40 B |
|      IEnumOnStack100_Normal | 428.895 ns | 2.9668 ns |  4.4406 ns | 0.0062 |      40 B |
| IEnumOnStack100_Accelerated | 705.237 ns | 7.7370 ns | 11.3408 ns | 0.0057 |      40 B |
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

