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
	public class LevelThree : Level
	{
		public LevelThree (GraphicsContext g, Collisions c, Player plr) : base(g, new Texture2D("/Application/Assets/Levels/sandlot.png", false), c, 2, 20, 50, "Level One", plr)
		{
			MaxEnemies = 30;
			c.P = plr;
			p.Scale = new Vector2(2f, 2f);
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
				e = new Blade (Graphics, new Vector3 (400 + rnd.Next (200, 400), 0 + rnd.Next (20, 401), 0), Collide, Difficulty);
				e.Player = Collide.P;
				e.CurrentLevel = this;
				//e.Difficulty = Difficulty;
				Collide.AddEnemy = e;
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
				case 4:
					it = new RifleObject(Graphics, e.p.Position, Collide);
					Collide.AddItem = it;
					break;
				case 5:
					it = new RifleAmmo(Graphics, e.p.Position, Collide);
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
	
			Plr.Position = new Vector3(20, 20, 0);
			
			//Spawn initial enemies
			EnemyCount = 0;
			SpawnEnemies ();
		}
	}
}

