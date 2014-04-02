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
		//Variables
		#region
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
		
		private int levelCount;
		public int LevelCount
		{
			get { return levelCount;}	
		}
		
		private float Difficulty;
		
		private Scene s;
		private Label l;
		
		#endregion
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
			backgrounds.Add (new Texture2D ("/Application/Assets/Levels/lsd.png", false));
			backgrounds.Add (new Texture2D ("/Application/Assets/Levels/test.png", false));
			levels = new Queue<Level> ();
			
			levelCount = 1;
			
			rnd = new Random ();
			s = new Scene();
			l = new Label();
			l.SetPosition(5, 5);
			l.Width = 400;
			Difficulty = 1;
			l.Text = "Level " + LevelCount + ", Difficulty: " + Math.Round(Difficulty) ;
			s.RootWidget.AddChildLast(l);
		}
		
		public void Initialize (int i)
		{
			Difficulty = 1f;
			RandomLevel.LevelDifficulty = 10;
			levels = new Queue<Level> ();
			Setup (i);
		}
		
		public void Initialize (Queue<Level> l)
		{
			Difficulty = 1f;
			RandomLevel.LevelDifficulty = 10;
			levels = l;
			currentLevel = levels.Dequeue ();
		}
		
		public void Initialize ()
		{
			infinite = true;
			Difficulty = 1f;
			RandomLevel.LevelDifficulty = 10;
			Setup ();
		}
		
		private void Setup (int i)
		{
			int dropRange, maxEnemies, texNum; 
			for (int l = 0; l < i; l++) {
				Difficulty += 0.2f;
				dropRange = rnd.Next (50, 301);
				maxEnemies = rnd.Next (5, 50);
				texNum = rnd.Next (0, backgrounds.Count);
				RandomLevel randL = new RandomLevel (graphics, collisions, backgrounds [texNum], (int)Math.Round(Difficulty), maxEnemies, dropRange, "" + i);
				levels.Enqueue (randL);
			}
			
			currentLevel = levels.Dequeue ();
		}
		
		private void Setup ()
		{
			int dropRange, maxEnemies, texNum; 
			dropRange = rnd.Next (50, 301);
			maxEnemies = rnd.Next (5, 50);
			texNum = rnd.Next (0, backgrounds.Count);
			RandomLevel randL = new RandomLevel (graphics, collisions, backgrounds [texNum], (int)Math.Round(Difficulty), 60, dropRange, "" + levelCount);
			Difficulty += 0.2f;
			levelCount++;
			currentLevel = randL;
		}
		
		public void NextLevel ()
		{
			if (infinite) {
				Setup ();
				currentLevel.NewGame ();
				l.Text = "Level " + LevelCount + ", Difficulty: " + Math.Round(Difficulty) ;
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
			UISystem.SetScene(s);
			UISystem.Render();
		}
	}
}

