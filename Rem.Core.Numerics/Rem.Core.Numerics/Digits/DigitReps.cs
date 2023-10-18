using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics.Digits;

/// <summary>
/// Static functionality relating to digit representations of numbers.
/// </summary>
public static class DigitReps
{
    #region Constants
    private const uint NegativeIntMinValue = int.MaxValue + 1u;
    private const ulong NegativeLongMinValue = long.MaxValue + 1ul;
    #endregion

    #region Representations
    #region Integral
    #region Fixed Size
    #region 32-Bit
    /// <summary>
    /// Gets a representation of the current <see cref="int"/> in the given base.
    /// </summary>
    /// <inheritdoc cref="InBase(BigInteger, BigInteger)"/>
    public static SignedIntegralDigitRep InBase(
        int n, [GreaterThanOrEqualToInteger(2)] int Base)
    {
        Throw.IfArgLessThan(2, Base, nameof(Base));

        // Treat the value as unsigned
        // Need to avoid reversing the sign of the min value directly since that is not possible
        var isNegative = n < 0;
        uint unsigned;
        if (isNegative) unsigned = n == int.MinValue ? NegativeIntMinValue : unchecked((uint)-n);
        else unsigned = unchecked((uint)n);

        return new(isNegative, Base, unsigned.GetDigitsInBaseUnchecked(unchecked((uint)Base)));
    }

    /// <summary>
    /// Gets a representation of the current <see cref="uint"/> in the given base.
    /// </summary>
    /// <inheritdoc cref="InBase(BigInteger, BigInteger)"/>
    public static UnsignedIntegralDigitRep InBase(
        uint n, [GreaterThanOrEqualToInteger(2)] uint Base)
        => new(Base, n.GetDigitsInBaseUnchecked(Throw.IfArgLessThan(2u, Base, nameof(Base))));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static DigitList GetDigitsInBaseUnchecked(this uint n, [GreaterThanOrEqualToInteger(2)] uint Base)
    {
        var digitListBuilder = DigitList.Builder.NewFromBaseSize(Base);

        // Get the digits of the representation in reverse order, then reverse the list
        while (n > 0)
        {
            digitListBuilder.Add(n % Base);
            n /= Base;
        }
        digitListBuilder.Reverse();

        return digitListBuilder.ToList();
    }
    #endregion

    #region 64-Bit
    /// <summary>
    /// Gets a representation of the current <see cref="long"/> in the given base.
    /// </summary>
    /// <inheritdoc cref="InBase(BigInteger, BigInteger)"/>
    public static SignedIntegralDigitRep InBase(
        long n, [GreaterThanOrEqualToInteger(2)] long Base)
    {
        Throw.IfArgLessThan(2, Base, nameof(Base));

        // Treat the value as unsigned
        // Need to avoid reversing the sign of the min value directly since that is not possible
        var isNegative = n < 0;
        ulong unsigned;
        if (isNegative) unsigned = n == long.MinValue ? NegativeLongMinValue : unchecked((ulong)-n);
        else unsigned = unchecked((ulong)n);

        return new(isNegative, Base, unsigned.GetDigitsInBaseUnchecked(unchecked((ulong)Base)));
    }

    /// <summary>
    /// Gets a representation of the current <see cref="ulong"/> in the given base.
    /// </summary>
    /// <inheritdoc cref="InBase(BigInteger, BigInteger)"/>
    public static UnsignedIntegralDigitRep InBase(
        ulong n, [GreaterThanOrEqualToInteger(2)] ulong Base)
        => new(Base, n.GetDigitsInBaseUnchecked(Throw.IfArgLessThan(2ul, Base, nameof(Base))));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static DigitList GetDigitsInBaseUnchecked(this ulong n, [GreaterThanOrEqualToInteger(2)] ulong Base)
    {
        var digitListBuilder = DigitList.Builder.NewFromBaseSize(Base);

        // Get the digits of the representation in reverse order, then reverse the list
        while (n > 0)
        {
            digitListBuilder.Add(n % Base);
            n /= Base;
        }
        digitListBuilder.Reverse();

        return digitListBuilder.ToList();
    }
    #endregion
    #endregion
    #endregion

    #region Arbitrary Size
    /// <summary>
    /// Gets a representation of the current <see cref="BigInteger"/> in the given base.
    /// </summary>
    /// <param name="n"></param>
    /// <param name="Base"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Base"/> was less than 2.</exception>
    public static SignedIntegralDigitRep InBase(
        BigInteger n, [GreaterThanOrEqualToInteger(2)] BigInteger Base)
    {
        Throw.IfArgLessThan(2, Base, nameof(Base));

        var isNegative = n.Sign < 0;
        if (isNegative) n = -n;

        var baseUnsigned = new BigUnsignedInteger(Base);

        var digitListBuilder = DigitList.Builder.NewFromBaseSize(baseUnsigned);

        // Get the digits of the representation in reverse order, then reverse the list
        while (!n.IsZero)
        {
            n = BigInteger.DivRem(n, Base, out var digit);
            digitListBuilder.Add(new BigUnsignedInteger(digit));
        }
        digitListBuilder.Reverse();

        return new(isNegative, Base, digitListBuilder.ToList());
    }

    /// <summary>
    /// Gets a representation of the current <see cref="BigInteger"/> in the given base.
    /// </summary>
    /// <param name="n"></param>
    /// <param name="Base"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Base"/> was less than 2.</exception>
    public static UnsignedIntegralDigitRep InBase(
        BigUnsignedInteger n, [GreaterThanOrEqualToInteger(2)] BigUnsignedInteger Base)
    {
        Throw.IfArgLessThan(2, Base, nameof(Base));

        var digitListBuilder = DigitList.Builder.NewFromBaseSize(Base);

        // Get the digits of the representation in reverse order, then reverse the list
        while (!n.IsZero)
        {
            n = BigUnsignedInteger.DivRem(n, Base, out var digit);
            digitListBuilder.Add(digit);
        }
        digitListBuilder.Reverse();

        return new(Base, digitListBuilder.ToList());
    }
    #endregion
    #endregion
}

