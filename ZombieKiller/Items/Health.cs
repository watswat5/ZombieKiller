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
	public class Health : Item
	{
		
		public Health (GraphicsContext gc, Vector3 position, Collisions col) : base(gc, position, new Texture2D("/Application/Assets/Items/healthpack.png", false), col)
		{
			StatEffectValue = 2;
			ItemClass = Item.ItemType.Health;
		}
		
		public override void PlayerCollide(Player p)
		{
			if(p.Health < p.MAX_HEALTH)
			{
				this.IsAlive = false;
				if(p.Health + StatEffectValue <= p.MAX_HEALTH)
					p.Health += StatEffectValue;
				else
					p.Health = p.MAX_HEALTH;
			}
		}
		
		public override Item Clone()
		{
			return new Health(Graphics, p.Position, Collide);
		}
		
	}
}

