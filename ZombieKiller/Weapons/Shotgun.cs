using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.UI;

//Chris Antepenko & C. Blake Becker
namespace ZombieKiller
{
	public class Shotgun : Weapon
	{
		public override string Description {
			get {
				return "Fires a spread of bullets.\nDeals moderate damage.\nEffective on groups of moderately strong enemies.";
			}
		}
		
		private int bulletsPerShot;
		private float dmgUp;
		
		//Fires five bullets in a spread out pattern
		public Shotgun (GraphicsContext g, Collisions col, Vector3 position, float rot) : base(g, col, position, rot, new Sound("/Application/Assets/Sounds/shotgun.wav"), new Texture2D("/Application/Assets/Weapons/shotgun.png", false), new Texture2D("/Application/Assets/Weapons/shotgunammo.png", false))
		{
			this.bulletsPerSecond = 1;
			p.Center = new Vector2 (0.5f, 1.0f);
			p.Scale = new Vector2 (1f, 1f);
			UpgradeScale = new Vector2 (2f, 2f);
			MaxBulletsInClip = 2;
			MaxAmmo = 20;
			CurrentAmmo = MaxAmmo;
			ReloadTime = 2000;
			RunSpeed = 10;
			Damage = 2;
			bulletsPerShot = 1;
			Cost = 40;
			UpgradeTexture = new Texture2D ("/Application/Assets/Items/shotgunobj.png", false);
			Type = Weapon.WeaponType.ShotGun;
		}
		
		public override string CurrentStats()
		{
			string stats = "Reload Speed: " + (double)ReloadTime/1000d +"\n"
						+ "Maximum Ammo: " + MaxAmmo + "\n"
						+ "Magazine Capacity: " + MaxBulletsInClip + "\n"
						+ "Ammo Drop Chance: " + Level.dropRate[7] + "\n"
						+ "Pellets Per Shot: " + (bulletsPerShot*2 + 1) + "\n"
						+ "Damage: " + Dmg ();
			return stats;
		}
		
		public override string NextStats()
		{
			string stats = "Reload Speed: " + (((double)ReloadTime/1000d) * 0.8d) +"\n"
						+ "Maximum Ammo: " + (MaxAmmo + 1) + "\n"
						+ "Magazine Capacity: " + MaxBulletsInClip + "\n"
						+ "Ammo Drop Chance: " + Level.dropRate[7] + "\n"
						+ "Pellets Per Shot: " + ((bulletsPerShot + 1)*2 + 1) + "\n"
						+ "Damage: " + FutureDmg();
			return stats;
		}
		
		//fires five bullets with a set spread
		public override void FireWeapon ()
		{
			float spread = .25f;
			
			for (int i = -bulletsPerShot; i <= bulletsPerShot; i++) {
				float newRot = (float)((p.Rotation) + Math.PI * i * spread / 12);
				b = new RubberBullet (Graphics, p.Position, newRot, Collide, (int)RunSpeed, Damage);
				b.Texture = new Texture2D ("/Application/Assets/Bullets/shotgunpellet.png", false);
				b.Scale = new Vector2 (0.2f, 0.2f);
				Collide.AddBullet = b;
			}
			
			BulletCount++;
		}
		
		public override void Upgrade ()
		{
			if (Collide.P.Money >= Cost) {
				Console.WriteLine ("Upgraded");
				ReloadTime = (int)(ReloadTime * 0.9);
				MaxAmmo += 4;
				CurrentAmmo = MaxAmmo;	
				bulletsPerShot += 1;
				Collide.P.Money -= Cost;
				dmgUp += .3f;
				Damage = Dmg ();
				Cost += 20;
			} else {
				Console.WriteLine ("NEM " + Collide.P.Money);
			}
			
		}
		
		private int Dmg()
		{
			if(dmgUp > 1.0f)
			{
				dmgUp = 0f;
				return Damage + 1;
			}
			return Damage;
		}
		
		private int FutureDmg()
		{
			if(dmgUp + 0.35f > 1.0f)
			{
				return Damage + 1;
			}
			return Damage;
		}
		
		public override void Render ()
		{
			p.Render ();
			if (BulletCount != MaxBulletsInClip) {
				for (int i = 0; i < MaxBulletsInClip - BulletCount; i++) {
					ammo = new Sprite (Graphics, AmmoTex);
					ammo.Scale = new Vector2 (.8f, .8f);
					ammo.Position.X = (Graphics.Screen.Rectangle.Width - 400) + (i * p.Width - 10);
					ammo.Position.Y = 40;
					ammo.Render ();
				}
			} else {
				reloadSprite.Width = (((ReloadTime - ReloadTimer) * Width * 2) / ReloadTime);
				reloadSprite.SetTextureCoord (0, 0, reloadSprite.Width, 100);
				reloadSprite.Render (); 
			}
			UISystem.SetScene(S);
			UISystem.Render();
		}
	}
}