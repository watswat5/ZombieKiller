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
	public class ExplosiveBullet : Bullet
	{
		private int shrapnel;
		private int shrapnelDmg;
		
		public int Shrapnel
		{
			get { return shrapnel;}
			set { shrapnel = value;}
		}
		
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
			
			Position += new Vector3((float)Math.Sin (p.Rotation) * RunSpeed, 0, 0);
			Position -= new Vector3(0, (float)Math.Cos (p.Rotation) * RunSpeed, 0);
		}
		
		public override void OnHurt ()
		{
			for (int i = 0; i < shrapnel; i++) {
				float rot = (float)(((float)i * 2f * Math.PI) / shrapnel);
				Collide.AddTempBullet = new RubberBullet (Graphics, p.Position, rot, Collide, (int)RunSpeed, shrapnelDmg);
			}
			IsAlive = false;
		}
		
		public override void Render ()
		{
			p.Render ();	
		}
		
	}
}

