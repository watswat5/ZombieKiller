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
	//A level can have a specific background, difficulty, enemy set, drop rate, etc.
	public abstract class Level
	{
		private GraphicsContext graphics;
		private Collisions col;
		private Texture2D tex;
		public Sprite p;
		public static Random rnd = new Random();
		private string name;
		private int diff;
		private int maxEnemies;
		private int enemyCount;
		private int dropRange;
		private bool finished;
		private Player plr;
		public Player Plr
		{
			get { return plr;}
			set { plr = value;}
		}
		public bool Finished
		{
			get { return finished;}
			set { finished = value;}
		}
		
		public GraphicsContext Graphics
		{
			get { return graphics;}
		}
		
		public Collisions Collide
		{
			get { return col;}
			set { col = value;}
		}
		
		public string Name
		{
			get {return name;}
			set {name = value;}
		}
		
		public int Difficulty
		{
			get {return diff;}
			set {diff = value;}
		}
		
		public int MaxEnemies
		{
			get {return maxEnemies;}
			set {maxEnemies = value;}
		}
		
		public int EnemyCount
		{
			get {return enemyCount;}
			set {enemyCount = value;}
		}
		
		public int DropRange
		{
			get {return dropRange;}
			set {dropRange = value;}
		}
		
		//First term is ammo drop rate, second is weapon drop rate. Higher # = Higher % drop rate.
		//From L to R: MG, Shotgun, Rifle, RPG, Shield (Boid Gun). First is health.  
		public static int[] dropRate = {20, 20, 20, 20, 5, 5, 5, 5, 15};
		
		public Level (GraphicsContext g, Texture2D t, Collisions col, int d, int max, int drprng, string name, Player plr)
		{
			graphics = g;
			tex = t;
			diff = d;
			maxEnemies = max;
			dropRange = drprng;
			this.name = name;
			p = new Sprite(graphics, tex);
			//p.Scale = new Vector2(2f, 2f);
			p.Width = graphics.Screen.Rectangle.Width;
			p.Height = graphics.Screen.Rectangle.Height;
			this.plr = plr;
			this.col = col;
		}
		
		public virtual void Render()
		{
			p.Render();
		}
		
		public abstract void Update();
		
		//Spawns enemies until the max count is reached.
		public abstract void SpawnEnemies();
		public abstract void NewGame();
		public abstract void Drop(Enemy e);
	}
}

