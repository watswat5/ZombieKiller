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
	//Projectile
	public class Bullet : Creature
	{
		private bool turning;
		private int numOfBounces;
		private const int MAX_BOUNCES = 3;
		
		private int damage;
		public int Damage
		{
			get { return damage;}
			set { damage = value;}
		}
		
		public Bullet (GraphicsContext gc, Vector3 position, float rot, Collisions col, int speed, int damage) : base(gc, position, new Texture2D("/Application/Assets/Weapons/bullet.png", false), col)
		{
			p.Rotation = rot;
			p.Center = new Vector2 (0.5f, 1f);
			p.Scale.X = 0.15f;
			p.Scale.Y = 0.30f;
			p.SetColor(new Vector4((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (1)));
			RunSpeed = speed;
			IsAlive = true;
			turning = false;
			numOfBounces = 0;
			this.damage = damage;
		}
		
		public override void Update (long EllapsedTime)
		{
			//Bullets bounce around screen before dying.
			if(numOfBounces < MAX_BOUNCES)
			{
				//Detects if bullet is off screen or turning
				if (!Collide.IsOnScreen (this) && !turning) {
					numOfBounces++;
					//Reflects rotation across X axis if enemy goes off screen to the left or right.
					if (p.Position.X < 1 || p.Position.X > Graphics.Screen.Rectangle.Width - 1)
						p.Rotation = -p.Rotation;
					//Reflects rotation across Y axis if enemy goes above or below screen.
					if (p.Position.Y < 1 || p.Position.Y > Graphics.Screen.Rectangle.Height - 1)
						p.Rotation = (float)Math.PI - p.Rotation;
					
					//Prevents bullet having seizure in the wall by turning again before it has re-entered the screen
					turning = true;
					
					//Stops turning if bullet is on screen
				} else if (Collide.IsOnScreen (this) && turning)
					turning = false;
			}else
				IsAlive = false;
			
			p.Position.X += (float)Math.Sin (p.Rotation) * RunSpeed;
			p.Position.Y -= (float)Math.Cos (p.Rotation) * RunSpeed;
		}
		
		public virtual void HurtEnemy (Enemy e)
		{
			
		}
		
		public override void Render ()
		{
			p.Render ();	
		}
		
	}
}

