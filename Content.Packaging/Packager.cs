using Robust.Packaging;
using Robust.Packaging.AssetProcessing;
using Robust.Packaging.AssetProcessing.Passes;
using Robust.Packaging.Utility;
using System.Diagnostics;
using System.IO.Compression;

namespace Content.Packaging
{
	//TODO double check async code, add stopwatches, addl loggers, arg parse, fix directory root, put resources and code files in zip
	public static partial class Packager
	{
		public static async Task BuildPlatform(PlatformReg platform, string project)
		{
			await ProcessHelpers.RunCheck(new ProcessStartInfo
			{
				FileName = "dotnet",
				ArgumentList =
				{
					"build",
					project,
					"--runtime", platform.Rid,
					"--no-self-contained", //should this be self contained?
					"-c", "Release",
					"--nologo",
					"-v", "q",
					"-p", "FullRelease=True"
				}
			});
		}

		public static async Task PublishPlatform(PlatformReg platform, string project)
		{
			await ProcessHelpers.RunCheck(new ProcessStartInfo
			{
				FileName = "dotnet",
				ArgumentList =
				{
					"publish",
					project,
					"--runtime", platform.Rid,
					"--no-self-contained", //should this be self contained?
					"-c", "Release",
					"--nologo",
					"-v", "m",
					"-p", "FullRelease=True"
				}
			});
		}

		public static async Task PackageServer(PlatformReg platform)
		{
			await using var zipFile =
				File.Open(Path.Combine(ReleaseDirectory, $"RobustTemplate.Server_{platform.Rid}.zip"), FileMode.Create, FileAccess.ReadWrite);
			using var zip = new ZipArchive(zipFile, ZipArchiveMode.Update);
			var writer = new AssetPassZipWriter(zip);

			await WriteServerResources(platform, writer);
			await writer.FinishedTask;
		}

		private static async Task WriteServerResources(PlatformReg platform, AssetPass pass)
		{
			var contentDir = "";
			CancellationToken cancel = default;

			var graph = new RobustClientAssetGraph();
			var inputPass = graph.Input;
			var passes = graph.AllPasses.ToList();
			pass.Dependencies.Add(new AssetPassDependency(graph.Output.Name));
			passes.Add(pass);
			AssetGraph.CalculateGraph(passes, null); //todo insert logger

			await RobustSharedPackaging.DoResourceCopy(
				$"../../../../RobustToolbox/bin/Server/{platform.Rid}/publish",
				inputPass,
				BinSkipFolders,
				cancel: cancel);

			//await RobustSharedPackaging.WriteContentAssemblies(
			//	inputPass,
			//	contentDir,
			//	"../../../../Content.Server",
			//	ServerContentAssemblies,
			//	"../../../../Resources/Assemblies",
			//	cancel);

			//await RobustServerPackaging.WriteServerResources(contentDir, inputPass, cancel);

			//inputPass.InjectFileFromDisk("Content.Client.zip", Path.Combine("release", "SS14.Client.zip"));
			inputPass.InjectFinished();
		}
	}

//	package_server(runtime: str, package_acz: bool) -> None:
//	copy_dir_into_zip(f"../RobustToolbox/bin/Server/{runtime}/publish", ".", server_zip, SERVER_BIN_SKIP_FOLDERS)
//	copy_content_assemblies(f"../bin/Content.Server/{runtime}/publish", "Resources/Assemblies", SERVER_CONTENT_ASSEMBLIES, server_zip)
//	copy_dir_into_zip("../Resources", "Resources", server_zip, SERVER_RES_SKIP_FOLDERS, SERVER_RES_SKIP_FILES)
//	copy_dir_into_zip("../RobustToolbox/Resources", "Resources", server_zip, SERVER_RES_SKIP_FOLDERS, SERVER_RES_SKIP_FILES)
//  if package_acz:
//      acz = package_client_acz(runtime)
//		server_zip.write(acz.filename, "Content.Client.zip")

//  package_client_acz(runtime: str) -> ZipFile:
//	copy_content_assemblies(f"../bin/Content.Client/{runtime}/publish", "Assemblies", CLIENT_CONTENT_ASSEMBLIES, acz_zip)
//	copy_dir_into_zip("../Resources", ".", acz_zip, CLIENT_RES_SKIP_FOLDERS, CLIENT_RES_SKIP_FILES)
//	copy_dir_into_zip("../RobustToolbox/Resources", ".", acz_zip, CLIENT_RES_SKIP_FOLDERS, CLIENT_RES_SKIP_FILES)

//  package_client_standalone(runtime: str) -> None:
//	copy_dir_into_zip(f"../RobustToolbox/bin/Client/{runtime}/publish", ".", client_zip, CLIENT_BIN_SKIP_FOLDERS)
//	copy_content_assemblies(f"../bin/Content.Client/{runtime}/publish", "Resources/Assemblies", CLIENT_CONTENT_ASSEMBLIES, client_zip)
//	copy_dir_into_zip("../Resources", "Resources", client_zip, CLIENT_RES_SKIP_FOLDERS, CLIENT_RES_SKIP_FILES)
//	copy_dir_into_zip("../RobustToolbox/Resources", "Resources", client_zip, CLIENT_RES_SKIP_FOLDERS, CLIENT_RES_SKIP_FILES)
}
