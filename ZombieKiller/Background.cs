using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace ZombieKiller
{
	public class Background
	{
		private GraphicsContext graphics;
		private Sprite p;
		
		public Background (GraphicsContext gc)
		{
			graphics = gc;
			Texture2D tex = new Texture2D ("/Application/Assets/background.png", false);
			p = new Sprite (graphics, tex);
			p.Position.X = 0;
			p.Position.Y = 0;
		}
		
		public void Render ()
		{
			p.Render ();		
		}
	}
}

