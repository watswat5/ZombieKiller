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
	public class MachineGun : Weapon
	{
		//Fires one bullet at a time a fast rate.
		private float dmgUp;
		
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
			Cost = 30;
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
						+ "Bullets Per Second: " + Math.Round (bulletsPerSecond, 2) + "\n"
						+ "Damage: " + Damage;
			return stats;
		}
		
		public override string NextStats()
		{
			string stats = "Reload Speed: " + (((double)ReloadTime/1000d) * 0.9d) +"\n"
						+ "Maximum Ammo: " + (MaxAmmo + 30) + "\n"
						+ "Magazine Capacity: " + MaxBulletsInClip + "\n"
						+ "Ammo Drop Chance: " + ((int)(Level.dropRate[1] * 1.2)) + "\n"
						+ "Bullets Per Second: " + Math.Round(bulletsPerSecond + .3f, 2)+ "\n"
						+ "Damage: " + (FutureDmg());
			return stats;
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
		
		public override void FireWeapon ()
		{
			Vector3 newPos = p.Position;
			
			newPos += new Vector3((float)Math.Sin (p.Rotation) * RunSpeed, 0, 0);
			newPos -= new Vector3(0, (float)Math.Cos (p.Rotation) * RunSpeed, 0);
			
			Collide.AddBullet = new RubberBullet (Graphics, newPos, p.Rotation + 0.08f * rnd.Next(-1, 2), Collide, (int)RunSpeed, Damage);
			BulletCount++;
		}
		
		public override void Upgrade ()
		{
			if (Collide.P.Money >= Cost) {
				ReloadTime = (int)(ReloadTime * 0.9);
				MaxAmmo += 30;
				dmgUp += .35f;
				Damage = Dmg ();
				CurrentAmmo = MaxAmmo;
				Level.dropRate[0] = (int)(Level.dropRate[0] * 1.2f);
				bulletsPerSecond += .2f;
				Collide.P.Money -= Cost;
				Cost += 5;
			} else {
			}
		}
	}
}

