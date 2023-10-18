using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics.Digits.Test;

/// <summary>
/// Tests of the <see cref="DigitList"/> class hierarchy.
/// </summary>
[TestClass]
public class DigitListTest
{
    /// <summary>
    /// Tests the <see cref="DigitList.Builder.NewFromBaseSize(BigInteger)"/> factory method and overloads.
    /// </summary>
    [TestMethod]
    public void TestNewBuilderFromBaseSize()
    {
        // Test the limits of the range of builder types based on the size of the base passed in
        // A list of a given digit type should be able to handle a base that is one too large for the type
        var tests = new (Type SmallerBuilderType, Type BiggerBuilderType, BigUnsignedInteger Limit)[]
        { 
            (typeof(ByteDigitList.Builder), typeof(UShortDigitList.Builder), byte.MaxValue + BigUnsignedInteger.One),
            (typeof(UShortDigitList.Builder), typeof(UIntDigitList.Builder), ushort.MaxValue + BigUnsignedInteger.One),
            (typeof(UIntDigitList.Builder), typeof(ULongDigitList.Builder), uint.MaxValue + BigUnsignedInteger.One),
            (typeof(ULongDigitList.Builder), typeof(BigUnsignedIntegerDigitList.Builder), ulong.MaxValue + BigUnsignedInteger.One),
        };

        foreach (var (smallerType, biggerType, limit) in tests)
        {
            Assert.IsInstanceOfType(DigitList.Builder.NewFromBaseSize(limit), smallerType);
            Assert.IsInstanceOfType(DigitList.Builder.NewFromBaseSize(limit + 1), biggerType);

            if (limit < ulong.MaxValue)
            {
                var fixedSizeLimit = (ulong)limit;
                Assert.IsInstanceOfType(DigitList.Builder.NewFromBaseSize(fixedSizeLimit), smallerType);
                Assert.IsInstanceOfType(DigitList.Builder.NewFromBaseSize(fixedSizeLimit + 1), biggerType);
            }

            if (limit < uint.MaxValue)
            {
                var fixedSizeLimit = (uint)limit;
                Assert.IsInstanceOfType(DigitList.Builder.NewFromBaseSize(fixedSizeLimit), smallerType);
                Assert.IsInstanceOfType(DigitList.Builder.NewFromBaseSize(fixedSizeLimit + 1), biggerType);
            }

            if (limit < ushort.MaxValue)
            {
                var fixedSizeLimit = (ushort)limit;
                Assert.IsInstanceOfType(DigitList.Builder.NewFromBaseSize(fixedSizeLimit), smallerType);
                Assert.IsInstanceOfType(DigitList.Builder.NewFromBaseSize(fixedSizeLimit + 1u), biggerType);
            }
        }
    }

    /// <summary>
    /// Tests the <see cref="DigitList.WithoutLeadingZeroes"/> method.
    /// </summary>
    [TestMethod]
    public void TestWithoutLeadingZeroes()
    {
        var listWithoutZeroes = ByteDigitList.CreateRange(1, 2, 3, 4, 5);
        var listWithZeroes = ByteDigitList.CreateRange(0, 0, 0, 1, 2, 3, 4, 5);
        Assert.AreEqual(listWithoutZeroes, listWithZeroes.WithoutLeadingZeroes());
        Assert.AreEqual(listWithoutZeroes, listWithoutZeroes.WithoutLeadingZeroes()); // Should be no change
    }

    /// <summary>
    /// Tests the <see cref="DigitList.SplitAtIndices(int[])"/> method.
    /// </summary>
    [TestMethod]
    public void TestSplitAtIndices()
    {
        var list = ByteDigitList.CreateRange(0, 1, 2, 3, 4, 5, 6);
        Assert.IsTrue(list.SplitAtIndices().SequenceEqual(new[] { list }));
        Assert.IsTrue(list.SplitAtIndices(0).SequenceEqual(new[] { ByteDigitList.Empty, list }));
        Assert.IsTrue(
            list.SplitAtIndices(2, 4).SequenceEqual(new[]
            {
                ByteDigitList.CreateRange(0, 1), ByteDigitList.CreateRange(2, 3), ByteDigitList.CreateRange(4, 5, 6)
            }));

        DigitList nonGenericList = list;
        Assert.IsTrue(nonGenericList.SplitAtIndices().SequenceEqual(new[] { nonGenericList }));
        Assert.IsTrue(nonGenericList.SplitAtIndices(0).SequenceEqual(new[] { ByteDigitList.Empty, nonGenericList }));
        Assert.IsTrue(
            nonGenericList.SplitAtIndices(2, 4).SequenceEqual(new[]
            {
                ByteDigitList.CreateRange(0, 1), ByteDigitList.CreateRange(2, 3), ByteDigitList.CreateRange(4, 5, 6)
            }));

        var singleton = ByteDigitList.CreateRange(3);
        Assert.IsTrue(singleton.SplitAtIndices(0).SequenceEqual(new[] { ByteDigitList.Empty, singleton }));

        Assert.ThrowsException<ArgumentNullException>(() => list.SplitAtIndices(null!));
        Assert.ThrowsException<IndexOutOfRangeException>(() => list.SplitAtIndices(7));
        Assert.ThrowsException<IndexOutOfRangeException>(() => list.SplitAtIndices(-1));
        Assert.ThrowsException<ArgumentException>(() => list.SplitAtIndices(4, 2));
    }

    /// <summary>
    /// Tests the <see cref="DigitList.IsEquivalentTo(DigitList)"/> method.
    /// </summary>
    [TestMethod]
    public void TestIsEquivalentTo()
    {
        var equivalentLists1 = CreateEquivalentLists(1, 2, 3, 4);
        var equivalentLists2 = CreateEquivalentLists(2, 3, 4, 5);
        var equivalentLists3 = CreateEquivalentLists(1, 2);

        foreach (var expected in equivalentLists1.Values)
        {
            foreach (var actual in equivalentLists1.Values)
            {
                Assert.That.ListsAreEquivalent(expected, actual);
            }
        }
        foreach (var lhs in equivalentLists1.Values)
        {
            foreach (var rhs in equivalentLists2.Values)
            {
                Assert.That.ListsAreNotEquivalent(lhs, rhs);
            }
        }
        foreach (var lhs in equivalentLists1.Values)
        {
            foreach (var rhs in equivalentLists3.Values)
            {
                Assert.That.ListsAreNotEquivalent(lhs, rhs);
            }
        }
    }

    private static ImmutableDictionary<Type, DigitList> CreateEquivalentLists(params byte[] digits)
        => ImmutableDictionary.CreateRange(new KeyValuePair<Type, DigitList>[]
        {
            new(typeof(byte), ByteDigitList.CreateRange(digits)),
            new(typeof(ushort), UShortDigitList.CreateRange(digits.Select(b => (ushort)b))),
            new(typeof(uint), UIntDigitList.CreateRange(digits.Select(b => (uint)b))),
            new(typeof(ulong), ULongDigitList.CreateRange(digits.Select(b => (ulong)b))),
            new(typeof(BigInteger), BigUnsignedIntegerDigitList.CreateRange(digits.Select(b => (BigUnsignedInteger)b))),
        });
}
