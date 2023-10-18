using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics.Digits;

#region Classes
#region Concrete
/// <summary>
/// A wrapper for a list of <see cref="byte"/> digits.
/// </summary>
/// <inheritdoc cref="ULongDigitList"/>
public sealed record class ByteDigitList : DigitList<byte>, IByteDigitList
{
    /// <summary>
    /// An empty <see cref="ByteDigitList"/>.
    /// </summary>
    public static readonly ByteDigitList Empty = new(ImmutableArray<byte>.Empty);

    #region Index
    ushort IUShortDigitList.this[int index] => Digits[index];

    uint IUIntDigitList.this[int index] => Digits[index];

    ulong IULongDigitList.this[int index] => Digits[index];

    [return: NonNegative]
    private protected override BigUnsignedInteger IndexInternal([NonNegative] int index)
        => Digits[index];
    #endregion

    #region Constructor
    private ByteDigitList([NonDefaultableStruct] ImmutableArray<byte> Digits) : base(Digits) { }
    #endregion

    #region Factory Methods
    /// <inheritdoc cref="CreateRange(IEnumerable{byte})"/>
    public static ByteDigitList CreateRange(params byte[] Digits)
        => new(Throw.IfArgNull(Digits, nameof(Digits)).ToImmutableArray());

    /// <summary>
    /// Creates a new <see cref="ByteDigitList"/> with the list of digits to wrap.
    /// </summary>
    /// <param name="Digits"></param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="Digits"/> was <see langword="null"/>.
    /// </exception>
    public static ByteDigitList CreateRange(IEnumerable<byte> Digits)
        => new(Throw.IfArgNull(Digits, nameof(Digits)).ToImmutableArray());

    /// <summary>
    /// Creates a new <see cref="ByteDigitList"/> with the list of digits to wrap.
    /// </summary>
    /// <param name="Digits"></param>
    /// <exception cref="StructArgumentDefaultException">
    /// <paramref name="Digits"/> was default.
    /// </exception>
    public static ByteDigitList CreateRange([NonDefaultableStruct] ImmutableArray<byte> Digits) => new(Digits);
    #endregion

    #region IEnumerable
    IEnumerator<ushort> IEnumerable<ushort>.GetEnumerator()
    {
        foreach (var b in Digits) yield return b;
    }

    IEnumerator<uint> IEnumerable<uint>.GetEnumerator()
    {
        foreach (var b in Digits) yield return b;
    }

    IEnumerator<ulong> IEnumerable<ulong>.GetEnumerator()
    {
        foreach (var b in Digits) yield return b;
    }

    private protected override IEnumerator<BigUnsignedInteger> GetEnumeratorInternal()
    {
        foreach (var b in Digits) yield return b;
    }
    #endregion

    #region Leading Zero Removal
    private protected override DigitList<byte> GenericWithoutLeadingZeroesInternal() => WithoutLeadingZeroes();

    /// <inheritdoc cref="DigitList.WithoutLeadingZeroes"/>
    public new ByteDigitList WithoutLeadingZeroes() => new(GetDigitsWithoutLeadingZeroes());
    #endregion

    #region Equivalence
    /// <inheritdoc/>
    public override bool IsEquivalentTo(DigitList other)
#pragma warning disable CS8509 // This should handle everything
        => other switch
#pragma warning restore CS8509
        {
            null => throw new ArgumentNullException(nameof(other)),
            BigUnsignedIntegerDigitList(var otherDigits) => otherDigits.SequenceEqual(Digits.Select(n => (BigUnsignedInteger)n)),
            ULongDigitList(var otherDigits) => otherDigits.SequenceEqual(Digits.Select(n => (ulong)n)),
            UIntDigitList(var otherDigits) => otherDigits.SequenceEqual(Digits.Select(n => (uint)n)),
            UShortDigitList(var otherDigits) => otherDigits.SequenceEqual(Digits.Select(n => (ushort)n)),
            ByteDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits),
        };
    #endregion

    #region Splitting
    private protected override IReadOnlyList<DigitList<byte>> SplitAtIndicesGenericInternal(int[] indices)
        => SplitAtIndices(indices);

    /// <inheritdoc cref="DigitList.SplitAtIndices(int[])"/>
    public new IReadOnlyList<ByteDigitList> SplitAtIndices(params int[] indices)
    {
        if (Throw.IfArgNull(indices, nameof(indices)).Length == 0) return ImmutableList.Create(this);
        else return SplitIntoArraySegmentsAtIndices(indices)
                        .Select(ds => new ByteDigitList(ds.ToImmutableArray()))
                        .ToImmutableList();
    }
    #endregion

    private protected override DigitList<byte>.Builder ToGenericBuilderInternal() => ToBuilder();

    /// <inheritdoc cref="DigitList.ToBuilder"/>
    public new Builder ToBuilder() => new(Digits.ToBuilder());

    /// <summary>
    /// A builder for a <see cref="ByteDigitList"/>.
    /// </summary>
    public new sealed class Builder : DigitList<byte>.Builder
    {
        /// <inheritdoc cref="DigitList{TDigit}.Builder.ListBuilderInternal"/>
        public ImmutableArray<byte>.Builder ListBuilder => ListBuilderInternal;

        /// <summary>
        /// Constructs a new instance of the <see cref="Builder"/> class.
        /// </summary>
        public Builder() : base() { }

        internal Builder(ImmutableArray<byte>.Builder ListBuilder) : base(ListBuilder) { }

        /// <inheritdoc/>
        public override void Add(byte Digit) => ListBuilderInternal.Add(Digit);

        /// <inheritdoc/>
        /// <exception cref="OverflowException">
        /// <paramref name="Digit"/> was too large for a <see cref="byte"/>.
        /// </exception>
        public override void Add(ushort Digit) => ListBuilderInternal.Add((byte)Digit);

        /// <inheritdoc/>
        /// <exception cref="OverflowException">
        /// <paramref name="Digit"/> was too large for a <see cref="byte"/>.
        /// </exception>
        public override void Add(uint Digit) => ListBuilderInternal.Add((byte)Digit);

        /// <inheritdoc/>
        /// <exception cref="OverflowException">
        /// <paramref name="Digit"/> was too large for a <see cref="byte"/>.
        /// </exception>
        public override void Add(ulong Digit) => ListBuilderInternal.Add((byte)Digit);

        /// <inheritdoc/>
        /// <exception cref="OverflowException">
        /// <paramref name="Digit"/> was too large for a <see cref="byte"/>.
        /// </exception>
        public override void Add([NonNegative] BigUnsignedInteger Digit)
            => ListBuilderInternal.Add((byte)Throw.IfArgNegative(Digit, nameof(Digit)));

        private protected override DigitList<byte> ToGenericListInternal() => ToList();

        /// <inheritdoc cref="DigitList.Builder.ToList"/>
        public new ByteDigitList ToList() => new(ListBuilderInternal.ToImmutable());
    }
}

/// <summary>
/// A wrapper for a list of <see cref="ushort"/> digits.
/// </summary>
public sealed record class UShortDigitList : DigitList<ushort>, IUShortDigitList
{
    /// <summary>
    /// An empty <see cref="UShortDigitList"/>.
    /// </summary>
    public static readonly UShortDigitList Empty = new(ImmutableArray<ushort>.Empty);

    #region Index
    uint IUIntDigitList.this[int index] => Digits[index];

    ulong IULongDigitList.this[int index] => Digits[index];

    [return: NonNegative]
    private protected override BigUnsignedInteger IndexInternal([NonNegative] int index) => Digits[index];
    #endregion

    #region Constructor
    private UShortDigitList([NonDefaultableStruct] ImmutableArray<ushort> Digits) : base(Digits) { }
    #endregion

    #region Factory Methods
    /// <inheritdoc cref="CreateRange(IEnumerable{ushort})"/>
    public static UShortDigitList CreateRange(params ushort[] Digits)
        => new(Throw.IfArgNull(Digits, nameof(Digits)).ToImmutableArray());

    /// <summary>
    /// Creates a new <see cref="UShortDigitList"/> with the list of digits to wrap.
    /// </summary>
    /// <param name="Digits"></param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="Digits"/> was <see langword="null"/>.
    /// </exception>
    public static UShortDigitList CreateRange(IEnumerable<ushort> Digits)
        => new(Throw.IfArgNull(Digits, nameof(Digits)).ToImmutableArray());

    /// <summary>
    /// Creates a new <see cref="UShortDigitList"/> with the list of digits to wrap.
    /// </summary>
    /// <param name="Digits"></param>
    /// <exception cref="StructArgumentDefaultException">
    /// <paramref name="Digits"/> was default.
    /// </exception>
    public static UShortDigitList CreateRange([NonDefaultableStruct] ImmutableArray<ushort> Digits) => new(Digits);
    #endregion

    #region IEnumerable
    IEnumerator<uint> IEnumerable<uint>.GetEnumerator()
    {
        foreach (var us in Digits) yield return us;
    }

    IEnumerator<ulong> IEnumerable<ulong>.GetEnumerator()
    {
        foreach (var us in Digits) yield return us;
    }

    private protected override IEnumerator<BigUnsignedInteger> GetEnumeratorInternal()
    {
        foreach (var us in Digits) yield return us;
    }
    #endregion

    #region Leading Zero Removal
    private protected override DigitList<ushort> GenericWithoutLeadingZeroesInternal() => WithoutLeadingZeroes();

    /// <inheritdoc cref="DigitList.WithoutLeadingZeroes"/>
    public new UShortDigitList WithoutLeadingZeroes() => new(GetDigitsWithoutLeadingZeroes());
    #endregion

    #region Equivalence
    /// <inheritdoc/>
    public override bool IsEquivalentTo(DigitList other)
#pragma warning disable CS8509 // This should handle everything
        => other switch
#pragma warning restore CS8509
        {
            null => throw new ArgumentNullException(nameof(other)),
            BigUnsignedIntegerDigitList(var otherDigits) => otherDigits.SequenceEqual(Digits.Select(n => (BigUnsignedInteger)n)),
            ULongDigitList(var otherDigits) => otherDigits.SequenceEqual(Digits.Select(n => (ulong)n)),
            UIntDigitList(var otherDigits) => otherDigits.SequenceEqual(Digits.Select(n => (uint)n)),
            UShortDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits),
            ByteDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits.Select(n => (ushort)n)),
        };
    #endregion

    #region Splitting
    private protected override IReadOnlyList<DigitList<ushort>> SplitAtIndicesGenericInternal(int[] indices)
        => SplitAtIndices(indices);

    /// <inheritdoc cref="DigitList.SplitAtIndices(int[])"/>
    public new IReadOnlyList<UShortDigitList> SplitAtIndices(params int[] indices)
    {
        if (Throw.IfArgNull(indices, nameof(indices)).Length == 0) return ImmutableList.Create(this);
        else return SplitIntoArraySegmentsAtIndices(indices)
                        .Select(ds => new UShortDigitList(ds.ToImmutableArray()))
                        .ToImmutableList();
    }
    #endregion

    private protected override DigitList<ushort>.Builder ToGenericBuilderInternal() => ToBuilder();

    /// <inheritdoc cref="DigitList.ToBuilder"/>
    public new Builder ToBuilder() => new(Digits.ToBuilder());

    /// <summary>
    /// A builder for a <see cref="UShortDigitList"/>.
    /// </summary>
    public new sealed class Builder : DigitList<ushort>.Builder
    {
        /// <inheritdoc cref="DigitList{TDigit}.Builder.ListBuilderInternal"/>
        public ImmutableArray<ushort>.Builder ListBuilder => ListBuilderInternal;

        /// <summary>
        /// Constructs a new instance of the <see cref="Builder"/> class.
        /// </summary>
        public Builder() : base() { }

        internal Builder(ImmutableArray<ushort>.Builder ListBuilder) : base(ListBuilder) { }

        /// <inheritdoc/>
        public override void Add(byte Digit) => ListBuilderInternal.Add(Digit);

        /// <inheritdoc/>
        public override void Add(ushort Digit) => ListBuilderInternal.Add(Digit);

        /// <inheritdoc/>
        /// <exception cref="OverflowException">
        /// <paramref name="Digit"/> was too large for a <see cref="ushort"/>.
        /// </exception>
        public override void Add(uint Digit) => ListBuilderInternal.Add((ushort)Digit);

        /// <inheritdoc/>
        /// <exception cref="OverflowException">
        /// <paramref name="Digit"/> was too large for a <see cref="ushort"/>.
        /// </exception>
        public override void Add(ulong Digit) => ListBuilderInternal.Add((ushort)Digit);

        /// <inheritdoc/>
        /// <exception cref="OverflowException">
        /// <paramref name="Digit"/> was too large for a <see cref="ushort"/>.
        /// </exception>
        public override void Add([NonNegative] BigUnsignedInteger Digit)
            => ListBuilderInternal.Add((ushort)Throw.IfArgNegative(Digit, nameof(Digit)));

        private protected override DigitList<ushort> ToGenericListInternal() => ToList();

        /// <inheritdoc cref="DigitList.Builder.ToList"/>
        public new UShortDigitList ToList() => new(ListBuilderInternal.ToImmutable());
    }
}

/// <summary>
/// A wrapper for a list of <see cref="uint"/> digits.
/// </summary>
public sealed record class UIntDigitList : DigitList<uint>, IUIntDigitList
{
    /// <summary>
    /// An empty <see cref="UIntDigitList"/>.
    /// </summary>
    public static readonly UIntDigitList Empty = new(ImmutableArray<uint>.Empty);

    #region Index
    /// <inheritdoc cref="DigitList.this[int]"/>
    public new uint this[[NonNegative] int index] => Digits[index];

    ulong IULongDigitList.this[int index] => Digits[index];

    [return: NonNegative]
    private protected override BigUnsignedInteger IndexInternal([NonNegative] int index) => Digits[index];
    #endregion

    #region Constructor
    private UIntDigitList([NonDefaultableStruct] ImmutableArray<uint> Digits) : base(Digits) { }
    #endregion

    #region Factory Methods
    /// <inheritdoc cref="CreateRange(IEnumerable{uint})"/>
    public static UIntDigitList CreateRange(params uint[] Digits)
        => new(Throw.IfArgNull(Digits, nameof(Digits)).ToImmutableArray());

    /// <summary>
    /// Creates a new <see cref="UIntDigitList"/> with the list of digits to wrap.
    /// </summary>
    /// <param name="Digits"></param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="Digits"/> was <see langword="null"/>.
    /// </exception>
    public static UIntDigitList CreateRange(IEnumerable<uint> Digits)
        => new(Throw.IfArgNull(Digits, nameof(Digits)).ToImmutableArray());

    /// <summary>
    /// Creates a new <see cref="UIntDigitList"/> with the list of digits to wrap.
    /// </summary>
    /// <param name="Digits"></param>
    /// <exception cref="StructArgumentDefaultException">
    /// <paramref name="Digits"/> was default.
    /// </exception>
    public static UIntDigitList CreateRange([NonDefaultableStruct] ImmutableArray<uint> Digits) => new(Digits);
    #endregion

    #region IEnumerable
    IEnumerator<ulong> IEnumerable<ulong>.GetEnumerator()
    {
        foreach (var ui in Digits) yield return ui;
    }

    private protected override IEnumerator<BigUnsignedInteger> GetEnumeratorInternal()
    {
        foreach (var ui in Digits) yield return ui;
    }
    #endregion

    #region Leading Zero Removal
    private protected override DigitList<uint> GenericWithoutLeadingZeroesInternal() => WithoutLeadingZeroes();

    /// <inheritdoc cref="DigitList.WithoutLeadingZeroes"/>
    public new UIntDigitList WithoutLeadingZeroes() => new(GetDigitsWithoutLeadingZeroes());
    #endregion

    #region Equivalence
    /// <inheritdoc/>
    public override bool IsEquivalentTo(DigitList other)
#pragma warning disable CS8509 // This should handle everything
        => other switch
#pragma warning restore CS8509
        {
            null => throw new ArgumentNullException(nameof(other)),
            BigUnsignedIntegerDigitList(var otherDigits) => otherDigits.SequenceEqual(Digits.Select(n => (BigUnsignedInteger)n)),
            ULongDigitList(var otherDigits) => otherDigits.SequenceEqual(Digits.Select(n => (ulong)n)),
            UIntDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits.Select(n => (uint)n)),
            UShortDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits.Select(n => (uint)n)),
            ByteDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits.Select(n => (uint)n)),
        };
    #endregion

    #region Splitting
    private protected override IReadOnlyList<DigitList<uint>> SplitAtIndicesGenericInternal(int[] indices)
        => SplitAtIndices(indices);

    /// <inheritdoc cref="DigitList.SplitAtIndices(int[])"/>
    public new IReadOnlyList<UIntDigitList> SplitAtIndices(params int[] indices)
    {
        if (Throw.IfArgNull(indices, nameof(indices)).Length == 0) return ImmutableList.Create(this);
        else return SplitIntoArraySegmentsAtIndices(indices)
                        .Select(ds => new UIntDigitList(ds.ToImmutableArray()))
                        .ToImmutableList();
    }
    #endregion

    private protected override DigitList<uint>.Builder ToGenericBuilderInternal() => ToBuilder();

    /// <inheritdoc cref="DigitList.ToBuilder"/>
    public new Builder ToBuilder() => new(Digits.ToBuilder());

    /// <summary>
    /// A builder for a <see cref="UIntDigitList"/>.
    /// </summary>
    public new sealed class Builder : DigitList<uint>.Builder
    {
        /// <inheritdoc cref="DigitList{TDigit}.Builder.ListBuilderInternal"/>
        public ImmutableArray<uint>.Builder ListBuilder => ListBuilderInternal;

        /// <summary>
        /// Constructs a new instance of the <see cref="Builder"/> class.
        /// </summary>
        public Builder() : base() { }

        internal Builder(ImmutableArray<uint>.Builder ListBuilder) : base(ListBuilder) { }

        /// <inheritdoc/>
        public override void Add(byte Digit) => ListBuilderInternal.Add(Digit);

        /// <inheritdoc/>
        public override void Add(ushort Digit) => ListBuilderInternal.Add(Digit);

        /// <inheritdoc/>
        public override void Add(uint Digit) => ListBuilderInternal.Add(Digit);

        /// <inheritdoc/>
        /// <exception cref="OverflowException">
        /// <paramref name="Digit"/> was too large for a <see cref="uint"/>.
        /// </exception>
        public override void Add(ulong Digit) => ListBuilderInternal.Add((uint)Digit);

        /// <inheritdoc/>
        /// <exception cref="OverflowException">
        /// <paramref name="Digit"/> was too large for a <see cref="uint"/>.
        /// </exception>
        public override void Add([NonNegative] BigUnsignedInteger Digit)
            => ListBuilderInternal.Add((uint)Throw.IfArgNegative(Digit, nameof(Digit)));

        private protected override DigitList<uint> ToGenericListInternal() => ToList();

        /// <inheritdoc cref="DigitList.Builder.ToList"/>
        public new UIntDigitList ToList() => new(ListBuilderInternal.ToImmutable());
    }
}

/// <summary>
/// A wrapper for a list of <see cref="ulong"/> digits.
/// </summary>
public sealed record class ULongDigitList : DigitList<ulong>, IULongDigitList
{
    /// <summary>
    /// An empty <see cref="ULongDigitList"/>.
    /// </summary>
    public static readonly ULongDigitList Empty = new(ImmutableArray<ulong>.Empty);

    #region Index
    /// <inheritdoc cref="DigitList.this[int]"/>
    public new ulong this[[NonNegative] int index] => Digits[index];

    [return: NonNegative]
    private protected override BigUnsignedInteger IndexInternal([NonNegative] int index) => Digits[index];
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new instance of this class.
    /// </summary>
    /// <param name="Digits"></param>
    private ULongDigitList([NonDefaultableStruct] ImmutableArray<ulong> Digits) : base(Digits) { }
    #endregion

    #region Factory Methods
    /// <inheritdoc cref="CreateRange(IEnumerable{ulong})"/>
    public static ULongDigitList CreateRange(params ulong[] Digits)
        => new(Throw.IfArgNull(Digits, nameof(Digits)).ToImmutableArray());

    /// <summary>
    /// Creates a new <see cref="ULongDigitList"/> with the list of digits to wrap.
    /// </summary>
    /// <param name="Digits"></param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="Digits"/> was <see langword="null"/>.
    /// </exception>
    public static ULongDigitList CreateRange(IEnumerable<ulong> Digits)
        => new(Throw.IfArgNull(Digits, nameof(Digits)).ToImmutableArray());

    /// <summary>
    /// Creates a new <see cref="ULongDigitList"/> with the list of digits to wrap.
    /// </summary>
    /// <param name="Digits"></param>
    /// <exception cref="StructArgumentDefaultException">
    /// <paramref name="Digits"/> was default.
    /// </exception>
    public static ULongDigitList CreateRange([NonDefaultableStruct] ImmutableArray<ulong> Digits) => new(Digits);
    #endregion

    #region IEnumerable
    /// <inheritdoc cref="DigitList.GetEnumerator"/>
    private protected override IEnumerator<BigUnsignedInteger> GetEnumeratorInternal()
    {
        foreach (var ul in Digits) yield return ul;
    }
    #endregion

    #region Leading Zero Removal
    private protected override DigitList<ulong> GenericWithoutLeadingZeroesInternal() => WithoutLeadingZeroes();

    /// <inheritdoc cref="DigitList.WithoutLeadingZeroes"/>
    public new ULongDigitList WithoutLeadingZeroes() => new(GetDigitsWithoutLeadingZeroes());
    #endregion

    #region Equivalence
    /// <inheritdoc/>
    public override bool IsEquivalentTo(DigitList other)
#pragma warning disable CS8509 // This should handle everything
        => other switch
#pragma warning restore CS8509
        {
            null => throw new ArgumentNullException(nameof(other)),
            BigUnsignedIntegerDigitList(var otherDigits) => otherDigits.SequenceEqual(Digits.Select(n => (BigUnsignedInteger)n)),
            ULongDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits),
            UIntDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits.Select(n => (ulong)n)),
            UShortDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits.Select(n => (ulong)n)),
            ByteDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits.Select(n => (ulong)n)),
        };
    #endregion

    #region Splitting
    private protected override IReadOnlyList<DigitList<ulong>> SplitAtIndicesGenericInternal(int[] indices)
        => SplitAtIndices(indices);

    /// <inheritdoc cref="DigitList.SplitAtIndices(int[])"/>
    public new IReadOnlyList<ULongDigitList> SplitAtIndices(params int[] indices)
    {
        if (Throw.IfArgNull(indices, nameof(indices)).Length == 0) return ImmutableList.Create(this);
        else return SplitIntoArraySegmentsAtIndices(indices)
                        .Select(ds => new ULongDigitList(ds.ToImmutableArray()))
                        .ToImmutableList();
    }
    #endregion

    private protected override DigitList<ulong>.Builder ToGenericBuilderInternal() => ToBuilder();

    /// <inheritdoc cref="DigitList.ToBuilder"/>
    public new Builder ToBuilder() => new(Digits.ToBuilder());

    /// <summary>
    /// A builder for a <see cref="ULongDigitList"/>.
    /// </summary>
    public new sealed class Builder : DigitList<ulong>.Builder
    {
        /// <inheritdoc cref="DigitList{TDigit}.Builder.ListBuilderInternal"/>
        public ImmutableArray<ulong>.Builder ListBuilder => ListBuilderInternal;

        /// <summary>
        /// Constructs a new instance of the <see cref="Builder"/> class.
        /// </summary>
        public Builder() : base() { }

        internal Builder(ImmutableArray<ulong>.Builder ListBuilder) : base(ListBuilder) { }

        /// <inheritdoc/>
        public override void Add(byte Digit) => ListBuilderInternal.Add(Digit);

        /// <inheritdoc/>
        public override void Add(ushort Digit) => ListBuilderInternal.Add(Digit);

        /// <inheritdoc/>
        public override void Add(uint Digit) => ListBuilderInternal.Add(Digit);

        /// <inheritdoc/>
        public override void Add(ulong Digit) => ListBuilderInternal.Add(Digit);

        /// <inheritdoc/>
        /// <exception cref="OverflowException">
        /// <paramref name="Digit"/> was too large for a <see cref="ulong"/>.
        /// </exception>
        public override void Add([NonNegative] BigUnsignedInteger Digit)
            => ListBuilderInternal.Add((ulong)Throw.IfArgNegative(Digit, nameof(Digit)));

        private protected override DigitList<ulong> ToGenericListInternal() => ToList();

        /// <inheritdoc cref="DigitList.Builder.ToList"/>
        public new ULongDigitList ToList() => new(ListBuilderInternal.ToImmutable());
    }
}

/// <summary>
/// A wrapper for a list of <see cref="BigUnsignedInteger"/> digits.
/// </summary>
public sealed record class BigUnsignedIntegerDigitList : DigitList<BigUnsignedInteger>
{
    /// <summary>
    /// An empty <see cref="BigUnsignedIntegerDigitList"/>.
    /// </summary>
    public static readonly BigUnsignedIntegerDigitList Empty = new(ImmutableArray<BigUnsignedInteger>.Empty);

    #region Index
    [return: NonNegative] private protected override BigUnsignedInteger IndexInternal([NonNegative] int index)
        => Digits[index];
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new instance of the <see cref="BigUnsignedIntegerDigitList"/> class with the list of digits to wrap.
    /// </summary>
    /// <remarks>
    /// This constructor is private since it does not do any checking of the signs of the digits (as the builder class
    /// ensures negative digits cannot be added).
    /// </remarks>
    /// <param name="Digits"></param>
    /// <exception cref="StructArgumentDefaultException">
    /// <paramref name="Digits"/> was default.
    /// </exception>
    private BigUnsignedIntegerDigitList([NonDefaultableStruct] ImmutableArray<BigUnsignedInteger> Digits) : base(Digits) { }
    #endregion

    #region Factory Methods
    /// <inheritdoc cref="CreateRange(IEnumerable{BigUnsignedInteger})"/>
    public static BigUnsignedIntegerDigitList CreateRange(params BigUnsignedInteger[] Digits)
        => CreateRange(Throw.IfArgNull(Digits, nameof(Digits)).ToImmutableArray());

    /// <summary>
    /// Creates a new <see cref="BigUnsignedIntegerDigitList"/> with the list of digits to wrap.
    /// </summary>
    /// <param name="Digits"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="Digits"/> was <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Digits"/> contained a negative number.
    /// </exception>
    public static BigUnsignedIntegerDigitList CreateRange(IEnumerable<BigUnsignedInteger> Digits)
        => CreateRange(Throw.IfArgNull(Digits, nameof(Digits)).ToImmutableArray());

    /// <summary>
    /// Creates a new <see cref="BigUnsignedIntegerDigitList"/> with the list of digits to wrap.
    /// </summary>
    /// <param name="Digits"></param>
    /// <exception cref="StructArgumentDefaultException">
    /// <paramref name="Digits"/> was default.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Digits"/> contained a negative number.
    /// </exception>
    public static BigUnsignedIntegerDigitList CreateRange([NonDefaultableStruct] ImmutableArray<BigUnsignedInteger> Digits)
    {
        Throw.IfStructArgDefault(Digits, nameof(Digits));

        foreach (var digit in Digits)
        {
            if (digit < 0)
            {
                throw new ArgumentOutOfRangeException(null, "Cannot construct a digit list with negative digits.");
            }
        }

        return new(Digits);
    }
    #endregion

    #region IEnumerable
    /// <inheritdoc cref="DigitList.GetEnumerator"/>
    public new ImmutableArray<BigUnsignedInteger>.Enumerator GetEnumerator() => Digits.GetEnumerator();

    private protected override IEnumerator<BigUnsignedInteger> GetEnumeratorInternal()
    {
        foreach (var bi in Digits) yield return bi;
    }
    #endregion

    #region Leading Zero Removal
    private protected override DigitList<BigUnsignedInteger> GenericWithoutLeadingZeroesInternal() => WithoutLeadingZeroes();

    /// <inheritdoc cref="DigitList.WithoutLeadingZeroes"/>
    public new BigUnsignedIntegerDigitList WithoutLeadingZeroes() => new(GetDigitsWithoutLeadingZeroes());
    #endregion

    #region Equivalence
    /// <inheritdoc/>
    public override bool IsEquivalentTo(DigitList other)
#pragma warning disable CS8509 // This should handle everything
        => other switch
#pragma warning restore CS8509
        {
            null => throw new ArgumentNullException(nameof(other)),
            BigUnsignedIntegerDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits),
            ULongDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits.Select(n => (BigUnsignedInteger)n)),
            UIntDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits.Select(n => (BigUnsignedInteger)n)),
            UShortDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits.Select(n => (BigUnsignedInteger)n)),
            ByteDigitList(var otherDigits) => Digits.SequenceEqual(otherDigits.Select(n => (BigUnsignedInteger)n)),
        };
    #endregion

    #region Splitting
    private protected override IReadOnlyList<DigitList<BigUnsignedInteger>> SplitAtIndicesGenericInternal(int[] indices)
        => SplitAtIndices(indices);

    /// <inheritdoc cref="DigitList.SplitAtIndices(int[])"/>
    public new IReadOnlyList<BigUnsignedIntegerDigitList> SplitAtIndices(params int[] indices)
    {
        if (Throw.IfArgNull(indices, nameof(indices)).Length == 0) return ImmutableList.Create(this);
        else return SplitIntoArraySegmentsAtIndices(indices)
                        .Select(ds => new BigUnsignedIntegerDigitList(ds.ToImmutableArray()))
                        .ToImmutableList();
    }
    #endregion

    private protected override DigitList<BigUnsignedInteger>.Builder ToGenericBuilderInternal() => ToBuilder();

    /// <inheritdoc cref="DigitList.ToBuilder"/>
    public new Builder ToBuilder() => new(Digits.ToBuilder());

    /// <summary>
    /// A builder for a <see cref="BigUnsignedIntegerDigitList"/>.
    /// </summary>
    public new sealed class Builder : DigitList<BigUnsignedInteger>.Builder
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="Builder"/> class.
        /// </summary>
        public Builder() : base() { }

        internal Builder(ImmutableArray<BigUnsignedInteger>.Builder ListBuilder) : base(ListBuilder) { }

        /// <inheritdoc/>
        public override void Add(byte Digit) => ListBuilderInternal.Add(Digit);

        /// <inheritdoc/>
        public override void Add(ushort Digit) => ListBuilderInternal.Add(Digit);

        /// <inheritdoc/>
        public override void Add(uint Digit) => ListBuilderInternal.Add(Digit);

        /// <inheritdoc/>
        public override void Add(ulong Digit) => ListBuilderInternal.Add(Digit);

        /// <inheritdoc/>
        public override void Add([NonNegative] BigUnsignedInteger Digit)
            => ListBuilderInternal.Add(Throw.IfArgNegative(Digit, nameof(Digit)));

        private protected override DigitList<BigUnsignedInteger> ToGenericListInternal() => ToList();

        /// <inheritdoc cref="DigitList.Builder.ToList"/>
        public new BigUnsignedIntegerDigitList ToList() => new(ListBuilderInternal.ToImmutable());
    }
}
#endregion

#region Abstract
/// <summary>
/// A wrapper for a list of digits of a specific generic type.
/// </summary>
/// <typeparam name="TDigit">
/// The type of digit stored in the list.
/// <para/>
/// This class cannot be extended outside of this assembly; therefore, the only possible values of this parameter are
/// <see cref="byte"/>, <see cref="ushort"/>, <see cref="uint"/>, <see cref="ulong"/> and <see cref="BigUnsignedInteger"/>.
/// </typeparam>
/// <param name="Digits">The list of digits to wrap.</param>
/// <exception cref="StructArgumentDefaultException"><paramref name="Digits"/> was the default.</exception>
public abstract record class DigitList<TDigit>(
    [NonDefaultableStruct] ImmutableArray<TDigit> Digits)
    : DigitList, IEnumerable<TDigit>
    where TDigit : struct, IEquatable<TDigit>, IFormattable
{
    #region Properties
    /// <inheritdoc/>
    public sealed override int Count => Digits.Length;

    /// <inheritdoc/>
    public sealed override DigitType DigitType => DigitType.FromRepresentedType(typeof(TDigit));

    #region Index
    /// <inheritdoc cref="DigitList.this[int]"/>
    public new TDigit this[[NonNegative] int index] => Digits[index];
    #endregion

    /// <summary>
    /// Gets an immutable array containing the digits the list is comprised of.
    /// </summary>
    [NonDefaultableStruct] public ImmutableArray<TDigit> Digits { get; }
        = Throw.IfStructArgDefault(Digits, nameof(Digits));
    #endregion

    #region IEnumerable
    private protected sealed override IEnumerator GetNonGenericEnumeratorInternal()
    {
        foreach (var d in Digits) yield return d;
    }

    /// <summary>
    /// Returns an <see cref="IEnumerator{T}"/> that can be used to iterate through the digits of the list.
    /// </summary>
    /// <returns></returns>
    public new ImmutableArray<TDigit>.Enumerator GetEnumerator() => Digits.GetEnumerator();

    IEnumerator<TDigit> IEnumerable<TDigit>.GetEnumerator()
    {
        foreach (var d in Digits) yield return d;
    }
    #endregion

    #region Equality
    /// <summary>
    /// Determines if this object is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public virtual bool Equals(DigitList<TDigit>? other) => other is not null && Digits.SequenceEqual(other.Digits);

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var d in Digits) hash.Add(d);
        return hash.ToHashCode();
    }
    #endregion

    /// <inheritdoc/>
    public sealed override string FormatAsList(
        string? separator = DefaultSeparator, string? digitFormat = DefaultFormat,
        IFormatProvider? digitFormatProvider = null)
        => string.Join(separator, Digits.Select(d => d.ToString(digitFormat, digitFormatProvider)));

    #region Splitting
    /// <inheritdoc cref="DigitList.SplitAtIndices(int[])"/>
    public new IReadOnlyList<DigitList<TDigit>> SplitAtIndices(params int[] indices)
        => SplitAtIndicesGenericInternal(indices);

    private protected sealed override IReadOnlyList<DigitList> SplitAtIndicesInternal(int[] indices)
        => SplitAtIndicesGenericInternal(indices);

    /// <inheritdoc cref="DigitList.SplitAtIndicesInternal(int[])"/>
    private protected abstract IReadOnlyList<DigitList<TDigit>> SplitAtIndicesGenericInternal(int[] indices);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected List<ImmutableArraySegment<TDigit>> SplitIntoArraySegmentsAtIndices(int[] indices)
    {
        var subArrays = new List<ImmutableArraySegment<TDigit>>();
        int lastIndex = 0;
        var superSegment = ImmutableArraySegment<TDigit>.Create(Digits);

        // Add all the subsegments indicated by the indices
        for (int i = 0; i < indices.Length; i++)
        {
            var currentIndex = indices[i];
            if (currentIndex < lastIndex)
            {
                if (currentIndex < 0) throw new IndexOutOfRangeException($"Indices must be non-negative.");
                else
                {
                    throw new ArgumentException(
                        "Indices passed in must be in (non-strictly) ascending order.", nameof(indices));
                }
            }
            else if (currentIndex >= Digits.Length)
            {
                throw new IndexOutOfRangeException($"Index {currentIndex} was out of range of the digit list.");
            }

            subArrays.Add(superSegment.Subsegment(lastIndex, currentIndex - lastIndex));

            lastIndex = currentIndex;
        }

        // Add the final subsegment
        subArrays.Add(superSegment.Subsegment(lastIndex, Digits.Length - lastIndex));

        return subArrays;
    }
    #endregion

    #region ToBuilder
    /// <inheritdoc cref="DigitList.ToBuilder"/>
    public new DigitList<TDigit>.Builder ToBuilder() => ToGenericBuilderInternal();

    private protected sealed override DigitList.Builder ToBuilderInternal() => ToGenericBuilderInternal();

    /// <inheritdoc cref="DigitList.ToBuilderInternal"/>
    private protected abstract DigitList<TDigit>.Builder ToGenericBuilderInternal();
    #endregion

    #region Leading Zero Removal
    private protected sealed override DigitList WithoutLeadingZeroesInternal() => WithoutLeadingZeroes();

    /// <inheritdoc cref="DigitList.WithoutLeadingZeroes"/>
    public new DigitList<TDigit> WithoutLeadingZeroes() => GenericWithoutLeadingZeroesInternal();

    /// <inheritdoc cref="DigitList.WithoutLeadingZeroesInternal"/>
    private protected abstract DigitList<TDigit> GenericWithoutLeadingZeroesInternal();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected ImmutableArray<TDigit> GetDigitsWithoutLeadingZeroes()
    {
        var numberToRemove = 0;
        foreach (var digit in Digits)
        {
            if (digit.Equals(default)) numberToRemove++;
            else break;
        }
        return numberToRemove == 0 ? Digits : Digits.RemoveRange(0, numberToRemove);
    }
    #endregion

    #region Deconstruction
    /// <summary>
    /// Deconstructs the current instance.
    /// </summary>
    /// <param name="Digits"></param>
    public void Deconstruct([NonDefaultableStruct] out ImmutableArray<TDigit> Digits) { Digits = this.Digits; }
    #endregion

    /// <summary>
    /// A builder for a <see cref="DigitList{TDigit}"/>.
    /// </summary>
    public abstract new class Builder : DigitList.Builder
    {
        /// <inheritdoc/>
        public sealed override int Count => ListBuilderInternal.Count;

        /// <summary>
        /// Gets the <see cref="ImmutableArray{T}.Builder"/> instance that will be used to create the digit list.
        /// </summary>
        private protected ImmutableArray<TDigit>.Builder ListBuilderInternal { get; }

        private protected Builder() : this(ImmutableArray.CreateBuilder<TDigit>()) { }

        private protected Builder(ImmutableArray<TDigit>.Builder ListBuilder) { this.ListBuilderInternal = ListBuilder; }

        /// <inheritdoc/>
        public sealed override void Reverse() => ListBuilderInternal.Reverse();

        /// <inheritdoc/>
        public sealed override void Clear() => ListBuilderInternal.Clear();

        private protected sealed override DigitList ToListInternal() => ToGenericListInternal();

        /// <inheritdoc cref="DigitList.Builder.ToListInternal"/>
        private protected abstract DigitList<TDigit> ToGenericListInternal();
    }
}

/// <summary>
/// A wrapper for a list of digits.
/// </summary>
/// <remarks>
/// This class is particularly useful in cases where a large number of digits need to be represented in an arbitrary
/// base.
/// If the base is small but the digit count is large, this class can save programmers lots of memory by allowing the
/// digits to be represented as a list of <see cref="byte"/> values rather than a list of a more memory-intensive type.
/// </remarks>
public abstract record class DigitList : IDigitList
{
    internal const string? DefaultFormat = "D";
    internal const string? DefaultSeparator = " ";

    /// <inheritdoc/>
    [NonNegative] public abstract int Count { get; }

    /// <summary>
    /// Gets a value representing the type of digit stored in this list.
    /// </summary>
    public abstract DigitType DigitType { get; }

    #region Index
    /// <inheritdoc/>
    [NonNegative] public BigUnsignedInteger this[[NonNegative] int index] => IndexInternal(index);

    /// <summary>
    /// Allows <see cref="this[int]"/> to be implemented on a smaller numeric type in subclasses, while also ensuring
    /// this class cannot be extended outside of this assembly.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    [return: NonNegative] private protected abstract BigUnsignedInteger IndexInternal([NonNegative] int index);
    #endregion

    #region IEnumerable
    IEnumerator IEnumerable.GetEnumerator() => GetNonGenericEnumeratorInternal();

    /// <summary>
    /// Allows <see cref="IEnumerable.GetEnumerator"/> to be implemented on a smaller generic numeric type, and also
    /// prevents this class from being extended outside of this assembly.
    /// </summary>
    /// <returns></returns>
    private protected abstract IEnumerator GetNonGenericEnumeratorInternal();

    /// <summary>
    /// Returns an enumerator that iterates through the digit list.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<BigUnsignedInteger> GetEnumerator() => GetEnumeratorInternal();

    /// <summary>
    /// Allows <see cref="GetEnumerator"/> to be implemented on a smaller generic numeric type, and also prevents this
    /// class from being extended outside of this assembly.
    /// </summary>
    /// <returns></returns>
    private protected abstract IEnumerator<BigUnsignedInteger> GetEnumeratorInternal();
    #endregion

    #region Leading Zero Removal
    /// <summary>
    /// Gets a digit list equivalent to the current instance with all leading zeroes removed.
    /// </summary>
    /// <returns></returns>
    public DigitList WithoutLeadingZeroes() => WithoutLeadingZeroesInternal();

    /// <summary>
    /// Allows the <see cref="WithoutLeadingZeroes"/> method return value to be further specified, while also ensuring
    /// that this type cannot be extended outside of this assembly.
    /// </summary>
    /// <returns></returns>
    private protected abstract DigitList WithoutLeadingZeroesInternal();
    #endregion

    #region Equivalence
    /// <summary>
    /// Determines if this list of digits is equivalent to the other list, ignoring the size of the representation of
    /// the digits themselves.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> was <see langword="null"/>.</exception>
    public abstract bool IsEquivalentTo(DigitList other);
    #endregion

    #region Splitting
    /// <summary>
    /// Splits the list into sublists, starting at the beginning of the list and using the indices passed in as
    /// end points of one or more sublists, finishing at the end of the list.
    /// </summary>
    /// <param name="indices">
    /// A list of indices to split the list on.
    /// <para/>
    /// The beginning and end of the list should be omitted - for example, passing in 3 and 5 for the indices will
    /// return sub-segments with the ranges [..3], [3..5], [5..].
    /// </param>
    /// <returns>
    /// A list of sublists based on the indices passed in, or a list containing the current instance if no indices
    /// were passed in.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="indices"/> was <see langword="null"/>.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// One of the supplied indices was negative or otherwise out of range of the list.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The list of indices was not increasing.
    /// </exception>
    public IReadOnlyList<DigitList> SplitAtIndices(params int[] indices) => SplitAtIndicesInternal(indices);

    /// <summary>
    /// Allows <see cref="SplitAtIndices(int[])"/> to be implemented on more specific types in subclasses.
    /// </summary>
    /// <param name="indices"></param>
    /// <returns></returns>
    private protected abstract IReadOnlyList<DigitList> SplitAtIndicesInternal(int[] indices);
    #endregion

    #region Factory
    /// <inheritdoc cref="EmptyFromBaseSize(ushort)"/>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Base"/> was negative.</exception>
    public static DigitList EmptyFromBaseSize([GreaterThanOrEqualToInteger(2)] BigUnsignedInteger Base)
        => EmptyFromDigitType(Bases.ShortestDigitType(Throw.IfArgNegative(Base, nameof(Base))));

    /// <inheritdoc cref="EmptyFromBaseSize(ushort)"/>
    public static DigitList EmptyFromBaseSize([GreaterThanOrEqualToInteger(2)] ulong Base)
        => EmptyFromDigitType(Bases.ShortestDigitType(Base));

    /// <inheritdoc cref="EmptyFromBaseSize(ushort)"/>
    public static DigitList EmptyFromBaseSize([GreaterThanOrEqualToInteger(2)] uint Base)
        => EmptyFromDigitType(Bases.ShortestDigitType(Base));

    /// <summary>
    /// Gets a new empty instance of this class capable of handling the smallest possible integral representation of
    /// digits in the given base.
    /// </summary>
    /// <remarks>
    /// Attempting to add digits larger than the base to the result of a call to <see cref="ToBuilder"/> on the return
    /// value of this method may cause an exception.
    /// </remarks>
    /// <param name="Base"></param>
    /// <returns></returns>
    public static DigitList EmptyFromBaseSize([GreaterThanOrEqualToInteger(2)] ushort Base)
        => EmptyFromDigitType(Bases.ShortestDigitType(Base));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static DigitList EmptyFromDigitType(DigitType Base) => Base.Value switch
    {
        DigitType.Values.BigUnsignedInteger => BigUnsignedIntegerDigitList.Empty,
        DigitType.Values.ULong => ULongDigitList.Empty,
        DigitType.Values.UInt => UIntDigitList.Empty,
        DigitType.Values.UShort => UShortDigitList.Empty,
        _ => ByteDigitList.Empty,
    };
    #endregion

    #region Builder
    /// <summary>
    /// Gets a builder object that can be used to get a modified copy of this list.
    /// </summary>
    /// <returns></returns>
    public Builder ToBuilder() => ToBuilderInternal();

    /// <summary>
    /// Allows the <see cref="ToBuilder"/> return type to be further specified in subclasses, while also preventing
    /// this class from being extended outside of this assembly.
    /// </summary>
    /// <returns></returns>
    private protected abstract Builder ToBuilderInternal();
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public sealed override string ToString() => ToString(digitFormatProvider: null);

    /// <summary>
    /// Gets a string that represents the current instance with the digits formatted using the specified digit format
    /// and separated using the specified separator string.
    /// </summary>
    /// <param name="separator">
    /// A separator to use to separate the digits when formatting.
    /// -or-
    /// a <see langword="null"/> reference to not include separators.
    /// </param>
    /// <param name="digitFormat">
    /// The format to use to format the digits.
    /// -or-
    /// A <see langword="null"/> reference to use the default format for the type of digits being represented.
    /// </param>
    /// <param name="digitFormatProvider">
    /// The provider to use to format the value.
    /// -or-
    /// a <see langword="null"/> reference to obtain the numeric format information from the current locale setting
    /// of the operating system.
    /// </param>
    /// <returns>The formatted value with the digits written in the specified format.</returns>
    /// <exception cref="FormatException">
    /// <paramref name="digitFormat"/> is not a supported numeric format for integral types.
    /// </exception>
    public string ToString(
        string? separator = DefaultSeparator, string? digitFormat = DefaultFormat,
        IFormatProvider? digitFormatProvider = null)
        => $"{{ {FormatAsList(separator: separator, digitFormat: digitFormat, digitFormatProvider)} }}";

    /// <summary>
    /// Formats the value of the current instance as a list of digits without braces.
    /// </summary>
    /// <param name="separator">
    /// A separator to use to separate the digits when formatting.
    /// -or-
    /// a <see langword="null"/> reference to not include separators.
    /// </param>
    /// <param name="digitFormat">
    /// The format to use to format the digits.
    /// -or-
    /// A <see langword="null"/> reference to use the default format for the type of digits being represented.
    /// </param>
    /// <param name="digitFormatProvider">
    /// The provider to use to format the value.
    /// -or-
    /// a <see langword="null"/> reference to obtain the numeric format information from the current locale setting
    /// of the operating system.
    /// </param>
    /// <returns>The formatted value with the digits written in the specified format.</returns>
    /// <exception cref="FormatException">
    /// <paramref name="digitFormat"/> is not a supported numeric format for integral types.
    /// </exception>
    public abstract string FormatAsList(
        string? separator = DefaultSeparator, string? digitFormat = DefaultFormat,
        IFormatProvider? digitFormatProvider = null);
    #endregion

    /// <summary>
    /// A builder for a <see cref="DigitList"/>.
    /// </summary>
    /// <remarks>
    /// Instances of this class store digits that can be used to construct lists of digits of various sizes; however,
    /// it should be noted that while methods for adding large integer types as digits (such as
    /// <see cref="BigUnsignedInteger"/>) are provided to make programming with the builders easier, these methods will throw
    /// exceptions if there is an overflow.
    /// </remarks>
    public abstract class Builder
    {
        /// <summary>
        /// Gets the number of digits currently in the builder.
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// Prevents this class from being extended outside of this assembly.
        /// </summary>
        private protected Builder() { }

        /// <inheritdoc cref="Add(ulong)"/>
        public abstract void Add(byte Digit);

        /// <inheritdoc cref="Add(ulong)"/>
        public abstract void Add(ushort Digit);

        /// <inheritdoc cref="Add(ulong)"/>
        public abstract void Add(uint Digit);

        /// <summary>
        /// Adds a digit to the list.
        /// </summary>
        /// <param name="Digit"></param>
        public abstract void Add(ulong Digit);

        /// <inheritdoc cref="Add(ulong)"/>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="Digit"/> was negative.</exception>
        public abstract void Add([NonNegative] BigUnsignedInteger Digit);

        /// <summary>
        /// Reverses the order of digits in the builder.
        /// </summary>
        public abstract void Reverse();

        /// <summary>
        /// Removes all digits from the builder.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Gets the <see cref="DigitList"/> represented by the current state of the builder.
        /// </summary>
        /// <returns></returns>
        public DigitList ToList() => ToListInternal();

        /// <summary>
        /// Allows the <see cref="ToList"/> method to be hidden in derived classes so that the return type matches
        /// the type of list being built.
        /// </summary>
        /// <returns></returns>
        private protected abstract DigitList ToListInternal();

        #region Factory Methods
        /// <inheritdoc cref="NewFromBaseSize(ushort)"/>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="Base"/> was negative.</exception>
        public static Builder NewFromBaseSize([GreaterThanOrEqualToInteger(2)] BigUnsignedInteger Base)
            => NewFromDigitType(Bases.ShortestDigitType(Throw.IfArgNegative(Base, nameof(Base))));

        /// <inheritdoc cref="NewFromBaseSize(ushort)"/>
        public static Builder NewFromBaseSize([GreaterThanOrEqualToInteger(2)] ulong Base)
            => NewFromDigitType(Bases.ShortestDigitType(Base));

        /// <inheritdoc cref="NewFromBaseSize(ushort)"/>
        public static Builder NewFromBaseSize([GreaterThanOrEqualToInteger(2)] uint Base)
            => NewFromDigitType(Bases.ShortestDigitType(Base));

        /// <summary>
        /// Gets a new instance of this class capable of handling the smallest possible integral representation of
        /// digits in the given base.
        /// </summary>
        /// <remarks>
        /// Attempting to add digits larger than the base to the result of this method may cause an exception.
        /// </remarks>
        /// <param name="Base"></param>
        /// <returns></returns>
        public static Builder NewFromBaseSize([GreaterThanOrEqualToInteger(2)] ushort Base)
            => NewFromDigitType(Bases.ShortestDigitType(Base));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Builder NewFromDigitType(DigitType minSize)
            => minSize.Value switch
            {
                DigitType.Values.BigUnsignedInteger => new BigUnsignedIntegerDigitList.Builder(),
                DigitType.Values.ULong => new ULongDigitList.Builder(),
                DigitType.Values.UInt => new UIntDigitList.Builder(),
                DigitType.Values.UShort => new UShortDigitList.Builder(),
                _ => new ByteDigitList.Builder(),
            };
        #endregion
    }
}
#endregion
#endregion

#region Interfaces
/// <summary>
/// An interface for types wrapping a list of <see cref="byte"/> digits.
/// </summary>
public interface IByteDigitList : IUShortDigitList, IEnumerable<byte>
{
    /// <inheritdoc cref="IDigitList.this[int]"/>
    public new byte this[[NonNegative] int index] { get; }
}

/// <summary>
/// An interface for types wrapping a list of <see cref="ushort"/> digits.
/// </summary>
public interface IUShortDigitList : IUIntDigitList, IEnumerable<ushort>
{
    /// <inheritdoc cref="IDigitList.this[int]"/>
    public new ushort this[[NonNegative] int index] { get; }
}

/// <summary>
/// An interface for types wrapping a list of <see cref="uint"/> digits.
/// </summary>
public interface IUIntDigitList : IULongDigitList, IEnumerable<uint>
{
    /// <inheritdoc cref="IDigitList.this[int]"/>
    public new uint this[[NonNegative] int index] { get; }
}

/// <summary>
/// An interface for types wrapping a list of <see cref="ulong"/> digits.
/// </summary>
public interface IULongDigitList : IDigitList, IEnumerable<ulong>
{
    /// <inheritdoc cref="IDigitList.this[int]"/>
    public new ulong this[[NonNegative] int index] { get; }
}

/// <summary>
/// An interface for types wrapping a list of digits.
/// </summary>
public interface IDigitList : IEnumerable<BigUnsignedInteger>
{
    /// <summary>
    /// Gets the number of digits in the list.
    /// </summary>
    [NonNegative] public int Count { get; }

    /// <summary>
    /// Gets the digit at the specified index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> was out of range.</exception>
    [NonNegative] public BigUnsignedInteger this[[NonNegative] int index] { get; }
}
#endregion
