using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.UI;

/*Chris Antepenko*/
namespace ZombieKiller
{
	//A weapon fires bullets.
	public abstract class Weapon : Creature
	{
		//Type of weapon
		public enum WeaponType
		{
			Rifle,
			MachineGun,
			ShotGun,
			RPG,
			AdminGun
		};
		private WeaponType type;

		public WeaponType Type {
			get { return this.type;}	
			set { type = value;}
		}
		
		//Reloading 
		public static Sprite reloadSprite;
		private static Texture2D reloadTex;
		private float width;
		public float Width
		{
			get { return width;}
			set { width = value;}
		}
		//Max bullet count
		private int MAX_BULLETS_IN_CLIP;

		public int MaxBulletsInClip {
			get { return MAX_BULLETS_IN_CLIP;}	
			set { MAX_BULLETS_IN_CLIP = value;}
		}
		
		private int maxAmmo;
		public int MaxAmmo
		{
			get { return maxAmmo;}
			set { maxAmmo = value;}
		}
		
		private int currentAmmo;
		public int CurrentAmmo
		{
			get { return currentAmmo;}
			set { currentAmmo = value;}
		}
		
		public int bullets {
			get { return bulletCount;}
			set { bulletCount = value;}
		}
		
		private int damage;

		public int Damage {
			get { return damage;}
			set { damage = value;}
		}
		
		//Timing
		private long DeltaTime;
		public float bulletsPerSecond;
		private long reloadTimer;
		private long pressTime;
		private int reloadTime;

		public int ReloadTime {
			get { return reloadTime;}
			set { reloadTime = value;}
		}
		
		public long ReloadTimer {
			get { return reloadTimer;}
			set { reloadTimer = value;}
		}
		
		//Current nmber of fired bullets 
		private int bulletCount;

		public int BulletCount {
			get { return bulletCount;}
			set { bulletCount = value;}
		}
		
		//Firing sound effect
		private Sound fire;
		private SoundPlayer firePlayer;

		public SoundPlayer FirePlayer {
			get { return firePlayer;}	
		}

		private Sound effect;
		
		//Texture and Sprite for ammo bar
		private Texture2D ammoTex;

		public Texture2D AmmoTex {
			get { return ammoTex;}
			set { ammoTex = value;}
		}

		public Sprite ammo;
		private Vector2 ammoScale;

		public Vector2 AmmoScale {
			get { return ammoScale;}
			set { ammoScale = value;}
		}
		
		//Each weapon can have a custom bullet
		public Bullet b;
			
		public Vector2 center {
			set{ p.Center = value;}	
		}
		
		public Weapon (GraphicsContext g, Collisions col, Vector3 position, float rot, Sound snd, Texture2D tex, Texture2D ammo) : base(g, position, tex, col)
		{
			effect = snd;
			DeltaTime = 0;
			bulletCount = 0;
			reloadTimer = 0;
			fire = snd;
			firePlayer = fire.CreatePlayer ();
			firePlayer.Volume = 0.15f;
			ammoTex = ammo;
			ammoScale = new Vector2 (0.15f, 0.15f);
			
			reloadTex = new Texture2D ("/Application/Assets/Player/reloading.png", false);
			reloadSprite = new Sprite (Graphics, reloadTex);
			reloadSprite.Scale = new Vector2 (0.5f, 0.2f);
			//reloadSprite.Center = new Vector2 (0.5f, 0.5f);
			reloadSprite.Position = new Vector3 (Graphics.Screen.Rectangle.Width - 400, 35f, 0f);
			width = reloadSprite.Width;
		}
		
		//Places weapon at correct position on player
		public void Update (Vector3 pos, float rot, GamePadData g, long ellapsedTime)
		{
			p.Position = pos;
			p.Rotation = rot;
			p.Position.X = pos.X;
			p.Position.Y = pos.Y;
			CheckTrigger (g, ellapsedTime); 
		} 	
		
		//Check the firing key and reload key
		public virtual void CheckTrigger (GamePadData gp, long ellapsedTime)
		{
			DeltaTime += ellapsedTime;
			pressTime += ellapsedTime;
			
			//First press will always fire AS LONG AS the player doesn't spam the button
			if ((gp.ButtonsDown & GamePadButtons.Cross) != 0 && bulletCount < MAX_BULLETS_IN_CLIP && pressTime > 1000 / bulletsPerSecond) {
				pressTime = 0;
				FireWeapon ();
				firePlayer.Play ();
				reloadTimer = 0;
				DeltaTime = 0;
				
				//Holding the button will fire the weapon
			} else if ((gp.Buttons & GamePadButtons.Cross) != 0 && bulletCount < MAX_BULLETS_IN_CLIP && DeltaTime > 1000 / bulletsPerSecond) {
				FireWeapon ();
				//firePlayer.Stop ();
				firePlayer.Play ();
				DeltaTime = 0;
				reloadTimer = 0;
				//reloadTimer = 0;
				
				//Reload if empty
			} else if (bulletCount == MAX_BULLETS_IN_CLIP)
				Reload (ellapsedTime);
			
			//Reload if user presses W
			else if ((gp.ButtonsDown & GamePadButtons.Triangle) != 0) {
				bulletCount = MAX_BULLETS_IN_CLIP;
				Reload (ellapsedTime);
			} else
				reloadTimer = 0;
			if (DeltaTime > 1000 / bulletsPerSecond)
				DeltaTime = 0;
		}
		
		//Reloading the weapon
		public virtual void Reload (long EllapsedTime)
		{
			if (currentAmmo > 0)
			{
				reloadTimer += EllapsedTime;
				if (reloadTimer > reloadTime)
				{
					if(MaxBulletsInClip <= currentAmmo)
					{
						bulletCount = 0;
						currentAmmo -= MaxBulletsInClip;
					}
					else
					{
						bulletCount -= currentAmmo;
						currentAmmo = 0;	
					}
				}
				DeltaTime = 0;
			}
		}
		
		//Firing the weapon
		public abstract void FireWeapon ();
		
		public void UIRender ()
		{
			UISystem.Render ();	
		}
		
		//No sprite sheets
		public override void Render ()
		{
			p.Render ();
			if (bulletCount != MAX_BULLETS_IN_CLIP) {
				//Renders an ammo bar with as many bullets as the player has left in their clip.
				for (int i = 0; i < MAX_BULLETS_IN_CLIP - bulletCount; i++) {
					ammo = new Sprite (Graphics, ammoTex);
					ammo.Scale = ammoScale;
					ammo.Position.X = (Graphics.Screen.Rectangle.Width - 400) + (i * (p.Width - 30));
					ammo.Position.Y = 40;
					ammo.Render ();
				}
			} else
			{
				reloadSprite.Width = (((reloadTime - reloadTimer) * width *2)/reloadTime);
				reloadSprite.SetTextureCoord(0,0, reloadSprite.Width, 100);
				reloadSprite.Render(); 
				
			}
		}
	}
}

