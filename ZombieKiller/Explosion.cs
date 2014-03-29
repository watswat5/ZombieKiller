using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

//Chris Antepenko & C. Blake Becker
namespace ZombieKiller
{
	//Death sprite
	public class Explosion : Creature
	{
		public Explosion (GraphicsContext gc, Vector3 position, Collisions col, Texture2D tex) : base(gc, position, tex, col)
		{
			CellSize = 32;
			p.Scale = new Vector2 (1f, 1f);
			p.Center = new Vector2 (0.5f, 0.5f);
		}
		
		public override void Update (long EllapsedTime)
		{
			FrameTime += EllapsedTime;
			if (FrameTime > FrameDuration) {
				if (ActiveFrame < FrameMax)
					ActiveFrame++;
				else
					IsAlive = false;
				FrameTime = 0;
			}
		}
	}
}

