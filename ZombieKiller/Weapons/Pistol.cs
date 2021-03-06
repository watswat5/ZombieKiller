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
	public class Pistol : Weapon
	{
		//Timing
		private long reloadTimer;
		private long pressTime;
		private int reloadTime;

		public Pistol (GraphicsContext g, Collisions col, Vector3 position, float rot) : base(g, col, position, rot, new Sound("/Application/Assets/Sounds/rifle.wav"), new Texture2D("/Application/Assets/Weapons/rifle2.png", false), new Texture2D("/Application/Assets/Weapons/rifleammo.png", false))
		{
			p.Center = new Vector2(0.5f, 1.0f);
			p.Scale = new Vector2(1f, 2f);
			this.bulletsPerSecond = 20;
			//bulletCount = 0;
			MaxBulletsInClip = 12;
			ReloadTime = 1000;
			RunSpeed = 20;
			AmmoScale = new Vector2(0.1f, 0.1f);
			Type = Weapon.WeaponType.Pistol;
			//pressTime = 100;
		}
		
		public override void CheckTrigger(GamePadData gp, long elapsedTime)
		{
			pressTime += elapsedTime;
			if ((gp.ButtonsDown & GamePadButtons.Cross) != 0 && BulletCount < MaxBulletsInClip && pressTime < 1000 / bulletsPerSecond) {
				pressTime = 0;
				FireWeapon ();
				FirePlayer.Play ();
				reloadTimer = 0;
				
				//Holding the button will fire the weapon
			} else if (BulletCount == MaxBulletsInClip)
				Reload (elapsedTime);
			if (pressTime > 1000 / bulletsPerSecond)
				pressTime = 0;
		}

		public override void FireWeapon ()
		{
			Collide.AddBullet = new Bullet (Graphics, p.Position, p.Rotation, Collide, (int)RunSpeed);
			BulletCount++;
			
		}
	}
}

