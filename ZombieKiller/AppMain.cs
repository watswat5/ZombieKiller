using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using System.Diagnostics;
using Sce.PlayStation.Core.Audio;

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
			Controls
		};
		private static GameState currentState;
		private static GraphicsContext graphics;
		public static Collisions collisions;
		private static Random rnd;
		
		//Timer
		private static Stopwatch clock;
		private static long StartTime;
		private static long EndTime;
		private static long DeltaTime;
		
		//Cheat code variables
		private static bool cheated;
		private static long up1, up2, down1, down2, left1, right1, left2, right2, circle, cross;
		
		private static Player player;
		
//		private static Weapon wep;
//		private static Weapon wep2;
//		private static Weapon wep3;
//		private static Weapon wep4;
//		
//		private static int weaponSelect;
//		private static List<Weapon> weapons;
		
		//Limits maximum enemies due to memory limitations
		private const int MAX_ENEMIES = 60;
		private static int enemyCount;
		private static Sprite bg;
		private static BgmPlayer bgMusic;
		private static Sprite paused;
		private static Sprite menu;
		private static Sprite dead;
		private static Sprite controls;
		
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
			rnd = new Random ();
			clock = new Stopwatch ();
			clock.Start ();
			
			//Backgrounds for different gamestataes
			paused = new Sprite (graphics, new Texture2D ("/Application/Assets/paused.png", false));
			menu = new Sprite (graphics, new Texture2D ("/Application/Assets/title.png", false));
			dead = new Sprite (graphics, new Texture2D ("/Application/Assets/deadscreen.png", false));
			bg = new Sprite (graphics, new Texture2D ("/Application/Assets/background.png", false));
			controls = new Sprite(graphics, new Texture2D("/Application/Assets/controls.png", false));
			
			//Music
			Bgm bgm = new Bgm ("/Application/Assets/Sounds/bg.mp3");
			bgMusic = bgm.CreatePlayer ();
			bgMusic.Loop = true;
			bgMusic.Volume = 0.25f;
			bgMusic.Play ();
			
			//Set up a new game
			NewGame ();
			
			//Load menu
			currentState = GameState.Menu;
		}
		//Runs the methods neccesary to create a clean game.
		public static void NewGame ()
		{
			//Collision Detection for Enemies and Bullets
			collisions = new Collisions (graphics);
			//Player
			player = new Player (graphics, new Vector3(20,20,0), collisions); 
			collisions.P = player;
			
			Item mgo = new ShotObject(graphics, new Vector3(200,200,0), collisions);
			collisions.AddItem = mgo;
			mgo = new MGObject(graphics, new Vector3(100,100,0), collisions);
			collisions.AddItem = mgo;
			mgo = new RifleObject(graphics, new Vector3(150,150,0), collisions);
			collisions.AddItem = mgo;
			
			//Creating weapons
//			weapons = new List<Weapon> ();
//			weaponSelect = 0;
			
//			wep = new Shotgun (graphics, collisions, new Vector3(player.p.Position.X, player.p.Position.Y, 0), player.p.Rotation);
//			wep2 = new Rifle (graphics, collisions, new Vector3(player.p.Position.X, player.p.Position.Y, 0), player.p.Rotation);
//			wep3 = new MachineGun (graphics, collisions, new Vector3(player.p.Position.X, player.p.Position.Y, 0), player.p.Rotation);
//			wep4 = new AdminGun (graphics, collisions, new Vector3(player.p.Position.X, player.p.Position.Y, 0), player.p.Rotation);
//			
//			player.Weapons.Add (wep3);
//			player.Weapons.Add (wep2);
//			player.Weapons.Add (wep); 
			
//			player.currentWeapon = player.Weapons[0];
			
			//Spawn initial enemies
			enemyCount = 0;
			SpawnEnemies ();
			
			cheated = false;
			
			//Cheat code
			up1 = 0;
			up2 = 0;
			down1 = 0;
			down2 = 0;
			left1 = 0;
			right1 = 0;
			left2 = 0;
			right2 = 0;
			circle = 0;
			cross = 0;	
		}
		
		//Spawns enemies until the max count is reached.
		public static void SpawnEnemies ()
		{
			for (int i = 0; i < MAX_ENEMIES - enemyCount; i++) {
				Enemy e;
				int type = rnd.Next (0, 4);
				if (type == 0) {
					e = new Blade (graphics, new Vector3(400 + rnd.Next (-200, 100), 300 + rnd.Next (-200, 200), 0), collisions);	
				} else if (type == 1 || type == 2) {
					e = new Zombie (graphics, new Vector3(400 + rnd.Next (200, 400), 450 + rnd.Next (-400, 401), 0), collisions);
				} else {
					e = new Boomer (graphics, new Vector3(400 + rnd.Next (200, 400), 450 + rnd.Next (-400, 401), 0), collisions);		
				}
				e.Player = player;
				collisions.AddEnemy = e;
			}
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
			}
		}
		
		//Update Ingame
		public static void UpdatePlaying (GamePadData gamePadData)
		{
		
			collisions.Update (DeltaTime);
			player.Update (DeltaTime);
			
			enemyCount = collisions.enemyCount;
			player.GPData = gamePadData;
			
			if((gamePadData.ButtonsDown & GamePadButtons.Circle) != 0)
				SpawnEnemies();	
			
			if (!cheated)
				Cheater (gamePadData);
			
			if ((gamePadData.ButtonsDown & GamePadButtons.Start) != 0) {
				currentState = GameState.Paused;
				bgMusic.Volume = 0.1f;
			}
			if (player.Health <= 0) {
				currentState = GameState.Dead;
				bgMusic.Volume = 0.1f;	
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
				currentState = GameState.Menu;
			}
		}
		
		//Update Main Menu
		public static void UpdateMenu (GamePadData gamePadData)
		{
			if ((gamePadData.ButtonsDown & GamePadButtons.Start) != 0) {
				//collisions.PurgeAssets ();
				NewGame ();
				currentState = GameState.Playing;
				bgMusic.Volume = .25f;
			}else if((gamePadData.ButtonsDown & GamePadButtons.Square) != 0)
			{
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
		
		//Render In game
		public static void RenderPlaying ()
		{
			//Background
			bg.Render ();
			
			//This is where bullet and enemy collisions are rendered
			collisions.Render (DeltaTime);
			
			player.Render ();
		}
		
		//Render Pause menu
		public static void RenderPaused ()
		{
			bg.Render ();
			//This is where bullet and enemy collisions are calculated and rendered
			collisions.Render (DeltaTime);
			//Renders current weapon only
			player.Render ();
			paused.Render ();
			
		}
		
		//Render death screen
		public static void RenderDead ()
		{
			bg.Render ();
			//This is where bullet and enemy collisions are calculated and rendered
			collisions.Render (DeltaTime);
			//Renders current weapon only
			player.Render ();
			dead.Render ();	
		}
		
		//Render Main Menu
		public static void RenderMenu ()
		{
			bg.Render ();
			menu.Render ();			
		}
		//Render Controls Menu
		public static void RenderControls ()
		{
			bg.Render ();
			controls.Render ();			
		}
		
		//Cheat code
		public static void Cheater (GamePadData gp)
		{
			up1 += DeltaTime;
			up2 += DeltaTime;
			down1 += DeltaTime;
			down2 += DeltaTime;
			left1 += DeltaTime;
			right1 += DeltaTime;
			left2 += DeltaTime;
			right2 += DeltaTime;
			circle += DeltaTime;
			cross += DeltaTime;
			if ((gp.ButtonsDown & GamePadButtons.Up) != 0)
				up1 = 0;
			if ((gp.ButtonsDown & GamePadButtons.Up) != 0 && up1 < 500)
				up2 = 0;
			if ((gp.ButtonsDown & GamePadButtons.Down) != 0 && up2 < 500)
				down1 = 0;
			if ((gp.ButtonsDown & GamePadButtons.Down) != 0 && down1 < 500)
				down2 = 0;
			if ((gp.ButtonsDown & GamePadButtons.Left) != 0 && down2 < 500)
				left1 = 0;
			if ((gp.ButtonsDown & GamePadButtons.Right) != 0 && left1 < 500)
				right1 = 0;
			if ((gp.ButtonsDown & GamePadButtons.Left) != 0 && right1 < 500)
				left2 = 0;
			if ((gp.ButtonsDown & GamePadButtons.Right) != 0 && left2 < 500)
				right2 = 0;
			if ((gp.ButtonsDown & GamePadButtons.Circle) != 0 && right2 < 500)
				circle = 0;
			if ((gp.ButtonsDown & GamePadButtons.Cross) != 0 && circle < 500) {
				up1 = 1000;
				up2 = 1000;
				down1 = 1000;
				down2 = 1000;
				left1 = 1000;
				right1 = 1000;
				left2 = 1000;
				right2 = 1000;
				circle = 1000;
				cross = 1000;
				cheated = true;
				Console.WriteLine ("CHEATED!");
				//player.Weapons.Add (wep4);
//				weaponSelect = 3;
			}
		}
		
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
			}
			// Present the screen
			graphics.SwapBuffers ();
		}
	}
}
