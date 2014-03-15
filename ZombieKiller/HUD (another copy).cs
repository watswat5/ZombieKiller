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
	public class HUDCopy
	{
		private GraphicsContext g;
		private Label position;
		private Label newXY;
		private Label spriteFrame;
		private Label bullets;
		private Label weapon;
		private Label magazine;

		public HUDCopy (GraphicsContext gc)
		{
			g = gc;
			UISystem.Initialize (g);
			
			Scene scn = new Scene ();
			
			position = new Label ();
			position.Width = 400;
			position.X = 10;
			position.Y = 10;
			
			newXY = new Label ();
			newXY.Width = 400;
			newXY.X = 10;
			newXY.Y = 30;
			
			spriteFrame = new Label ();
			spriteFrame.Width = 400;
			spriteFrame.X = 10;
			spriteFrame.Y = 50;
			
			bullets = new Label ();
			bullets.Width = 400;
			bullets.X = 400;
			bullets.Y = 10;
			
			weapon = new Label ();
			weapon.Width = 200;
			weapon.X = 10;
			weapon.Y = 100;
			
			magazine = new Label ();
			magazine.Width = 200;
			magazine.X = 10;
			magazine.Y = 100;
			
			scn.RootWidget.AddChildLast (magazine);
			scn.RootWidget.AddChildLast (weapon);
			scn.RootWidget.AddChildLast (bullets);
			scn.RootWidget.AddChildLast (spriteFrame);
			scn.RootWidget.AddChildLast (newXY);
			scn.RootWidget.AddChildLast (position);
			
			
			UISystem.SetScene (scn, null);
		}
		
		public void UpdatePosition (float x, float y)
		{
			position.Text = "Position: " + x + ", " + y;
		}
		
		public void UpdateNewXY (float x, float y)
		{
			newXY.Text = "New X/Y: " + x + ", " + y;
		}
		
		public void UpdateSpriteFrame (int frame)
		{
			spriteFrame.Text = "Frame: " + frame;
		}
		
		public void UpdateBullets (int bulletC)
		{
			bullets.Text = "Bullets: " + bulletC;
		}
		
		public void UpdateWeapon (int weapons)
		{
			weapon.Text = "Weapon: " + weapons;
		}
		
		public void UpdateMagazine (int bullets, int maxBullets)
		{
			if (maxBullets - bullets > 0)
				magazine.Text = "Clip: " + (maxBullets - bullets);
			if (maxBullets == bullets)
				magazine.Text = "Reloading...";
		}
		
		public void Render ()
		{
			UISystem.Render ();	
		}
	}
}

