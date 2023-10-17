using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using Rem.Core.Attributes;
using Rem.Core.ComponentModel;

namespace Rem.Core.Numerics;

/// <summary>
/// Represents an unsigned integer of infinite precision.
/// </summary>
/// <remarks>
/// Note that, unlike finite-precision integral types, this type is a strict subset of the corresponding signed
/// integral type (<see cref="BigInteger"/>), since there are no sufficiently large instances of this type that cannot
/// be expressed as the signed type - however, it may be useful to have this type in scenarios when ensuring
/// a positive sign is necessary, but may be expensive if it must be repeatedly checked.
/// </remarks>
public readonly record struct BigUnsignedInteger
    : IComparable, IComparable<long>, IComparable<ulong>, IComparable<BigInteger>, IComparable<BigUnsignedInteger>
#if NET7_0_OR_GREATER
      , IUnsignedNumber<BigUnsignedInteger>, IBinaryInteger<BigUnsignedInteger>
#endif
{
    #region Constants
    /// <summary>
    /// The one value of this type.
    /// </summary>
    public static readonly BigUnsignedInteger One = new(1);

    /// <summary>
    /// The zero value of this type.
    /// </summary>
    public static readonly BigUnsignedInteger Zero = new(0);
    #endregion

    #region Properties And Fields
#if NET7_0_OR_GREATER
    static BigUnsignedInteger INumberBase<BigUnsignedInteger>.One => One;

    static BigUnsignedInteger INumberBase<BigUnsignedInteger>.Zero => Zero;

    static int INumberBase<BigUnsignedInteger>.Radix => 2;

    static BigUnsignedInteger IAdditiveIdentity<BigUnsignedInteger, BigUnsignedInteger>.AdditiveIdentity => Zero;

    static BigUnsignedInteger IMultiplicativeIdentity<BigUnsignedInteger, BigUnsignedInteger>.MultiplicativeIdentity
        => One;
#endif

    /// <summary>
    /// Gets whether or not this instance is zero.
    /// </summary>
    public bool IsZero => _value.IsZero;

    /// <summary>
    /// Gets whether or not this instance is one.
    /// </summary>
    public bool IsOne => _value.IsOne;

    /// <summary>
    /// Gets whether or not this instance is odd.
    /// </summary>
    public bool IsOdd => !_value.IsEven;

    /// <summary>
    /// Gets whether or not this instance is even.
    /// </summary>
    public bool IsEven => _value.IsEven;

    /// <summary>
    /// Stores the value of this instance.
    /// </summary>
    private readonly BigInteger _value;
    #endregion

    #region Constructor
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BigUnsignedInteger(BigInteger value) { _value = value; }
    #endregion

    #region Comparison
    #region CompareTo
    /// <summary>
    /// Compares this value to another integer.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(long other) => _value.CompareTo(other);

    /// <summary>
    /// Compares this value to another integer.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(ulong other) => _value.CompareTo(other);

    /// <summary>
    /// Compares this value to another integer.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(BigInteger other) => _value.CompareTo(other);

    /// <summary>
    /// Compares this value to another integer.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(BigUnsignedInteger other) => _value.CompareTo(other._value);

    /// <summary>
    /// Compares this value to another integral value.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// <see cref="BigUnsignedInteger"/> values cannot be compared with objects of the type of <paramref name="obj"/>.
    /// </exception>
    /// <exception cref="ArgumentNullException"><paramref name="obj"/> was <see langword="null"/>.</exception>
    public int CompareTo(object? obj) => obj switch
    {
        BigInteger signedOther => _value.CompareTo(signedOther),
#if NET7_0_OR_GREATER
        _ when Maths.IsInt128Representable(obj, out var i) => _value.CompareTo(i),
        _ when Maths.IsUInt128Representable(obj, out var i) => _value.CompareTo(i),
#else
        _ when Maths.IsInt64Representable(obj, out var i) => _value.CompareTo(i),
        _ when Maths.IsUInt64Representable(obj, out var i) => _value.CompareTo(i),
#endif
        null => throw new ArgumentNullException(
                    nameof(obj),
                    $"Cannot compare {nameof(BigUnsignedInteger)} with null."),
        _ => throw new ArgumentException(
                $"Cannot compare {nameof(BigUnsignedInteger)} with value of type {obj.GetType()}."),
    };
    #endregion

    #region Operators
    /// <summary>
    /// Determines if a <see cref="BigUnsignedInteger"/> value is greater than or equal to another.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >=(BigUnsignedInteger left, BigUnsignedInteger right) => left._value >= right._value;

    /// <summary>
    /// Determines if a <see cref="BigUnsignedInteger"/> value is less than or equal to another.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <=(BigUnsignedInteger left, BigUnsignedInteger right) => left._value <= right._value;

    /// <summary>
    /// Determines if a <see cref="BigUnsignedInteger"/> value is greater than or equal to another integer.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >=(BigUnsignedInteger left, BigInteger right) => left._value >= right;

    /// <summary>
    /// Determines if a <see cref="BigUnsignedInteger"/> value is less than or equal to another integer.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <=(BigUnsignedInteger left, BigInteger right) => left._value <= right;

    /// <summary>
    /// Determines if an integer is greater than or equal to a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >=(BigInteger left, BigUnsignedInteger right) => left >= right._value;

    /// <summary>
    /// Determines if an integer is less than or equal to a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <=(BigInteger left, BigUnsignedInteger right) => left <= right._value;

    /// <summary>
    /// Determines if a <see cref="BigUnsignedInteger"/> value is greater than another.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >(BigUnsignedInteger left, BigUnsignedInteger right) => left._value > right._value;

    /// <summary>
    /// Determines if a <see cref="BigUnsignedInteger"/> value is less than another.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <(BigUnsignedInteger left, BigUnsignedInteger right) => left._value < right._value;
    #endregion

    #region Max / Min
#if NET7_0_OR_GREATER
    static BigUnsignedInteger INumberBase<BigUnsignedInteger>.MaxMagnitude(BigUnsignedInteger x, BigUnsignedInteger y)
        => Max(x, y);

    static BigUnsignedInteger INumberBase<BigUnsignedInteger>.MaxMagnitudeNumber(
            BigUnsignedInteger x, BigUnsignedInteger y)
        => Max(x, y);
#endif

    /// <summary>
    /// Gets the maximum of two <see cref="BigUnsignedInteger"/> values.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static BigUnsignedInteger Max(BigUnsignedInteger x, BigUnsignedInteger y)
        => new(BigInteger.Max(x._value, y._value));

#if NET7_0_OR_GREATER
    static BigUnsignedInteger INumberBase<BigUnsignedInteger>.MinMagnitude(
            BigUnsignedInteger x, BigUnsignedInteger y)
        => Min(x, y);

    static BigUnsignedInteger INumberBase<BigUnsignedInteger>.MinMagnitudeNumber(
            BigUnsignedInteger x, BigUnsignedInteger y)
        => Min(x, y);
#endif

    /// <summary>
    /// Gets the minimum of two <see cref="BigUnsignedInteger"/> values.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static BigUnsignedInteger Min(BigUnsignedInteger x, BigUnsignedInteger y)
        => new(BigInteger.Min(x._value, y._value));
    #endregion
    #endregion

    #region Arithmetic
    /// <summary>
    /// Negates the <see cref="BigUnsignedInteger"/> passed in, returning its additive inverse as
    /// a <see cref="BigInteger"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [return: NonPositive]
    public static BigInteger operator -(BigUnsignedInteger value) => -value._value;

    /// <summary>
    /// Returns the <see cref="BigUnsignedInteger"/> passed in.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator +(BigUnsignedInteger value) => value;

    /// <summary>
    /// Computes the sum of two <see cref="BigUnsignedInteger"/> values.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator +(BigUnsignedInteger first, BigUnsignedInteger second)
        => new(first._value + second._value);

    /// <summary>
    /// Computes the sum of a <see cref="BigUnsignedInteger"/> and another integer.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigInteger operator +(BigUnsignedInteger first, BigInteger second) => first._value + second;

    /// <summary>
    /// Computes the sum of a <see cref="BigUnsignedInteger"/> and another integer.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigInteger operator +(BigInteger first, BigUnsignedInteger second) => first + second._value;

    /// <summary>
    /// Computes the difference between two <see cref="BigUnsignedInteger"/> values.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    /// <exception cref="OverflowException"></exception>
    public static BigUnsignedInteger operator -(BigUnsignedInteger first, BigUnsignedInteger second)
        => first >= second
            ? new(first._value - second._value)
            : throw new OverflowException("Result of subtraction would be negative.");

    /// <summary>
    /// Computes the difference between a <see cref="BigUnsignedInteger"/> and another integer.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigInteger operator -(BigUnsignedInteger first, BigInteger second) => first._value - second;

    /// <summary>
    /// Computes the difference between an integer and a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigInteger operator -(BigInteger first, BigUnsignedInteger second) => first - second._value;

    /// <summary>
    /// Computes the product of two <see cref="BigUnsignedInteger"/> values.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator *(BigUnsignedInteger first, BigUnsignedInteger second)
        => new(first._value * second._value);

    /// <summary>
    /// Computes the product of a <see cref="BigUnsignedInteger"/> and another integer.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigInteger operator *(BigUnsignedInteger first, BigInteger second) => first._value * second;

    /// <summary>
    /// Computes the product of a <see cref="BigUnsignedInteger"/> and another integer.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigInteger operator *(BigInteger first, BigUnsignedInteger second) => first * second._value;

    /// <summary>
    /// Computes the quotient of two <see cref="BigUnsignedInteger"/> values.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator /(BigUnsignedInteger first, [NonZero] BigUnsignedInteger second)
        => new(first._value / second._value);

    /// <summary>
    /// Computes the quotient of a <see cref="BigUnsignedInteger"/> and another integer.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigInteger operator /(BigUnsignedInteger first, [NonZero] BigInteger second)
        => first._value / second;

    /// <summary>
    /// Computes the quotient of an integer and a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigInteger operator /(BigInteger first, [NonZero] BigUnsignedInteger second)
        => first / second._value;

    /// <summary>
    /// Computes the remainder of two <see cref="BigUnsignedInteger"/> values.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator %(BigUnsignedInteger first, [NonZero] BigUnsignedInteger second)
        => new(first._value % second._value);

    /// <summary>
    /// Computes the remainder of a <see cref="BigUnsignedInteger"/> and another integer.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator %(BigUnsignedInteger first, [NonZero] BigInteger second)
        => new(first._value % second); // Will always be nonnegative

    /// <summary>
    /// Computes the remainder of an integer and a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigInteger operator %(BigInteger first, [NonZero] BigUnsignedInteger second)
        => first % second._value;

    /// <summary>
    /// Decrements the given <see cref="BigUnsignedInteger"/> by one.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator --(BigUnsignedInteger value) => new(value._value - 1);

    /// <summary>
    /// Increments the given <see cref="BigUnsignedInteger"/> by one.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator ++(BigUnsignedInteger value) => new(value._value + 1);

    /// <summary>
    /// Computes the quotient of the given dividend and divisor, returning the remainder in an <see langword="out"/>
    /// parameter.
    /// </summary>
    /// <param name="dividend"></param>
    /// <param name="divisor"></param>
    /// <param name="remainder"></param>
    /// <returns></returns>
    public static BigUnsignedInteger DivRem(BigUnsignedInteger dividend, [NonZero] BigUnsignedInteger divisor,
                                            out BigUnsignedInteger remainder)
    {
        var signedQuotient = BigInteger.DivRem(dividend._value, divisor._value, out var signedRemainder);
        remainder = new(signedRemainder);
        return new(signedQuotient);
    }

    /// <summary>
    /// Computes the quotient of the given dividend and divisor, returning the remainder in an <see langword="out"/>
    /// parameter.
    /// </summary>
    /// <param name="dividend"></param>
    /// <param name="divisor"></param>
    /// <param name="remainder"></param>
    /// <returns></returns>
    public static BigInteger DivRem(BigUnsignedInteger dividend, [NonZero] BigInteger divisor,
                                    out BigUnsignedInteger remainder)
    {
        var signedQuotient = BigInteger.DivRem(dividend._value, divisor, out var signedRemainder);
        remainder = new(signedRemainder); // Must be nonnegative
        return signedQuotient;
    }

    /// <summary>
    /// Computes the quotient of the given dividend and divisor, returning the remainder in an <see langword="out"/>
    /// parameter.
    /// </summary>
    /// <param name="dividend"></param>
    /// <param name="divisor"></param>
    /// <param name="remainder"></param>
    /// <returns></returns>
    public static BigInteger DivRem(BigInteger dividend, [NonZero] BigUnsignedInteger divisor,
                                    out BigInteger remainder)
        => BigInteger.DivRem(dividend, divisor._value, out remainder);

    /// <summary>
    /// Computes the base-10 log of the given <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double Log10(BigUnsignedInteger value) => BigInteger.Log10(value._value);

    /// <summary>
    /// Computes the natural (base e) logarithm of the given <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double Log(BigUnsignedInteger value) => BigInteger.Log(value._value);

    /// <summary>
    /// Computes the greatest common divisor of two inte
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigUnsignedInteger GreatestCommonDivisor(BigUnsignedInteger first, BigUnsignedInteger second)
        => new(BigInteger.GreatestCommonDivisor(first, second));

#if NET7_0_OR_GREATER
    /// <summary>
    /// Returns the value passed in.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    static BigUnsignedInteger INumberBase<BigUnsignedInteger>.Abs(BigUnsignedInteger value) => value;

    /// <summary>
    /// Gets the base-2 log of the given <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static BigUnsignedInteger Log2(BigUnsignedInteger value) => new(BigInteger.Log2(value._value));

    [DoesNotReturn]
    static BigUnsignedInteger IUnaryNegationOperators<BigUnsignedInteger, BigUnsignedInteger>.operator -(
        BigUnsignedInteger value)
    {
        throw new InvalidOperationException("Cannot compute a positive negation of a positive value.");
    }

    /// <summary>
    /// Determines if the given <see cref="BigUnsignedInteger"/> is a power of 2.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsPow2(BigUnsignedInteger value) => BigInteger.IsPow2(value._value);
#endif
    #endregion

    #region Bitwise Operations
    /// <summary>
    /// Computes the bitwise AND of two <see cref="BigUnsignedInteger"/> values.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator &(BigUnsignedInteger first, BigUnsignedInteger second)
        => new(first._value & second._value);

    /// <summary>
    /// Computes the bitwise AND of two <see cref="BigUnsignedInteger"/> values.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator &(BigUnsignedInteger first, BigInteger second)
        => new(first._value & second); // Always nonnegative

    /// <summary>
    /// Computes the bitwise AND of an integer and a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator &(BigInteger first, BigUnsignedInteger second)
        => new(first & second._value); // Always nonnegative

    /// <summary>
    /// Computes the bitwise OR of two <see cref="BigUnsignedInteger"/> values.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator |(BigUnsignedInteger first, BigUnsignedInteger second)
        => new(first._value | second._value);

    /// <summary>
    /// Computes the bitwise OR of a <see cref="BigUnsignedInteger"/> and another integer.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigInteger operator |(BigUnsignedInteger first, BigInteger second) => first._value | second;

    /// <summary>
    /// Computes the bitwise OR of an integer and a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static BigInteger operator |(BigInteger first, BigUnsignedInteger second) => first | second._value;

    /// <summary>
    /// Computes the bitwise complement of a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [return: NonPositive]
    public static BigInteger operator ~(BigUnsignedInteger value) => ~value._value;

    /// <summary>
    /// Computes the bitwise exclusive-OR (XOR) of two <see cref="BigUnsignedInteger"/> values.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator ^(BigUnsignedInteger left, BigUnsignedInteger right)
        => new(left._value ^ right._value);

    /// <summary>
    /// Computes the left shift of the given <see cref="BigUnsignedInteger"/> by the given amount.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="shiftAmount"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator <<(BigUnsignedInteger value, int shiftAmount)
        => new(value._value << shiftAmount);

    /// <summary>
    /// Computes the left shift of the given <see cref="BigUnsignedInteger"/> by the given amount.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="shiftAmount"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator >>(BigUnsignedInteger value, int shiftAmount)
        => new(value._value >> shiftAmount);

    /// <summary>
    /// Computes the left shift of the given <see cref="BigUnsignedInteger"/> by the given amount.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="shiftAmount"></param>
    /// <returns></returns>
    public static BigUnsignedInteger operator >>>(BigUnsignedInteger value, int shiftAmount)
        => new(value._value >> shiftAmount);

#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
    /// <summary>
    /// Gets the byte count of this instance.
    /// </summary>
    /// <returns></returns>
    public int GetByteCount()
    {
        return _value.GetByteCount();
    }
#endif

#if NET7_0_OR_GREATER
    [DoesNotReturn]
    static BigUnsignedInteger IBitwiseOperators<BigUnsignedInteger, BigUnsignedInteger, BigUnsignedInteger>.operator ~(
            BigUnsignedInteger value)
        => throw new InvalidOperationException("Cannot find positive ones complement of positive number.");

    int IBinaryInteger<BigUnsignedInteger>.GetShortestBitLength()
        => (_value as IBinaryInteger<BigInteger>).GetShortestBitLength();

    /// <summary>
    /// Computes the number of bits that are set in the given <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static BigUnsignedInteger PopCount(BigUnsignedInteger value) => new(BigInteger.PopCount(value._value));
#endif
    #endregion

    #region Conversion
    /// <summary>
    /// Implicitly casts a <see cref="ulong"/> to a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="ul"></param>
    public static implicit operator BigUnsignedInteger(uint ui) => new(ui);

    /// <summary>
    /// Implicitly casts a <see cref="ulong"/> to a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="ul"></param>
    public static implicit operator BigUnsignedInteger(ulong ul) => new(ul);

    /// <summary>
    /// Explicitly casts a <see cref="long"/> to a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="l"></param>
    /// <exception cref="InvalidCastException">The value being cast was negative.</exception>
    public static explicit operator BigUnsignedInteger(long l)
        => l >= 0
            ? new(l)
            : throw NegativeCast();

#if NET7_0_OR_GREATER
    /// <summary>
    /// Implicitly casts a <see cref="UInt128"/> to a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="ul"></param>
    public static implicit operator BigUnsignedInteger(UInt128 ul) => new(ul);

    /// <summary>
    /// Explicitly casts a <see cref="Int128"/> to a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="l"></param>
    /// <exception cref="InvalidCastException">The value being cast was negative.</exception>
    public static explicit operator BigUnsignedInteger(Int128 l)
        => l >= 0
            ? new(l)
            : throw NegativeCast();
#endif

    /// <summary>
    /// Explicitly converts a <see cref="BigUnsignedInteger"/> to a <see cref="byte"/>.
    /// </summary>
    /// <param name="bi"></param>
    /// <exception cref="OverflowException"></exception>
    public static explicit operator byte(BigUnsignedInteger bi) => (byte)bi._value;

    /// <summary>
    /// Explicitly converts a <see cref="BigUnsignedInteger"/> to a <see cref="sbyte"/>.
    /// </summary>
    /// <param name="bi"></param>
    /// <exception cref="OverflowException"></exception>
    public static explicit operator sbyte(BigUnsignedInteger bi) => (sbyte)bi._value;

    /// <summary>
    /// Explicitly converts a <see cref="BigUnsignedInteger"/> to a <see cref="short"/>.
    /// </summary>
    /// <param name="bi"></param>
    /// <exception cref="OverflowException"></exception>
    public static explicit operator short(BigUnsignedInteger bi) => (short)bi._value;

    /// <summary>
    /// Explicitly converts a <see cref="BigUnsignedInteger"/> to a <see cref="ushort"/>.
    /// </summary>
    /// <param name="bi"></param>
    /// <exception cref="OverflowException"></exception>
    public static explicit operator ushort(BigUnsignedInteger bi) => (ushort)bi._value;

    /// <summary>
    /// Explicitly converts a <see cref="BigUnsignedInteger"/> to a <see cref="int"/>.
    /// </summary>
    /// <param name="bi"></param>
    /// <exception cref="OverflowException"></exception>
    public static explicit operator int(BigUnsignedInteger bi) => (int)bi._value;

    /// <summary>
    /// Explicitly converts a <see cref="BigUnsignedInteger"/> to a <see cref="uint"/>.
    /// </summary>
    /// <param name="bi"></param>
    /// <exception cref="OverflowException"></exception>
    public static explicit operator uint(BigUnsignedInteger bi) => (uint)bi._value;

    /// <summary>
    /// Explicitly converts a <see cref="BigUnsignedInteger"/> to a <see cref="long"/>.
    /// </summary>
    /// <param name="bi"></param>
    /// <exception cref="OverflowException"></exception>
    public static explicit operator long(BigUnsignedInteger bi) => (long)bi._value;

    /// <summary>
    /// Explicitly converts a <see cref="BigUnsignedInteger"/> to a <see cref="ulong"/>.
    /// </summary>
    /// <param name="bi"></param>
    /// <exception cref="OverflowException"></exception>
    public static explicit operator ulong(BigUnsignedInteger bi) => (ulong)bi._value;

#if NET7_0_OR_GREATER
    /// <summary>
    /// Explicitly converts a <see cref="BigUnsignedInteger"/> to a <see cref="Int128"/>.
    /// </summary>
    /// <param name="bi"></param>
    /// <exception cref="OverflowException"></exception>
    public static explicit operator Int128(BigUnsignedInteger bi) => (Int128)bi._value;

    /// <summary>
    /// Explicitly converts a <see cref="BigUnsignedInteger"/> to a <see cref="UInt128"/>.
    /// </summary>
    /// <param name="bi"></param>
    /// <exception cref="OverflowException"></exception>
    public static explicit operator UInt128(BigUnsignedInteger bi) => (UInt128)bi._value;
#endif

    /// <summary>
    /// Implicitly casts a <see cref="BigUnsignedInteger"/> to a <see cref="BigInteger"/>.
    /// </summary>
    /// <param name="bi"></param>
    public static implicit operator BigInteger(BigUnsignedInteger bi) => bi._value;

    /// <summary>
    /// Explicitly casts a <see cref="BigInteger"/> to a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="bi"></param>
    /// <exception cref="InvalidCastException">The value being cast is negative.</exception>
    public static explicit operator BigUnsignedInteger(BigInteger bi)
        => bi >= 0
            ? new(bi)
            : throw NegativeCast();

    /// <summary>
    /// Explicitly converts a <see cref="double"/> to a <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="d"></param>
    /// <exception cref="InvalidCastException">The value being cast is negative.</exception>
    public static explicit operator BigUnsignedInteger(double d)
    {
        var bi = (BigInteger)d;
        if (bi < 0) throw NegativeCast();
        else return new(bi);
    }

    /// <summary>
    /// Explicitly converts a <see cref="BigUnsignedInteger"/> to a <see cref="double"/>.
    /// </summary>
    /// <param name="i"></param>
    public static explicit operator double(BigUnsignedInteger i) => (double)i._value;

#if NET7_0_OR_GREATER
    static bool INumberBase<BigUnsignedInteger>.TryConvertFromChecked<TOther>(
            TOther value, out BigUnsignedInteger result)
        => TryConvertFromChecked(value, out BigInteger signedResult)
            ? HandleSignedTryParseResult(signedResult, out result)
            : Try.Failure(out result);

    static bool INumberBase<BigUnsignedInteger>.TryConvertFromSaturating<TOther>(
            TOther value, out BigUnsignedInteger result)
        => TryConvertFromSaturating(value, out BigInteger signedResult)
            ? HandleSignedTryParseResult(signedResult, out result)
            : Try.Failure(out result);

    static bool INumberBase<BigUnsignedInteger>.TryConvertFromTruncating<TOther>(
            TOther value, out BigUnsignedInteger result)
        => TryConvertFromTruncating(value, out BigInteger signedResult)
            ? HandleSignedTryParseResult(signedResult, out result)
            : Try.Failure(out result);

    static bool INumberBase<BigUnsignedInteger>.TryConvertToChecked<TOther>(
        BigUnsignedInteger value, [MaybeNullWhen(false)] out TOther result)
        => TryConvertToChecked(value._value, out result);

    static bool INumberBase<BigUnsignedInteger>.TryConvertToSaturating<TOther>(
            BigUnsignedInteger value, [MaybeNullWhen(false)] out TOther result)
        => TryConvertToSaturating(value._value, out result);

    static bool INumberBase<BigUnsignedInteger>.TryConvertToTruncating<TOther>(
            BigUnsignedInteger value, [MaybeNullWhen(false)] out TOther result)
        => TryConvertToTruncating(value._value, out result);
#endif
    #endregion

    #region Classification
    /// <summary>
    /// Determines if the given <see cref="BigUnsignedInteger"/> is an even integer.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsEvenInteger(BigUnsignedInteger value) => value.IsEven;

    /// <summary>
    /// Determines if the given <see cref="BigUnsignedInteger"/> is an odd integer.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsOddInteger(BigUnsignedInteger value) => value.IsOdd;

#if NET7_0_OR_GREATER
    static bool INumberBase<BigUnsignedInteger>.IsCanonical(BigUnsignedInteger value) => value._value.IsCanonical();
    static bool INumberBase<BigUnsignedInteger>.IsComplexNumber(BigUnsignedInteger value) => false;
    static bool INumberBase<BigUnsignedInteger>.IsFinite(BigUnsignedInteger value) => true;
    static bool INumberBase<BigUnsignedInteger>.IsImaginaryNumber(BigUnsignedInteger value) => false;
    static bool INumberBase<BigUnsignedInteger>.IsInfinity(BigUnsignedInteger value) => false;
    static bool INumberBase<BigUnsignedInteger>.IsInteger(BigUnsignedInteger value) => true;
    static bool INumberBase<BigUnsignedInteger>.IsNaN(BigUnsignedInteger value) => false;
    static bool INumberBase<BigUnsignedInteger>.IsNegative(BigUnsignedInteger value) => false;
    static bool INumberBase<BigUnsignedInteger>.IsNegativeInfinity(BigUnsignedInteger value) => false;
    static bool INumberBase<BigUnsignedInteger>.IsNormal(BigUnsignedInteger value) => true;
    static bool INumberBase<BigUnsignedInteger>.IsPositive(BigUnsignedInteger value) => true;
    static bool INumberBase<BigUnsignedInteger>.IsPositiveInfinity(BigUnsignedInteger value) => false;
    static bool INumberBase<BigUnsignedInteger>.IsRealNumber(BigUnsignedInteger value) => true;
    static bool INumberBase<BigUnsignedInteger>.IsSubnormal(BigUnsignedInteger value) => false;
    static bool INumberBase<BigUnsignedInteger>.IsZero(BigUnsignedInteger value) => value.IsZero;

    /// <summary>
    /// Gets the trailing zero count of this value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static BigUnsignedInteger TrailingZeroCount(BigUnsignedInteger value)
        => new(BigInteger.TrailingZeroCount(value._value));
#endif
    #endregion

    #region Reading / Writing
#if NET7_0_OR_GREATER
    static bool IBinaryInteger<BigUnsignedInteger>.TryReadBigEndian(
            ReadOnlySpan<byte> source, bool isUnsigned, out BigUnsignedInteger value)
        => StaticIndirection.TryReadBigEndian<BigInteger>(source, isUnsigned, out var signedValue)
            ? HandleSignedTryParseResult(in signedValue, out value)
            : Try.Failure(out value);

    static bool IBinaryInteger<BigUnsignedInteger>.TryReadLittleEndian(
            ReadOnlySpan<byte> source, bool isUnsigned, out BigUnsignedInteger value)
        => StaticIndirection.TryReadLittleEndian<BigInteger>(source, isUnsigned, out var signedValue)
            ? HandleSignedTryParseResult(in signedValue, out value)
            : Try.Failure(out value);

    bool IBinaryInteger<BigUnsignedInteger>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
        => (_value as IBinaryInteger<BigInteger>).TryWriteBigEndian(destination, out bytesWritten);

    bool IBinaryInteger<BigUnsignedInteger>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
        => (_value as IBinaryInteger<BigInteger>).TryWriteLittleEndian(destination, out bytesWritten);
#endif

    /// <summary>
    /// Parses a <see cref="BigUnsignedInteger"/> from a series of characters.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static BigUnsignedInteger Parse(ReadOnlySpan<char> s) => Parse(s, null);

    /// <summary>
    /// Parses a <see cref="BigUnsignedInteger"/> using the given format provider.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static BigUnsignedInteger Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => HandleSignedParseResult(
            BigInteger.Parse(
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                s,
#else
                s.SpanToString(),
#endif
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                NumberStyles.Integer,
#endif
                provider));

    /// <summary>
    /// Parses a <see cref="BigUnsignedInteger"/> using the given number style and format provider.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="style"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static BigUnsignedInteger Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        var result = BigInteger.Parse(
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            s,
#else
            s.SpanToString(),
#endif
            style, provider);

        return HandleSignedParseResult(result);
    }

    /// <summary>
    /// Parses a <see cref="BigUnsignedInteger"/> from a <see cref="string"/>.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static BigUnsignedInteger Parse(string s) => Parse(s, null);

    /// <summary>
    /// Parses a <see cref="BigUnsignedInteger"/> using the given format provider.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static BigUnsignedInteger Parse(string s, IFormatProvider? provider)
        => HandleSignedParseResult(BigInteger.Parse(s, provider));

    /// <summary>
    /// Parses a <see cref="BigUnsignedInteger"/> using the given number style and format provider.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="style"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static BigUnsignedInteger Parse(string s, NumberStyles style, IFormatProvider? provider)
        => HandleSignedParseResult(BigInteger.Parse(s, style, provider));

    /// <summary>
    /// Attempts to parse a <see cref="BigUnsignedInteger"/> from a span of characters.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="provider"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool TryParse(
            ReadOnlySpan<char> s, IFormatProvider? provider, out BigUnsignedInteger result)
        => TryParse(s, NumberStyles.Integer, provider, out result);

    /// <summary>
    /// Attempts to parse a <see cref="BigUnsignedInteger"/> from a span of characters.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="style"></param>
    /// <param name="provider"></param>
    /// <param name="result"></param>
    /// <returns>Whether or not the parse succeeded.</returns>
    public static bool TryParse(
            ReadOnlySpan<char> s,
            NumberStyles style, IFormatProvider? provider,
            out BigUnsignedInteger result)
        => BigInteger.TryParse(
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                s,
#else
                s.SpanToString(),
#endif
                style, provider, out var signedResult)
            ? HandleSignedTryParseResult(signedResult, out result)
            : Try.Failure(out result);

    /// <summary>
    /// Attempts to parse a <see cref="BigUnsignedInteger"/> from a <see cref="string"/>.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="provider"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out BigUnsignedInteger result)
        => TryParse(s, NumberStyles.Integer, provider, out result);

    /// <summary>
    /// Attempts to parse a <see cref="BigUnsignedInteger"/> from a <see cref="string"/>.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="style"></param>
    /// <param name="provider"></param>
    /// <param name="result"></param>
    /// <returns>Whether or not the parse succeeded.</returns>
    public static bool TryParse(
            [NotNullWhen(true)] string? s,
            NumberStyles style, IFormatProvider? provider,
            out BigUnsignedInteger result)
        => BigInteger.TryParse(s, style, provider, out var signedResult)
            ? HandleSignedTryParseResult(signedResult, out result)
            : Try.Failure(out result);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
    /// <summary>
    /// Formats this instance into a span of characters.
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="charsWritten"></param>
    /// <param name="format"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public bool TryFormat(
        Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
            => _value.TryFormat(destination, out charsWritten, format, provider);
#endif

    /// <summary>
    /// Gets a string representing this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => _value.ToString();

    /// <summary>
    /// Formats a string with this instance.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public string ToString(string? format) => ToString(format, null);

    /// <summary>
    /// Formats a string with this instance, using the given format provider.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="formatProvider"></param>
    /// <returns></returns>
    public string ToString(string? format, IFormatProvider? formatProvider) => _value.ToString(format, formatProvider);
    #endregion

    #region Helpers
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool HandleSignedTryParseResult(in BigInteger signedResult, out BigUnsignedInteger result)
        => signedResult < 0
            ? Try.Failure(out result)
            : Try.Success(out result, new(signedResult));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static BigUnsignedInteger HandleSignedParseResult(BigInteger signedResult)
        => signedResult < 0
            ? throw NegativeCast()
            : new(signedResult);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static InvalidCastException NegativeCast()
        => new($"Cannot cast negative value to {nameof(BigUnsignedInteger)}.");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static FormatException NegativeParse()
        => new($"Cannot parse a negative integer as an instance of {nameof(BigUnsignedInteger)}.");

    #region Static Indirection
#if NET7_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryConvertToChecked<T, TOther>(T value, [MaybeNullWhen(false)] out TOther result)
    where T : INumberBase<T>
    where TOther : INumberBase<TOther>
        => T.TryConvertToChecked(value, out result);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryConvertToSaturating<T, TOther>(T value, [MaybeNullWhen(false)] out TOther result)
    where T : INumberBase<T>
    where TOther : INumberBase<TOther>
        => T.TryConvertToSaturating(value, out result);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryConvertToTruncating<T, TOther>(T value, [MaybeNullWhen(false)] out TOther result)
    where T : INumberBase<T>
    where TOther : INumberBase<TOther>
        => T.TryConvertToTruncating(value, out result);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryConvertFromChecked<T, TOther>(TOther value, [MaybeNullWhen(false)] out T result)
    where T : INumberBase<T>
    where TOther : INumberBase<TOther>
        => T.TryConvertFromChecked(value, out result);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryConvertFromSaturating<T, TOther>(TOther value, [MaybeNullWhen(false)] out T result)
    where T : INumberBase<T>
    where TOther : INumberBase<TOther>
        => T.TryConvertFromSaturating(value, out result);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryConvertFromTruncating<T, TOther>(TOther value, [MaybeNullWhen(false)] out T result)
    where T : INumberBase<T>
    where TOther : INumberBase<TOther>
        => T.TryConvertFromTruncating(value, out result);
#endif
    #endregion
    #endregion
}

#if NET7_0_OR_GREATER
file static class StaticIndirection
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCanonical<T>(this T i) where T : INumberBase<T> => T.IsCanonical(i);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryReadBigEndian<T>(ReadOnlySpan<byte> source, bool isUnsigned, out T value)
    where T : IBinaryInteger<T>
        => T.TryReadBigEndian(source, isUnsigned, out value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryReadLittleEndian<T>(ReadOnlySpan<byte> source, bool isUnsigned, out T value)
    where T : IBinaryInteger<T>
        => T.TryReadLittleEndian(source, isUnsigned, out value);

}
#endif

#if !(NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER)
file static class FileExtensions
{
    /// <summary>
    /// Converts the current <see cref="ReadOnlySpan{T}"/> to a <see cref="string"/>.
    /// </summary>
    /// <remarks>
    /// This is important for versions where the <see cref="BigInteger"/> class does not contain a
    /// <see cref="BigInteger.TryParse"/> overload that takes in a <see cref="ReadOnlySpan{T}"/>.
    /// </remarks>
    /// <param name="span"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SpanToString(in this ReadOnlySpan<char> span)
    {
        unsafe
        {
            fixed (char* chs = &span.GetPinnableReference())
            {
                return new(chs);
            }
        }
    }
}
#endif

