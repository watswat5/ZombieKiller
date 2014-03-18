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
	public class ExplosiveBullet : Bullet
	{
		private int shrapnel;
		private int shrapnelDmg;
		
		public ExplosiveBullet (GraphicsContext gc, Vector3 position, float rot, Collisions col, int speed, int damage) : base(gc, position, rot, col, speed, damage, new Texture2D("/Application/Assets/Bullets/rocket.png", false))
		{
			shrapnel = 16;
			shrapnelDmg = 2;
			p.Scale = new Vector2 (.4f, .8f);
		}
		
		public override void Update (long EllapsedTime)
		{
			if (!Collide.IsOnScreen (this))
				IsAlive = false;
			;
			p.Position.X += (float)Math.Sin (p.Rotation) * RunSpeed;
			p.Position.Y -= (float)Math.Cos (p.Rotation) * RunSpeed;
		}
		
		public override void OnHurt ()
		{
			for (int i = 0; i < shrapnel; i++) {
				float rot = (float)(((float)i * 2f * Math.PI) / shrapnel);
				Collide.AddTempBullet = new ExplosiveBullet (Graphics, p.Position, rot, Collide, (int)RunSpeed, shrapnelDmg);
			}
			IsAlive = false;
		}
		
		public override void Render ()
		{
			p.Render ();	
		}
		
	}
}

