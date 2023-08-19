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
		private ICursor _defaultCursor = default!;

		public void Initialize()
		{
			_defaultCursor = MakeCursor(_defaultCursorPath, _defaultHotSpot);
			_userInterfaceManager.WorldCursor = _defaultCursor;
		}

		private ICursor MakeCursor(string imagePath, Vector2i hotSpot)
		{
			var stream = _cResourceCache.ContentFileRead(imagePath);
			var image = Image.Load<Rgba32>(stream);
			var cursor = _clyde.CreateCursor(image, hotSpot);
			return cursor;
		}

		public void SetControlCursor(Control control)
		{
			foreach (var child in control.Children)
			{
				SetControlCursor(child);
			}
			if (control.DefaultCursorShape == Control.CursorShape.Arrow)
			{
				control.CustomCursorShape = _defaultCursor;
			}
		}
	}

	public static class CustomCursor
	{
		public static void SetCursor(Control control)
		{
			IoCManager.Resolve<CursorManager>().SetControlCursor(control);
		}
	}

}
