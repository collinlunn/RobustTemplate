using Content.Packaging;
using Robust.Packaging;
using System;
using System.IO;
using System.Linq;

//placeholder for CLI args
var selectedPlatforms = Packager.DefaultPlatforms;
var republish = true;
Directory.SetCurrentDirectory("../../../..");

IPackageLogger logger = new PackageLoggerConsole();

if (republish && Directory.Exists("bin"))
{
	logger.Info("Clearing old build.");
	Directory.Delete(Packager.ContentBin, recursive: true);
	Directory.Delete(Packager.EngineBin, recursive: true);
}
if (Directory.Exists(Packager.ReleaseDir))
{
	logger.Info("Clearing old release.");
	Directory.Delete(Packager.ReleaseDir, recursive: true);
}
Directory.CreateDirectory(Packager.ReleaseDir); //crashes when making zip if this folder is not found

foreach (var platform in selectedPlatforms)
{
	if (republish)
	{
		foreach (var project in Packager.AllReleasedProjects)
		{
			await Packager.BuildPlatform(platform, project, logger);
			await Packager.PublishPlatform(platform, project, logger);
		}
	}
	await Packager.PackageClientAcz(platform, logger);
	await Packager.PackageClientStandalone(platform, logger);
	await Packager.PackageServer(platform, logger);
}
logger.Info("Packaging script completed.");