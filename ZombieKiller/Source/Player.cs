using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

/*Chris Antepenko*/
namespace ZombieKiller
{
	//Main player object.
	public class Player : Creature
	{
		private float turnSpeed;
		private int health;
		public int MAX_HEALTH = 10;
		private Sprite healthBar;
		private Sprite healthEmpty;
		private Sound hurt;
		public SoundPlayer hurtPlayer;
		private GamePadData gp;
		public GamePadData GPData
		{
			set { gp = value;}	
		}
		public int Health
		{
			get { return health;}	
			set { health = value;}
		}
		float newX, newY;
		public Weapon currentWeapon;
		
		public Player (GraphicsContext gc, Vector3 position, Collisions col) : base(gc, position, new Texture2D("/Application/Assets/Player/player.png", false), col)
		{
			//Basic movement values
			RunSpeed = 2;
			turnSpeed = 3;
			
			//Played when the player is hurt
			hurt = new Sound("/Application/Assets/Sounds/hurt.wav");
			hurtPlayer = hurt.CreatePlayer();
			
			//Health bar
			Texture2D tex = new Texture2D("/Application/Assets/Player/health.png", false);
			healthBar = new Sprite(gc, tex);
			healthBar.Scale = new Vector2(0.15f, 0.15f);
			healthBar.Position = new Vector3(gc.Screen.Rectangle.Width - 400, 0f, 0f);
			health = MAX_HEALTH;
			
			tex = new Texture2D("/Application/Assets/Player/healthempty.png", false);
			healthEmpty = new Sprite(Graphics, tex);
			healthEmpty.Scale = new Vector2(0.15f, 0.15f);
			healthEmpty.Position = new Vector3(gc.Screen.Rectangle.Width - 400, 0f, 0f);
		}
		
		public override void Update(long ElapsedTime){
			FrameTime += ElapsedTime;

			//New coordinates for player based on angle and player speed
			newX = (float) Math.Sin(-Rotation * (3.14/180)) * RunSpeed;
			newY = (float) Math.Cos(-Rotation * (3.14/180)) * RunSpeed;
			
			//forward movement
			if((gp.Buttons & GamePadButtons.Up) != 0){
				//right/left wall detection for forward movement
				if((p.Position.X + newX < Graphics.Screen.Rectangle.Width || newX > 0) && (p.Position.X - newX > 0 || newX < 0))
					p.Position.X -= newX;
				
				//top/bottom wall detection for forward movement
				if((p.Position.Y + newY < Graphics.Screen.Rectangle.Height || newY > 0) && (p.Position.Y - newY > 0 || newY < 0))
					p.Position.Y -= newY;
				
				//Sprite sheet advancement
				if(FrameTime >= FrameDuration){
					if(ActiveFrame < FrameMax - 1)
						ActiveFrame++;
					else
						ActiveFrame = 0;
					FrameTime = 0;
				}
			}
			
			//Reverse movement
			else if((gp.Buttons & GamePadButtons.Down) != 0){
				 //right/left wall detection for reverse movement
				if((p.Position.X - newX < Graphics.Screen.Rectangle.Width || newX < 0) && (p.Position.X + newX > 0 || newX > 0))
					p.Position.X += newX;
				
				//top/bottom wall detection for reverse movement
				if((p.Position.Y - newY < Graphics.Screen.Rectangle.Height || newY < 0) && (p.Position.Y + newY > 0 || newY > 0))
					p.Position.Y += newY;
				
				//Sprite sheet advancement
				if(FrameTime >= FrameDuration){
					if(ActiveFrame > 0)
						ActiveFrame--;
					else
						ActiveFrame = FrameMax - 1;
					FrameTime = 0;
				}
			} else
				ActiveFrame = 0;
			
			//Rotation adjustment
			if((gp.Buttons & GamePadButtons.Left) != 0){
				Rotation -= turnSpeed;
				//Degrees to radians
				p.Rotation = (float)(3.14/180) * Rotation;
			}
			
			if((gp.Buttons & GamePadButtons.Right) != 0){
				Rotation += turnSpeed;
				//Degrees to radians
				p.Rotation = (float)(3.14/180) * Rotation;
			}
			
		}
		
		public override void Render()
		{
			//Sprite sheet rendering
			p.SetTextureCoord (CellSize * ActiveFrame, 0, CellSize * (ActiveFrame + 1) - 1, CellSize);
			p.Render ();	
			
			//Renders the healthbar with a size proportionate to total health.
			healthBar.SetTextureCoord(0, 0, (200 * (MAX_HEALTH/10f)) * health - 1, 200);
			healthBar.Width = (200 * (MAX_HEALTH/10f)) * health - 1;
			healthBar.Render ();
			
			//Empty outline for health bar
			healthEmpty.Render();
			
		}
		
	}
}

