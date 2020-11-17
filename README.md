# RefTools
Tools to work with C# (.NET) managed references.

# Examples
## Iterators
A continuous block of memory (such as an array or stack-alloced block) can be iterated over using pointer arithemtic.
Something similar can be implemented in `C#` thanks to ref locals and ref returns.

If we define two methods, `Begin` and `End`
```csharp
    internal static class Iter
    {
        public static ref readonly T Begin<T>(this ReadOnlySpan<T> @this) where T : unmanaged
            => ref @this[0];

        public static ref readonly T End<T>(this ReadOnlySpan<T> @this) where T : unmanaged
            => ref Inc(in @this[@this.Length - 1]);
    }
```
then an iteration over a continuous block of memory can happen in the following manner
```csharp
        public void Test_Inc_HeapArray()
        {
            var z = new ulong[1024];

            ReadOnlySpan<ulong> x = new ulong[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            };
            ulong sum = 0;

            for (
                ref readonly var itt = ref x.Begin();
                !AreSame(in itt, in x.End());
                itt = ref Inc(in itt)
            )
              sum += itt;

            Assert.AreEqual(sum, 120);
        }
```
Here, `End()` points to the place 'after' the last element, which can be dangerous and needs testing, especially when garbage collection happens after the reference is obtained.
The logic of `for`-loop is strighforward: intialize iterator variable with a reference to `Begin()`, continue while `itt` is not equal to `End()` (can be replaced by `IsAddressLessThan` for safety), at the end of each step increment `itt` by 1 (using a shortcut to `Add(in itt, 1)`).
The `readonly` keyword allows for some additional compile-time checks, preventing from writing to the place, pointed by `ref readonly`. With a number of helpers which wrap `Unsafe.*` methods and provide overloads for `ref readonly` aka `in` parameters, the code looks clean.

The same can be done for e.g. multiple iterators:
```csharp

      for (
          ref readonly var xItt = ref x.Begin(), yItt = ref y.Begin();
          !AreSame(in xItt, in x.End()) && !AreSame(in yItt, in y.End());
          xItt = ref Inc(in xItt), yItt = ref Inc(in yItt)
      )
        if (xItt != yItt)
          throw new Exception();
```
