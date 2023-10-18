using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Rem.Core.Attributes;

namespace Rem.Core.Numerics;

public static partial class Maths
{
    /// <summary>
    /// Computes the integer square root of the given value.
    /// </summary>
    /// <param name="n">The value to compute the integer square root of.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="n"/> was negative.
    /// </exception>
    public static BigInteger IntegerSqrt(BigInteger n)
    {
        if (n.IsZero) return 0;
        if (n < 0) throw SqrtOfNegative(nameof(n), n);
        if (n < 4) return 1;
        if (n < 9) return 2;
        if (n < 16) return 3;

        var mostSigBitShift = n.SqrtBitLengthOfNonNegative() - 1;
        if (mostSigBitShift % 2 != 0) mostSigBitShift--;

        BigInteger temp;
        BigInteger root = BigInteger.Zero;
        BigInteger bit = BigInteger.One << mostSigBitShift; // Start with the most significant bit

        for (; bit > 0; bit >>= 2)
        {
            temp = root + bit;
            root >>= 1;
            if (n >= temp)
            {
                n -= temp;
                root += bit;
            }
        }

        return root;
    }

    /// <summary>
    /// Computes the integer square root of the given value.
    /// </summary>
    /// <param name="n">The value to compute the integer square root of.</param>
    /// <returns></returns>
    public static BigUnsignedInteger IntegerSqrt(BigUnsignedInteger n)
    {
        if (n.IsZero) return 0;
        if (n < 0) throw SqrtOfNegative(nameof(n), n);
        if (n < 4) return 1;
        if (n < 9) return 2;
        if (n < 16) return 3;

        var mostSigBitShift = n.SqrtBitLengthOfNonNegative() - 1;
        if (mostSigBitShift % 2 != 0) mostSigBitShift--;

        BigUnsignedInteger temp;
        BigUnsignedInteger root = BigUnsignedInteger.Zero;
        BigUnsignedInteger bit = BigUnsignedInteger.One << mostSigBitShift; // Start with the most significant bit

        for (; bit > 0; bit >>= 2)
        {
            temp = root + bit;
            root >>= 1;
            if (n >= temp)
            {
                n -= temp;
                root += bit;
            }
        }

        return root;
    }
}

file static class FileExtensions
{
    private static readonly (BigInteger Value, int BitLength)[] StoredShiftValues;
    private static readonly (BigUnsignedInteger Value, int BitLength)[] StoredUnsignedShiftValues;

    static FileExtensions()
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static BigInteger MakeMaxOfBits(int bits) => (BigInteger.One << bits) - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static BigUnsignedInteger MakeUnsignedMaxOfBits(int bits) => (BigUnsignedInteger.One << bits) - 1;

        StoredShiftValues = new (BigInteger, int)[]
        {
            (MakeMaxOfBits(512), 512),
            (MakeMaxOfBits(256), 256),
            (MakeMaxOfBits(128), 128),
            (long.MaxValue, 63),
        };

        StoredUnsignedShiftValues = new (BigUnsignedInteger, int)[]
        {
            (MakeUnsignedMaxOfBits(512), 512),
            (MakeUnsignedMaxOfBits(256), 256),
            (MakeUnsignedMaxOfBits(128), 128),
            (ulong.MaxValue, 63),
        };
    }

#if NET5_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static int SqrtBitLengthOfNonNegative([NonNegative] this BigInteger bi)
    {
        var totalBitLength = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ShiftByBits(int bits)
        {
            bi >>= bits;
            totalBitLength += bits;
        }

        while (bi > StoredShiftValues[0].Value) ShiftByBits(StoredShiftValues[0].BitLength);

        for (int i = 1; i < StoredShiftValues.Length; i++)
        {
            if (bi > StoredShiftValues[i].Value) ShiftByBits(StoredShiftValues[i].BitLength);
        }

        while (bi > 0) ShiftByBits(1);

        return totalBitLength;
    }

#if NET5_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static int SqrtBitLengthOfNonNegative([NonNegative] this BigUnsignedInteger bi)
    {
        var totalBitLength = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ShiftByBits(int bits)
        {
            bi >>= bits;
            totalBitLength += bits;
        }

        while (bi > StoredShiftValues[0].Value) ShiftByBits(StoredShiftValues[0].BitLength);

        for (int i = 1; i < StoredShiftValues.Length; i++)
        {
            if (bi > StoredShiftValues[i].Value) ShiftByBits(StoredShiftValues[i].BitLength);
        }

        while (bi > 0) ShiftByBits(1);

        return totalBitLength;
    }
}
