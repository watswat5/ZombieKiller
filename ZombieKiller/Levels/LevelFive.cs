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
	public class LevelFive : Level
	{
		public LevelFive (GraphicsContext g, Collisions c, Player plr) : base(g, new Texture2D("/Application/Assets/Levels/grassfield.png", false), c, 3, 20, 50, "Level One", plr)
		{
			MaxEnemies = 10;
			c.P = plr;
			p.Scale = new Vector2 (2f, 2f);
		}
		
		public override void Update ()
		{
			if (Collide.Enemies.Count <= 0) {
				Finished = true;	
			}
		}
		
		public override void SpawnEnemies ()
		{
			Enemy e = new BoomerBoss (Graphics, new Vector3 (900, 500, 0), Collide, Difficulty); 
			e.CurrentLevel = this;
			Collide.AddEnemy = e;
			e = new ZombieBoss (Graphics, new Vector3 (100, 500, 0), Collide, Difficulty); 
			e.CurrentLevel = this;
			Collide.AddEnemy = e;
			e.CurrentLevel = this;
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
			Collide.PurgeAssets ();
	
			Plr.Position = new Vector3(20, 20, 0);
			
			//Spawn initial enemies
			EnemyCount = 0;
			SpawnEnemies ();
		}
	}
}

