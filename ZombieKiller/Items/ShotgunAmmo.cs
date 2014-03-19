using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

/*Chris Antepenko*/
namespace ZombieKiller
{
	//Grabbable Item (IE. Health)
	public class ShotgunAmmo : Item
	{
		public ShotgunAmmo (GraphicsContext gc, Vector3 position, Collisions col) : base(gc, position, new Texture2D("/Application/Assets/Items/shotgunammopack.png", false), col)
		{
			StatEffectValue = 5;
			ItemClass = Item.ItemType.ShotAmmo;
		}
		
		public override void PlayerCollide (Player p)
		{
			//Checks if current weapon is same as ammo type
			if(p.currentWeapon.Type == Weapon.WeaponType.ShotGun && p.currentWeapon.bullets > 0)
			{
				if(p.currentWeapon.bullets - StatEffectValue > 0)
					p.currentWeapon.bullets -= StatEffectValue;
				else
					p.currentWeapon.bullets = 0;
				this.IsAlive = false;
			}
		}
		
		public override Item Clone()
		{
			return new ShotgunAmmo(Graphics, p.Position, Collide);
		}
	}
}

