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
			if(p.currentWeapon.Type == Weapon.WeaponType.ShotGun && p.currentWeapon.CurrentAmmo < p.currentWeapon.MaxAmmo)
			{
				if(p.currentWeapon.CurrentAmmo + StatEffectValue <= p.currentWeapon.MaxAmmo)
					p.currentWeapon.CurrentAmmo += StatEffectValue;
				else
					p.currentWeapon.CurrentAmmo = p.currentWeapon.MaxAmmo;

				this.IsAlive = false;
			}
		}
		
		public override Item Clone()
		{
			return new ShotgunAmmo(Graphics, p.Position, Collide);
		}
	}
}

