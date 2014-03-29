using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

//Chris Antepenko & C. Blake Becker
namespace ZombieKiller
{
	//Grabbable Item (IE. Health)
	public class ShotObject : Item
	{
		public ShotObject (GraphicsContext gc, Vector3 position, Collisions col) : base(gc, position, new Texture2D("/Application/Assets/Items/shotgunobj.png", false), col)
		{
			StatEffectValue = 2;
			ItemClass = Item.ItemType.ShotObject;
			p.Height = 16;
			p.Width = 40;
			p.Scale = new Vector2(1.3f, 1.3f);
		}
		
		public override void PlayerCollide (Player p)
		{
			//Check if player has the weapon already
			bool hasGun = false;
			if(p.Weapons.Count > 0)
			{
				for(int i = 0; i < p.Weapons.Count; i++)
				{
					Weapon w = p.Weapons[i] as Shotgun;
					if(w != null)
					{
						hasGun = true;
						break;
					}
				}
			}
			
			//Gives the player the weapon
			if(!hasGun)
			{
				Weapon w = new Shotgun(Graphics, Collide, p.p.Position, p.p.Rotation);
				p.Weapons.Add (w);
				p.WeaponSelect = p.Weapons.IndexOf(w);
				this.IsAlive = false;
			}
			
			else if(p.Weapons.Count > 0)
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
		}
		
		public override Item Clone()
		{
			return new ShotObject(Graphics, p.Position, Collide);
		}
	}
}

