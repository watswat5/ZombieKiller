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
	public class MachineGun : Weapon
	{
		//Fires one bullet at a time a fast rate.
		
		public override string Description {
			get {
				return "Fires a single bullet several times a second.\nFairly weak.\nGood for large groups of weak enemies.";
			}
		}
		
		public MachineGun (GraphicsContext g, Collisions col, Vector3 position, float rot) : base(g, col, position, rot, new Sound("/Application/Assets/Sounds/machinegun.wav"), new Texture2D("/Application/Assets/Weapons/machinegun.png", false), new Texture2D("/Application/Assets/Weapons/machinegunammo.png", false))
		{
			this.bulletsPerSecond = 6;

			p.Center = new Vector2 (0.5f, 0.9f);
			p.Scale = new Vector2 (1f, 1f);
			
			UpgradeScale = new Vector2 (2f, 2f);
			
			MaxBulletsInClip = 30;
			MaxAmmo = 90;
			CurrentAmmo = MaxAmmo;
			ReloadTime = 3000;
			RunSpeed = 10;
			Damage = 1;
			UpgradeTexture = new Texture2D ("/Application/Assets/Items/mgobject.png", false);
			Type = Weapon.WeaponType.MachineGun;
		}
		
		public override string CurrentStats()
		{
			string stats = "Reload Speed: " + (double)ReloadTime/1000d +"\n"
						+ "Maximum Ammo: " + MaxAmmo + "\n"
						+ "Magazine Capacity: " + MaxBulletsInClip + "\n"
						+ "Ammo Drop Chance: " + Level.dropRate[1] + "\n"
						+ "Damage: " + Damage;
			return stats;
		}
		
		public override string NextStats()
		{
			string stats = "Reload Speed: " + (((double)ReloadTime/1000d) * 0.8d) +"\n"
						+ "Maximum Ammo: " + (MaxAmmo + 1) + "\n"
						+ "Magazine Capacity: " + MaxBulletsInClip + "\n"
						+ "Ammo Drop Chance: " + Level.dropRate[1] + "\n"
						+ "Damage: " + Damage;
			return stats;
		}
		
		public override void FireWeapon ()
		{
			Collide.AddBullet = new RubberBullet (Graphics, p.Position, p.Rotation, Collide, (int)RunSpeed, Damage);
			BulletCount++;
		}
		
		public override void Upgrade ()
		{
			
		}
	}
}

