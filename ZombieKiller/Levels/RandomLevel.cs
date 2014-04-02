using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using System.Diagnostics;
using Sce.PlayStation.Core.Audio;

//Chris Antepenko & C. Blake Becker
namespace ZombieKiller
{
	public class RandomLevel : Level
	{
		public static int LevelDifficulty = 50;
		
		public RandomLevel (GraphicsContext g, Collisions c, Texture2D tex, int diff, int maxE, int drpRng, string lCount) : base(g, tex, c, diff, maxE, drpRng, lCount, c.P)
		{
			MaxEnemies = maxE;
		}
		
		public override void Update ()
		{
			if (Collide.Enemies.Count <= 0) {
				Finished = true;	
			}
		}
		
		public override void SpawnEnemies ()
		{
			for (int i = 0; i < MaxEnemies; i++) {
				Enemy e;
				int choice = rnd.Next (0, 6);
				switch (choice	) {
				case 0:
					e = new Blade (Graphics, new Vector3 (400 + rnd.Next (200, 400), 0 + rnd.Next (20, 401), 0), Collide, Difficulty);
					break;
				case 1:
					e = new Boomer (Graphics, new Vector3 (400 + rnd.Next (200, 400), 400 + rnd.Next (20, 401), 0), Collide, Difficulty);
					break;
				case 2:
					e = new Boomer (Graphics, new Vector3 (400 + rnd.Next (200, 400), 400 + rnd.Next (20, 401), 0), Collide, Difficulty);
					break;
				case 3:
					e = new Zombie (Graphics, new Vector3 (400 + rnd.Next (200, 400), 400 + rnd.Next (20, 401), 0), Collide, Difficulty);
					break;	
				case 4:
					e = new Zombie (Graphics, new Vector3 (400 + rnd.Next (200, 400), 400 + rnd.Next (20, 401), 0), Collide, Difficulty);
					break;	
				default:
					e = new Zombie (Graphics, new Vector3 (400 + rnd.Next (200, 400), 400 + rnd.Next (20, 401), 0), Collide, Difficulty);
					break;
				}
				e.Player = Collide.P;
				Collide.AddEnemy = e;
				e.CurrentLevel = this;
				//e.Difficulty = Difficulty;
				
				EnemyCount++;
			}
		}
		
		public override void Drop(Enemy e)
		{
			int i = 0;
			Item j;
			bool picked = false;
			
			for(int d = 0; d < dropRate.Length; d++)
			{
				if(!picked)
				{
					for(int o = 0; o < dropRate[d]; o++)
					{
						int n = rnd.Next (0, DropRange);
						if(n == DropRange - 3)
						{
							i = d + 1;
							picked = true;
						}
					}
				}
			}
				
			
			switch(i)
			{
			case 0:
				break;
			case 1:
				j = new MGObject(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 2:
				j = new ShotObject(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 3:
				j = new RifleObject(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 4:
				j = new RPGObject(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 5:
				j = new MGAmmo(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 6:
				j = new ShotgunAmmo(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 7:
				j = new RifleAmmo(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 8:
				j = new RPGAmmo(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 9:
				j = new Health(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			default:
				break;
			}
			
		}
		
		public override void NewGame ()
		{
			Collide.PurgeAssets ();
	
			Item mgo;
			
			int rand = rnd.Next(0, 5);
			switch(rand)
			{
			case 0:
				mgo = new RPGObject (Graphics, new Vector3 (rnd.Next (0, Graphics.Screen.Rectangle.Width), rnd.Next (0, Graphics.Screen.Rectangle.Height), 0), Collide);
				break;
			case 1:
				mgo = new RifleObject (Graphics, new Vector3 (rnd.Next (0, Graphics.Screen.Rectangle.Width), rnd.Next (0, Graphics.Screen.Rectangle.Height), 0), Collide);
				break;
			case 2:
				mgo = new MGObject (Graphics, new Vector3 (rnd.Next (0, Graphics.Screen.Rectangle.Width), rnd.Next (0, Graphics.Screen.Rectangle.Height), 0), Collide);
				break;
			case 3:
				mgo = new ShotObject (Graphics, new Vector3 (rnd.Next (0, Graphics.Screen.Rectangle.Width), rnd.Next (0, Graphics.Screen.Rectangle.Height), 0), Collide);
				break;
			case 4:
				mgo = new BoidObject (Graphics, new Vector3 (rnd.Next (0, Graphics.Screen.Rectangle.Width), rnd.Next (0, Graphics.Screen.Rectangle.Height), 0), Collide);
				break;
			default:
				mgo = new MGObject (Graphics, new Vector3 (rnd.Next (0, Graphics.Screen.Rectangle.Width), rnd.Next (0, Graphics.Screen.Rectangle.Height), 0), Collide);
				break;
			}
			
			Collide.AddItem = mgo;
			
			mgo = new MGObject (Graphics, new Vector3 (rnd.Next (0, Graphics.Screen.Rectangle.Width), rnd.Next (0, Graphics.Screen.Rectangle.Height), 0), Collide);
			Collide.AddItem = mgo;
			
			Plr.Position = new Vector3(20, 20, 0);
			
			//Spawn initial enemies
			EnemyCount = 0;
			Console.WriteLine(LevelDifficulty + ", " + Difficulty);
			LevelDifficulty = InfinityGen (LevelDifficulty);
			GC.Collect();
		}
		
		private Vector3 RandomVector()
		{
			Vector3 v = Vector3.Zero;
			
			v.X = rnd.Next(100, Graphics.Screen.Rectangle.Width);
			v.Y = rnd.Next(100, Graphics.Screen.Rectangle.Height);
			
			return v;
		}
		
		public int InfinityGen (int diff)
		{
			int startDiff = diff;
			int currDiff = 0;
			Random rand = new Random ();
			Enemy e;
			do {
				int x = rand.Next (0, 12);
				switch (x) {
				case 0:
					e = new Zombie (Graphics, RandomVector(), Collide, Difficulty);
					e.CurrentLevel = this;
					break;
				case 1:
					e = new Zombie (Graphics, RandomVector(), Collide, Difficulty);
					e.CurrentLevel = this;
					break;
				case 2:
					e = new Zombie (Graphics,RandomVector(), Collide, Difficulty);
					e.CurrentLevel = this;
					break;
				case 3:
					e = new Boomer (Graphics, RandomVector(), Collide, Difficulty);
					e.CurrentLevel = this;
					break;
				case 4:
					e = new Boomer (Graphics, RandomVector(), Collide, Difficulty);
					e.CurrentLevel = this;
					break;
				case 5:
					e = new Blade (Graphics, RandomVector(), Collide, Difficulty);
					e.CurrentLevel = this;
					break;
					
				case 6:
					e = new Boomer (Graphics, RandomVector(), Collide, Difficulty);
					e.CurrentLevel = this;
					break;
				case 7:
					e = new Blade (Graphics, RandomVector(), Collide, Difficulty);
					e.CurrentLevel = this;
					break;
				case 8:
					e = new ZombieBoss (Graphics, RandomVector(), Collide, Difficulty);
					e.CurrentLevel = this;
					break;
				case 9:
					e = new Zombie (Graphics, RandomVector(), Collide, Difficulty);
					e.CurrentLevel = this;
					break;
				case 10:
					e = new Blade (Graphics, RandomVector(), Collide, Difficulty);
					e.CurrentLevel = this;
					break;
				case 11:
					e = new Boomer (Graphics, RandomVector(), Collide, Difficulty);
					e.CurrentLevel = this;
					break;
				default:
					e = new BoomerBoss (Graphics, RandomVector(), Collide, Difficulty);
					e.CurrentLevel = this;
					break;
				}
				Collide.AddEnemy = e;
				EnemyCount++;
				currDiff += SumDiff ();
				if(currDiff > startDiff)
					break;
			}
			while (EnemyCount < MaxEnemies);
			return (int)(currDiff * 1.0);
		}
			
		private int SumDiff ()
		{
			int sum = 0;
			foreach (Enemy e in Collide.Enemies) {
				sum += e.LiteralDifficulty;
			}
			return sum;
		}
	}
}

