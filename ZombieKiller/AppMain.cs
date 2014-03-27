using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.UI;

/*Chris Antepenko*/

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
			HighScore
		};
		
		private static GameState currentState;
		private static GraphicsContext graphics;
		public static Collisions collisions;
		private static Random rnd;
		private static Upgrade upgrade;
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
		private static List<Level> levels;
		private static int currentLevel;
		
		private static Bgm death, win, bgm;
		
		private static Keyboard k;
		
		private static List<HighScore> highScores;
		private static StringWriter sw;
		private static StreamReader sr;
		
		private static Scene s;
		private static Label hScores;
		
		public static void Main (string[] args)
		{
			Initialize ();

			while (true) {
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
			
			UISystem.Initialize(graphics);
			
			sw = new StringWriter();
			//sr = new StreamReader("highscores.txt");
			
			rnd = new Random ();
			clock = new Stopwatch ();
			clock.Start ();
			
			s = new Scene();
			
			hScores = new Label();
			hScores.Height = 300;
			
			s.RootWidget.AddChildLast(hScores);

			highScores = new List<HighScore>();
			
			//Backgrounds for different gamestataes
			paused = new Sprite (graphics, new Texture2D ("/Application/Assets/paused.png", false));
			menu = new Sprite (graphics, new Texture2D ("/Application/Assets/title.png", false));
			dead = new Sprite (graphics, new Texture2D ("/Application/Assets/deadscreen.png", false));
			winner = new Sprite (graphics, new Texture2D ("/Application/Assets/winner.png", false));
			controls = new Sprite (graphics, new Texture2D ("/Application/Assets/controls.png", false));
			
			//Music
			bgm = new Bgm ("/Application/Assets/Sounds/bg.mp3");
			bgMusic = bgm.CreatePlayer ();
			bgMusic.Loop = true;
			bgMusic.Volume = 0.1f;
			bgMusic.Play ();
			
			win = new Bgm("/Application/Assets/Sounds/win.mp3");
			death = new Bgm("/Application/Assets/Sounds/dead.mp3");
			
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
			upgrade = new Upgrade (graphics, Plr);
			
			k = new Keyboard(graphics, 337);

			upgrade.Plr = Plr;
			
			//Init levels
			levels = new List<Level> ();
			levels.Add (new LevelOne (graphics, collisions, Plr));
			levels.Add (new LevelTwo (graphics, collisions, Plr));
			levels.Add (new LevelThree (graphics, collisions, Plr));	
//			levels.Add (new LevelFour (graphics, collisions, Plr));	
//			levels.Add (new LevelFive (graphics, collisions, Plr));
			//Load menu
			currentState = GameState.Menu;
			currentLevel = 0;
			
//			ReadHighScores();
//			foreach(HighScore h in highScores)
//				Console.WriteLine(h);
		}
		
		public static void ReadHighScores()
		{
//			if(!File.Exists("/highscores.txt"))
//			{
//				StreamWriter sw = new StreamWriter("/highscores.txt");
//				sw.Close();
//			}
			
			sr = new StreamReader("highscores.txt");
			
			while(!sr.EndOfStream)
			{
				highScores.Add(new HighScore(sr.ReadLine()));
			}
			sr.Close();
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
				UpdateWinner(gamePadData);
				break;
			case GameState.HighScore:
				UpdateHighScore(gamePadData);
				break;
			}
			//Console.WriteLine(DeltaTime);
		}
		
		//Update Ingame
		public static void UpdatePlaying (GamePadData gamePadData)
		{
			if (levels [currentLevel].Finished)
				currentState = GameState.Upgrade;
			levels [currentLevel].Collide.Update (DeltaTime, gamePadData);
			levels [currentLevel].Update ();
			
			if ((gamePadData.ButtonsDown & GamePadButtons.Start) != 0) {
				currentState = GameState.Paused;
				bgMusic.Volume = 0.1f;
			}
			if (levels [currentLevel].Plr.Health <= 0) {
				bgMusic.Dispose();
				bgMusic = death.CreatePlayer();
				bgMusic.Volume = 0.3f;
				bgMusic.Loop = true;
				bgMusic.Play();
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
				bgMusic.Dispose();
				bgMusic = bgm.CreatePlayer();
				bgMusic.Volume = 0.1f;
				bgMusic.Loop = true;
				bgMusic.Play();
				NewGame ();
			}
		}
		
		//Update Main Menu
		public static void UpdateMenu (GamePadData gamePadData)
		{
			if ((gamePadData.ButtonsDown & GamePadButtons.Start) != 0) {
				levels [currentLevel].NewGame (); 
				currentState = GameState.Playing;
				bgMusic.Volume = .25f;
			} else if ((gamePadData.ButtonsDown & GamePadButtons.Square) != 0) {
				currentState = GameState.Controls;
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
			
			if(currentLevel == levels.Count - 1)
			{
				currentState = GameState.Winner;
				bgMusic.Dispose();
				bgMusic = win.CreatePlayer();
				bgMusic.Volume = 0.3f;
				bgMusic.Loop = true;
				bgMusic.Play();	
			}
			else if ((gamePadData.ButtonsDown & GamePadButtons.Start) != 0) {
				currentState = GameState.Playing;
				//levels[currentLevel].Collide = null;
				currentLevel++;
				levels [currentLevel].NewGame ();
				//levels[currentLevel].Plr = levels[currentLevel - 1].Plr;
				bgMusic.Volume = .25f;
			}
		}
		
		//Update Winner Screen
		public static void UpdateWinner(GamePadData gp)
		{
			if((gp.ButtonsDown & GamePadButtons.Start) != 0)
			{
				currentState = GameState.HighScore;	
			}
		}
		
		//Update High Score Screen
		public static void UpdateHighScore(GamePadData gp)
		{
			k.Update(gp);
			string s = "";
			foreach(HighScore a in highScores)
				s = s + "\n" + a;
			hScores.Text = s;
			if(k.Finished)
			{			
				highScores.Add (new HighScore(k.ReturnResult() + "," + Plr.Score));
				
				bgMusic.Dispose();
				bgMusic = bgm.CreatePlayer();
				bgMusic.Volume = 0.1f;
				bgMusic.Loop = true;
				bgMusic.Play();
				currentState = GameState.Menu;	
				NewGame();
			}
		}
		
		//Render In game
		public static void RenderPlaying ()
		{
			levels [currentLevel].Render ();
			
			//Background
			//bg.Render ();
			
			//This is where bullet and enemy collisions are rendered
			levels [currentLevel].Collide.Render (DeltaTime);
			
			
		}
		
		//Render Pause menu
		public static void RenderPaused ()
		{
			levels [currentLevel].Render ();
			//This is where bullet and enemy collisions are calculated and rendered
			collisions.Render (DeltaTime);
			//Renders current weapon only
			paused.Render ();
			
		}
		
		//Render death screen
		public static void RenderDead ()
		{
			levels [currentLevel].Render ();
			//This is where bullet and enemy collisions are calculated and rendered
			levels [currentLevel].Collide.Render (DeltaTime);
			//Renders current weapon only
			dead.Render ();	
		}
		
		//Render Main Menu
		public static void RenderMenu ()
		{
			levels [currentLevel].p.Render ();
			menu.Render ();			
		}
		//Render Controls Menu
		public static void RenderControls ()
		{
			levels [currentLevel].p.Render ();
			controls.Render ();			
		}
		
		//Render Upgrade Menu
		public static void RenderUpgrade ()
		{
			levels [currentLevel].p.Render ();
			upgrade.Render ();
			//controls.Render ();			
		}
		
		//Render Winner Menu
		public static void RenderWinner ()
		{
			winner.Render ();		
		}
		
		//Render High Score Menu
		public static void RenderHighScore ()
		{
			k.Render ();
			UISystem.SetScene(s);
			UISystem.Render();
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
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
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
				RenderWinner();
				break;
			case GameState.HighScore:
				RenderHighScore();
				break;
			}
			
			// Present the screen
			graphics.SwapBuffers ();
		}
	}
}
