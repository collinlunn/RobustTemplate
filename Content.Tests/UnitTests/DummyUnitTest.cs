using System;
using NUnit.Framework;

namespace Content.Tests.UnitTests.Dummy
{
    [TestFixture] 
	//[TestOf(typeof(DummyComponent))]
	public sealed class DummyUnitTest
    {
        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 4)]
        public void DoubleNumber(int value, int expected)
        {
            Assert.That(value*2, Is.EqualTo(expected));
        }

        [Test]
		[TestCase(0, 0, true)]
		[TestCase(1, 0, false)]
        public void AreEqual(int a, int b, bool expected)
        {
            Assert.That(a==b, Is.EqualTo(expected));
        }
    }
}
