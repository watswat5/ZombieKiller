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
	public class AdminGun : Weapon
	{
		//Fires a circle of boids.
		//Each boid does massive damage to enemies while following the player.
		//An effective shield for tough situations.
		
		private int bulletNum;
		
		public override string Description {
			get {
				return "Fires a cloud of player-seeking boids.\nEach boid deals massive damage to enemies.\nActs as an effective shield.";
			}
		}
		
		public AdminGun (GraphicsContext g, Collisions col, Vector3 position, float rot) : base(g, col, position, rot, new Sound("/Application/Assets/Sounds/boid.wav"), new Texture2D("/Application/Assets/Weapons/shield.png", false), new Texture2D("/Application/Assets/items/shieldammo.png", false))
		{
			bulletsPerSecond = 1;

			FirePlayer.Volume = 1.5f;
			
			p.Center = new Vector2 (0.5f, 1.0f);
			p.Scale = new Vector2 (1f, 1f);
			
			UpgradeScale = new Vector2 (2f, 2f);
			
			MaxBulletsInClip = 1;
			
			MaxAmmo = 1;
			
			CurrentAmmo = MaxAmmo;
			
			ReloadTime = 10000;
			
			RunSpeed = 20;
			
			Damage = 5;
			
			Cost = 10;
			
			bulletNum = 16;
			
			AmmoScale = new Vector2 (0.8f, 0.8f);
			
			UpgradeTexture = new Texture2D ("/Application/Assets/Items/shieldammo.png", false);
			
			Type = Weapon.WeaponType.AdminGun;
		}
		
		public override string CurrentStats ()
		{
			string stats = "Reload Speed: " + (double)ReloadTime / 1000d + "\n"
						+ "Maximum Ammo: " + MaxAmmo + "\n"
						+ "Magazine Capacity: " + MaxBulletsInClip + "\n"
						+ "Ammo Drop Chance: " + Level.dropRate [5] + "\n"
						+ "Damage: " + Damage + "\n"
						+ "Boid Cout: " + bulletNum;
			return stats;
		}
		
		public override string NextStats ()
		{
			string stats = "Reload Speed: " + (((double)ReloadTime / 1000d) * 0.9d) + "\n"
						+ "Maximum Ammo: " + (MaxAmmo + 1) + "\n"
						+ "Magazine Capacity: " + MaxBulletsInClip + "\n"
						+ "Ammo Drop Chance: " + Level.dropRate [5] + "\n"
						+ "Damage: " + Damage + "\n"
						+ "Boid Cout: " + (bulletNum + 2);
			return stats;
		}
		
		public override void FireWeapon ()
		{
			for (int i = 0; i < bulletNum; i++) {
				Vector3 pos = p.Position;
				float rot = p.Rotation + (float)(Math.PI / (float)bulletNum) * 2 * i;
				pos.X += (float)(Math.Sin (rot)) * RunSpeed;
				pos.Y -= (float)(Math.Cos (rot)) * RunSpeed;
				Collide.AddBullet = new BoidBullet (Graphics, pos, rot, Collide, (int)RunSpeed, Damage);
			}
			bullets++;
		}
		
		public override void Upgrade ()
		{
			if (Collide.P.Money >= Cost) {
				Console.WriteLine ("Upgraded");
				ReloadTime = (int)(ReloadTime * 0.9);
				MaxAmmo += 1;
				CurrentAmmo = MaxAmmo;	
				bulletNum += 2;
				Collide.P.Money -= Cost;
				Cost += 15;
			} else {
				Console.WriteLine ("NEM " + Collide.P.Money);
			}
		}
	}
}

