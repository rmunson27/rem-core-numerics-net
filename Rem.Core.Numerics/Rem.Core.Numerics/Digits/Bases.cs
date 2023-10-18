using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics.Digits;

/// <summary>
/// Helper functionality relating to integral bases.
/// </summary>
public static class Bases
{
    #region Constants
    /// <summary>
    /// The maximum base for which all digits can be stored in a <see cref="ulong"/>.
    /// </summary>
    /// <remarks>
    /// This value is equal to 2^64 = 18,446,744,073,709,551,616.
    /// <para/>
    /// Using any base above this value will require all digits to be stored in a <see cref="BigInteger"/>.
    /// </remarks>
    public static readonly BigUnsignedInteger MaxULongDigitBase = ulong.MaxValue + BigUnsignedInteger.One;

    /// <summary>
    /// The maximum base for which all digits can be stored in a <see cref="uint"/>.
    /// </summary>
    /// <remarks>
    /// This value is equal to 2^32 = 4,294,967,296.
    /// </remarks>
    public const ulong MaxUIntDigitBase = uint.MaxValue + 1uL;

    /// <summary>
    /// The maximum base for which all digits can be stored in a <see cref="ushort"/>.
    /// </summary>
    /// <remarks>
    /// This value is equal to 2^16 = 65,536.
    /// </remarks>
    public const uint MaxUShortDigitBase = ushort.MaxValue + 1u;

    /// <summary>
    /// The maximum base for which all digits can be stored in a <see cref="byte"/>.
    /// </summary>
    public const ushort MaxByteDigitBase = 256;
    #endregion

    #region Methods
    /// <inheritdoc cref="ShortestDigitType(ushort)"/>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Base"/> was negative.</exception>
    public static DigitType ShortestDigitType([GreaterThanOrEqualToInteger(2)] BigInteger Base)
        => Throw.IfArgLessThan(2, Base, nameof(Base)).CompareTo(MaxULongDigitBase) switch
        {
            > 0 => DigitType.BigUnsignedInteger,
            0 => DigitType.ULong,
            < 0 => ShortestDigitTypeInternal((ulong)Base),
        };

    /// <inheritdoc cref="ShortestDigitType(ushort)"/>
    public static DigitType ShortestDigitType([GreaterThanOrEqualToInteger(2)] ulong Base)
        => ShortestDigitTypeInternal(Base);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static DigitType ShortestDigitTypeInternal([GreaterThanOrEqualToInteger(2)] ulong Base) => Base switch
    {
        > MaxUIntDigitBase => DigitType.ULong,
        > MaxUShortDigitBase => DigitType.UInt,
        > MaxByteDigitBase => DigitType.UShort,
        _ => DigitType.Byte,
    };

    /// <inheritdoc cref="ShortestDigitType(ushort)"/>
    public static DigitType ShortestDigitType([GreaterThanOrEqualToInteger(2)] uint Base) => Base switch
    {
        > MaxUShortDigitBase => DigitType.UInt,
        > MaxByteDigitBase => DigitType.UShort,
        _ => DigitType.Byte,
    };

    /// <summary>
    /// Gets a value representing the shortest possible integral representation of digits in the given base.
    /// </summary>
    /// <param name="Base"></param>
    /// <returns></returns>
    public static DigitType ShortestDigitType([GreaterThanOrEqualToInteger(2)] ushort Base)
        => Base > MaxByteDigitBase ? DigitType.UShort : DigitType.Byte;
    #endregion
}
