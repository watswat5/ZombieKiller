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

		public Bullet (GraphicsContext gc, Vector3 position, float rot, Collisions col, int speed) : base(gc, position, new Texture2D("/Application/Assets/Weapons/bullet.png", false), col)
		{
			p.Rotation = rot;
			p.Center = new Vector2 (0.5f, 1f);
			p.Scale.X = 0.15f;
			p.Scale.Y = 0.30f;
			p.SetColor(new Vector4((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (1)));
			RunSpeed = speed;
			IsAlive = true;
		}
		
		public override void Update (long EllapsedTime)
		{
			//Bullet movement math
			p.Position.X -= (float)Math.Sin (-p.Rotation) * RunSpeed;
			p.Position.Y -= (float)Math.Cos (-p.Rotation) * RunSpeed;
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

