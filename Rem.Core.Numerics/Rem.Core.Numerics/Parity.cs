using System;
using System.Numerics;

namespace Rem.Core.Numerics;

/// <summary>
/// Represents a parity, either even or odd.
/// </summary>
public readonly record struct Parity
{
    /// <inheritdoc cref="Labels.Even"/>
    public static readonly Parity Even = new(Labels.Even);

    /// <inheritdoc cref="Labels.Odd"/>
    public static readonly Parity Odd = new(Labels.Odd);

    /// <summary>
    /// Gets the <see langword="enum"/> value used to uniquely label this instance.
    /// </summary>
    public Labels Label { get; }

    private Parity(Labels Label) { this.Label = Label; }

    #region IsParityOf
    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="byte"/>.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public bool IsParityOf(byte i) => (Labels.Odd & (Labels)i) == Label;

    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="sbyte"/>.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public bool IsParityOf(sbyte i) => (Labels.Odd & unchecked((Labels)i)) == Label;

    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="short"/>.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public bool IsParityOf(short i) => (Labels.Odd & unchecked((Labels)i)) == Label;

    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="ushort"/>.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public bool IsParityOf(ushort i) => (Labels.Odd & unchecked((Labels)i)) == Label;

    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="int"/>.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public bool IsParityOf(int i) => (Labels.Odd & unchecked((Labels)i)) == Label;

    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="uint"/>.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public bool IsParityOf(uint i) => (Labels.Odd & unchecked((Labels)i)) == Label;

    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="long"/>.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public bool IsParityOf(long i) => (Labels.Odd & unchecked((Labels)i)) == Label;

    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="ulong"/>.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public bool IsParityOf(ulong i) => (Labels.Odd & unchecked((Labels)i)) == Label;

#if NET7_0_OR_GREATER
    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="Int128"/>.
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool IsParityOf(Int128 b) => (Labels.Odd & unchecked((Labels)(byte)b)) == Label;

    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="UInt128"/>.
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool IsParityOf(UInt128 b) => (Labels.Odd & unchecked((Labels)(byte)b)) == Label;
#endif

    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="long"/>.
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool IsParityOf(BigInteger b) => Label switch
    {
        Labels.Even => b.IsEven,
        _ => !b.IsEven,
    };

    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="BigUnsignedInteger"/>.
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool IsParityOf(BigUnsignedInteger b) => Label switch
    {
        Labels.Even => b.IsEven,
        _ => b.IsOdd,
    };

    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="float"/>.
    /// </summary>
    /// <remarks>
    /// This function will return <see langword="false"/> if <paramref name="f"/> is not a whole number or is infinite.
    /// </remarks>
    /// <param name="f"></param>
    /// <returns></returns>
    public bool IsParityOf(float f)
        => f < 0
            ? (-f) % 2 == (byte)Label
            : f % 2 == (byte)Label;

    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="double"/>.
    /// </summary>
    /// <remarks>
    /// This function will return <see langword="false"/> if <paramref name="f"/> is not a whole number or is infinite.
    /// </remarks>
    /// <param name="f"></param>
    /// <returns></returns>
    public bool IsParityOf(double f)
        => f < 0
            ? (-f) % 2 == (byte)Label
            : f % 2 == (byte)Label;

#if NET6_0_OR_GREATER
    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="Half"/>.
    /// </summary>
    /// <remarks>
    /// This function will return <see langword="false"/> if <paramref name="f"/> is not a whole number or is infinite.
    /// </remarks>
    /// <param name="f"></param>
    /// <returns></returns>
    public bool IsParityOf(Half f)
        => f < (Half)0
            ? (-(float)f) % 2 == (byte)Label
            : (float)f % 2 == (byte)Label;
#endif

    /// <summary>
    /// Determines if this parity is the parity of the given <see cref="decimal"/>.
    /// </summary>
    /// <remarks>
    /// This function will return <see langword="false"/> if <paramref name="d"/> is not a whole number.
    /// </remarks>
    /// <param name="d"></param>
    /// <returns></returns>
    public bool IsParityOf(decimal d)
        => d < 0
            ? (-d) % 2 == (byte)Label
            : d % 2 == (byte)Label;
    #endregion

    /// <summary>
    /// Determines if this <see cref="Parity"/> is equal to another.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Parity other) => Label == other.Label;

    /// <summary>
    /// Gets a hash code for this instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => (int)Label;

    /// <summary>
    /// Gets a string representing this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Label switch
    {
        Labels.Even => nameof(Even),
        _ => nameof(Odd),
    };

    /// <summary>
    /// The <see langword="enum"/> used to label instances of type <see cref="Parity"/>.
    /// </summary>
    public enum Labels : byte
    {
        /// <summary>
        /// Represents even numbers.
        /// </summary>
        /// <remarks>
        /// This is the default.
        /// </remarks>
        Even = 0,

        /// <summary>
        /// Represents odd numbers.
        /// </summary>
        Odd = 1,
    }
}
