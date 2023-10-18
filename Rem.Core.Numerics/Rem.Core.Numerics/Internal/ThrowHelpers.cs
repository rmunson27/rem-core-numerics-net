using System;
using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;
using Rem.Core.ComponentModel;

namespace Rem.Core.Numerics;

internal static class Throw
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T IfArgNull<T>(T? param, string paramName)
        => param is null ? throw new ArgumentNullException(paramName) : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly T IfStructArgDefault<T>(
        in T param, string paramName)
    where T : struct, IDefaultableStruct
    {
        if (param.IsDefault) throw new StructArgumentDefaultException(paramName);
        else return ref param;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly T IfCurrentInstanceDefault<T>(in T param)
    where T : struct, IDefaultableStruct
    {
        if (param.IsDefault) throw new DefaultInstanceException();
        else return ref param;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ImmutableArray<T> IfStructArgDefault<T>(
            ImmutableArray<T> param, string paramName)
        => param.IsDefault
            ? throw new StructArgumentDefaultException(paramName)
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte IfArgLessThan(sbyte n, sbyte param, string paramName)
        => param < n
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be less than {n}.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte IfArgLessThan(byte n, byte param, string paramName)
        => param < n
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be less than {n}.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort IfArgLessThan(ushort n, ushort param, string paramName)
        => param < n
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be less than {n}.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short IfArgLessThan(short n, short param, string paramName)
        => param < n
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be less than {n}.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint IfArgLessThan(uint n, uint param, string paramName)
        => param < n
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be less than {n}.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IfArgLessThan(int n, int param, string paramName)
        => param < n
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be less than {n}.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong IfArgLessThan(ulong n, ulong param, string paramName)
        => param < n
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be less than {n}.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long IfArgLessThan(long n, long param, string paramName)
        => param < n
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be less than {n}.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BigInteger IfArgLessThan(BigInteger n, BigInteger param, string paramName)
        => param < n
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be less than {n}.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BigUnsignedInteger IfArgLessThan(BigUnsignedInteger n, BigUnsignedInteger param, string paramName)
        => param < n
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be less than {n}.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte IfArgNegative(sbyte param, string paramName)
        => param < 0
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be negative.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte IfArgNegative(byte param, string paramName)
        => param < 0
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be negative.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort IfArgNegative(ushort param, string paramName)
        => param < 0
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be negative.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short IfArgNegative(short param, string paramName)
        => param < 0
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be negative.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint IfArgNegative(uint param, string paramName)
        => param < 0
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be negative.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IfArgNegative(int param, string paramName)
        => param < 0
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be negative.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong IfArgNegative(ulong param, string paramName)
        => param < 0
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be negative.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long IfArgNegative(long param, string paramName)
        => param < 0
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be negative.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BigInteger IfArgNegative(BigInteger param, string paramName)
        => param < 0
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be negative.")
            : param;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BigUnsignedInteger IfArgNegative(BigUnsignedInteger param, string paramName)
        => param < 0
            ? throw new ArgumentOutOfRangeException(paramName, param, $"Value cannot be negative.")
            : param;
}
