using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.YAMLLinter
{
	[TestFixture]
	internal sealed class YAMLLinterTest
	{
		[Test]
		public async Task RunLinterTest()
		{
			Assert.That(await Program.RunLinter(), Is.EqualTo(0)); //returns 0 if no errors
		}
	}
}
