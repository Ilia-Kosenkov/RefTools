using System;
using NUnit.Framework;
using RefTools;
using static RefTools.Ref;

namespace Tests
{
 

    [TestFixture]
    public class IterTest
    {
        [Test]
        public void Test_Inc_HeapArray()
        {
            //var z = new ulong[1024];
            //z[5] = 10;

            ReadOnlySpan<ulong> x = new ulong[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            };
            ulong sum = 0;
            for (
                ref readonly var itt = ref x.Begin();
                IsLess(in itt, in x.End());
                itt = ref Inc(in itt)
            )
            {
                //GC.Collect(2, GCCollectionMode.Forced, true, true);
                sum += itt;
            }

            Assert.AreEqual(sum, 120);
        }

        [Test]
        public void Test_For_HeapArray()
        {

            ReadOnlySpan<ulong> x = new ulong[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            };
            ulong sum = 0;

            for (
                var i = 0;
                i < x.Length;
                ++i
            )
                sum += x[i];


            Assert.AreEqual(sum, 120);
        }

        [Test]
        public unsafe void Test_Ptr_HeapArray()
        {

            ReadOnlySpan<ulong> x = new ulong[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            };
            ulong sum = 0;

            fixed (ulong* ptr = x)
            {
                var end = ptr + x.Length;
                for (
                    var itt = ptr;
                    itt != end;
                    ++itt
                )
                    sum += *itt;
            }


            Assert.AreEqual(sum, 120);
        }

    }
}
