using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using Rem.Core.ComponentModel;

namespace Rem.Core.Numerics;

/// <summary>
/// Provides several useful mathematical operations.
/// </summary>
public static partial class Maths
{
    #region Integer Representable
    /// <summary>
    /// Determines if the given <see cref="object"/> is a numerical value with a built-in implicit conversion to
    /// a value of type <see cref="BigInteger"/>, setting the value in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="o">The <see cref="object"/> to attempt to implicitly cast to type <see cref="BigInteger"/>.</param>
    /// <param name="value">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="BigInteger"/> value when this method
    /// returns <see langword="true"/>.
    /// </param>
    /// <inheritdoc cref="IsInt64Representable"/>
    public static bool IsBigIntegerRepresentable([NotNullWhen(true)] object? o, out BigInteger value)
        => IsIntegralRepresentable.BigInteger(o, out value);

    #region 128-Bit
#if NET7_0_OR_GREATER
    /// <summary>
    /// Determines if the given <see cref="object"/> is a numerical value with a built-in implicit conversion to
    /// a value of type <see cref="UInt128"/>, setting the value in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="o">The <see cref="object"/> to attempt to implicitly cast to type <see cref="UInt128"/>.</param>
    /// <param name="value">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="UInt128"/> value when this method
    /// returns <see langword="true"/>.
    /// </param>
    /// <inheritdoc cref="IsInt64Representable"/>
    public static bool IsUInt128Representable([NotNullWhen(true)] object? o, out UInt128 value)
        => IsIntegralRepresentable.UInt128(o, out value);

    /// <summary>
    /// Determines if the given <see cref="object"/> is a numerical value with a built-in implicit conversion to
    /// a value of type <see cref="Int128"/>, setting the value in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="o">The <see cref="object"/> to attempt to implicitly cast to type <see cref="Int128"/>.</param>
    /// <param name="value">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="Int128"/> value when this method
    /// returns <see langword="true"/>.
    /// </param>
    /// <inheritdoc cref="IsInt64Representable"/>
    public static bool IsInt128Representable([NotNullWhen(true)] object? o, out Int128 value)
        => IsIntegralRepresentable.Int128(o, out value);
#endif
    #endregion

    /// <summary>
    /// Determines if the given <see cref="object"/> is a numerical value with a built-in implicit conversion to
    /// a value of type <see cref="ulong"/>, setting the value in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="o">The <see cref="object"/> to attempt to implicitly cast to type <see cref="ulong"/>.</param>
    /// <param name="value">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="ulong"/> value when this method
    /// returns <see langword="true"/>.
    /// </param>
    /// <inheritdoc cref="IsInt64Representable"/>
    public static bool IsUInt64Representable([NotNullWhen(true)] object? o, out ulong value)
        => IsIntegralRepresentable.UInt64(o, out value);

    /// <summary>
    /// Determines if the given <see cref="object"/> is a numerical value with a built-in implicit conversion to
    /// a value of type <see cref="long"/>, setting the value in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="o">The <see cref="object"/> to attempt to implicitly cast to type <see cref="long"/>.</param>
    /// <param name="value">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="long"/> value when this method
    /// returns <see langword="true"/>.
    /// </param>
    /// <returns>
    /// Whether or not <paramref name="o"/> is a numerical value with a built-in implicit conversion to the value
    /// <paramref name="value"/> is set to when the method returns.
    /// </returns>
    public static bool IsInt64Representable([NotNullWhen(true)] object? o, out long value)
        => IsIntegralRepresentable.Int64(o, out value);

    /// <summary>
    /// Determines if the given <see cref="object"/> is a numerical value with a built-in implicit conversion to
    /// a value of type <see cref="uint"/>, setting the value in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="o">The <see cref="object"/> to attempt to implicitly cast to type <see cref="uint"/>.</param>
    /// <param name="value">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="uint"/> value when this method
    /// returns <see langword="true"/>.
    /// </param>
    /// <inheritdoc cref="IsInt64Representable"/>
    public static bool IsUInt32Representable([NotNullWhen(true)] object? o, out uint value)
        => IsIntegralRepresentable.UInt32(o, out value);

    /// <summary>
    /// Determines if the given <see cref="object"/> is a numerical value with a built-in implicit conversion to
    /// a value of type <see cref="int"/>, setting the value in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="o">The <see cref="object"/> to attempt to implicitly cast to type <see cref="int"/>.</param>
    /// <param name="value">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="int"/> value when this method
    /// returns <see langword="true"/>.
    /// </param>
    /// <inheritdoc cref="IsInt64Representable"/>
    public static bool IsInt32Representable([NotNullWhen(true)] object? o, out int value)
        => IsIntegralRepresentable.Int32(o, out value);

    /// <summary>
    /// Determines if the given <see cref="object"/> is a numerical value with a built-in implicit conversion to
    /// a value of type <see cref="ushort"/>, setting the value in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="o">The <see cref="object"/> to attempt to implicitly cast to type <see cref="ushort"/>.</param>
    /// <param name="value">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="ushort"/> value when this method
    /// returns <see langword="true"/>.
    /// </param>
    /// <inheritdoc cref="IsInt64Representable"/>
    public static bool IsUInt16Representable([NotNullWhen(true)] object? o, out ushort value)
        => IsIntegralRepresentable.UInt16(o, out value);

    /// <summary>
    /// Determines if the given <see cref="object"/> is a numerical value with a built-in implicit conversion to
    /// a value of type <see cref="short"/>, setting the value in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="o">The <see cref="object"/> to attempt to implicitly cast to type <see cref="short"/>.</param>
    /// <param name="value">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="short"/> value when this method
    /// returns <see langword="true"/>.
    /// </param>
    /// <inheritdoc cref="IsInt64Representable"/>
    public static bool IsInt16Representable([NotNullWhen(true)] object? o, out short value)
        => IsIntegralRepresentable.Int16(o, out value);

    private static class IsIntegralRepresentable
    {
        #region Signed
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BigInteger(object? o, out BigInteger value) => o switch
        {
            BigInteger i => Try.Success(out value, i),

#if NET7_0_OR_GREATER
            UInt128 i => Try.Success(out value, i),
            _ => Int128(o, out var i) ? Try.Success(out value, i) : Try.Failure(out value),
#else
            ulong i => Try.Success(out value, i),
            _ => Int64(o, out var i) ? Try.Success(out value, i) : Try.Failure(out value),
#endif
        };

#if NET7_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Int128(object? o, out Int128 value) => o switch
        {
            Int128 i => Try.Success(out value, i),
            ulong i => Try.Success(out value, i),
            _ => Int64(o, out var i) ? Try.Success(out value, i) : Try.Failure(out value),
        };
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Int64(object? o, out long value) => o switch
        {
            long i => Try.Success(out value, i),
            uint i => Try.Success(out value, i),
            _ => Int32(o, out var i) ? Try.Success(out value, i) : Try.Failure(out value),
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Int32(object? o, out int value) => o switch
        {
            int i => Try.Success(out value, i),
            ushort i => Try.Success(out value, i),
            _ => Int16(o, out var i) ? Try.Success(out value, i) : Try.Failure(out value),
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Int16(object? o, out short value) => o switch
        {
            short i => Try.Success(out value, i),
            sbyte i => Try.Success(out value, i),
            byte i => Try.Success(out value, i),
            _ => Try.Failure(out value),
        };
        #endregion

        #region Unsigned
#if NET7_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool UInt128(object? o, out UInt128 value) => o switch
        {
            UInt128 i => Try.Success(out value, i),
            _ => UInt64(o, out var i) ? Try.Success(out value, i) : Try.Failure(out value),
        };
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool UInt64(object? o, out ulong value) => o switch
        {
            ulong i => Try.Success(out value, i),
            _ => UInt32(o, out var i) ? Try.Success(out value, i) : Try.Failure(out value),
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool UInt32(object? o, out uint value) => o switch
        {
            uint i => Try.Success(out value, i),
            _ => UInt16(o, out var i) ? Try.Success(out value, i) : Try.Failure(out value),
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool UInt16(object? o, out ushort value) => o switch
        {
            ushort i => Try.Success(out value, i),
            byte i => Try.Success(out value, i),
            _ => Try.Failure(out value),
        };
        #endregion
    }
    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ArgumentOutOfRangeException SqrtOfNegative(string paramName, object value)
        => new(paramName, value, "Cannot take the square root of a negative.");
}
