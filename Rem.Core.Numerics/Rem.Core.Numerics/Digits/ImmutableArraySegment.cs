using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics.Digits;

/// <summary>
/// Represents a segment of an immutable array.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct ImmutableArraySegment<T> : IReadOnlyList<T>, IDefaultableStruct
{
    /// <inheritdoc/>
    public T this[int index] => Array[Offset + index];

    /// <summary>
    /// Gets the index offset of the segment.
    /// </summary>
    public int Offset { get; }

    /// <summary>
    /// Gets the count of the segment.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Gets the array this instance is a segment of.
    /// </summary>
    public ImmutableArray<T> Array { get; }

    /// <inheritdoc/>
    public bool IsDefault => Array.IsDefault;

    /// <summary>
    /// Constructs a new instance of the <see cref="ImmutableArraySegment{T}"/> class from the immutable array
    /// passed in with the specified start and count.
    /// </summary>
    /// <param name="Array"></param>
    /// <param name="Offset"></param>
    /// <param name="Count"></param>
    private ImmutableArraySegment(ImmutableArray<T> Array, int Offset, int Count)
    {
        this.Array = Array;
        this.Offset = Offset;
        this.Count = Count;
    }

    /// <summary>
    /// Creates a new <see cref="ImmutableArraySegment{T}"/> containing the entire array passed in.
    /// </summary>
    /// <param name="Array"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException"><paramref name="Array"/> was default.</exception>
    [return: NonDefaultableStruct]
    public static ImmutableArraySegment<T> Create([NonDefaultableStruct] ImmutableArray<T> Array)
        => new(Array, 0, Throw.IfStructArgDefault(Array, nameof(Array)).Length);

    /// <summary>
    /// Creates a new <see cref="ImmutableArraySegment{T}"/> by subsegmenting the current instance.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DefaultInstanceException">
    /// This method was called on the default.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The offset and count could cause the result to exceed the bounds of the original segment.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Either <paramref name="Offset"/> or <paramref name="Count"/> was negative.
    /// </exception>
    [DoesNotReturnIfInstanceDefault]
    [return: NonDefaultableStruct]
    public ImmutableArraySegment<T> Subsegment([NonNegative] int Offset, [NonNegative] int Count)
    {
        Throw.IfCurrentInstanceDefault(this);
        Throw.IfArgNegative(Offset, nameof(Offset));
        Throw.IfArgNegative(Count, nameof(Count));

        var newOffset = this.Offset + Offset;

        if (newOffset + Count > this.Offset + this.Count)
        {
            throw new InvalidOperationException(
                "Cannot construct an immutable array sub-segment that exceeds the bounds of the original segment.");
        }

        return new(Array, newOffset, Count);
    }

    /// <summary>
    /// Gets a new <see cref="ImmutableArray{T}"/> with contents copied from this segment.
    /// </summary>
    /// <returns></returns>
    public ImmutableArray<T> ToImmutableArray()
    {
        var builder = ImmutableArray.CreateBuilder<T>();
        var enumerator = GetEnumerator();
        while (enumerator.MoveNext()) builder.Add(enumerator.Current);
        return builder.ToImmutable();
    }

    /// <summary>
    /// Gets an object that can be used to iterate through the segment.
    /// </summary>
    /// <returns></returns>
    public Enumerator GetEnumerator() => new(in this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumeratorObject();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumeratorObject();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IEnumerator<T> GetEnumeratorObject()
    {
        var finalIndex = Offset + Count - 1;
        for (int i = Offset; i <= finalIndex; i++)
        {
            yield return Array[i];
        }
    }

    /// <summary>
    /// An enumerator for the <see cref="ImmutableArraySegment{T}"/> struct.
    /// </summary>
    public struct Enumerator
    {
        private const int BeforeStartIndex = -1;
        private const int AfterEndIndex = -2;

        private ImmutableArray<T> Array { get; }
        private int Offset { get; }
        private int Count { get; }

        /// <summary>
        /// Gets the current item in the enumeration.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The enumeration has not yet started, or is already finished.
        /// </exception>
        public T Current => CurrentIndex switch
        {
            BeforeStartIndex => throw new InvalidOperationException("Enumeration has not started. Call MoveNext."),
            AfterEndIndex => throw new InvalidOperationException("Enumeration has already finished."),
            _ => Array[CurrentIndex],
        };

        private int CurrentIndex { get; set; }

        internal Enumerator(in ImmutableArraySegment<T> segment)
        {
            Array = segment.Array;
            Offset = segment.Offset;
            Count = segment.Count;
            CurrentIndex = BeforeStartIndex;
        }

        /// <summary>
        /// Advances to the next item in the enumeration.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            switch (CurrentIndex)
            {
                case BeforeStartIndex:
                    if (Count == 0)
                    {
                        CurrentIndex = AfterEndIndex;
                        return false;
                    }
                    else
                    {
                        CurrentIndex = Offset;
                        return true;
                    }

                case AfterEndIndex: return false;

                default:
                    CurrentIndex++;
                    if (CurrentIndex == Offset + Count)
                    {
                        CurrentIndex = AfterEndIndex;
                        return false;
                    }
                    else return true;
            }
        }

        /// <summary>
        /// Resets the enumeration.
        /// </summary>
        public void Reset()
        {
            CurrentIndex = BeforeStartIndex;
        }
    }
}
