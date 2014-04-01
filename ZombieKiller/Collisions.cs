using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

//Chris Antepenko & C. Blake Becker
namespace ZombieKiller
{
	//Handles collisions between all objects.
	public class Collisions
	{
		//Variables
		#region
		private GraphicsContext graphics;
		private List<Bullet> bullets;
		private List<Bullet> tempBullets;
		private List<Enemy> enemies;
		private List<Enemy> tempEnemies;
		private List<Explosion> explosions;
		private List<Item> items;
		private SoundPlayer deathPlayer;
		//private long DeltaTime;
		private long hurtTimer;
		
		//Player
		private Player player;

		public Player P {
			get { return player;}
			set { player = value;}
		}
		
		//Garbage Collection
		private bool NeedCleanUp;
		
		public List<Enemy> Enemies {
			get { return enemies;}	
		}
		
		public int bulletCount {
			get{ return BulletCount ();}
		}

		public int enemyCount {
			get{ return EnemyCount ();}	
		}

		public Bullet AddBullet {
			set { bullets.Add (value);}	
		}
		
		public Bullet AddTempBullet {
			set { tempBullets.Add (value);}	
		}

		public Enemy AddEnemy {
			set { enemies.Add (value);}	
		}
		
		public Enemy AddTempEnemy {
			set { tempEnemies.Add (value);}	
		}
		
		public Explosion AddExplosion {
			set { explosions.Add (value);}	
		}
		
		public Item AddItem {
			set { items.Add (value);}	
		}
		
		private int BulletCount ()
		{
			int i = 0;
			foreach (Bullet b in bullets)
				i++;
			return i;
		}
		
		private int EnemyCount ()
		{
			int i = 0;
			foreach (Enemy e in enemies)
				i++;
			return i;
		}
		#endregion
		public Collisions (GraphicsContext g)
		{
			graphics = g;
			bullets = new List<Bullet> ();
			enemies = new List<Enemy> ();
			explosions = new List<Explosion> ();
			items = new List<Item> ();
			tempBullets = new List<Bullet> ();
			tempEnemies = new List<Enemy> ();
			NeedCleanUp = false;
			hurtTimer = 0;
		}
		
		//Collision detection
		public bool IsColliding (Creature obj1, Creature obj2)
		{
			if (Vector3.DistanceSquared (obj1.p.Position, obj2.p.Position) < Math.Pow (obj2.p.Width / 2, 2))
				return true;
			else
				return false;
		}
		
		public void PurgeAssets ()
		{
			for (int i = items.Count - 1; i >= 0; i--) {
				items.RemoveAt (i);	
			}
			for (int i = bullets.Count - 1; i >= 0; i--) {
				bullets.RemoveAt (i);	
			}
			for (int i = explosions.Count - 1; i >= 0; i--) {
				explosions.RemoveAt (i);	
			}
		}
		
		public void Update (long TimeChange, GamePadData gp)
		{
			tempBullets = new List<Bullet> ();
			NeedCleanUp = false;
			
			//Collision detection between enemies and bullets
			foreach (Bullet b in bullets) {
				foreach (Enemy e in enemies) {
					if (e.IsAlive) {
						if (IsColliding (b, e)) {
							e.OnHurt (b);
							b.OnHurt ();
							if (!e.IsAlive) {
								//Play sound
								if (deathPlayer != null)
									deathPlayer.Stop ();
								deathPlayer = e.Death.CreatePlayer ();
								deathPlayer.Play ();
							}
							
							NeedCleanUp = true;
							break;
						}
					}
				}
			} 
			
			//Collision detection between enemies and player
			hurtTimer += TimeChange;
			foreach (Enemy e in enemies) {
				if (hurtTimer > 500) {
					if (IsColliding (e, player)) {
						Console.WriteLine ("HIT");
						e.HurtPlayer (player);
						player.hurtPlayer.Play ();
						hurtTimer = 0;
						break;
					}
				}
			}
			
			//Collision detection between Player and Items
			foreach (Item i in items) {
				if (IsColliding (i, player)) {
					i.PlayerCollide (player);
					NeedCleanUp = true;
				}
			}
			
			foreach (Enemy e in enemies) {
				if (e.IsAlive == false) {
					NeedCleanUp = true;
					break;
				}
			}
						
			foreach (Bullet b in bullets) {
				b.Update (TimeChange);
			}	
			foreach (Explosion e in explosions) {
				e.Update (TimeChange);
			}
			foreach (Enemy e in enemies) {
				e.Update (TimeChange);
			}
			player.Update (TimeChange, gp);
			
			//Add the new enemies
			foreach (Bullet b in tempBullets) {
				bullets.Add (b);
			}
			foreach (Enemy e in tempEnemies) {
				enemies.Add (e);
				Console.WriteLine("ADDED");
			}
			
			if (NeedCleanUp) {
//				for(int i = tempBullets.Count - 1; i >= 0; i--)
//				{
//					if(tempBullets[i].IsAlive == false)
//						tempBullets.RemoveAt(i);
//				}
				for (int i = bullets.Count - 1; i >= 0; i--) {
					if (bullets [i].IsAlive == false)
						bullets.RemoveAt (i);
				}
				for (int i = enemies.Count - 1; i >= 0; i--) {
					if (enemies [i].IsAlive == false)
						enemies.RemoveAt (i);
				}
				for (int i = explosions.Count - 1; i >= 0; i--) {
					if (explosions [i].IsAlive == false)
						explosions.RemoveAt (i);
				}
				for (int i = items.Count - 1; i >= 0; i--) {
					if (!items [i].IsAlive)
						items.RemoveAt (i);
				}
			}
			tempEnemies = new List<Enemy> ();
		}
		
		//Screen edge detection
		public bool IsOnScreen (Creature obj)
		{
			if (obj.p.Position.X < 0)
				return false;
			if (obj.p.Position.X > graphics.Screen.Rectangle.Width)
				return false;
			if (obj.p.Position.Y < 0)
				return false;
			if (obj.p.Position.Y > graphics.Screen.Rectangle.Height)
				return false;
			return true;
		}
		
		//Offscreen detection
		public bool FarOffScreen (Creature obj)
		{
			if (obj.p.Position.X < (-10))
				return true;
			if (obj.p.Position.X > graphics.Screen.Rectangle.Width + 10)
				return true;
			if (obj.p.Position.Y < (-10))
				return true;
			if (obj.p.Position.Y > graphics.Screen.Rectangle.Height + 10)
				return true;
			return false;
		}
				
		public void Render (long TimeChange)
		{
			foreach (Item i in items) {
				i.Render ();	
			}
			foreach (Bullet b in bullets) {
				b.Render ();
			}	
			foreach (Explosion e in explosions) {
				e.Render ();
			}
			foreach (Enemy e in enemies) {
				e.Render ();
			}
			player.Render ();
		}
	}
}

