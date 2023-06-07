using System;
using System.Numerics;

namespace Rem.Core.Numerics.Test;

public partial class MathsTest
{
    /// <summary>
    /// Tests a few <see cref="BigInteger"/> square root values for very large integers.
    /// </summary>
    [TestMethod]
    public void TestIntegerSqrt_LargeValues()
    {
        Assert.AreEqual(
            expected: BigInteger.Parse("4835703278458516698824703"), // Too large for a ulong
            actual: Maths.IntegerSqrt(BigInteger.Parse("23384026197294446691258957323460528314494919639040")));
    }
}
