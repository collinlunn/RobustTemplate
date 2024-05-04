using Robust.Packaging;
using Robust.Packaging.AssetProcessing;
using Robust.Packaging.AssetProcessing.Passes;
using Robust.Packaging.Utility;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Content.Packaging
{
	public static partial class Packager
	{
		public static async Task BuildPlatform(PlatformReg platform, string project, IPackageLogger logger)
		{
			logger.Info($"Started building {project} for {platform.Rid}.");

			await ProcessHelpers.RunCheck(new ProcessStartInfo
			{
				FileName = "dotnet",
				ArgumentList =
				{
					"build",
					project,
					"--runtime", platform.Rid,
					"--self-contained", "true",
					"-c", "Release",
					"--nologo",
					"-v", "q",
					"-p", "FullRelease=True"
				}
			});
			logger.Info($"Finished building {project} for {platform.Rid}");
		}

		public static async Task PublishPlatform(PlatformReg platform, string project, IPackageLogger logger)
		{
			logger.Info($"Started publishing {project} for {platform.Rid}.");

			await ProcessHelpers.RunCheck(new ProcessStartInfo
			{
				FileName = "dotnet",
				ArgumentList =
				{
					"publish",
					project,
					"--runtime", platform.Rid,
					"--self-contained", "true",
					"-c", "Release",
					"--nologo",
					"--no-build",
					"-v", "m",
					"-p", "FullRelease=True"
				}
			});
			logger.Info($"Finished publishing {project} for {platform.Rid}");
		}

		private static FileStream MakeZip(PlatformReg platform, string name)
		{
			return File.Open(
				name,
				FileMode.Create,
				FileAccess.ReadWrite);
		}

		public static async Task PackageClientAcz(PlatformReg platform, IPackageLogger logger)
		{
			using var zip = MakeZip(platform, ClientAczName(platform));
			using var zipArchive = new ZipArchive(zip, ZipArchiveMode.Update);
			var zipWriter = new AssetPassZipWriter(zipArchive);
			await PassClientAcz(platform, zipWriter, logger, default);
			await zipWriter.FinishedTask;
		}

		public static async Task PackageClientStandalone(PlatformReg platform, IPackageLogger logger)
		{
			using var zip = MakeZip(platform, ClientZipName(platform));
			using var zipArchive = new ZipArchive(zip, ZipArchiveMode.Update);
			var zipWriter = new AssetPassZipWriter(zipArchive);

			await PassClientStandalone(platform, zipWriter, logger, default);
			await zipWriter.FinishedTask;
		}

		public static async Task PackageServer(PlatformReg platform, IPackageLogger logger)
		{
			using var zip = MakeZip(platform, ServerZipName(platform));
			using var zipArchive = new ZipArchive(zip, ZipArchiveMode.Update);
			var zipWriter = new AssetPassZipWriter(zipArchive);

			await PassServer(platform, zipWriter, logger, default);
			await zipWriter.FinishedTask;
		}

		public static async Task PassClientAcz(
			PlatformReg platform,
			AssetPass pass,
			IPackageLogger logger,
			CancellationToken cancel)
		{
			var graph = new RobustClientAssetGraph();
			pass.Dependencies.Add(new AssetPassDependency(graph.Output.Name));
			AssetGraph.CalculateGraph(graph.AllPasses.Append(pass).ToArray(), logger);

			var inputPass = graph.Input;

			await RobustSharedPackaging.WriteContentAssemblies(
				inputPass,
				ContentDir,
				ContentClientBin(platform),
				ClientContentAssemblies,
				cancel: cancel);

			await RobustClientPackaging.WriteClientResources(ContentDir, pass, cancel);
			await RobustClientPackaging.WriteClientResources(EngineDir, inputPass, cancel);

			inputPass.InjectFinished();
		}

		private static async Task PassClientStandalone(
			PlatformReg platform,
			AssetPass pass,
			IPackageLogger logger,
			CancellationToken cancel)
		{
			var graph = new StandaloneClientAssetGraph();
			pass.Dependencies.Add(new AssetPassDependency(graph.Output.Name));
			AssetGraph.CalculateGraph(graph.AllPasses.Append(pass).ToArray(), logger);

			var inputPassCore = graph.InputCore;
			await RobustSharedPackaging.DoResourceCopy(
				EngineClientBin(platform),
				inputPassCore,
				BinSkipFolders,
				cancel: cancel);

			var inputPassResources = graph.InputResources;
			await RobustSharedPackaging.WriteContentAssemblies(
				inputPassResources,
				ContentDir,
				ContentClientBin(platform),
				ClientContentAssemblies,
				cancel: cancel);
			await RobustClientPackaging.WriteClientResources(ContentDir, inputPassResources, cancel);
			await RobustClientPackaging.WriteClientResources(EngineDir, inputPassResources, cancel);

			inputPassCore.InjectFinished();
			inputPassResources.InjectFinished();
		}

		private static async Task PassServer(
			PlatformReg platform,
			AssetPass pass,
			IPackageLogger logger,
			CancellationToken cancel)
		{
			var graph = new RobustServerAssetGraph();
			pass.Dependencies.Add(new AssetPassDependency(graph.Output.Name));
			AssetGraph.CalculateGraph(graph.AllPasses.Append(pass).ToList());

			var inputPassCore = graph.InputCore;
			await RobustSharedPackaging.DoResourceCopy(
				EngineServerBin(platform),
				inputPassCore,
				BinSkipFolders,
				cancel: cancel);

			var inputPassResources = graph.InputResources;
			await RobustSharedPackaging.WriteContentAssemblies(
				inputPassResources,
				ContentDir,
				ContentServerBin(platform),
				ServerContentAssemblies,
				cancel: cancel);
			await RobustServerPackaging.WriteServerResources(ContentDir, inputPassResources, cancel);

			inputPassCore.InjectFileFromDisk(ServerAczName, ClientAczName(platform));

			inputPassCore.InjectFinished();
			inputPassResources.InjectFinished();
		}
	}
}
