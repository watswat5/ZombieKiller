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
	public class Shotgun : Weapon
	{
		//Fires five bullets in a spread out pattern
		public Shotgun (GraphicsContext g, Collisions col, Vector3 position, float rot) : base(g, col, position, rot, new Sound("/Application/Assets/Sounds/shotgun.wav"), new Texture2D("/Application/Assets/Weapons/shotgun.png", false), new Texture2D("/Application/Assets/Weapons/shotgunammo.png", false))
		{
			this.bulletsPerSecond = 1;
			p.Center = new Vector2(0.5f, 1.0f);
			p.Scale = new Vector2(1f, 1f);
			MaxBulletsInClip = 2;
			ReloadTime = 2000;
			RunSpeed = 10;
			Type = Weapon.WeaponType.ShotGun;
		}
		
		//fires five bullets with a set spread
		public override void FireWeapon ()
		{
			float spread = .25f;
			
			for (int i = -2; i < 3; i++) {
				float newRot = (float)((p.Rotation) + Math.PI * i * spread / 12);
				b = new Bullet (Graphics, p.Position, newRot, Collide, (int)RunSpeed);
				b.Texture = new Texture2D ("/Application/Assets/Weapons/shotgunpellet.png", false);
				b.Scale = new Vector2 (0.2f, 0.2f);
				Collide.AddBullet = b;
			}
			
			BulletCount++;
		}
		
		public override void Render ()
		{
			p.Render ();
			if(BulletCount !=MaxBulletsInClip)
			{
				for (int i = 0; i < MaxBulletsInClip - BulletCount; i++) {
					ammo = new Sprite (Graphics, AmmoTex);
					ammo.Scale = new Vector2(.8f, .8f);
					ammo.Position.X = (Graphics.Screen.Rectangle.Width - 400) + (i * p.Width - 10);
					ammo.Position.Y = 40;
					ammo.Render ();
				}
			}else
				reloadSprite.Render();
		}
	}
}

