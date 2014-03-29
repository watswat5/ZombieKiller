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
	public abstract class Bullet : Creature
	{
		private int damage;

		public int Damage {
			get { return damage;}
			set { damage = value;}
		}
		
		public Bullet (GraphicsContext gc, Vector3 position, float rot, Collisions col, int speed, int damage, Texture2D tex) : base(gc, position, tex, col)
		{
			p.Rotation = rot;
			p.Center = new Vector2 (0.5f, 1f);
			p.Scale.X = 0.15f;
			p.Scale.Y = 0.30f;
			//p.SetColor(new Vector4((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (1)));
			RunSpeed = speed;
			IsAlive = true;
			this.damage = damage;
		}
		
		public abstract void OnHurt ();
		
		public override void Render ()
		{
			p.Render ();	
		}
		
	}
}

