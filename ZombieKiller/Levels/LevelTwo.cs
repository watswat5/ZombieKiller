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
	public class LevelTwo : Level
	{
		public LevelTwo (GraphicsContext g, Collisions c, Player plr) : base(g, new Texture2D("/Application/Assets/Levels/lava.png", false), c, 2, 20, 50, "Level One", plr)
		{
			MaxEnemies = 60;
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
				e = new Boomer (Graphics, new Vector3 (400 + rnd.Next (200, 400), 450 + rnd.Next (-400, 401), 0), Collide, Difficulty);
				e.Player = Collide.P;
				e.CurrentLevel = this;
				//e.Difficulty = Difficulty;
				Collide.AddEnemy = e;
				EnemyCount++;
			}
		}
		
//		public override void Drop(Enemy e)
//		{
//			Plr.Money += e.Value;
//			Item it;
//			int rand = rnd.Next (1, DropRange);
//			List<Vector2> drops = new List<Vector2>();
//
//			for(int i = 0; i < dropRate.Length; i++)
//			{
//				if(dropRate[i] != 0)
//				{
//					if(rand%dropRate[i] == 0)
//					{
//						drops.Add(new Vector2(i, dropRate[i]));
//					}
//				}
//			}
//			
//			int max = 0;
//			for(int i = 0; i < drops.Count; i++)
//				if(drops[i].Y > drops[max].Y)
//					max = i;
//			
//			if(drops.Count != 0)
//			{
//				int x = (int)drops[max].X;
//				switch(x)
//				{
//				case 0:
//					it = new MGObject(Graphics, e.p.Position, Collide);
//					Collide.AddItem = it;
//					break;
//				case 1:
//					it = new MGAmmo(Graphics, e.p.Position, Collide);
//					Collide.AddItem = it;
//					break;
//				case 2:
//					it = new ShotObject(Graphics, e.p.Position, Collide);
//					Collide.AddItem = it;
//					break;
//				case 3:
//					it = new ShotgunAmmo(Graphics, e.p.Position, Collide);
//					Collide.AddItem = it;
//					break;
//				default:
//					break;
//				}
//			}
////			Item i;
////			if(rand%10== 0)
////				i = new MGObject(Graphics, pos, Collide);
////			else if(rand%5 == 0)
////				i = new MGAmmo(Graphics, pos, Collide);
////			else
////				return;
////			Collide.AddItem = i;
//		}
		
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
				j = new MGAmmo(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 2:
				j = new ShotgunAmmo(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 5:
				j = new MGObject(Graphics, e.Position, Collide);
				Collide.AddItem = j;
				break;
			case 6:
				j = new ShotObject(Graphics, e.Position, Collide);
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
//			//Collision Detection for Enemies and Bullets
//			Collide = new Collisions (Graphics);
//			//Player
//			Collide.P = Plr;
			Collide.PurgeAssets();
	
			Plr.Position = new Vector3(20,20,0);
			//Spawn initial enemies
			EnemyCount = 0;
			SpawnEnemies ();
		}
	}
}

