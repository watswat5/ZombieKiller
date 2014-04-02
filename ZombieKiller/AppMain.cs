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

//Chris Antepenko & C. Blake Becker

namespace ZombieKiller
{
	public class AppMain
	{
		private enum GameState
		{
			Playing,
			Paused,
			Dead,
			Menu,
			Controls,
			Upgrade,
			Winner,
			HighScoreEntry,
			HighScoreView
		};
		
		private static bool Running;
		private static GameState currentState;
		private static GraphicsContext graphics;
		public static Collisions collisions;
		private static Random rnd;
		private static Upgrade upgrade;
		private static LevelManager lvlMan;
		//Timer
		private static Stopwatch clock;
		private static long StartTime;
		private static long EndTime;
		private static long DeltaTime;
		
		//Limits maximum enemies due to memory limitations
		private const int MAX_ENEMIES = 60;
		private static Sprite winner;
		private static BgmPlayer bgMusic;
		private static Sprite paused;
		private static Sprite menu;
		private static Sprite dead;
		private static Sprite controls;
		private static Player Plr;
		private static Queue<Level> levels;
		private static Bgm death, win, bgm;
		private static Keyboard k;
		private static List<HighScore> highScores;
		private static Scene s;
		private static Label hScores;
		
		public static void Main (string[] args)
		{
			Initialize ();

			while (Running) {
				StartTime = clock.ElapsedMilliseconds;
				SystemEvents.CheckEvents ();
				Update ();
				Render ();
				EndTime = clock.ElapsedMilliseconds;
				DeltaTime = EndTime - StartTime;
			}
		}

		public static void Initialize ()
		{
			// Set up the graphics system
			graphics = new GraphicsContext ();
			
			Running = true;
			
			UISystem.Initialize (graphics);
			
			StringWriter sw = new StringWriter ();
			//sr = new StreamReader("highscores.txt");
			
			rnd = new Random ();
			clock = new Stopwatch ();
			clock.Start ();
			
			s = new Scene ();
			
			hScores = new Label ();
			hScores.Height = 300;
			
			s.RootWidget.AddChildLast (hScores);

			highScores = new List<HighScore> ();
			
			k = new Keyboard(graphics, 295);
			
			float Height = graphics.Screen.Rectangle.Height;
			float Width = graphics.Screen.Rectangle.Width;
			
			//Backgrounds for different gamestataes
			paused = new Sprite (graphics, new Texture2D ("/Application/Assets/paused.png", false));
			menu = new Sprite (graphics, new Texture2D ("/Application/Assets/title.png", false));
			dead = new Sprite (graphics, new Texture2D ("/Application/Assets/deadscreen.png", false));
			winner = new Sprite (graphics, new Texture2D ("/Application/Assets/winner.png", false));
			controls = new Sprite (graphics, new Texture2D ("/Application/Assets/controls.png", false));
			
			paused.Height = Height;
			paused.Width = Width;
			
			menu.Height = Height;
			menu.Width = Width;
			
			dead.Height = Height;
			dead.Width = Width;
			
			winner.Height = Height;
			winner.Width = Width;
			
			controls.Height = Height;
			controls.Width = Width;
			
			//Music
			bgm = new Bgm ("/Application/Assets/Sounds/bg.mp3");
			bgMusic = bgm.CreatePlayer ();
			bgMusic.Loop = true;
			bgMusic.Volume = 0.1f;
			bgMusic.Play ();
			
			win = new Bgm ("/Application/Assets/Sounds/win.mp3");
			death = new Bgm ("/Application/Assets/Sounds/dead.mp3");
			
			upgrade = new Upgrade(graphics);
			
			//Set up a new game
			//NewGame ();
			NewGame ();
		}
		
		//Generates the objects for a new game
		public static void NewGame ()
		{
			//New collisions object replaces any old ones, freeing memory.
			collisions = new Collisions (graphics);
			
			Plr = new Player (graphics, new Vector3 (20, 20, 0), collisions);
			collisions.P = Plr; 
			
			//upgrade = new Upgrade (graphics);
			upgrade.Plr = Plr;
			
			k.Reset();			
			//Init levels
//			levels = new Queue<Level> ();
//			levels.Enqueue (new LevelOne (graphics, collisions, Plr));
//			levels.Enqueue (new LevelTwo (graphics, collisions, Plr));
//			levels.Enqueue (new LevelThree (graphics, collisions, Plr));	
//			levels.Enqueue (new LevelFour (graphics, collisions, Plr));	
//			levels.Enqueue (new LevelFive (graphics, collisions, Plr));
			
			lvlMan = new LevelManager(graphics, collisions);
			lvlMan.Initialize();
			
			
			//Load menu
			currentState = GameState.Menu;
			
			if(highScores.Count < 5)
			{
				highScores = new List<HighScore>();
				ReadHighScores();
				highScores.Sort();
				highScores.Reverse();
				foreach(HighScore h in highScores)
					Console.WriteLine(h);
			}
		}
		
		public static void ReadHighScores ()
		{
			if(!File.Exists("/Documents/highscores.txt"))
			{
				using(StreamWriter writer = new StreamWriter("/Documents/highscores.txt", true))
				{
					for(int i = 0; i < 5; i++)
					{
				  		writer.WriteLine("AAA,000");
					}
					writer.Close();
				}
			}
			
			StreamReader sr = new StreamReader ("/Documents/highscores.txt");
			
			while (!sr.EndOfStream) {
				highScores.Add (new HighScore (sr.ReadLine ()));
			}
			sr.Close ();
		}
		
		public static void WriteHighScores ()
		{
			StreamWriter sw = new StreamWriter("/Documents/highscores.txt");
			highScores.Sort();
			highScores.Reverse();
			foreach(HighScore h in highScores)
			{
		  		sw.WriteLine(h);
			}
			sw.Close();
		}
		
		public static void Update ()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData (0);
		
			switch (currentState) {
			case GameState.Playing:
				UpdatePlaying (gamePadData);
				break;	
			case GameState.Paused:
				UpdatePaused (gamePadData);
				break;
			case GameState.Menu:
				UpdateMenu (gamePadData);
				break;
			case GameState.Dead:
				UpdateDead (gamePadData);
				break;
			case GameState.Controls:
				UpdateControls (gamePadData);
				break;
			case GameState.Upgrade:
				UpdateUpgrade (gamePadData);
				break;
			case GameState.Winner:
				UpdateWinner (gamePadData);
				break;
			case GameState.HighScoreEntry:
				UpdateHighScoreEntry (gamePadData);
				break;
			case GameState.HighScoreView:
				UpdateHighScoreView (gamePadData);
				break;
			}
			//Console.WriteLine(DeltaTime);
		}
		
		//Update Ingame
		public static void UpdatePlaying (GamePadData gamePadData)
		{
			if (lvlMan.Finished)
				currentState = GameState.Upgrade;
			lvlMan.Update (DeltaTime, gamePadData);
			if ((gamePadData.ButtonsDown & GamePadButtons.Start) != 0) {
				currentState = GameState.Paused;
				bgMusic.Volume = 0.1f;
			}
			if (lvlMan.Health <= 0) {
				bgMusic.Dispose ();
				bgMusic = death.CreatePlayer ();
				bgMusic.Volume = 0.3f;
				bgMusic.Loop = true;
				bgMusic.Play ();
				currentState = GameState.Dead;
			}
		}
		
		//Update Paused
		public static void UpdatePaused (GamePadData gamePadData)
		{
			if ((gamePadData.ButtonsDown & GamePadButtons.Start) != 0) {
				currentState = GameState.Playing;
				bgMusic.Volume = .25f;
			}
		}
		
		//Update Death Screen
		public static void UpdateDead (GamePadData gamePadData)
		{
			if ((gamePadData.ButtonsDown & GamePadButtons.Start) != 0) {
				bgMusic.Dispose ();
				bgMusic = bgm.CreatePlayer ();
				bgMusic.Volume = 0.1f;
				bgMusic.Loop = true;
				bgMusic.Play ();
				highScores.Sort();
				if(Plr.Score < highScores[0].Score)
					currentState = GameState.HighScoreView;
				else
					currentState = GameState.HighScoreEntry;
			}
		}
		
		//Update Main Menu
		public static void UpdateMenu (GamePadData gamePadData)
		{
			//Start game
			if ((gamePadData.ButtonsDown & GamePadButtons.Start) != 0) {
				lvlMan.NewGame (); 
				currentState = GameState.Playing;
				bgMusic.Volume = .25f;
			} 
			//Load controls
			else if ((gamePadData.ButtonsDown & GamePadButtons.Square) != 0) {
				currentState = GameState.Controls;
			} 
			//Quit
			else if ((gamePadData.ButtonsDown & GamePadButtons.Select) != 0) {
				bgMusic.Dispose ();
				Running = false;
			}
		}
		
		//Update Controls Menu
		public static void UpdateControls (GamePadData gamePadData)
		{
			if ((gamePadData.ButtonsDown & GamePadButtons.Start) != 0) {
				currentState = GameState.Menu;
				bgMusic.Volume = .25f;
			}
		}
		
		//Update Upgrade Menu
		public static void UpdateUpgrade (GamePadData gamePadData)
		{
			upgrade.Update (gamePadData);
			
			if (!lvlMan.InfiniteMode && lvlMan.EndGame) {
				currentState = GameState.Winner;
				bgMusic.Dispose ();
				bgMusic = win.CreatePlayer ();
				bgMusic.Volume = 0.3f;
				bgMusic.Loop = true;
				bgMusic.Play ();	
			} else if ((gamePadData.ButtonsDown & GamePadButtons.Start) != 0) {
				currentState = GameState.Playing;
				lvlMan.NextLevel();
				lvlMan.NewGame ();
				bgMusic.Volume = .25f;
			}
		}
		
		//Update Winner Screen
		public static void UpdateWinner (GamePadData gp)
		{
			if ((gp.ButtonsDown & GamePadButtons.Start) != 0) {
				highScores.Sort();
				if(Plr.Score > highScores[0].Score)
					currentState = GameState.HighScoreEntry;
				else
					currentState = GameState.HighScoreView;
				
			}
		}
		
		//Update High Score Entry Screen
		public static void UpdateHighScoreEntry (GamePadData gp)
		{
			k.Update (gp);
//			string s = "";
//			foreach (HighScore a in highScores)
//				s = s + "\n" + a;
//			hScores.Text = s;
			if (k.Finished) {	
				highScores.Sort ();
				highScores.RemoveAt(0);
				highScores.Add (new HighScore (k.ReturnResult () + "," + Plr.Score));
				WriteHighScores();
				hScores.Text = "";
				currentState = GameState.HighScoreView;	
			}
		}
		
		//Update High Score View Screen
		public static void UpdateHighScoreView (GamePadData gp)
		{
			if(hScores.Text.Equals(""))
			{
				highScores.Sort();
				string s = "";
				for (int a = highScores.Count - 1; a >= 0; a--)
					s = s + "\n" + highScores[a];
				hScores.Text = s;
				hScores.TextColor = new UIColor(1,1,1,1);
			}
			if((gp.ButtonsDown & GamePadButtons.Start) != 0)
			{
				bgMusic.Dispose ();
				bgMusic = bgm.CreatePlayer ();
				bgMusic.Volume = 0.1f;
				bgMusic.Loop = true;
				bgMusic.Play ();
				hScores.Text = "";
				currentState = GameState.Menu;	
				NewGame ();		
			}
		}
		
		//Render In game
		public static void RenderPlaying ()
		{
			lvlMan.Render (DeltaTime);
			
			//This is where bullet and enemy collisions are rendered
			//levels [currentLevel].Collide.Render (DeltaTime);
			
			
		}
		
		//Render Pause menu
		public static void RenderPaused ()
		{
//			levels [currentLevel].Render ();
//			//This is where bullet and enemy collisions are calculated and rendered
//			collisions.Render (DeltaTime);
			lvlMan.Render (DeltaTime);			
			//Renders current weapon only
			paused.Render ();
			
		}
		
		//Render death screen
		public static void RenderDead ()
		{
//			levels [currentLevel].Render ();
//			//This is where bullet and enemy collisions are calculated and rendered
//			levels [currentLevel].Collide.Render (DeltaTime);
			
			lvlMan.Render (DeltaTime);
			
			//Renders current weapon only
			dead.Render ();	
		}
		
		//Render Main Menu
		public static void RenderMenu ()
		{
			//levels [currentLevel].p.Render ();
			menu.Render ();			
		}
		//Render Controls Menu
		public static void RenderControls ()
		{
			//levels [currentLevel].p.Render ();
			controls.Render ();			
		}
		
		//Render Upgrade Menu
		public static void RenderUpgrade ()
		{
			//levels [currentLevel].p.Render ();
			upgrade.Render ();
			//controls.Render ();			
		}
		
		//Render Winner Menu
		public static void RenderWinner ()
		{
			winner.Render ();		
		}
		
		//Render High Score Menu
		public static void RenderHighScoreEntry ()
		{
			k.Render ();
//			UISystem.SetScene (s);
//			UISystem.Render ();
		}
		
		public static void RenderHighScoreView ()
		{
			UISystem.SetScene (s);
			UISystem.Render ();
		}
		
		//Cheat code
//		public static void Cheater (GamePadData gp)
//		{
//			up1 += DeltaTime;
//			up2 += DeltaTime;
//			down1 += DeltaTime;
//			down2 += DeltaTime;
//			left1 += DeltaTime;
//			right1 += DeltaTime;
//			left2 += DeltaTime;
//			right2 += DeltaTime;
//			circle += DeltaTime;
//			cross += DeltaTime;
//			if ((gp.ButtonsDown & GamePadButtons.Up) != 0)
//				up1 = 0;
//			if ((gp.ButtonsDown & GamePadButtons.Up) != 0 && up1 < 500)
//				up2 = 0;
//			if ((gp.ButtonsDown & GamePadButtons.Down) != 0 && up2 < 500)
//				down1 = 0;
//			if ((gp.ButtonsDown & GamePadButtons.Down) != 0 && down1 < 500)
//				down2 = 0;
//			if ((gp.ButtonsDown & GamePadButtons.Left) != 0 && down2 < 500)
//				left1 = 0;
//			if ((gp.ButtonsDown & GamePadButtons.Right) != 0 && left1 < 500)
//				right1 = 0;
//			if ((gp.ButtonsDown & GamePadButtons.Left) != 0 && right1 < 500)
//				left2 = 0;
//			if ((gp.ButtonsDown & GamePadButtons.Right) != 0 && left2 < 500)
//				right2 = 0;
//			if ((gp.ButtonsDown & GamePadButtons.Circle) != 0 && right2 < 500)
//				circle = 0;
//			if ((gp.ButtonsDown & GamePadButtons.Cross) != 0 && circle < 500) {
//				up1 = 1000;
//				up2 = 1000;
//				down1 = 1000;
//				down2 = 1000;
//				left1 = 1000;
//				right1 = 1000;
//				left2 = 1000;
//				right2 = 1000;
//				circle = 1000;
//				cross = 1000;
//				cheated = true;
//				Console.WriteLine ("CHEATED!");
//				//player.Weapons.Add (wep4);
////				weaponSelect = 3;
//			}
//		}
		
		public static void Render ()
		{
			// Clear the screen
			graphics.SetClearColor (0.5f, 0.5f, 0.5f, 0.0f);
			graphics.Clear ();
			
			switch (currentState) {
			case GameState.Playing:
				RenderPlaying ();
				break;	
			case GameState.Paused:
				RenderPaused ();
				break;
			case GameState.Menu:
				RenderMenu ();
				break;
			case GameState.Dead:
				RenderDead ();
				break;
			case GameState.Controls:
				RenderControls ();
				break;
			case GameState.Upgrade:
				RenderUpgrade ();
				break;
			case GameState.Winner:
				RenderWinner ();
				break;
			case GameState.HighScoreEntry:
				RenderHighScoreEntry ();
				break;
			case GameState.HighScoreView:
				RenderHighScoreView ();
				break;
			}
			
			// Present the screen
			graphics.SwapBuffers ();
		}
	}
}
