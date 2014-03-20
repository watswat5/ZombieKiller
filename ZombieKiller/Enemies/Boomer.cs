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
	//Explodes on contact with player.
	public class Boomer : Enemy
	{
		
		public Boomer (GraphicsContext gc, Vector3 position, Collisions col, int d) : base(gc, position, new Texture2D("/Application/Assets/Enemies/boomer.png", false), col, new Texture2D("/Application/Assets/Enemies/explode.png", false))
		{
			Difficulty = d;
			RunSpeed = 0.5f;
			explode.FrameDuration = 100;
			explode.Scale = new Vector2 (1.2f, 1.2f);
			FrameDuration = 100;
			enemyType = Types.Boomer;
			Damage = 3 * Difficulty;
			Health = 1 * Difficulty;
			Death = new Sound ("/Application/Assets/Sounds/boomerhurt.wav");
		}
		
		public override void Update (long ElapsedTime)
		{
			FrameTime += ElapsedTime;
			
			//Get difference in x and y between player and enemy.
			Vector3 playerPos = this.Player.p.Position;
			DeltaX = (float)p.Position.X - (float)playerPos.X;
			DeltaY = (float)p.Position.Y - (float)playerPos.Y;
			
			//Find rotation of zombie that looks at player
			Rotation = (float)Math.Atan2 ((double)DeltaX, (double)DeltaY);
			p.Rotation = -Rotation;
			
			//Calculate new position based on angle
			p.Position.X -= (float)Math.Sin (Rotation) * RunSpeed;
			p.Position.Y -= (float)Math.Cos (Rotation) * RunSpeed;
			
			//Advance sprite sheet
			if (FrameTime > FrameDuration) {
				if (ActiveFrame < FrameMax - 1)
					ActiveFrame++;
				else
					ActiveFrame = 0;
				FrameTime = 0;
			}				
		}
		
		public override void HurtPlayer (Player plr)
		{
			plr.Alpha += Alpha;
			if (plr.Health >= Damage)
				plr.Health -= Damage;
			else
				plr.Health = 0;
			this.IsAlive = false;
			this.explode.p.Position = this.p.Position;
			Collide.AddExplosion = this.explode;
		}
		
		public override void OnHurt (Bullet b)
		{
			//Destroy enemy and bullet, add death sprite
			if(Health > b.Damage)
				Health -= b.Damage;
			else
				Die ();
		}
		
		public override void Die()
		{
			this.IsAlive = false;
			Explode.p.Position = p.Position;
			Collide.AddExplosion = (Explode);	
			Drop ();
		}
		
	}
}

