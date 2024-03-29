﻿using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Utility;
using Robust.Shared.Timing;
using System.Collections.Generic;
using System.Linq;

namespace Content.Tests.IntegrationTests
{
	[TestFixture]
	public sealed class YAMLLinterTest
	{
		[Test]
		public async Task RunLinterTest()
		{
			Assert.That(await RunLinter(), Is.EqualTo(0)); //returns 0 if no errors
		}

		public static async Task<int> RunLinter()
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var errors = await RunValidation();

			if (errors.Count == 0)
			{
				Console.WriteLine($"No errors found in {(int)stopwatch.Elapsed.TotalMilliseconds} ms.");
				return 0;
			}

			foreach (var (file, errorHashset) in errors)
			{
				foreach (var errorNode in errorHashset)
				{
					Console.WriteLine($"::error file={file},line={errorNode.Node.Start.Line},col={errorNode.Node.Start.Column}::{file}({errorNode.Node.Start.Line},{errorNode.Node.Start.Column})  {errorNode.ErrorReason}");
				}
			}

			Console.WriteLine($"{errors.Count} errors found in {(int)stopwatch.Elapsed.TotalMilliseconds} ms.");
			return -1;
		}

		private static async Task<Dictionary<string, HashSet<ErrorNode>>> ValidateClient()
		{
			var settings = new TestProcessSettings();
			var client = await TestProcessManager.GetTestClient(settings);

			var cPrototypeManager = client.ResolveDependency<IPrototypeManager>();
			var clientErrors = new Dictionary<string, HashSet<ErrorNode>>();

			await client.WaitPost(() =>
			{
				clientErrors = cPrototypeManager.ValidateDirectory(new ResPath("/Prototypes"));
			});

			client.Dispose();
			return clientErrors;
		}

		private static async Task<Dictionary<string, HashSet<ErrorNode>>> ValidateServer()
		{
			var settings = new TestProcessSettings();
			var server = await TestProcessManager.GetTestServer(settings);

			var sPrototypeManager = server.ResolveDependency<IPrototypeManager>();
			var serverErrors = new Dictionary<string, HashSet<ErrorNode>>();

			await server.WaitPost(() =>
			{
				serverErrors = sPrototypeManager.ValidateDirectory(new ResPath("/Prototypes"));
			});

			server.Dispose();
			return serverErrors;
		}

		public static async Task<Dictionary<string, HashSet<ErrorNode>>> RunValidation()
		{
			var allErrors = new Dictionary<string, HashSet<ErrorNode>>();

			var serverErrors = await ValidateServer();
			var clientErrors = await ValidateClient();

			foreach (var (key, val) in serverErrors)
			{
				// Include all server errors marked as always relevant
				var newErrors = val.Where(n => n.AlwaysRelevant).ToHashSet();

				// We include sometimes-relevant errors if they exist both for the client & server
				if (clientErrors.TryGetValue(key, out var clientVal))
					newErrors.UnionWith(val.Intersect(clientVal));

				if (newErrors.Count != 0)
					allErrors[key] = newErrors;
			}

			// Finally add any always-relevant client errors.
			foreach (var (key, val) in clientErrors)
			{
				var newErrors = val.Where(n => n.AlwaysRelevant).ToHashSet();
				if (newErrors.Count == 0)
					continue;

				if (allErrors.TryGetValue(key, out var errors))
					errors.UnionWith(val.Where(n => n.AlwaysRelevant));
				else
					allErrors[key] = newErrors;
			}

			return allErrors;
		}
	}
}
