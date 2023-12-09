using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.ContentPack;
using Robust.Shared.Utility;
using System.Numerics;

namespace Content.Client.MainMenu
{
	public sealed class TitleArtControl : Control
	{
		private readonly ResPath TitleArtPath = new("/Textures/Interface/titleArt.png");

		private AnimatedTextureRect AnimatedTexRect = new();

		public TitleArtControl()
		{
			var sprite = new SpriteSpecifier.Texture(TitleArtPath);
			AnimatedTexRect.SetFromSpriteSpecifier(sprite);
			AddChild(AnimatedTexRect);
			AnimatedTexRect.DisplayRect.Stretch = TextureRect.StretchMode.Scale;
		}
	}
}
