using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rem.Core.Attributes;
using Rem.Core.ComponentModel;

namespace Rem.Core.Numerics;

/// <summary>
/// Represents a strategy of rounding a value to an adjacent whole number.
/// </summary>
public readonly record struct RoundingStrategy
{
    /// <inheritdoc cref="DirectedRoundingStrategy.ToZero"/>
    public static DirectedRoundingStrategy ToZero => DirectedRoundingStrategy.ToZero;

    /// <inheritdoc cref="DirectedRoundingStrategy.AwayFromZero"/>
    public static DirectedRoundingStrategy AwayFromZero => DirectedRoundingStrategy.AwayFromZero;

    /// <inheritdoc cref="DirectedRoundingStrategy.ToNegativeInfinity"/>
    public static DirectedRoundingStrategy ToNegativeInfinity => DirectedRoundingStrategy.ToNegativeInfinity;

    /// <inheritdoc cref="DirectedRoundingStrategy.ToPositiveInfinity"/>
    public static DirectedRoundingStrategy ToPositiveInfinity => DirectedRoundingStrategy.ToPositiveInfinity;

    /// <inheritdoc cref="ToNearestRoundingStrategy.ToEven"/>
    public static ToNearestRoundingStrategy ToNearestThenToEven => ToNearestRoundingStrategy.ToEven;

    /// <inheritdoc cref="ToNearestRoundingStrategy.ToOdd"/>
    public static ToNearestRoundingStrategy ToNearestThenToOdd => ToNearestRoundingStrategy.ToOdd;

    /// <inheritdoc cref="ToNearestRoundingStrategy.ToZero"/>
    public static ToNearestRoundingStrategy ToNearestThenToZero => ToNearestRoundingStrategy.ToZero;

    /// <inheritdoc cref="ToNearestRoundingStrategy.AwayFromZero"/>
    public static ToNearestRoundingStrategy ToNearestThenAwayFromZero => ToNearestRoundingStrategy.AwayFromZero;

    /// <inheritdoc cref="ToNearestRoundingStrategy.ToNegativeInfinity"/>
    public static ToNearestRoundingStrategy ToNearestThenToNegativeInfinity
        => ToNearestRoundingStrategy.ToNegativeInfinity;

    /// <inheritdoc cref="ToNearestRoundingStrategy.ToPositiveInfinity"/>
    public static ToNearestRoundingStrategy ToNearestThenToPositiveInfinity
        => ToNearestRoundingStrategy.ToPositiveInfinity;

    /// <summary>
    /// Gets whether or not this is a strategy of rounding to the nearest whole number.
    /// </summary>
    /// <remarks>
    /// This is the complement of <see cref="IsDirected"/>.
    /// </remarks>
    public bool IsToNearest => !IsDirected;

    /// <summary>
    /// Gets whether or not this is a directed rounding strategy.
    /// </summary>
    /// <remarks>
    /// This is the complement of <see cref="IsToNearest"/>.
    /// </remarks>
    public bool IsDirected { get; }

    /// <summary>
    /// The value backing this instance.
    /// </summary>
    private Union Value { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private RoundingStrategy(ToNearestRoundingStrategy ToNearest)
    {
        Value = new() { ToNearest = ToNearest };
        IsDirected = false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private RoundingStrategy(DirectedRoundingStrategy Directed)
    {
        Value = new() { Directed = Directed };
        IsDirected = true;
    }

    [StructLayout(LayoutKind.Explicit)]
    private readonly struct Union
    {
        public ToNearestRoundingStrategy ToNearest
        {
            get => _toNearest;
            init => _toNearest = value;
        }

        [FieldOffset(0)]
        private readonly ToNearestRoundingStrategy _toNearest;

        public DirectedRoundingStrategy Directed
        {
            get => _directed;
            init => _directed = value;
        }

        [FieldOffset(0)]
        private readonly DirectedRoundingStrategy _directed;
    }

    /// <summary>
    /// Determines if this instance is a <see cref="ToNearestRoundingStrategy"/>, returning it in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="toNearest"></param>
    /// <returns></returns>
    public bool TryGetToNearest(out ToNearestRoundingStrategy toNearest)
        => IsDirected
            ? Try.Failure(out toNearest)
            : Try.Success(out toNearest, Value.ToNearest);

    /// <summary>
    /// Determines if this instance is a <see cref="ToNearestRoundingStrategy"/>, returning it in an
    /// <see langword="out"/> parameter if so and returning the equivalent <see cref="DirectedRoundingStrategy"/>
    /// in an <see langword="out"/> parameter if not.
    /// </summary>
    /// <param name="toNearest"></param>
    /// <param name="directed"></param>
    /// <returns></returns>
    public bool TryGetToNearest(out ToNearestRoundingStrategy toNearest, out DirectedRoundingStrategy directed)
        => IsDirected
            ? Try.Failure(out toNearest, out directed, Value.Directed)
            : Try.Success(out toNearest, Value.ToNearest, out directed);

    /// <summary>
    /// Determines if this instance is a <see cref="DirectedRoundingStrategy"/>, returning it in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="directed"></param>
    /// <returns></returns>
    public bool TryGetDirected(out DirectedRoundingStrategy directed)
        => IsDirected
            ? Try.Success(out directed, Value.Directed)
            : Try.Failure(out directed);

    /// <summary>
    /// Determines if this instance is a <see cref="DirectedRoundingStrategy"/>, returning it in an
    /// <see langword="out"/> parameter if so and reurning the equivalent <see cref="ToNearestRoundingStrategy"/>
    /// in an <see langword="out"/> parameter if not.
    /// </summary>
    /// <param name="directed"></param>
    /// <param name="toNearest"></param>
    /// <returns></returns>
    public bool TryGetDirected(out DirectedRoundingStrategy directed, out ToNearestRoundingStrategy toNearest)
        => IsDirected
            ? Try.Success(out directed, Value.Directed, out toNearest)
            : Try.Failure(out directed, out toNearest, Value.ToNearest);

    /// <summary>
    /// Determines if this instance is equal to another instance of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(RoundingStrategy other)
        => IsDirected
            ? other.IsDirected && Value.Directed == other.Value.Directed
            : other.IsToNearest && Value.ToNearest == other.Value.ToNearest;

    /// <summary>
    /// Gets a hash code representing this instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(IsToNearest, (int)Value.ToNearest.Label);

    /// <summary>
    /// Implicitly converts a <see cref="ToNearestRoundingStrategy"/> to an equivalent <see cref="RoundingStrategy"/>.
    /// </summary>
    /// <param name="strategy"></param>
    public static implicit operator RoundingStrategy(ToNearestRoundingStrategy strategy) => new(strategy);

    /// <summary>
    /// Implicitly converts a <see cref="ToNearestRoundingStrategy"/> to an equivalent <see cref="RoundingStrategy"/>.
    /// </summary>
    /// <param name="strategy"></param>
    public static explicit operator ToNearestRoundingStrategy(RoundingStrategy strategy)
        => strategy.IsToNearest
            ? strategy.Value.ToNearest
            : throw new InvalidCastException(
                $"Cannot cast type {nameof(DirectedRoundingStrategy)} to type {nameof(ToNearestRoundingStrategy)}.");

    /// <summary>
    /// Implicitly converts a <see cref="DirectedRoundingStrategy"/> to an equivalent <see cref="RoundingStrategy"/>.
    /// </summary>
    /// <param name="strategy"></param>
    public static implicit operator RoundingStrategy(DirectedRoundingStrategy strategy) => new(strategy);

    /// <summary>
    /// Implicitly converts a <see cref="DirectedRoundingStrategy"/> to an equivalent <see cref="RoundingStrategy"/>.
    /// </summary>
    /// <param name="strategy"></param>
    public static explicit operator DirectedRoundingStrategy(RoundingStrategy strategy)
        => strategy.IsDirected
            ? strategy.Value.Directed
            : throw new InvalidCastException(
                $"Cannot cast type {nameof(ToNearestRoundingStrategy)} to type {nameof(DirectedRoundingStrategy)}.");
}

/// <summary>
/// Represents a strategy for rounding to the nearest whole number.
/// </summary>
public readonly record struct ToNearestRoundingStrategy
{
    /// <inheritdoc cref="Labels.ToEven"/>
    public static readonly ToNearestRoundingStrategy ToEven = new(Labels.ToEven);

    /// <inheritdoc cref="Labels.ToOdd"/>
    public static readonly ToNearestRoundingStrategy ToOdd = new(Labels.ToOdd);

    /// <inheritdoc cref="Labels.ToZero"/>
    public static readonly ToNearestRoundingStrategy ToZero = new(Labels.ToZero);

    /// <inheritdoc cref="Labels.AwayFromZero"/>
    public static readonly ToNearestRoundingStrategy AwayFromZero = new(Labels.AwayFromZero);

    /// <inheritdoc cref="Labels.ToNegativeInfinity"/>
    public static readonly ToNearestRoundingStrategy ToNegativeInfinity = new(Labels.ToNegativeInfinity);

    /// <inheritdoc cref="Labels.ToPositiveInfinity"/>
    public static readonly ToNearestRoundingStrategy ToPositiveInfinity = new(Labels.ToPositiveInfinity);

    /// <summary>
    /// Gets a label used to uniquely identify this instance.
    /// </summary>
    public Labels Label { get; }

    private ToNearestRoundingStrategy([NameableEnum] Labels Value) { this.Label = Value; }

    /// <summary>
    /// Gets the strategy of rounding to the nearest whole number, resolving midpoints by rounding to the nearest
    /// number of the given parity.
    /// </summary>
    /// <param name="Parity"></param>
    /// <returns></returns>
    public static ToNearestRoundingStrategy ToParity(Parity Parity) => Parity == Parity.Even ? ToEven : ToOdd;

    /// <summary>
    /// Used to uniquely identify values of type <see cref="ToNearestRoundingStrategy"/>.
    /// </summary>
    public enum Labels : byte
    {
        /// <summary>
        /// Represents the strategy of rounding to the nearest whole number, resolving midpoints by rounding to the
        /// nearest even number.
        /// </summary>
        /// <remarks>
        /// This is the default.
        /// </remarks>
        ToEven,

        /// <summary>
        /// Represents the strategy of rounding to the nearest whole number, resolving midpoints by rounding to the
        /// nearest odd number.
        /// </summary>
        ToOdd,

        /// <summary>
        /// Represents the strategy of rounding to the nearest whole number, resolving midpoints by rounding
        /// towards zero.
        /// </summary>
        ToZero,

        /// <summary>
        /// Represents the strategy of rounding to the nearest whole number, resolving midpoints by rounding
        /// away from zero.
        /// </summary>
        AwayFromZero,

        /// <summary>
        /// Represents the strategy of rounding to the nearest whole number, resolving midpoints by rounding
        /// down, towards negative infinity.
        /// </summary>
        ToNegativeInfinity,

        /// <summary>
        /// Represents the strategy of rounding to the nearest whole number, resolving midpoints by rounding
        /// up, towards positive infinity.
        /// </summary>
        ToPositiveInfinity,
    }

    /// <summary>
    /// Implicitly converts a named <see cref="Labels"/> instance to the <see cref="ToNearestRoundingStrategy"/>
    /// instance it represents. 
    /// </summary>
    /// <param name="label"></param>
    /// <exception cref="InvalidEnumArgumentException"><paramref name="label"/> was unnamed.</exception>
    public static implicit operator ToNearestRoundingStrategy([NameableEnum] Labels label)
        => label is Labels.ToZero or Labels.AwayFromZero
                 or Labels.ToNegativeInfinity or Labels.ToPositiveInfinity
                 or Labels.ToEven or Labels.ToOdd
            ? new(label)
            : throw new InvalidEnumArgumentException($"Unnamed {nameof(ToNearestRoundingStrategy)} label.");

    /// <summary>
    /// Implicitly converts a <see cref="ToNearestRoundingStrategy"/> to its label.
    /// </summary>
    /// <param name="strategy"></param>
    public static implicit operator Labels(ToNearestRoundingStrategy strategy) => strategy.Label;
}

/// <summary>
/// Represents a directed rounding strategy.
/// </summary>
public readonly record struct DirectedRoundingStrategy
{
    /// <inheritdoc cref="Labels.ToZero"/>
    public static readonly DirectedRoundingStrategy ToZero = new(Labels.ToZero);

    /// <inheritdoc cref="Labels.AwayFromZero"/>
    public static readonly DirectedRoundingStrategy AwayFromZero = new(Labels.AwayFromZero);

    /// <inheritdoc cref="Labels.ToNegativeInfinity"/>
    public static readonly DirectedRoundingStrategy ToNegativeInfinity = new(Labels.ToNegativeInfinity);

    /// <inheritdoc cref="Labels.ToPositiveInfinity"/>
    public static readonly DirectedRoundingStrategy ToPositiveInfinity = new(Labels.ToPositiveInfinity);

    /// <summary>
    /// Gets a label used to uniquely identify this instance.
    /// </summary>
    public Labels Label { get; }

    private DirectedRoundingStrategy([NameableEnum] Labels Label) { this.Label = Label; }

    /// <summary>
    /// Used to uniquely identify values of type <see cref="DirectedRoundingStrategy"/>.
    /// </summary>
    public enum Labels : byte
    {
        /// <summary>
        /// Represents the strategy of directed rounding towards zero.
        /// </summary>
        /// <remarks>
        /// This is the default.
        /// </remarks>
        ToZero,

        /// <summary>
        /// Represents the strategy of directed rounding away from zero.
        /// </summary>
        AwayFromZero,

        /// <summary>
        /// Represents the strategy of directed rounding towards negative infinity.
        /// </summary>
        ToNegativeInfinity,

        /// <summary>
        /// Represents the strategy of directed rounding towards positive infinity.
        /// </summary>
        ToPositiveInfinity,
    }

    /// <summary>
    /// Implicitly converts a named <see cref="Labels"/> instance to the <see cref="DirectedRoundingStrategy"/>
    /// instance it represents. 
    /// </summary>
    /// <param name="label"></param>
    /// <exception cref="InvalidEnumArgumentException"><paramref name="label"/> was unnamed.</exception>
    public static implicit operator DirectedRoundingStrategy([NameableEnum] Labels label)
        => label is Labels.ToZero or Labels.AwayFromZero
            ? new(label)
            : throw new InvalidEnumArgumentException($"Unnamed {nameof(DirectedRoundingStrategy)} label.");

    /// <summary>
    /// Implicitly converts a <see cref="DirectedRoundingStrategy"/> to its label.
    /// </summary>
    /// <param name="strategy"></param>
    public static implicit operator Labels(DirectedRoundingStrategy strategy) => strategy.Label;
}
