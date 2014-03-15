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
		
		public MachineGun (GraphicsContext g, Collisions col, Vector3 position, float rot) : base(g, col, position, rot, new Sound("/Application/Assets/Sounds/machinegun.wav"), new Texture2D("/Application/Assets/Weapons/machinegun.png", false), new Texture2D("/Application/Assets/Weapons/machinegunammo.png", false))
		{
			this.bulletsPerSecond = 6;

			p.Center = new Vector2(0.5f, 0.9f);
			p.Scale = new Vector2(1f, 1f);
			MaxBulletsInClip = 30;
			ReloadTime = 3000;
			RunSpeed = 10;
			Damage = 2;
			Type = Weapon.WeaponType.MachineGun;
		}
		
		public override void FireWeapon ()
		{
			Collide.AddBullet = new RubberBullet (Graphics, p.Position, p.Rotation, Collide, (int)RunSpeed, Damage);
			BulletCount++;
		}
	}
}

