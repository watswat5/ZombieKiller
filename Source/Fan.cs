using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

/*Chris Antepenko*/

namespace ZombieKiller
{
	//My adoring fans are faster than zombies.
	public class Fan : Enemy
	{
		
		public Fan (GraphicsContext gc, int x, int y, Collisions col) : base(gc, x, y, new Texture2D("/Application/Assets/fan.png", false), col, new Sound("/Application/Assets/hurt.wav"), new Texture2D("/Application/Assets/deadfan.png", false))
		{
			runSpeed = 3;
			type = 2;
		}
		
		public override void Update (long ElapsedTime, Vector3 playerPos)
		{
			//Same movement math as zombies
			frameTime += ElapsedTime;
			deltax = (float)X - (float)playerPos.X;
			deltay = (float)Y - (float)playerPos.Y;
			
			rotation = (float)Math.Atan2 ((double)deltax, (double)deltay);
			
			p.Rotation = -rotation;
			
			newX = (float)Math.Sin (-rotation) * runSpeed;
			newY = (float)Math.Cos (-rotation) * runSpeed;
			p.Position.X += newX;
			p.Position.Y -= newY;
			//Sprite sheet advancement
			if (frameTime > FRAME_DURATION) {
				if (activeFrame < FRAME_MAX - 1)
					activeFrame++;
				else
					activeFrame = 0;
				frameTime = 0;
			}				
		}
	}
}

