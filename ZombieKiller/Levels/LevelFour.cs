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
	public class LevelFour : Level
	{
		public LevelFour (GraphicsContext g, Collisions c, Player plr) : base(g, new Texture2D("/Application/Assets/Levels/grassfield.png", false), c, 3, 20, 250, "Level Four", plr)
		{
			MaxEnemies = 60;
			c.P = plr;
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
					e = new Blade (Graphics, new Vector3 (400 + rnd.Next (200, 400), 0 + rnd.Next (20, 401), 0), Collide, Difficulty, this);
					break;
				case 1:
					e = new Boomer (Graphics, new Vector3 (400 + rnd.Next (200, 400), 400 + rnd.Next (20, 401), 0), Collide, Difficulty, this);
					break;
				case 2:
					e = new Boomer (Graphics, new Vector3 (400 + rnd.Next (200, 400), 400 + rnd.Next (20, 401), 0), Collide, Difficulty, this);
					break;
				case 3:
					e = new Zombie (Graphics, new Vector3 (400 + rnd.Next (200, 400), 400 + rnd.Next (20, 401), 0), Collide, Difficulty, this);
					break;	
				case 4:
					e = new Zombie (Graphics, new Vector3 (400 + rnd.Next (200, 400), 400 + rnd.Next (20, 401), 0), Collide, Difficulty, this);
					break;	
				default:
					e = new Zombie (Graphics, new Vector3 (400 + rnd.Next (200, 400), 400 + rnd.Next (20, 401), 0), Collide, Difficulty, this);
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
				Console.WriteLine("NULL 0");
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
				Console.WriteLine("NULL D");
				break;
			}
			
		}
		
		public override void NewGame ()
		{
			Collide.PurgeAssets ();
	
			Item mgo = new RPGObject (Graphics, new Vector3 (rnd.Next (0, Graphics.Screen.Rectangle.Width), rnd.Next (0, Graphics.Screen.Rectangle.Height), 0), Collide);
			Collide.AddItem = (mgo);
			
			Plr.Position = new Vector3(20, 20, 0);
			
			//Spawn initial enemies
			EnemyCount = 0;
			SpawnEnemies ();
		}
	}
}

