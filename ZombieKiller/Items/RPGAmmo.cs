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
	public class RPGAmmo : Item
	{
		public RPGAmmo (GraphicsContext gc, Vector3 position, Collisions col) : base(gc, position, new Texture2D("/Application/Assets/Items/rpgammo.png", false), col)
		{
			StatEffectValue = 1;
			ItemClass = Item.ItemType.RPGAmmo;
		}
		
		public override void PlayerCollide (Player p)
		{
			if(p.currentWeapon.Type == Weapon.WeaponType.RPG && p.currentWeapon.CurrentAmmo < p.currentWeapon.MaxAmmo)
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
			return new RPGAmmo(Graphics, p.Position, Collide);
		}
	}
}

