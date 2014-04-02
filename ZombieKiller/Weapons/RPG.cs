using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

//Chris Antepenko & C. Blake Becker
namespace ZombieKiller
{
	public class RPG : Weapon
	{
		public override string Description {
			get {
				return "Fires a missile.\nMissiles explode into shrapnel.\nGood against spread out groups of enemies.";
			}
		}
		
		private int shrapnel = 16;
		
		public RPG (GraphicsContext g, Collisions col, Vector3 position, float rot) : base(g, col, position, rot, new Sound("/Application/Assets/Sounds/rifle.wav"), new Texture2D("/Application/Assets/Weapons/cannon.png", false), new Texture2D("/Application/Assets/Weapons/rocketammo.png", false))
		{
			p.Center = new Vector2 (0.5f, 0.7f);
			p.Scale = new Vector2 (.5f, 1f);
			UpgradeScale = new Vector2 (2f, 2f);
			this.bulletsPerSecond = 1;
			MaxAmmo = 6;
			CurrentAmmo = 2;
			MaxBulletsInClip = 1;
			ReloadTime = 2000;
			RunSpeed = 10;
			Damage = 10;
			Cost = 50;
			AmmoScale = new Vector2 (.6f, .6f);
			UpgradeTexture = new Texture2D ("/Application/Assets/Items/cannonobject.png", false);
			Type = Weapon.WeaponType.RPG;
		}
		
		public override string CurrentStats()
		{
			string stats = "Reload Speed: " + (double)ReloadTime/1000d +"\n"
						+ "Maximum Ammo: " + MaxAmmo + "\n"
						+ "Magazine Capacity: " + MaxBulletsInClip + "\n"
						+ "Ammo Drop Chance: " + Level.dropRate[7] + "\n"
						+ "Shrapnel Shards: " + shrapnel + "\n"
						+ "Damage: " + Damage;
			return stats;
		}
		
		public override string NextStats()
		{
			string stats = "Reload Speed: " + (((double)ReloadTime/1000d) * 0.8d) +"\n"
						+ "Maximum Ammo: " + (MaxAmmo + 1) + "\n"
						+ "Magazine Capacity: " + MaxBulletsInClip + "\n"
						+ "Ammo Drop Chance: " + Level.dropRate[7] + "\n"
						+ "Shrapnel Shards: " + (shrapnel + 2) + "\n"
						+ "Damage: " + Damage;
			return stats;
		}
		
		public override void FireWeapon ()
		{
			Vector3 newPos = p.Position;
			
			newPos += new Vector3((float)Math.Sin (p.Rotation) * RunSpeed, 0, 0);
			newPos -= new Vector3(0, (float)Math.Cos (p.Rotation) * RunSpeed, 0);
			
			ExplosiveBullet b = new ExplosiveBullet (Graphics, newPos, p.Rotation, Collide, (int)RunSpeed, Damage);
			b.Shrapnel = shrapnel;
			Collide.AddTempBullet = b;
			BulletCount++;
		}
		
		public override void Upgrade ()
		{
			if (Collide.P.Money >= Cost) {
				ReloadTime = (int)(ReloadTime * 0.8);
				MaxAmmo += 1;
				shrapnel += 2;
				CurrentAmmo = MaxAmmo;	
				Collide.P.Money -= Cost;
				Cost += 30;
			} else {
			}
		}
	}
}

