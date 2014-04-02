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
	public class Zombie : Enemy
	{
		private static Texture2D tex = new Texture2D("/Application/Assets/Enemies/zombie.png", false);
		public Zombie (GraphicsContext gc, Vector3 position, Collisions col, int d) : base(gc, position, tex, col, new Texture2D("/Application/Assets/Enemies/deadzombie.png", false))
		{
			Difficulty = d;
			LiteralDifficulty = 1 * Difficulty;
			RunSpeed = .8f;
			Damage = 1 * Difficulty;
			Health = 1 * Difficulty;
			Value = 1 * Difficulty;
			FrameDuration = 100;
			Player = Collide.P;
			enemyType = Types.Zombie;
			Death = new Sound ("/Application/Assets/Sounds/zombiehurt.wav");
		}
		
		public override void Update (long ElapsedTime)
		{
			Vector3 playerPos = Player.p.Position;
			
			//Find X and Y difference between this and the player
			FrameTime += ElapsedTime;
			DeltaX = (float)Position.X - (float)playerPos.X;
			DeltaY = (float)Position.Y - (float)playerPos.Y;
			
			//Find rotation of zombie that looks at player
			Rotation = -(float)Math.Atan2 ((double)DeltaX, (double)DeltaY);
			//p.Rotation = -Rotation;
			
			//Calculate new position based on angle
			Vector3 vel = Vector3.Zero;
			vel -= new Vector3((float)Math.Sin (-Rotation) * RunSpeed, 0, 0);
			vel -= new Vector3(0, (float)Math.Cos (-Rotation) * RunSpeed, 0);
			
			avoidNeighbors(vel);
			
			Position += vel;
			
			//Advance sprite sheet
			if (FrameTime > FrameDuration) {
				if (ActiveFrame < FrameMax - 1)
					ActiveFrame++;
				else
					ActiveFrame = 0;
				FrameTime = 0;
			}				
		}
		
	    public void avoidNeighbors (Vector3 v)
		{
			Vector3 avoidanceVector = new Vector3 (0, 0, 0);
			int nearNeighborCount = 0;
			//Vector3 oldVel = v;
			//Vector3 vel = v;
			Vector3 oldVel = v;
			foreach (Creature z in Collide.Enemies)
			{
				if ((z != this) && (Vector3.Distance (z.p.Position, this.p.Position) < 100))
				{
					nearNeighborCount++;
					avoidanceVector += Vector3.Subtract (p.Position, z.p.Position) * 2.0f / Vector3.Distance (z.p.Position, this.p.Position);
				}
				
				if (nearNeighborCount > 0) {
					v = oldVel * 0.9f + avoidanceVector.Normalize () * 0.5f;
				} else {
					v = oldVel; 
				}
				Position += v * .01f;
			}
			
		}
		
		public override void HurtPlayer (Player plr)
		{
			plr.Alpha += Alpha;
			if (plr.Health >= Damage)
				plr.Health -= Damage;
			else
				plr.Health = 0;	
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
			Player.Money += Value;
			Player.Score += Value;
			CurrentLevel.Drop(this);
		}
	}
}

