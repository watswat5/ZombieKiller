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
	public class RPG : Weapon
	{

		public RPG (GraphicsContext g, Collisions col, Vector3 position, float rot) : base(g, col, position, rot, new Sound("/Application/Assets/Sounds/rifle.wav"), new Texture2D("/Application/Assets/Weapons/cannon.png", false), new Texture2D("/Application/Assets/Weapons/rocketammo.png", false))
		{
			p.Center = new Vector2(0.5f, 0.7f);
			p.Scale = new Vector2(.5f, 1f);
			this.bulletsPerSecond = 1;
			//bulletCount = 0;
			MaxBulletsInClip = 1;
			ReloadTime = 2000;
			RunSpeed = 10;
			Damage = 10;
			AmmoScale = new Vector2(.6f, .6f);
			Type = Weapon.WeaponType.RPG;
		}
		
		public override void FireWeapon ()
		{
			Collide.AddBullet = new ExplosiveBullet (Graphics, p.Position, p.Rotation, Collide, (int)RunSpeed, Damage);
			BulletCount++;
		}
	}
}

