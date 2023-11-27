using System.Collections;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Rem.Core.Numerics.Digits;

namespace Rem.Core.Numerics;

internal sealed record class SmallBaseRatioDigitRep
{
    public byte Sign { get; }

    public byte Base { get; }

    /// <summary>
    /// Gets the length of the decimals
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Stores the number of bits per digit in this representation.
    /// </summary>
    private byte BitsPerDigit { get; }

    /// <summary>
    /// Gets the raw segments of the representation.
    /// </summary>
    public ImmutableArray<ulong> RawSegments { get; }

    public SmallBaseRatioDigitRep(byte Base, ImmutableArray<ulong> RawSegments)
    {
        BitsPerDigit = CalculateBitsPerDigit(Base);

    }

    /// <summary>
    /// Calculates the number of bits required to store digits in the given base.
    /// </summary>
    /// <param name="Base"></param>
    /// <returns></returns>
    private static byte CalculateBitsPerDigit(byte Base) => Base switch
    {
        < 2 => throw Bases.Invalid(nameof(Base), Base),
        2 => 1,
        <= 4 => 2,
        <= 8 => 3,
        <= 16 => 4,
        <= 32 => 5,
        <= 64 => 6,
        <= 128 => 7,
        _ => 8,
    };
}
