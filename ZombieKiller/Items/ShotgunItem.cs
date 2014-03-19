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
	public class ShotgunItem : Item
	{
		public ShotgunItem (GraphicsContext gc, Vector3 position, Collisions col) : base(gc, position, new Texture2D("/Application/Assets/Items/shotgunammopack.png", false), col)
		{
			StatEffectValue = 0;
			ItemClass = Item.ItemType.ShotAmmo;
		}
		
		public override void PlayerCollide (Player p)
		{
			foreach(Weapon w in p.Weapons)
			{
				if(w is Shotgun)
				{
					return;
				}
			}
			
			p.Weapons.Add(new Shotgun(p.Graphics, p.Collide, new Vector3(p.p.Position.X, p.p.Position.Y, 0), p.p.Rotation));
		}
		
		public override Item Clone()
		{
			return new ShotgunItem(Graphics, p.Position, Collide);
		}
	}
}

