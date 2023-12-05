using System.Collections.Generic;
using System.Linq;

namespace Content.Packaging
{
	public static partial class Packager
	{
		public readonly record struct PlatformReg(string Rid, string TargetOs, bool BuildByDefault);

		public static readonly IReadOnlyList<PlatformReg> AllPlatforms = new List<PlatformReg>()
		{
			new ("win-x64", "Windows", true),
			new ("linux-x64", "Linux", false),
			new ("linux-arm64", "Linux", false),
			new ("osx-x64", "MacOS", false),
			// Non-default platforms (i.e. for Watchdog Git)
			new ("win-x86", "Windows", false),
			new ("linux-x86", "Linux", false),
			new ("linux-arm", "Linux", false),
		};

		public static IReadOnlyList<PlatformReg> DefaultPlatforms => AllPlatforms
			.Where(o => o.BuildByDefault)
			.ToList();

		public static readonly IReadOnlyList<string> AllReleasedProjects = new List<string>()
		{
			"RobustToolbox/Robust.Client/Robust.Client.csproj",
			"Content.Client/Content.Client.csproj",

			"RobustToolbox/Robust.Server/Robust.Server.csproj",
			"Content.Server/Content.Server.csproj",
		};

		private static readonly IReadOnlyList<string> ServerContentAssemblies = new List<string>()
		{
			"Content.Server",
			"Content.Shared",
			//"Content.Server.Database",
			//"Content.Shared.Database",
		};

		private static readonly IReadOnlyList<string> ClientContentAssemblies = new List<string>()
		{
			"Content.Client",
			"Content.Shared",
			//"Content.Shared.Database"
		};

		private static readonly IReadOnlySet<string> BinSkipFolders = new HashSet<string>()
		{
			// Roslyn localization files, screw em.
			"cs",
			"de",
			"es",
			"fr",
			"it",
			"ja",
			"ko",
			"pl",
			"pt-BR",
			"ru",
			"tr",
			"zh-Hans",
			"zh-Hant"
		};

		public const string ReleaseDir = "release";
		private const string ContentDir = "";
		private const string EngineDir = "RobustToolbox";

		public const string ContentBin = "bin";
		public const string EngineBin = $"{EngineDir}/bin";

		private static string ClientAczName(PlatformReg platform) => $"{ReleaseDir}/RobustTemplate.Client_Acz_{platform.Rid}.zip";
		private static string ClientZipName(PlatformReg platform) => $"{ReleaseDir}/RobustTemplate.Client_Standalone_{platform.Rid}.zip";
		private static string ServerZipName(PlatformReg platform) => $"{ReleaseDir}/RobustTemplate.Server_{platform.Rid}.zip";
		private const string ServerAczName = "Content.Client.zip";

		private static string ContentServerBin(PlatformReg platform) => $"Content.Server/{platform.Rid}/publish";
		private static string ContentClientBin(PlatformReg platform) => $"Content.Client/{platform.Rid}/publish";
		private static string EngineServerBin(PlatformReg platform) => $"RobustToolbox/bin/Server/{platform.Rid}/publish";
		private static string EngineClientBin(PlatformReg platform) => $"RobustToolbox/bin/Client/{platform.Rid}/publish";


	}
}
