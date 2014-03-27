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
	public class BoomerBoss : Enemy
	{
		private Sprite healthBar;
		private int MAX_HEALTH;
		
		private const long SPAWN_TIME = 1000;
		private long spawnTimer;
		
		public BoomerBoss (GraphicsContext gc, Vector3 position, Collisions col, int d) : base(gc, position, new Texture2D("/Application/Assets/Enemies/boomer.png", false), col, new Texture2D("/Application/Assets/Enemies/explode.png", false))
		{
			Difficulty = d;
			RunSpeed = 0.3f;
			explode.FrameDuration = 100;
			explode.Scale = new Vector2 (2f, 2f);
			FrameDuration = 100;
			enemyType = Types.Boomer;
			Damage = 3 * Difficulty;
			Health = 30 * Difficulty; 
			MAX_HEALTH = Health;
			Value = 20 * Difficulty;
			Alpha = .5f;
			Death = new Sound ("/Application/Assets/Sounds/boomerhurt.wav");
			
			Player = Collide.P;
			
			healthBar = new Sprite(Graphics, new Texture2D("/Application/Assets/Player/health.png", false));
			healthBar.Scale = new Vector2(.47f, .2f);
			healthBar.Position = new Vector3(0, 500, 0);
			
			p.Scale = new Vector2(2.2f, 2.2f);
		}
		
		public override void Update (long ElapsedTime)
		{
			FrameTime += ElapsedTime;
			spawnTimer += ElapsedTime;
			
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
			
			//spawnTimer boomer
			if(spawnTimer > SPAWN_TIME && Collide.enemyCount < CurrentLevel.MaxEnemies)
			{
				Boomer b = new Boomer(Graphics, Position, Collide, Difficulty);
				b.CurrentLevel = CurrentLevel;
				Collide.AddTempEnemy = (b);
				spawnTimer = 0;
				Console.WriteLine("SPAWNED ");
			}
			
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
			Player.Money += Value;
			Player.Score += Value;
			CurrentLevel.Drop(this);
		}
		
		public override void Render()
		{
			p.SetTextureCoord (CellSize * ActiveFrame, 0, CellSize * (ActiveFrame + 1) - 1, CellSize);
			p.Render ();
			
			//Renders the healthbar with a size proportionate to total health.
			healthBar.SetTextureCoord(0, 0, (2000 / (MAX_HEALTH)) * Health, 200);
			healthBar.Width = ((2000/MAX_HEALTH) *  Health - 1);
			healthBar.Render ();	
		}
		
	}
}

