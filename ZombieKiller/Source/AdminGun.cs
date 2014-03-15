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
		//Fires five bullets in a spread out pattern. Only attainable via cheating.
		public AdminGun (GraphicsContext g, Collisions col, Vector3 position, float rot) : base(g, col, position, rot, new Sound("/Application/Assets/Sounds/shotgun.wav"), new Texture2D("/Application/Assets/Weapons/shotgun.png", false), new Texture2D("/Application/Assets/Weapons/machinegun.png", false))
		{
			this.bulletsPerSecond = 2;

			p.Center = new Vector2(0.5f, 1.0f);
			p.Scale = new Vector2(1f, 1f);
			
			MaxBulletsInClip = 100;
			
			ReloadTime = 1;
			
			RunSpeed = 20;
		}
		
		public override void FireWeapon ()
		{
			//This is why it's the "AdminGun"- on game servers, only administrators should have one.
			for (int i = 0; i < 24; i++) {
				Collide.AddBullet = new Bullet (Graphics, p.Position, p.Rotation + (float)(3.14159 / 24) * 2 * i, Collide, (int)RunSpeed);
			}
		}
		
		public override void Render ()
		{
			p.Render ();	
		}
	}
}

