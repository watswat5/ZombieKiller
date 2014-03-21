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
	public class LevelFour : Level
	{
		public LevelFour (GraphicsContext g, Collisions c, Player plr) : base(g, new Texture2D("/Application/Assets/Levels/test.png", false), c, 3, 20, 50, "Level One", plr)
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
			for (int i = 0; i < MaxEnemies - EnemyCount; i++) {
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
			Plr.Money += e.Value;
			Item it;
			int rand = rnd.Next (1, DropRange);
			List<Vector2> drops = new List<Vector2>();

			for(int i = 0; i < dropRate.Length; i++)
			{
				if(dropRate[i] != 0)
				{
					if(rand%dropRate[i] == 0)
					{
						drops.Add(new Vector2(i, dropRate[i]));
					}
				}
			}
			
			int max = 0;
			for(int i = 0; i < drops.Count; i++)
				if(drops[i].Y > drops[max].Y)
					max = i;
			
			if(drops.Count != 0)
			{
				int x = (int)drops[max].X;
				switch(x)
				{
				case 0:
					it = new MGObject(Graphics, e.p.Position, Collide);
					Collide.AddItem = it;
					break;
				case 1:
					it = new MGAmmo(Graphics, e.p.Position, Collide);
					Collide.AddItem = it;
					break;
				case 2:
					it = new ShotObject(Graphics, e.p.Position, Collide);
					Collide.AddItem = it;
					break;
				case 3:
					it = new ShotgunAmmo(Graphics, e.p.Position, Collide);
					Collide.AddItem = it;
					break;
				default:
					break;
				}
			}
		}
		
		public override void NewGame ()
		{
			Collide.PurgeAssets ();
			Item mgo = new ShotObject (Graphics, new Vector3 (200, 200, 0), Collide);
			Collide.AddItem = mgo;
			mgo = new MGObject (Graphics, new Vector3 (100, 100, 0), Collide);
			Collide.AddItem = mgo;
			mgo = new RifleObject (Graphics, new Vector3 (150, 150, 0), Collide);
			Collide.AddItem = mgo;
			mgo = new RPGObject (Graphics, new Vector3 (80, 80, 0), Collide);
			Collide.AddItem = mgo;
	
			//Spawn initial enemies
			EnemyCount = 0;
			SpawnEnemies ();
		}
	}
}

