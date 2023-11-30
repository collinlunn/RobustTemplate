using System.Collections.Generic;
using System.Linq;

namespace Content.Packaging
{
    public static partial class Packager
    {
		public readonly record struct PlatformReg(string Rid, string TargetOs, bool BuildByDefault);

		public static readonly List<PlatformReg> AllPlatforms = new()
		{
			new PlatformReg("win-x64", "Windows", true),
			new PlatformReg("linux-x64", "Linux", false),
			new PlatformReg("linux-arm64", "Linux", false),
			new PlatformReg("osx-x64", "MacOS", false),
			// Non-default platforms (i.e. for Watchdog Git)
			new PlatformReg("win-x86", "Windows", false),
			new PlatformReg("linux-x86", "Linux", false),
			new PlatformReg("linux-arm", "Linux", false),
		};

		//temporary
		public static List<string> PlatformRidsDefault => AllPlatforms
			.Where(o => o.BuildByDefault)
			.Select(o => o.Rid)
			.ToList();

		public static List<PlatformReg> DefaultPlatforms => AllPlatforms
			.Where(o => o.BuildByDefault)
			.ToList();

		public static readonly List<string> AllReleasedProjects = new()
		{
			//"../../../../RobustToolbox/Robust.Client/Robust.Client.csproj",
			//"../../../../Content.Client/Content.Client.csproj",
			"../../../../RobustToolbox/Robust.Server/Robust.Server.csproj",
			//"../../../../Content.Server/Content.Server.csproj",
		};

		public static readonly string ReleaseDirectory = "../../../../release";

		private static List<string> SERVER_BIN_SKIP_FOLDERS = new()
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

        private static List<string> CLIENT_BIN_SKIP_FOLDERS = new()
        {

        };

        private static List<string> SERVER_RES_SKIP_FOLDERS = new()
        {
			//Content
			"Textures",
            "Fonts",
            "Audio",
            "Shaders",
            "Fonts",
            "keybinds.yml",

			//Engine
			"EngineFonts",
        };

        private static List<string> CLIENT_RES_SKIP_FOLDERS = new()
        {
            "Commands",
        };

        private static List<string> SERVER_RES_SKIP_FILES = new()
        {
            ".gitignore",
        };

        private static List<string> CLIENT_RES_SKIP_FILES = new()
        {
            ".gitignore",
        };

        private static List<string> SERVER_CONTENT_ASSEMBLIES = new()
        {
            "Content.Server",
            "Content.Shared",
        };

        private static List<string> CLIENT_CONTENT_ASSEMBLIES = new()
        {
            "Content.Client",
            "Content.Shared",
        };

		//###stuff

		private static readonly List<string> ServerContentAssemblies = new()
		{
			//"Content.Server.Database",
			"../../../../Content.Server",
			"../../../../Content.Shared",
			//"Content.Shared.Database",
		};

		private static readonly List<string> ServerExtraAssemblies = new()
		{
			// Python script had Npgsql. though we want Npgsql.dll as well soooo
			"Npgsql",
			"Microsoft",
		};

		private static readonly List<string> ServerNotExtraAssemblies = new()
		{
			"Microsoft.CodeAnalysis",
		};

		private static readonly HashSet<string> BinSkipFolders = new()
		{
			// Roslyn localization files, screw em.
			"../../../../cs",
			"../../../../de",
			"../../../../es",
			"../../../../fr",
			"../../../../it",
			"../../../../ja",
			"../../../../ko",
			"../../../../pl",
			"../../../../pt-BR",
			"../../../../ru",
			"../../../../tr",
			"../../../../zh-Hans",
			"../../../../zh-Hant"
		};
	}
}
