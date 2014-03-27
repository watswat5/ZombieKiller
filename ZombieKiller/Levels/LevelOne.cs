using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using System.Diagnostics;
using Sce.PlayStation.Core.Audio;
//Chris Antepenko
namespace ZombieKiller
{
	public class LevelOne : Level
	{
		public LevelOne (GraphicsContext g, Collisions c, Player plr) : base(g, new Texture2D("/Application/Assets/Levels/background.png", false), c, 1, 40, 50, "Level One", plr)
		{		
			c.P = plr;
			p.Scale = new Vector2(2,2);
		}
		
		public override void Update()
		{
			if(Collide.Enemies.Count <= 0)
			{
				Finished = true;	
			}
		}
		
		public override void SpawnEnemies ()
		{
			for (int i = 0; i < MaxEnemies; i++) {
				Enemy e;
				e = new Zombie (Graphics, new Vector3 (400 + rnd.Next (200, 400), 0+  rnd.Next (0, 401), 0), Collide, Difficulty);
				e.Player = Collide.P;
				e.CurrentLevel = this;
				
				Collide.AddEnemy = e;
				EnemyCount++;
				Console.WriteLine(Collide.Enemies.Count);
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
						int n = rnd.Next (0, 101);
						if(n == 50)
						{
							i = d;
							picked = true;
						}
					}
				}
			}
				
			
			switch(i)
			{
			case 0:
				Console.WriteLine("NULL");
				break;
			case 1:
				j = new Health(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 2:
				j = new MGObject(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 3:
				j = new MGAmmo(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 4:
				j = new ShotObject(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 5:
				j = new ShotgunAmmo(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 6:
				j = new RifleObject(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 7:
				j = new RifleAmmo(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 8:
				j = new RPGObject(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 9:
				j = new RPGAmmo(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			}
			
		}
		
		public override void NewGame ()
		{
//			//Collision Detection for Enemies and Bullets
//			Collide = new Collisions (Graphics);
//			//Player
//			Collide.P = Plr;
			Collide.PurgeAssets();
			Item mgo = new MGObject (Graphics, new Vector3 (100, 100, 0), Collide);
			Collide.AddItem = (mgo);
			
			Plr.Position = new Vector3(20, 20, 0);
			
			//Spawn initial enemies
			EnemyCount = 0;
			SpawnEnemies ();
		}
	}
}

