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
	public class AdminGun : Weapon
	{
		//Fires a circle of boids.
		//Each boid does massive damage to enemies while following the player.
		//An effective shield for tough situations.
		public AdminGun (GraphicsContext g, Collisions col, Vector3 position, float rot) : base(g, col, position, rot, new Sound("/Application/Assets/Sounds/shotgun.wav"), new Texture2D("/Application/Assets/Weapons/shotgun.png", false), new Texture2D("/Application/Assets/Weapons/machinegun.png", false))
		{
			bulletsPerSecond = 1;

			p.Center = new Vector2 (0.5f, 1.0f);
			p.Scale = new Vector2 (1f, 1f);
			
			MaxBulletsInClip = 1;
			
			ReloadTime = 10000;
			
			RunSpeed = 20;
			
			Damage = 20;
			
			Type = Weapon.WeaponType.AdminGun;
		}
		
		public override void FireWeapon ()
		{
			for (int i = 0; i < 24; i++) {
				Vector3 pos = p.Position;
				float rot = p.Rotation + (float)(3.14159 / 24) * 2 * i;
				pos.X += (float)(Math.Sin (rot)) * RunSpeed;
				pos.Y -= (float)(Math.Cos (rot)) * RunSpeed;
				Collide.AddBullet = new BoidBullet (Graphics, pos, rot, Collide, (int)RunSpeed, Damage);
			}
			bullets++;
		}
		
		public override void Render ()
		{
			p.Render ();
			if (bullets > 0)
				reloadSprite.Render ();
		}
	}
}

