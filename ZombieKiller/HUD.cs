using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;

/*Chris Antepenko*/

namespace ZombieKiller
{
	//Draws some debug info to the screen.
	public class HUD
	{
		private GraphicsContext g;
		private Label enemyCount;
		private Label magazine;
		private Label fps;
		private Label cheater;
		private Label health;
		private long FPSTime;
		
		public HUD (GraphicsContext g)
		{
			this.g = g;
			UISystem.Initialize (g);
			FPSTime = 0;
			
			Scene scn = new Scene ();
			
			enemyCount = new Label ();
			enemyCount.X = 10;
			enemyCount.Y = 30;
			enemyCount.Width = 300;
			enemyCount.Text = "Enemies:";
			
			scn.RootWidget.AddChildLast (enemyCount);
			
			health = new Label ();
			health.X = g.Screen.Rectangle.Width - 475;
			health.Y = 0;
			health.Width = 300;
			health.TextColor = new UIColor (0.68f, 0f, 0f, 1f);
			health.Text = "Health:";
			
			scn.RootWidget.AddChildLast (health);
			
			magazine = new Label ();
			magazine.X = g.Screen.Rectangle.Width - 400;
			magazine.Y = 30;
			magazine.Width = 300;
			magazine.TextColor = new UIColor (0.68f, 0f, 0f, 1f);
			magazine.Text = " ";
			
			scn.RootWidget.AddChildLast (magazine);
			
			fps = new Label ();
			fps.X = 400;
			fps.Y = 10;
			fps.Width = 300;
			fps.Text = " ";
			
			scn.RootWidget.AddChildLast (fps);
			
			cheater = new Label ();
			cheater.X = 10;
			cheater.Y = g.Screen.Rectangle.Height - 25;
			cheater.Width = 300;
			fps.Text = " ";
			
			scn.RootWidget.AddChildLast (cheater);
			
			UISystem.SetScene (scn, null);
		}
		
		public void UpdateMagazine (int bullets, int maxbullets)
		{
			if (maxbullets - bullets == 0)
				magazine.Text = "Reloading...";
			else
				magazine.Text = "";
		}
		
		public void UpdateEnemyCount (int enemies)
		{
			enemyCount.Text = "Enemies: " + enemies;
		}
		
		public void UpdateFPS (long EllapsedTime)
		{
			FPSTime += EllapsedTime;
			if (EllapsedTime != 0 && FPSTime > 200) {
				fps.Text = "FPS: " + (int)(1000 / EllapsedTime);
				FPSTime = 0;	
			}
		}
		
		public void UpdateCheat (bool cheated)
		{
			if (cheated)
				cheater.Text = "CHEATER!";
			else
				cheater.Text = "";
		}
		
		public void UpdateHealth (int health)
		{
			this.health.Text = "Health: " + health;
		}
		
		public void Render ()
		{
			UISystem.Render ();
		}
	}
}

