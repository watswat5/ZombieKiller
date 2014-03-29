using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

//Chris Antepenko & C. Blake Becker
namespace ZombieKiller
{
	//Projectile
	public class RubberBullet : Bullet
	{
		private bool turning;
		private int numOfBounces;
		private const int MAX_BOUNCES = 3;
		
		public RubberBullet (GraphicsContext gc, Vector3 position, float rot, Collisions col, int speed, int damage) : base(gc, position, rot, col, speed, damage, new Texture2D("/Application/Assets/Bullets/bullet.png", false))
		{
			
		}
		
		public override void Update (long EllapsedTime)
		{
			//Bullets bounce around screen before dying.
			if (numOfBounces < MAX_BOUNCES) {
				//Detects if bullet is off screen or turning
				if (!Collide.IsOnScreen (this) && !turning) {
					numOfBounces++;
					//Reflects rotation across X axis if enemy goes off screen to the left or right.
					if (Position.X < 1 || Position.X > Graphics.Screen.Rectangle.Width - 1)
						p.Rotation = -p.Rotation;
					//Reflects rotation across Y axis if enemy goes above or below screen.
					if (Position.Y < 1 || Position.Y > Graphics.Screen.Rectangle.Height - 1)
						p.Rotation = (float)Math.PI - p.Rotation;
					
					//Prevents bullet having seizure in the wall by turning again before it has re-entered the screen
					turning = true;
					
					//Stops turning if bullet is on screen
				} else if (Collide.IsOnScreen (this) && turning)
					turning = false;
			} else
				IsAlive = false;
			
			Position += new Vector3((float)Math.Sin (p.Rotation) * RunSpeed, 0, 0);
			Position -= new Vector3(0, (float)Math.Cos (p.Rotation) * RunSpeed, 0);
		}
		
		public override void OnHurt ()
		{
			IsAlive = false;
		}
		
		public override void Render ()
		{
			p.Render ();	
		}
		
	}
}

