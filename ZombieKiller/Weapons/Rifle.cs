using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

/*Chris Antepenko*/
namespace ZombieKiller
{
	public class Rifle : Weapon
	{
		public override string Description {
			get {
				return "Fires a penetrating bullet very fast.\nDeals damage best on enemies in a line.";
			}
		}
		
		private int penetration;
		
		public Rifle (GraphicsContext g, Collisions col, Vector3 position, float rot) : base(g, col, position, rot, new Sound("/Application/Assets/Sounds/rifle.wav"), new Texture2D("/Application/Assets/Weapons/rifle2.png", false), new Texture2D("/Application/Assets/Weapons/rifleammo.png", false))
		{
			p.Center = new Vector2 (0.5f, 1.0f);
			p.Scale = new Vector2 (1f, 2f);
			this.bulletsPerSecond = 1;
			UpgradeScale = new Vector2 (4f, 4f);			
			MaxAmmo = 15;
			CurrentAmmo = 15;
			MaxBulletsInClip = 5;
			ReloadTime = 3000;
			RunSpeed = 20;
			Damage = 5;
			penetration = 2;
			Cost = 40;
			AmmoScale = new Vector2 (0.1f, 0.1f);
			UpgradeTexture = new Texture2D ("/Application/Assets/Items/rifleobj.png", false);
			Type = Weapon.WeaponType.Rifle;
		}
		
		public override string CurrentStats()
		{
			string stats = "Reload Speed: " + (double)ReloadTime/1000d +"\n"
						+ "Maximum Ammo: " + MaxAmmo + "\n"
						+ "Magazine Capacity: " + MaxBulletsInClip + "\n"
						+ "Ammo Drop Chance: " + Level.dropRate[7] + "\n"
						+ "Enemy Penetration: " + penetration + "\n"
						+ "Damage: " + Damage;
			return stats;
		}
		
		public override string NextStats()
		{
			string stats = "Reload Speed: " + (((double)ReloadTime/1000d) * 0.8d) +"\n"
						+ "Maximum Ammo: " + (MaxAmmo + 1) + "\n"
						+ "Magazine Capacity: " + MaxBulletsInClip + "\n"
						+ "Ammo Drop Chance: " + Level.dropRate[7] + "\n"
						+ "Enemy Penetration: " + (penetration + 1) + "\n"
						+ "Damage: " + Damage;
			return stats;
		}
		
		//Four bullets as one. Easy penetration effect.
		public override void FireWeapon ()
		{
			for(int i = 0; i < penetration; i++)
				Collide.AddBullet = new RubberBullet (Graphics, p.Position, p.Rotation, Collide, (int)RunSpeed, Damage);
			BulletCount++;
			
		}

		public override void Upgrade ()
		{
			if (Collide.P.Money >= Cost) {
				Console.WriteLine ("Upgraded");
				ReloadTime = (int)(ReloadTime * 0.9);
				MaxAmmo += 5;
				CurrentAmmo = MaxAmmo;	
				penetration += 1;
				Collide.P.Money -= Cost;
				Cost += 30;
			} else {
				Console.WriteLine ("NEM " + Collide.P.Money);
			}
		}
		
	}
}

