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
		public LevelThree (GraphicsContext g, Collisions c, Player plr) : base(g, new Texture2D("/Application/Assets/Levels/background.png", false), c, 2, 20, 50, "Level One", plr)
		{
			MaxEnemies = 60;
			c.P = plr;
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
			for (int i = 0; i < MaxEnemies - EnemyCount; i++) {
				Enemy e;
				e = new Blade (Graphics, new Vector3 (400 + rnd.Next (200, 400), 450 + rnd.Next (-400, 401), 0), Collide, Difficulty);
				e.Player = Collide.P;
				//e.Difficulty = Difficulty;
				Collide.AddEnemy = e;
				EnemyCount++;
			}
		}
		
		public override void NewGame ()
		{
//			//Collision Detection for Enemies and Bullets
//			Collide = new Collisions (Graphics);
//			//Player
//			Collide.P = Plr;
			Collide.PurgeAssets();
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

