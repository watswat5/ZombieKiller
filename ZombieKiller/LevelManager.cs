using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.UI;

namespace ZombieKiller
{
	public class LevelManager
	{
		private GraphicsContext graphics;
		private Collisions collisions;
		private Random rnd;
		private Player plr;
		private Queue<Level> levels;
		private List<Texture2D> backgrounds;
		private Level currentLevel;
		private bool infinite;

		public bool InfiniteMode {
			get { return infinite;}
			set { infinite = value;}
		}
		
		public bool Finished {
			get { return currentLevel.Finished;}
		}
		
		public bool EndGame {
			get { return levels.Count <= 0;}	
		}
		
		public int Health {
			get { return plr.Health;}	
		}
		
		public LevelManager (GraphicsContext g, Collisions c)
		{
			graphics = g;
			collisions = c;
			plr = c.P;
			
			backgrounds = new List<Texture2D> ();
			
			backgrounds.Add (new Texture2D ("/Application/Assets/Levels/background.png", false));
			backgrounds.Add (new Texture2D ("/Application/Assets/Levels/lava.png", false));
			backgrounds.Add (new Texture2D ("/Application/Assets/Levels/grassfield.png", false));
			backgrounds.Add (new Texture2D ("/Application/Assets/Levels/sandlot.png", false));
			
			levels = new Queue<Level> ();
			
			rnd = new Random ();
		}
		
		public void Initialize (int i)
		{
			levels = new Queue<Level> ();
			Setup (i);
		}
		
		public void Initialize (Queue<Level> l)
		{
			levels = l;
			currentLevel = levels.Dequeue ();
		}
		
		public void Initialize ()
		{
			infinite = true;
			;
			Setup ();
		}
		
		private void Setup (int i)
		{
			int dropRange, maxEnemies, levelDiff, texNum; 
			for (int l = 0; l < i; l++) {
				dropRange = rnd.Next (50, 301);
				maxEnemies = rnd.Next (5, 50);
				levelDiff = 2;//rnd.Next(1, 5);
				texNum = rnd.Next (0, backgrounds.Count);
				RandomLevel randL = new RandomLevel (graphics, collisions, backgrounds [texNum], levelDiff, maxEnemies, dropRange);
				levels.Enqueue (randL);
			}
			
			currentLevel = levels.Dequeue ();
		}
		
		private void Setup ()
		{
			int dropRange, maxEnemies, levelDiff, texNum; 
			dropRange = rnd.Next (50, 301);
			maxEnemies = rnd.Next (5, 50);
			levelDiff = 2;//rnd.Next(1, 5);
			texNum = rnd.Next (0, backgrounds.Count);
			RandomLevel randL = new RandomLevel (graphics, collisions, backgrounds [texNum], levelDiff, maxEnemies, dropRange);
			currentLevel = randL;
		}
		
		public void NextLevel ()
		{
			if (infinite) {
				Setup ();
				currentLevel.NewGame ();
			} else {
				if (levels.Count > 0) {
					currentLevel = levels.Dequeue ();
					currentLevel.NewGame ();	
				} else
					return;
			}
		}
		
		public void NewGame ()
		{
			currentLevel.NewGame ();	
		}
		
		public int infinityGen (int diff)
		{
			int startDiff = diff;
			int currDiff = 0;
			Random rand = new Random ();
			while (currDiff < startDiff) {
				int x = rand.Next (0, 5);
				switch (x) {
				case 0:
					collisions.AddEnemy = new Zombie (graphics, new Vector3 
					                              (rand.Next (graphics.Screen.Rectangle.Width / 2),
					 								rand.Next (graphics.Screen.Rectangle.Width + 50),
					 								0), collisions, startDiff);
					break;
				case 1:
					collisions.AddEnemy = new Boomer (graphics, new Vector3 
					                              (rand.Next (graphics.Screen.Rectangle.Width / 2),
					 								rand.Next (graphics.Screen.Rectangle.Width + 50),
					 								0), collisions, startDiff);
					;
					break;
				case 2:
					collisions.AddEnemy = new Blade (graphics, new Vector3 
					                              (rand.Next (graphics.Screen.Rectangle.Width / 2),
					 								rand.Next (graphics.Screen.Rectangle.Width + 50),
					 								0), collisions, startDiff);
					break;
				case 3:
					collisions.AddEnemy = new ZombieBoss (graphics, new Vector3 
					                              (rand.Next (graphics.Screen.Rectangle.Width / 2),
					 								rand.Next (graphics.Screen.Rectangle.Width + 50),
					 								0), collisions, startDiff);
					break;
				case 4:
					collisions.AddEnemy = new BoomerBoss (graphics, new Vector3 
					                              (rand.Next (graphics.Screen.Rectangle.Width / 2),
					 								rand.Next (graphics.Screen.Rectangle.Width + 50),
					 								0), collisions, startDiff);
					break;
				}
				currDiff += SumDiff ();
			}
			return (int)(SumDiff () * 1.1);
		}
			
		private int SumDiff ()
		{
			int sum = 0;
			foreach (Enemy e in collisions.Enemies) {
				sum += e.Difficulty;
			}
			return sum;
		}
		
		public void Update (long Delta, GamePadData gp)
		{
			currentLevel.Update ();	
			collisions.Update (Delta, gp);
		}
		
		public void Render (long Delta)
		{
			currentLevel.Render ();
			collisions.Render (Delta);
		}
	}
}

