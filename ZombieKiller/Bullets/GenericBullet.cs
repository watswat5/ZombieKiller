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
	public class GenericBullet : Bullet
	{
		public GenericBullet (GraphicsContext gc, Vector3 position, float rot, Collisions col, int speed, int damage) : base(gc, position, rot, col, speed, damage, new Texture2D("/Application/Assets/Bullets/bullet.png", false))
		{
			
		}
		
		public override void Update (long EllapsedTime)
		{
			if(!Collide.IsOnScreen(this))
				IsAlive = false;
			p.Position.X += (float)Math.Sin (p.Rotation) * RunSpeed;
			p.Position.Y -= (float)Math.Cos (p.Rotation) * RunSpeed;
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

