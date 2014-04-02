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
		
		private int Difficulty;
		
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
			Difficulty = 50;
			RandomLevel.LevelDifficulty = Difficulty;
			levels = new Queue<Level> ();
			Setup (i);
		}
		
		public void Initialize (Queue<Level> l)
		{
			Difficulty = 50;
			RandomLevel.LevelDifficulty = Difficulty;
			levels = l;
			currentLevel = levels.Dequeue ();
		}
		
		public void Initialize ()
		{
			infinite = true;
			Difficulty = 50;
			RandomLevel.LevelDifficulty = Difficulty;
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
			Difficulty = RandomLevel.LevelDifficulty;
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
			currentLevel.NewGame();
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

