using System;

namespace Rem.Core.Numerics.Test;

/// <summary>
/// Tests the <see cref="RoundingStrategy"/> struct and its component structs.
/// </summary>
[TestClass]
public class RoundingStrategyTest
{
    /// <summary>
    /// Tests try methods of the <see cref="RoundingStrategy"/> class.
    /// </summary>
    [TestMethod]
    public void TestTry()
    {
        RoundingStrategy toNearestToEven = ToNearestRoundingStrategy.ToEven,
                         directedToZero = DirectedRoundingStrategy.ToZero;

#pragma warning disable IDE0018 // Don't inline variable declaration - cleaner to do it here
        ToNearestRoundingStrategy actualToNearest;
        DirectedRoundingStrategy actualDirected;
#pragma warning restore IDE0018

        Assert.That.Succeeds(
            toNearestToEven.TryGetToNearest(out actualToNearest),
            ToNearestRoundingStrategy.ToEven, ref actualToNearest);
        Assert.IsFalse(toNearestToEven.TryGetDirected(out _));
        Assert.That.Succeeds(
            toNearestToEven.TryGetToNearest(out actualToNearest, out _),
            ToNearestRoundingStrategy.ToEven, ref actualToNearest);
        Assert.IsFalse(toNearestToEven.TryGetDirected(out _, out _));

        Assert.That.Succeeds(
            directedToZero.TryGetDirected(out actualDirected),
            DirectedRoundingStrategy.ToZero, ref actualDirected);
        Assert.IsFalse(directedToZero.TryGetToNearest(out _));
        Assert.That.Succeeds(
            directedToZero.TryGetDirected(out actualDirected, out _),
            DirectedRoundingStrategy.ToZero, ref actualDirected);
        Assert.IsFalse(directedToZero.TryGetToNearest(out _, out _));
    }
}
