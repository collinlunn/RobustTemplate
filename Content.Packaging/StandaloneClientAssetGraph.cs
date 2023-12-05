using Robust.Packaging.AssetProcessing;
using Robust.Packaging.AssetProcessing.Passes;
using System.Collections.Generic;

namespace Robust.Packaging;

/// <summary>
/// Slightly modified copy of <see cref="RobustServerAssetGraph"/>.
/// </summary>
public sealed class StandaloneClientAssetGraph
{
	public AssetPassPipe Output { get; }

	/// <summary>
	/// Input pass for core server files, such as <c>Robust.Server.exe</c>.
	/// </summary>
	/// <seealso cref="InputResources"/>
	public AssetPassPipe InputCore { get; }

	public AssetPassPipe PresetPassesCore { get; }

	/// <summary>
	/// Normalizes text files in core files.
	/// </summary>
	public AssetPassNormalizeText NormalizeTextCore { get; }

	/// <summary>
	/// Input pass for server resource files. Everything that will go into <c>Resources/</c>.
	/// </summary>
	/// <remarks>
	/// Do not prefix file paths with <c>Resources/</c>, the asset pass will automatically remap them.
	/// </remarks>
	/// <seealso cref="InputCore"/>
	public AssetPassPipe InputResources { get; }
	public AssetPassPipe PresetPassesResources { get; }

	/// <summary>
	/// Normalizes text files in resources.
	/// </summary>
	public AssetPassNormalizeText NormalizeTextResources { get; }

	/// <summary>
	/// Responsible for putting resources into the "<c>Resources/</c>" folder.
	/// </summary>
	public AssetPassPrefix PrefixResources { get; }

	/// <summary>
	/// Collection of all passes in this preset graph.
	/// </summary>
	public IReadOnlyCollection<AssetPass> AllPasses { get; }

	/// <param name="parallel">Should inputs be run in parallel. Should only be turned off for debugging.</param>
	public StandaloneClientAssetGraph(bool parallel = true)
	{
		Output = new AssetPassPipe { Name = "RobustServerAssetGraphOutput", CheckDuplicates = true };

		//
		// Core files
		//

		// The code injecting the list of source files is assumed to be pretty single-threaded.
		// We use a parallelizing input to break out all the work on files coming in onto multiple threads.
		InputCore = new AssetPassPipe { Name = "RobustServerAssetGraphInputCore", Parallelize = parallel };
		PresetPassesCore = new AssetPassPipe { Name = "RobustServerAssetGraphPresetPassesCore" };
		NormalizeTextCore = new AssetPassNormalizeText { Name = "RobustServerAssetGraphNormalizeTextCore" };

		PresetPassesCore.AddDependency(InputCore);
		NormalizeTextCore.AddDependency(PresetPassesCore).AddBefore(Output);
		Output.AddDependency(PresetPassesCore);
		Output.AddDependency(NormalizeTextCore);

		//
		// Resource files
		//

		// Ditto about parallelizing
		InputResources = new AssetPassPipe { Name = "RobustServerAssetGraphInputResources", Parallelize = parallel };
		PresetPassesResources = new AssetPassPipe { Name = "RobustServerAssetGraphPresetPassesResources" };
		NormalizeTextResources = new AssetPassNormalizeText { Name = "RobustServerAssetGraphNormalizeTextResources" };
		PrefixResources = new AssetPassPrefix("Resources/") { Name = "RobustServerAssetGraphPrefixResources" };

		PresetPassesResources.AddDependency(InputResources);
		NormalizeTextResources.AddDependency(PresetPassesResources).AddBefore(PrefixResources);
		PrefixResources.AddDependency(PresetPassesResources);
		PrefixResources.AddDependency(NormalizeTextResources);
		Output.AddDependency(PrefixResources);

		AllPasses = new AssetPass[]
		{
			Output,
			InputCore,
			PresetPassesCore,
			NormalizeTextCore,
			InputResources,
			PresetPassesResources,
			NormalizeTextResources,
			PrefixResources,
		};
	}
}
