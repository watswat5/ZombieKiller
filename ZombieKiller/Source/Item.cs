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
	public abstract class Item : Creature
	{
		//How much the particular item affects the player status (ammo or health)
		private int statEffectValue;
		public int StatEffectValue
		{
			get { return statEffectValue;}
			set { statEffectValue = value;}
		}
		
		//Item type
		public enum ItemType {Health, RifleAmmo, MGAmmo, ShotAmmo};
		private ItemType itemType;
		public ItemType ItemClass
		{
			get { return itemType;}
			set { itemType = value;}
		}
		
		public Item (GraphicsContext gc, Vector3 position, Texture2D tex, Collisions col) : base(gc, position, tex, col)
		{
			
		}
		
		//Method to run when player picks item up
		public abstract void PlayerCollide(Player p);
	}
}

