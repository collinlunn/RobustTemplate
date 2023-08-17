using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using Robust.Client.UserInterface;

namespace Content.Client.UI
{
	public sealed class CursorManager
	{
		[Dependency] private readonly IClyde _clyde = default!;
		[Dependency] private readonly IResourceCache _cResourceCache = default!;
		[Dependency] private readonly IUserInterfaceManager _userInterfaceManager = default!;

		private Vector2i _defaultHotSpot = new Vector2i(0, 0);
		private string _defaultCursorPath = "/Textures/testMouse.png";

		public void Initialize()
		{
			var cursor = MakeCursor(_defaultCursorPath, _defaultHotSpot);
			_userInterfaceManager.WorldCursor = cursor;
		}

		private ICursor MakeCursor(string imagePath, Vector2i hotSpot)
		{
			var stream = _cResourceCache.ContentFileRead(imagePath);
			var image = Image.Load<Rgba32>(stream);
			var cursor = _clyde.CreateCursor(image, hotSpot);
			return cursor;
		}
	}
}
