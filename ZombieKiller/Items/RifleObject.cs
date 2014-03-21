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
	public class RifleObject : Item
	{
		public RifleObject (GraphicsContext gc, Vector3 position, Collisions col) : base(gc, position, new Texture2D("/Application/Assets/Items/rifleobj.png", false), col)
		{
			StatEffectValue = 5;
			ItemClass = Item.ItemType.RifleObject;
			p.Width = 50;
			p.Height = 25;
			p.Scale = new Vector2(1.2f, 1.2f);
		}
		
		public override void PlayerCollide (Player p)
		{
			//Check if player has the weapon already
			bool hasGun = false;
			for(int i = 0; i < p.Weapons.Count; i++)
			{
				Weapon w = p.Weapons[i] as Rifle;
				if(w != null)
				{
					hasGun = true;
					break;
				}
			}
			
			//Gives the player the weapon
			if(!hasGun)
			{
				Weapon w = new Rifle(Graphics, Collide, p.p.Position, p.p.Rotation);
				p.Weapons.Add (w);
				p.WeaponSelect = p.Weapons.IndexOf(w);
				this.IsAlive = false;
			}
			
			//Checks if current weapon is same as ammo type
			if(p.currentWeapon.Type == Weapon.WeaponType.Rifle && p.currentWeapon.CurrentAmmo < p.currentWeapon.MaxAmmo)
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
			return new RifleObject(Graphics, p.Position, Collide);
		}
	}
}

