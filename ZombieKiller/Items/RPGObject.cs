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
	public class RPGObject : Item
	{
		public RPGObject (GraphicsContext gc, Vector3 position, Collisions col) : base(gc, position, new Texture2D("/Application/Assets/Items/cannonobject.png", false), col)
		{
			StatEffectValue = 1;
			ItemClass = Item.ItemType.MGObject;
			p.Width = 88;
			p.Height = 54;
			p.Scale = new Vector2(.6f,.6f);
		}
		
		public override void PlayerCollide (Player p)
		{
			//Check if player has the weapon already
			bool hasGun = false;
			for(int i = 0; i < p.Weapons.Count; i++)
			{
				Weapon w = p.Weapons[i] as MachineGun;
				if(w != null)
				{
					hasGun = true;
					break;
				}
			}
			
			//Gives the player the weapon
			if(!hasGun)
			{
				Weapon w = new RPG(Graphics, Collide, p.p.Position, p.p.Rotation);
				p.Weapons.Add (w);
				p.WeaponSelect = p.Weapons.IndexOf(w);
				this.IsAlive = false;
			}
			
			//Checks if current weapon is same as ammo type
			else if(p.currentWeapon.Type == Weapon.WeaponType.RPG && p.currentWeapon.bullets > 0)
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

