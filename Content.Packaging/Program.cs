using Content.Packaging;
using Robust.Packaging;
using System;
using System.IO;
using System.Linq;

IPackageLogger logger = new PackageLoggerConsole();

var selectedPlatformRids = Packager.PlatformRidsDefault; //get value from CL args with default as fallback

var selectedPlatforms = Packager.AllPlatforms
	.Where(p => selectedPlatformRids.Contains(p.Rid));

if (!selectedPlatforms.Any())
{
	selectedPlatforms = Packager.DefaultPlatforms;
}

//logger.Info("Clearing old build.");
//if (Directory.Exists("../../../../bin"))
//{
//	Directory.Delete("../../../../bin", recursive: true);
//	Directory.Delete("../../../../RobustToolbox/bin", recursive: true);
//}

logger.Info("Clearing old release.");
if (Directory.Exists(Packager.ReleaseDirectory))
	Directory.Delete(Packager.ReleaseDirectory, recursive: true);

Directory.CreateDirectory(Packager.ReleaseDirectory); //will crash if this folder does not exist, when trying to make zip file

foreach (var platform in selectedPlatforms)
{
	foreach (var project in Packager.AllReleasedProjects)
	{
		await Packager.BuildPlatform(platform, project);
		await Packager.PublishPlatform(platform, project);
	}
	//await Packager.PackageServer(platform);
}
Console.ReadLine();