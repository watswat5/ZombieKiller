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
			Value = 1 * Difficulty;
			Alpha = .5f;
			Death = new Sound ("/Application/Assets/Sounds/boomerhurt.wav");
		}
		
		public override void Update (long ElapsedTime)
		{
			FrameTime += ElapsedTime;
			
			//Get difference in x and y between player and enemy.
			Vector3 playerPos = Player.p.Position;
			DeltaX = (float)Position.X - (float)playerPos.X;
			DeltaY = (float)Position.Y - (float)playerPos.Y;
			
			//Find rotation of zombie that looks at player
			Rotation = (float)Math.Atan2 ((double)DeltaX, (double)DeltaY);
			p.Rotation = -Rotation;
			
			//Calculate new position based on angle
			Position += new Vector3((float)Math.Sin (-Rotation) * RunSpeed, 0, 0);
			Position -= new Vector3(0, (float)Math.Cos (-Rotation) * RunSpeed, 0);
			
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
			this.explode.p.Position = this.Position;
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
			Explode.p.Position = Position;
			Collide.AddExplosion = (Explode);	
			CurrentLevel.Drop(this);
		}
		
	}
}

