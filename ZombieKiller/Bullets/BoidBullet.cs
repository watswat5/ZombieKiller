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
	public class BoidBullet : Bullet
	{
		private Vector3 vel;
		
		private Player plr;
		public Player Plr
		{
			get { return plr;}
			set { plr = value;}
		}
		
		public BoidBullet (GraphicsContext gc, Vector3 position, float rot, Collisions col, int speed, int damage) : base(gc, position, rot, col, speed, damage, new Texture2D("/Application/Assets/Bullets/bullet.png", false))
		{
			vel = new Vector3(0,0,0);
			plr = Collide.P;
			p.Rotation = plr.p.Rotation;
		}
		
		public override void Update (long EllapsedTime)
		{
			Vector3 diff = Vector3.Subtract (Plr.p.Position, p.Position);
			
			if (diff.Length () > 1) {
				vel += Vector3.Normalize (diff) / 5;
			}
            
			if ((vel.X != 0) || (vel.Y != 0)) {
				p.Rotation = FMath.Atan2 (vel.Y, vel.X);
			}
			
			float velLength = vel.Length ();
			
			if (velLength > (float)RunSpeed) {
				vel = vel.Normalize ();
				vel *= RunSpeed;
			}

			p.Position += vel;
			
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

