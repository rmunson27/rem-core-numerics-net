using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics.Digits.Test;

/// <summary>
/// Extensions for the <see cref="Assert"/> class.
/// </summary>
internal static class AssertExtensions
{
    /// <summary>
    /// Asserts that the two <see cref="DigitList"/> instances are equivalent.
    /// </summary>
    /// <param name="expected"></param>
    /// <param name="rhs"></param>
    /// <param name="message"></param>
    public static void ListsAreNotEquivalent(this Assert _, DigitList lhs, DigitList rhs, string message = "")
    {
        Assert.IsFalse(
            lhs.IsEquivalentTo(rhs),
            $"({lhs.DigitType.RepresentedType}) digit list {lhs} was equivalent to"
                + $" ({rhs.DigitType.RepresentedType}) digit list {rhs}"
                + (string.IsNullOrEmpty(message) ? "." : $": {message}"));

        Assert.IsFalse(
            rhs.IsEquivalentTo(lhs),
            $"({rhs.DigitType.RepresentedType}) digit list {rhs} was equivalent to"
                + $" ({lhs.DigitType.RepresentedType}) digit list {lhs}"
                + (string.IsNullOrEmpty(message) ? "." : $": {message}"));
    }

    /// <summary>
    /// Asserts that the two <see cref="DigitList"/> instances are equivalent.
    /// </summary>
    /// <param name="expected"></param>
    /// <param name="actual"></param>
    /// <param name="message"></param>
    public static void ListsAreEquivalent(this Assert _, DigitList expected, DigitList actual, string message = "")
    {
        Assert.IsTrue(
            expected.IsEquivalentTo(actual),
            $"({actual.DigitType.RepresentedType}) digit list {actual} was not equivalent to expected"
                + $"({expected.DigitType.RepresentedType}) digit list {expected}"
                + (string.IsNullOrEmpty(message) ? "." : $": {message}"));
    }

    /// <summary>
    /// Asserts that the digit representation passed in equals the one described by the expected parts.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="expectedBase"></param>
    /// <param name="expectedDigits"></param>
    /// <param name="actualRep"></param>
    /// <param name="message"></param>
    public static void DigitRepEquals(
        this Assert _,
        BigInteger expectedBase, DigitList expectedDigits,
        UnsignedIntegralDigitRep actualRep,
        string message = "")
    {
        Assert.AreEqual(expectedBase, actualRep.Base, message);
        Assert.AreEqual(expectedDigits, actualRep.Digits, message);
    }

    /// <summary>
    /// Asserts that the digit representation passed in equals the one described by the expected parts.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="expectedIsNegative"></param>
    /// <param name="expectedBase"></param>
    /// <param name="expectedDigits"></param>
    /// <param name="actualRep"></param>
    /// <param name="message"></param>
    public static void DigitRepEquals(
        this Assert _,
        bool expectedIsNegative, BigInteger expectedBase, DigitList expectedDigits,
        SignedIntegralDigitRep actualRep,
        string message = "")
    {
        Assert.AreEqual(expectedIsNegative, actualRep.IsNegative, message);
        Assert.AreEqual(expectedBase, actualRep.Base, message);
        Assert.AreEqual(expectedDigits, actualRep.Digits, message);
    }
}
