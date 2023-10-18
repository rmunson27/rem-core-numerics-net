using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics.Digits;

/// <summary>
/// A representation of an unsigned integral value in a given base.
/// </summary>
public sealed record class UnsignedIntegralDigitRep
{
    /// <summary>
    /// Gets the base of the representation.
    /// </summary>
    [GreaterThanOrEqualToInteger(2)] public BigInteger Base { get; }

    /// <summary>
    /// Gets the digits of the representation, with no leading zeroes.
    /// </summary>
    public DigitList Digits { get; }

    /// <summary>
    /// Constructs a new instance of the <see cref="UnsignedIntegralDigitRep"/> class with the digits of
    /// the representation.
    /// </summary>
    internal UnsignedIntegralDigitRep(BigInteger Base, DigitList Digits)
    {
        this.Base = Base;
        this.Digits = Digits;
    }

    /// <summary>
    /// Creates a new <see cref="UnsignedIntegralDigitRep"/> with the base and digits passed in.
    /// </summary>
    /// <remarks>
    /// Leading zeroes will be stripped off of the digit list before creation.
    /// </remarks>
    /// <param name="Base"></param>
    /// <param name="Digits"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="Digits"/> was <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Base"/> was less than 2.</exception>
    public static UnsignedIntegralDigitRep Create([GreaterThanOrEqualToInteger(2)] BigInteger Base, DigitList Digits)
    {
        Throw.IfArgLessThan(2, Base, nameof(Base));
        Digits = Throw.IfArgNull(Digits, nameof(Digits)).WithoutLeadingZeroes();
        return new(Base, Digits);
    }

    /// <summary>
    /// Deconstructs the current instance.
    /// </summary>
    /// <param name="Base"></param>
    /// <param name="Digits"></param>
    public void Deconstruct([GreaterThanOrEqualToInteger(2)] out BigInteger Base, out DigitList Digits)
    {
        Base = this.Base;
        Digits = this.Digits;
    }

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => ToString(digitFormat: DigitList.DefaultFormat);

    /// <summary>
    /// Gets a string that represents the current instance with the digits and base formatted using the provided
    /// number format.
    /// </summary>
    /// <param name="separator">
    /// A separator to use to separate the digits when formatting.
    /// -or-
    /// a <see langword="null"/> reference to not include separators.
    /// </param>
    /// <param name="numberFormat">
    /// The format to use to format the digits and base.
    /// -or-
    /// A <see langword="null"/> reference to use the default format for the type of the digits and the base.
    /// </param>
    /// <param name="numberFormatProvider">
    /// The provider to use to format the value.
    /// -or-
    /// a <see langword="null"/> reference to obtain the numeric format information from the current locale setting
    /// of the operating system.
    /// </param>
    /// <returns></returns>
    public string ToString(
        string? separator = DigitList.DefaultSeparator, string? numberFormat = DigitList.DefaultFormat,
        IFormatProvider? numberFormatProvider = null)
        => ToString(
            separator: separator, numberFormat, numberFormat, numberFormatProvider);

    /// <summary>
    /// Gets a string that represents the current instance with the digits and base formatted using the provided
    /// number formats.
    /// </summary>
    /// <param name="separator">
    /// A separator to use to separate the digits when formatting.
    /// -or-
    /// a <see langword="null"/> reference to not include separators.
    /// </param>
    /// <param name="digitFormat">
    /// The format to use to format the digits.
    /// -or-
    /// A <see langword="null"/> reference to use the default format for the type of the digits.
    /// </param>
    /// <param name="baseFormat">
    /// The format to use to format the base.
    /// -or-
    /// A <see langword="null"/> reference to use the default format for the base.
    /// </param>
    /// <param name="numberFormatProvider">
    /// The provider to use to format the value.
    /// -or-
    /// a <see langword="null"/> reference to obtain the numeric format information from the current locale setting
    /// of the operating system.
    /// </param>
    /// <returns></returns>
    public string ToString(
        string? separator = DigitList.DefaultSeparator,
        string? digitFormat = DigitList.DefaultFormat, string? baseFormat = "D",
        IFormatProvider? numberFormatProvider = null)
        => Digits.Count switch
        {
            0 => "0",
            1 => Digits[0].ToString(digitFormat, numberFormatProvider),
            _ => Digits.ToString(separator: separator, digitFormat: digitFormat, numberFormatProvider)
                    + $" (Base {Base.ToString(baseFormat, numberFormatProvider)})",
        };
    #endregion

}
