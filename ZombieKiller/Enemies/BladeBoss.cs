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
	
	
	//A spinning blade boss of death and despair
	public class BladeBoss : Enemy
	{		
		public float rot;
		private bool turning;
		private int maxE;
		public BladeBoss (GraphicsContext gc, Vector3 position, Collisions col, int d, Level curL) : base(gc, position, new Texture2D("/Application/Assets/Enemies/blade.png", false), col, new Texture2D("/Application/Assets/Enemies/deadblade.png", false), curL)
		{
			maxE = 10;
			Difficulty = d;
			LiteralDifficulty = (10 + maxE * 3)* Difficulty;
			RunSpeed = 3;
			enemyType = Types.Blade;
			rot = (float)(2 * Math.PI * rnd.NextDouble ());
			Damage = 2 * Difficulty;
			Health = 1 * Difficulty;
			Value = 2 * Difficulty;
			Death = new Sound ("/Application/Assets/Sounds/bladehurt.wav");
			SpawnBaldes();
		}
		
		public override void Update (long ElapsedTime)
		{
			//Detects if blade is off screen or turning
			if (!Collide.IsOnScreen (this) && !turning) {
				//Reflects rotation across X axis if enemy goes off screen to the left or right.
				if (Position.X < 1 || Position.X > Graphics.Screen.Rectangle.Width - 1)
					rot = -rot;
				//Reflects rotation across Y axis if enemy goes above or below screen.
				if (Position.Y < 1 || Position.Y > Graphics.Screen.Rectangle.Height - 1)
					rot = (float)Math.PI - rot;
				
				//Prevents blade having seizure in the wall by turning again before it has re-entered the screen
				turning = true;
				
				//Stops turning if blade is on screen
			} else if (Collide.IsOnScreen (this) && turning)
				turning = false;
			
			//Calculate new position based on angle
			Position += new Vector3 ((float)Math.Sin (rot) * RunSpeed, 0, 0);
			Position -= new Vector3 (0, (float)Math.Cos (rot) * RunSpeed, 0);
			
			if (Collide.FarOffScreen (this))
				IsAlive = false;
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
			if (Health > b.Damage)
				Health -= b.Damage;
			else
				Die ();
		}
		
		public override void Die ()
		{
			this.IsAlive = false;
			Explode.Position = Position;
			Collide.AddExplosion = (Explode);	
			Player.Money += Value;
			Player.Score += Value;
			CurrentLevel.Drop (this);
		}
		
		//Spawns boid blades
		private void SpawnBaldes()
		{
			for (int i = 0; i < maxE; i++) {
				Vector3 pos = p.Position;
				float rot = p.Rotation + (float)(Math.PI / (float)maxE) * 2 * i;
				pos.X += (float)(Math.Sin (rot)) * (RunSpeed + 10);
				pos.Y -= (float)(Math.Cos (rot)) * (RunSpeed + 10);
				Enemy e = new BoidBlade (Graphics, pos, Collide, Difficulty, this, CurrentLevel);
				e.CurrentLevel = CurrentLevel;
				Collide.AddTempEnemy = e;
			}
		}
		
		//No sprite sheet, just a rotation.
		public override void Render ()
		{
			p.Rotation += 15 * (float)(Math.PI / 180);
			p.Render ();
		}
	}
}

