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
	public class Square : Enemy
	{
		private Random rnd;
		private bool turning;

		public Square (GraphicsContext gc, int x, int y, Collisions col) : base(gc, x, y, new Texture2D("/Application/Assets/zombie2.png", false), col, new Sound("/Application/Assets/hurt.wav"), new Texture2D("/Application/Assets/deadsquare.png", false))
		{
			runSpeed = 1;
			FRAME_DURATION = 100;
			
			rnd = new Random ();
			p.Rotation = (float)(rnd.NextDouble() * Math.PI * 2);
			turning = false;
			
			type = 0;
		}
		//POLYMORPHISM, BABY!
		public override void Update (long ElapsedTime, Vector3 playerPos)
		{
			//Detects if square is off screen or turning
			if (!collision.IsOnScreen (this) && !turning) {
				
				//Direction (dir) 0 and 1 are for left and right changing of rotation
				int dir = rnd.Next (0, 2);
				
				if (p.Position.X < 1 || p.Position.X > graphics.Screen.Rectangle.Width - 1)
					p.Rotation = 0 - p.Rotation;
				if (p.Position.Y < 1 || p.Position.Y > graphics.Screen.Rectangle.Height - 1)
					p.Rotation = (float)Math.PI - p.Rotation;
				
				//Prevents square having seizure in the wall
				turning = true;
				
			//Stops turning if square is on screen
			} else if (collision.IsOnScreen (this) && turning)
				turning = false;
			
			//Calculate new position based on angle
			newX = (float)Math.Sin (p.Rotation) * runSpeed;
			newY = (float)Math.Cos (p.Rotation) * runSpeed;
			p.Position.X += newX;
			p.Position.Y -= newY;
			
			//Advance sprite sheet
			frameTime += ElapsedTime;
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

