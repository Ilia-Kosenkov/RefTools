﻿using NUnit.Framework;
using System;
using System.Runtime.CompilerServices;
using static RefTools.Ref;

namespace Tests
{
    [TestFixture]
    public class AddrTests
    {
        [Test]
        public unsafe void Test_CompareAddrWithPointer()
        {
            var filler = new ulong[1024];
            filler[42] = 42;
            var buff = new ulong[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16
            };

            ref var begin = ref buff[0];

            var addrRef1 = AddrOf(in begin);
            var pointerRef1 = (nint)Unsafe.AsPointer(ref begin);
            IntPtr fixedRef1;
            fixed (ulong* tempPtr = buff)
                fixedRef1 = new IntPtr(tempPtr);

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);

            var addrRef2 = AddrOf(in begin);
            var pointerRef2 = new IntPtr(Unsafe.AsPointer(ref begin));
            IntPtr fixedRef2;
            fixed (ulong* tempPtr = buff)
                fixedRef2 = new IntPtr(tempPtr);

            Assume.That((long) addrRef1, Is.Not.EqualTo((long) addrRef2));
            Assume.That((long) pointerRef1, Is.Not.EqualTo((long) pointerRef2));
            Assume.That((long) fixedRef1, Is.Not.EqualTo((long) fixedRef2));


            Assert.AreEqual((long) addrRef1, (long) pointerRef1);
            Assert.AreEqual((long)addrRef1, (long)fixedRef1);

            Assert.AreEqual((long) addrRef2, (long) pointerRef2);
            Assert.AreEqual((long)addrRef2, (long)fixedRef2);



        }
    }
}
