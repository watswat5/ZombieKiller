using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;
using System.Diagnostics;
//Chris Antepenko & C. Blake Becker

namespace ZombieKiller
{
	public class Upgrade
	{
		private GraphicsContext graphics;
		private Player plr;
		private int currentWeapon;
		private Sprite weapon;
		private Sprite background;
		private Vector3 position;
		private Scene scene;
		private Label currentStats, nextStats, description, cost, controls;
		
		public Player Plr
		{
			get { return plr;}
			set { plr = value;}
		}
		
		public Upgrade (GraphicsContext g)//, Player p)
		{
			graphics = g;
			currentWeapon = 0;
			
			UISystem.Initialize(graphics);
			
			//plr = p;
			
			scene = new Scene();
			
			nextStats = new Label();
			currentStats = new Label();
			description = new Label();
			cost = new Label();
			controls = new Label();
			
			currentStats.X = graphics.Screen.Rectangle.Width/2 - 150;
			currentStats.Y = 0;
			currentStats.Height = 300;
			currentStats.Width = 300;
			
			nextStats.X = graphics.Screen.Rectangle.Width/2 + 200;
			nextStats.Y = 0;
			nextStats.Height = 300;
			nextStats.Width = 300;
			
			description.X = graphics.Screen.Rectangle.Width/2 - 150;
			description.Width = 600;
			description.Height = 300;
			description.Y = 140;
			
			cost.X = graphics.Screen.Rectangle.Width/2 - 150;
			cost.Y = 400;
			cost.Width = 400;
			
			scene.RootWidget.AddChildLast(currentStats);
			scene.RootWidget.AddChildLast(nextStats);
			scene.RootWidget.AddChildLast(description);
			scene.RootWidget.AddChildLast(cost);
						
			position = new Vector3(150, 200, 0);
			
			background = new Sprite(graphics, new Texture2D("/Application/Assets/UI/upgrade.png", false));
			background.Center = new Vector2(0,0);
			background.Height = graphics.Screen.Rectangle.Height;
			background.Width = graphics.Screen.Rectangle.Width;
		}
		
		public void Update(GamePadData gp)
		{
			if ((gp.ButtonsDown & GamePadButtons.L) != 0) {
				if (currentWeapon < plr.Weapons.Count - 1)
				{
					currentWeapon++;
//					weapon = new Sprite(graphics, plr.Weapons[currentWeapon].UpgradeTexture);
//					weapon.Scale = plr.Weapons[currentWeapon].UpgradeScale;
//					weapon.Position = position;
				}
				else
				{
					currentWeapon = 0;
//					weapon = new Sprite(graphics, plr.Weapons[currentWeapon].UpgradeTexture);
//					weapon.Scale = plr.Weapons[currentWeapon].UpgradeScale;
//					weapon.Position = position;
				}
			}
			if ((gp.ButtonsDown & GamePadButtons.R) != 0) {
				if (currentWeapon > 0)
				{
					currentWeapon--;
//					weapon = new Sprite(graphics, plr.Weapons[currentWeapon].UpgradeTexture);
//					weapon.Scale = plr.Weapons[currentWeapon].UpgradeScale;
//					weapon.Position = position;
				}
				else
				{
					currentWeapon = plr.Weapons.Count - 1;
//					weapon = new Sprite(graphics, plr.Weapons[currentWeapon].UpgradeTexture);
//					weapon.Scale = plr.Weapons[currentWeapon].UpgradeScale;
//					weapon.Position = position;
				}
			}
			
			currentStats.Text = plr.Weapons[currentWeapon].CurrentStats();
			nextStats.Text = plr.Weapons[currentWeapon].NextStats();
			description.Text = plr.Weapons[currentWeapon].Description;
			cost.Text = "Cost: " + plr.Weapons[currentWeapon].Cost + ", Curent Money: " + plr.Money;
			
			if ((gp.ButtonsDown & GamePadButtons.Triangle) != 0)
			{
				plr.Weapons[currentWeapon].Upgrade();	
			}			
		}
		
		public void Render()
		{
			if(currentWeapon >= plr.Weapons.Count || currentWeapon < 0)
				currentWeapon = 0;
			
			weapon = new Sprite(graphics, plr.Weapons[currentWeapon].UpgradeTexture);
			weapon.Position = position;
			weapon.Scale = plr.Weapons[currentWeapon].UpgradeScale;
			background.Render();
			weapon.Render();
			UISystem.SetScene(scene);
			UISystem.Render ();
		}
	}
}

