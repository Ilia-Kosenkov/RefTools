using System;
using NUnit.Framework;
using static RefTools.Ref;

namespace Tests
{
    internal static class Iter
    {
        public static ref readonly T Begin<T>(this ReadOnlySpan<T> @this) where T : unmanaged
            => ref @this[0];

        public static ref readonly T End<T>(this ReadOnlySpan<T> @this) where T : unmanaged
            => ref Inc(in @this[@this.Length - 1]);
    }

    [TestFixture]
    public class IterTest
    {
        [Test]
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
            {
                GC.Collect(2, GCCollectionMode.Forced, true, true);
                GC.Collect(2, GCCollectionMode.Forced, true, true);

                sum += itt;
            }


            Assert.AreEqual(sum, 120);
        }

    }
}
