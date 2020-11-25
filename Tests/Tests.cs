using NUnit.Framework;
using System;
using static RefTools.Ref;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test_Inc_HeapArray()
        {
            var x = new ulong[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            };

            ref readonly var end = ref x[x.Length - 1];
            ulong sum = 0;

            for (
                ref readonly var itt = ref x[0];
                IsLess(in itt, in end) || AreSame(in itt, in end);
                itt = ref Inc(in itt)
                )
                sum += itt;

            Assert.AreEqual(sum, 120);
        }

        [Test]
        public void Test_Dec_HeapArray()
        {
            var x = new ulong[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            };

            ref readonly var start = ref x[0];
            ulong sum = 0;

            for (
                ref readonly var itt = ref x[x.Length - 1];
                IsLess(in start, in itt) || AreSame(in start, in itt);
                itt = ref Dec(in itt)
                )
                sum += itt;

            Assert.AreEqual(sum, 120);
        }

        [Test]
        public void Test_Inc_Stack()
        {
            ReadOnlySpan<ulong> x = stackalloc ulong[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            };

            ref readonly var end = ref x[x.Length - 1];
            ulong sum = 0;

            for (
                ref readonly var itt = ref x[0];
                IsLess(in itt, in end) || AreSame(in itt, in end);
                itt = ref Inc(in itt)
            )
                sum += itt;

            Assert.AreEqual(sum, 120);
        }

        [Test]
        public void Test_Dec_Stack()
        {
            ReadOnlySpan<ulong> x = stackalloc ulong[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            };

            ref readonly var start = ref x[0];
            ulong sum = 0;

            for (
                ref readonly var itt = ref x[x.Length - 1];
                IsLess(in start, in itt) || AreSame(in start, in itt);
                itt = ref Dec(in itt)
            )
                sum += itt;

            Assert.AreEqual(sum, 120);
        }

        [Test]
        public void Test_ElementOffset_Stack()
        {
            ReadOnlySpan<ulong> x = stackalloc ulong[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            };

            Assert.AreEqual(ItemOffset(in x[0], in x[0]), IntPtr.Zero);
            Assert.AreEqual(ItemOffset(in x[0], in x[1]), (IntPtr)1);
            Assert.AreEqual(ItemOffset(in x[1], in x[2]), (IntPtr)1);
            Assert.AreEqual(ItemOffset(in x[1], in x[x.Length - 1]), (IntPtr)(x.Length - 2));
            Assert.AreEqual(ItemOffset(in x[0], in x[x.Length - 1]), (IntPtr)(x.Length - 1));

        }

        [Test]
        public void Test_IncDecMut_HeapArray()
        {
            var x = new ulong[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
            };
            var y = new ulong[x.Length];

            ref readonly var xEnd = ref x[x.Length - 1];
            ref readonly var yStart = ref y[0];

            ref readonly var xItt = ref x[0];
            ref var yItt = ref y[y.Length - 1];

            for (
                ;
                Compare(in xItt, in xEnd) <= 0 && Compare(in yItt, in yStart) >= 0;
                xItt = ref Inc(in xItt), yItt = ref DecMut(ref yItt)
            )
                yItt = xItt;

            for(
                ref readonly ulong xItt2 = ref x[0], yItt2 = ref y[y.Length - 1];
                Compare(in xItt2, in xEnd) <= 0 && Compare(in yItt2, in yStart) >= 0;
                xItt2 = ref Inc(in xItt2), yItt2 = ref Dec(in yItt2)
            )
                Assert.AreEqual(xItt2, yItt2);

        }

        [Test]
        public void Test_Nulls()
        {
            ref readonly var @null = ref NullRef<uint>();

            Assert.IsTrue(IsNull(in @null));
        }
    }
}