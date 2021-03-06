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
	
	//Base enemy class
	public abstract class Enemy : Creature
	{
		//What type of enemy this is.
		public enum Types
		{
			Zombie,
			Blade,
			Boomer
		};
		
		private Types type;
		
		public Types enemyType {
			get { return type;}
			set { type = value;}
		}
		
		//Player
		private Player player;

		public Player Player {
			get { return player;}
			set { player = value;}
		}
		
		//How much damage the enemy does.
		private int diff;

		public int Difficulty {
			get { return diff;}
			set { diff = value;}
		}
		
		private int litDiff;

		public int LiteralDifficulty {
			get { return litDiff;}
			set { litDiff = value;}
		}
		
		private int dmg;

		public int Damage {
			get { return dmg;}	
			set { dmg = value;}
		}
		
		private float alpha;

		public float Alpha {
			get { return alpha;}
			set { alpha = value;}
		}
		
		//Death sound
		private Sound death;

		public Sound Death {
			get { return death;}	
			set { death = value;}
		}
		
		//Death sprite
		public Explosion explode;

		public Explosion Explode {
			get { return explode;}	
		}
		
		//Used for finding angles between Player and Enemy
		private float deltax;

		public float DeltaX {
			get { return deltax;}
			set { deltax = value;}
		}

		private float deltay;

		public float DeltaY {
			get { return deltay;}
			set { deltay = value;}
		}
		
		private int health;

		public int Health {
			get { return health;}
			set { health = value;}
		}
		
		private int val;

		public int Value {
			get { return val;}
			set { val = value;}
		}
		
		private Level currentLevel;
		public Level CurrentLevel {
			get { return currentLevel;}
			set { currentLevel = value;}
		}
		
		public Enemy (GraphicsContext gc, Vector3 position, Texture2D tex, Collisions col, Texture2D explode, Level curL) : base(gc, position, tex, col)
		{
			//Death sprite
			this.explode = new Explosion (gc, position, col, explode);
			alpha = .3f;
			Player = col.P;
			CurrentLevel = curL;
		}
		
		public virtual void Drop ()
		{
			Collide.P.Money += Value;
			//Odds of dropping an item
			int drop = rnd.Next (0, 101);
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
				
			case 4:
				Collide.AddItem = new RPGAmmo (Graphics, p.Position, Collide);
				break;
				
			}	
			
			
		}
		
		//Method run if enemy collides with player
		public abstract void HurtPlayer (Player plr);
		
		//Method to run on collision with bullet
		public abstract void OnHurt (Bullet b);
		
		//Method to run on death
		public abstract void Die ();
	}
}

