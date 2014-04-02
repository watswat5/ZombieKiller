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
	public class ZombieBoss : Enemy
	{
		private Sprite healthBar;
		private int MAX_HEALTH;
		
		public ZombieBoss (GraphicsContext gc, Vector3 position, Collisions col, int d) : base(gc, position, new Texture2D("/Application/Assets/Enemies/zombie.png", false), col, new Texture2D("/Application/Assets/Enemies/deadzombie.png", false))
		{
			Difficulty = d;
			LiteralDifficulty = 10 * Difficulty;
			RunSpeed = 1;
			Damage = 2 * Difficulty;
			Health = 60 * Difficulty;
			MAX_HEALTH = Health;
			Value = 20 * Difficulty;
			FrameDuration = 100;
			enemyType = Types.Zombie;
			Death = new Sound ("/Application/Assets/Sounds/zombiehurt.wav");
			p.Scale = new Vector2(2.2f, 2.2f);
			Player = Collide.P;
			
			Explode.Scale = new Vector2(4, 4);
			
			healthBar = new Sprite(Graphics, new Texture2D("/Application/Assets/Enemies/health.png", false));
			healthBar.Scale = new Vector2(.47f, .1f);
			healthBar.Position = position;
			healthBar.Center = new Vector2(0.5f, -2f);
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
			
			healthBar.Position = Position;
			healthBar.Rotation = p.Rotation;
			
			//Calculate new position based on angle
			Position -= new Vector3((float)Math.Sin (-Rotation) * RunSpeed, 0, 0);
			Position -= new Vector3(0, (float)Math.Cos (-Rotation) * RunSpeed, 0);
			
			//avoidNeighbors();
			//Advance sprite sheet
			if (FrameTime > FrameDuration) {
				if (ActiveFrame < FrameMax - 1)
					ActiveFrame++;
				else
					ActiveFrame = 0;
				FrameTime = 0;
			}				
		}
		
	    public void avoidNeighbors ()
		{
			Vector3 avoidanceVector = new Vector3 (0, 0, 0);
			int nearNeighborCount = 0;
			Vector3 oldVel = new Vector3((float)Math.Sin (p.Rotation), (float)Math.Cos (p.Rotation), RunSpeed);
			Vector3 vel = Vector3.Zero;
			foreach (Creature z in Collide.Enemies)
			{
				if ((z != this) && (Vector3.Distance (z.p.Position, this.p.Position) < 100))
				{
					nearNeighborCount++;
					avoidanceVector += Vector3.Subtract (p.Position, z.p.Position) * 5.0f / Vector3.Distance (z.p.Position, this.p.Position);
				}
				
				if (nearNeighborCount > 0) {
					vel = oldVel * 0.8f + avoidanceVector.Normalize () * 0.2f;
				} else {
					vel = oldVel; 
				}
				p.Position.X += vel.X * 1;
				p.Position.Y -= vel.Y * 1;
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
		
		public override void Render()
		{
			p.SetTextureCoord (CellSize * ActiveFrame, 0, CellSize * (ActiveFrame + 1) - 1, CellSize);
			p.Render ();
			
			//Renders the healthbar with a size proportionate to total health.
			healthBar.SetTextureCoord(0, 0, (200 / (MAX_HEALTH)) * Health, 200);
			healthBar.Width = ((200/MAX_HEALTH) *  Health - 1);
			healthBar.Render ();
		}
		
	}
}

