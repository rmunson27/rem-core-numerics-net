using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics.Digits;

/// <summary>
/// Represents the storage type of a digit.
/// </summary>
public readonly record struct DigitType
{
    #region Constants
    /// <summary>
    /// A <see cref="DigitType"/> representing 8-bit <see cref="byte"/> digits.
    /// </summary>
    public static readonly DigitType Byte = new(Values.Byte);

    /// <summary>
    /// A <see cref="DigitType"/> representing 16-bit <see cref="ushort"/> digits.
    /// </summary>
    public static readonly DigitType UShort = new(Values.UShort);

    /// <summary>
    /// A <see cref="DigitType"/> representing 32-bit <see cref="uint"/> digits.
    /// </summary>
    public static readonly DigitType UInt = new(Values.UInt);

    /// <summary>
    /// A <see cref="DigitType"/> representing 64-bit <see cref="ulong"/> digits.
    /// </summary>
    public static readonly DigitType ULong = new(Values.ULong);

    /// <summary>
    /// A <see cref="DigitType"/> representing arbitrary precision (non-negative)
    /// <see cref="Numerics.BigUnsignedInteger"/> digits.
    /// </summary>
    public static readonly DigitType BigUnsignedInteger = new(Values.BigUnsignedInteger);

    private static readonly ImmutableDictionary<Type, DigitType> RepresentedTypeDictionary
        = ImmutableDictionary.CreateRange(new KeyValuePair<Type, DigitType>[]
        {
            new(typeof(byte), Byte),
            new(typeof(ushort), UShort),
            new(typeof(uint), UInt),
            new(typeof(ulong), ULong),
            new(typeof(BigUnsignedInteger), BigUnsignedInteger),
        });
    #endregion

    #region Properties
    /// <summary>
    /// Gets the digit type this instance represents.
    /// </summary>
    public Type RepresentedType => Value switch
    {
        Values.Byte => typeof(byte),
        Values.UShort => typeof(ushort),
        Values.UInt => typeof(uint),
        Values.ULong => typeof(ulong),
        _ => typeof(BigUnsignedInteger),
    };

    /// <summary>
    /// Gets an enumeration value uniquely identifying this instance.
    /// </summary>
    public Values Value { get; }
    #endregion

    #region Constructor
    private DigitType([NameableEnum] Values Value) { this.Value = Value; }
    #endregion

    #region Methods
    /// <summary>
    /// Gets a <see cref="DigitType"/> representing the type passed in.
    /// </summary>
    /// <param name="RepresentedType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="RepresentedType"/> was not one of <see cref="byte"/>, <see cref="ushort"/>, <see cref="uint"/>,
    /// <see cref="ulong"/> or <see cref="Numerics.BigUnsignedInteger"/>.
    /// </exception>
    public static DigitType FromRepresentedType(Type RepresentedType)
        => RepresentedTypeDictionary.TryGetValue(RepresentedType, out var result)
            ? result
            : throw new ArgumentException(
                $"Type {RepresentedType} does not represent a digit type - parameter '{nameof(RepresentedType)}' must"
                    + $" be one of {typeof(byte)}, {typeof(ushort)}, {typeof(uint)}, {typeof(ulong)}"
                    + $" or {typeof(BigUnsignedInteger)}.");

    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Determines if this instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(DigitType other) => Value == other.Value;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Value.GetHashCode();
    #endregion

    #region Types
    /// <summary>
    /// Represents all possible values of this struct as an enumeration.
    /// </summary>
    public enum Values : byte
    {
        /// <summary>
        /// Represents 8-bit <see cref="byte"/> digitss.
        /// </summary>
        Byte = 1,

        /// <summary>
        /// Represents 16-bit <see cref="ushort"/> digits.
        /// </summary>
        UShort = 2,

        /// <summary>
        /// Represents 32-bit <see cref="uint"/> digits.
        /// </summary>
        UInt = 4,

        /// <summary>
        /// Represents 64-bit <see cref="ulong"/> digits.
        /// </summary>
        ULong = 8,

        /// <summary>
        /// Represents arbitrary precision <see cref="Numerics.BigUnsignedInteger"/> digits.
        /// </summary>
        BigUnsignedInteger = 255,
    }
    #endregion
}

