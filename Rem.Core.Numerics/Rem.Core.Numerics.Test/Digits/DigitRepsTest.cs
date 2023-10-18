using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics.Digits.Test;

using static DigitReps;

/// <summary>
/// Tests of the <see cref="DigitReps"/> class.
/// </summary>
[TestClass]
public class DigitRepsTest
{
    private const string UnsignedCategoryName = "UnsignedIntegral";
    private const string SignedCategoryName = "SignedIntegral";

    /// <summary>
    /// Tests the <see cref="UnsignedIntegralDigitRep.Create(BigInteger, DigitList)"/> method.
    /// </summary>
    [TestMethod, TestCategory(UnsignedCategoryName)]
    public void TestUnsignedIntegralCreate()
    {
        // Should strip off all leading zeroes
        Assert.That.DigitRepEquals(
            12, ByteDigitList.CreateRange(1, 0),
            UnsignedIntegralDigitRep.Create(12, ByteDigitList.CreateRange(0, 0, 0, 1, 0)));

        // Should be unchanged
        Assert.That.DigitRepEquals(
            12, ByteDigitList.CreateRange(1, 2, 3),
            UnsignedIntegralDigitRep.Create(12, ByteDigitList.CreateRange(1, 2, 3)));
    }

    /// <summary>
    /// Tests the methods for generating digit representations of unsigned integral values.
    /// </summary>
    [TestMethod, TestCategory(UnsignedCategoryName)]
    public void TestUnsignedIntegralRepresentations()
    {
        // Zero tests
        Assert.That.DigitRepEquals(12, ByteDigitList.CreateRange(), InBase(0u, 12));
        Assert.That.DigitRepEquals(12, ByteDigitList.CreateRange(), InBase(0ul, 12));

        Assert.That.DigitRepEquals(10, ByteDigitList.CreateRange(3, 4, 5, 6, 7, 0, 1), InBase(3456701u, 10));
        Assert.That.DigitRepEquals(10, ByteDigitList.CreateRange(3, 4, 5, 6, 7, 0, 1), InBase(3456701ul, 10));
        Assert.That.DigitRepEquals(30000, UShortDigitList.CreateRange(1, 4, 23), InBase(900120023u, 30000));
        Assert.That.DigitRepEquals(30000, UShortDigitList.CreateRange(1, 4, 23), InBase(900120023ul, 30000));
    }

    /// <summary>
    /// Tests the <see cref="SignedIntegralDigitRep.Create(bool, BigInteger, DigitList)"/> method.
    /// </summary>
    [TestMethod, TestCategory(SignedCategoryName)]
    public void TestSignedIntegralCreate()
    {
        // Should strip off all leading zeroes
        Assert.That.DigitRepEquals(
            true, 12, ByteDigitList.CreateRange(1, 0),
            SignedIntegralDigitRep.Create(true, 12, ByteDigitList.CreateRange(0, 0, 0, 1, 0)));

        // Should set the `IsNegative` flag to `false` since the representation is equivalent to zero
        Assert.That.DigitRepEquals(
            false, 12, ByteDigitList.CreateRange(),
            SignedIntegralDigitRep.Create(true, 12, ByteDigitList.CreateRange(0, 0)));

        // Should be unchanged
        Assert.That.DigitRepEquals(
            true, 12, ByteDigitList.CreateRange(1, 2, 3),
            SignedIntegralDigitRep.Create(true, 12, ByteDigitList.CreateRange(1, 2, 3)));
    }

    /// <summary>
    /// Tests the methods for generating digit representations of signed integral values.
    /// </summary>
    [TestMethod, TestCategory(SignedCategoryName)]
    public void TestSignedIntegralRepresentation()
    {
        // Zero tests
        Assert.That.DigitRepEquals(false, 12, ByteDigitList.CreateRange(), InBase(0, 12));
        Assert.That.DigitRepEquals(false, 12, ByteDigitList.CreateRange(), InBase(0L, 12));

        Assert.That.DigitRepEquals(false, 10, ByteDigitList.CreateRange(4, 2, 1, 3, 0, 5), InBase(421305, 10));
        Assert.That.DigitRepEquals(false, 10, ByteDigitList.CreateRange(4, 2, 1, 3, 0, 5), InBase(421305L, 10));
        Assert.That.DigitRepEquals(
            false, 10, ByteDigitList.CreateRange(4, 2, 1, 3, 0, 5), InBase(new BigInteger(421305), 10));
        Assert.That.DigitRepEquals(true, 300, UShortDigitList.CreateRange(4, 20, 2, 2), InBase(-109800602, 300));
        Assert.That.DigitRepEquals(true, 300, UShortDigitList.CreateRange(4, 20, 2, 2), InBase(-109800602L, 300));
        Assert.That.DigitRepEquals(
            true, 300, UShortDigitList.CreateRange(4, 20, 2, 2), InBase(new BigInteger(-109800602), 300));
    }
}
