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
	public class MGAmmo : Item
	{
		public MGAmmo (GraphicsContext gc, Vector3 position, Collisions col) : base(gc, position, new Texture2D("/Application/Assets/Items/machinegunammopack.png", false), col)
		{
			StatEffectValue = 30;
			ItemClass = Item.ItemType.MGAmmo;
		}
		
		public override void PlayerCollide (Player p)
		{
			//Checks if current weapon is same as ammo type
			if(p.currentWeapon.Type == Weapon.WeaponType.MachineGun && p.currentWeapon.bullets > 0)
			{
				if(p.currentWeapon.bullets - StatEffectValue > 0)
					p.currentWeapon.bullets -= StatEffectValue;
				else
					p.currentWeapon.bullets = 0;
				this.IsAlive = false;
			}
		}
	}
}

