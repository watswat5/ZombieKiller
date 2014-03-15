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
	public class Zombie : Enemy
	{
		
		public Zombie (GraphicsContext gc, Vector3 position, Collisions col) : base(gc, position, new Texture2D("/Application/Assets/Enemies/zombie.png", false), col, new Texture2D("/Application/Assets/Enemies/deadzombie.png", false))
		{
			RunSpeed = 1;
			Damage = 1;
			FrameDuration = 100;
			enemyType = Types.Zombie;
			Death = new Sound ("/Application/Assets/Sounds/zombiehurt.wav");
		}
		
		public override void Update (long ElapsedTime)
		{
			Vector3 playerPos = this.Player.p.Position;
			
			//Find X and Y difference between this and the player
			FrameTime += ElapsedTime;
			DeltaX = (float)p.Position.X - (float)playerPos.X;
			DeltaY = (float)p.Position.Y - (float)playerPos.Y;
			
			//Find rotation of zombie that looks at player
			Rotation = (float)Math.Atan2 ((double)DeltaX, (double)DeltaY);
			p.Rotation = -Rotation;
			
			//Calculate new position based on angle
			p.Position.X += (float)Math.Sin (-Rotation) * RunSpeed;;
			p.Position.Y -= (float)Math.Cos (-Rotation) * RunSpeed;;
			
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
			if (plr.Health >= Damage)
				plr.Health -= Damage;
			else
				plr.Health = 0;	
		}
		
		public override void OnDeath (Bullet b)
		{
			//Destroy enemy and bullet, add death sprite
			b.IsAlive = false;
			this.IsAlive = false;
			Explode.p.Position = p.Position;
			Collide.AddExplosion = (Explode);	
			
			//Odds of dropping an item
			int drop = rnd.Next (0, 40);
			//Item drop
			switch (drop) {
			case 0:
				Collide.AddItem = new Health (Graphics, p.Position, Collide);
				break;
				
			case 1:
				Collide.AddItem = new MGAmmo (Graphics, p.Position, Collide);
				break;
				
			case 2:
				Collide.AddItem = new RifleAmmo (Graphics, p.Position, Collide);
				break;
				
			case 3:
				Collide.AddItem = new ShotgunAmmo (Graphics, p.Position, Collide);
				break;
				
			}	
		}
	}
}

