using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.UI;

//Chris Antepenko & C. Blake Becker
namespace ZombieKiller
{
	//Main player object.
	public class Player : Creature
	{
		#region
		private float turnSpeed;
		private int health;
		public int MAX_HEALTH = 10;
		private Sprite healthBar;
		private Sprite healthEmpty;
		public Sprite DamageScreen;
		private Sound hurt;
		public SoundPlayer hurtPlayer;
		private GamePadData gp;
		private float width;
		public GamePadData GPData
		{
			set { gp = value;}	
		}
		public int Health
		{
			get { return health;}	
			set { health = value;}
		}
		
		private int money;
		public int Money
		{
			get { return money;}
			set { money = value;}
		}
		
		private int score;
		public int Score
		{
			get { return score;}
			set { score = value;}
		}
		
		//Alpha value for blood splatter effect
		private float alpha;
		public float Alpha
		{
			get { return alpha;}
			set {
				alpha = value;
				if(alpha > 1.0f)
					alpha = 1.0f;
				if(alpha < 0.0f)
					alpha = 0.0f;
			}
		}
		
		float newX, newY;
		
		//For weapon selection
		private List<Weapon> weapons;
		public List<Weapon> Weapons
		{
			get { return weapons;}
		}
		
		public Weapon currentWeapon;
		private int weaponSelect; 
		public int WeaponSelect
		{
			get { return weaponSelect;}
			set { weaponSelect = value;}
		}
		
		//For rendering current money
		private Scene s;
		public Scene S
		{
			get { return s;}
		}
		private Label moneyL;
		private Label scoreL;
		#endregion
		public Player (GraphicsContext gc, Vector3 position, Collisions col) : base(gc, position, new Texture2D("/Application/Assets/Player/player.png", false), col)
		{
			//Basic movement values
			RunSpeed = 2;
			turnSpeed = 3;
			
			Money = 0;
			
			s = new Scene();
			
			moneyL = new Label();
			moneyL.SetPosition(Graphics.Screen.Rectangle.Width - 470, 5);
			moneyL.TextColor = new UIColor(.68f, 0, 0, 1);
			
			scoreL = new Label();
			scoreL.SetPosition(Graphics.Screen.Rectangle.Width - 600, 5);
			scoreL.TextColor = new UIColor(.68f, 0, 0, 1);
			
			s.RootWidget.AddChildLast(moneyL);
			s.RootWidget.AddChildLast(scoreL);
			//Played when the player is hurt
			hurt = new Sound("/Application/Assets/Sounds/hurt.wav");
			hurtPlayer = hurt.CreatePlayer();
			
			//Health bar
			Texture2D tex = new Texture2D("/Application/Assets/Player/health.png", false);
			healthBar = new Sprite(gc, tex);
			healthBar.Scale = new Vector2(0.15f, 0.15f);
			healthBar.Position = new Vector3(gc.Screen.Rectangle.Width - 400, 0f, 0f);
			health = MAX_HEALTH;
			width = healthBar.Width;
			
			tex = new Texture2D("/Application/Assets/Player/healthempty.png", false);
			healthEmpty = new Sprite(Graphics, tex);
			healthEmpty.Scale = new Vector2(0.15f, 0.15f);
			healthEmpty.Position = new Vector3(gc.Screen.Rectangle.Width - 400, 0f, 0f);
			
			//Red damage effect
			tex = new Texture2D("/Application/Assets/Player/damage.png", false);
			DamageScreen = new Sprite(Graphics, tex);
			DamageScreen.SetColor(1,1,1,0);
			DamageScreen.Width = Graphics.Screen.Rectangle.Width;
			DamageScreen.Height = Graphics.Screen.Rectangle.Height;
			
			weapons = new List<Weapon>();
			//weapons.Add (new AdminGun(gc, Collide, Position, p.Rotation));
			//currentWeapon = weapons[weaponSelect];
		}
		
		public void Update(long ElapsedTime, GamePadData gp){
			FrameTime += ElapsedTime;
			if(weapons.Count > 0)
			{
				currentWeapon = weapons[weaponSelect];
				//Weapon Selection
				if ((gp.ButtonsDown & GamePadButtons.L) != 0) {
					if (weaponSelect < Weapons.Count - 1)
					{
						weaponSelect++;
						currentWeapon = weapons[weaponSelect];
					}
					else
					{
						weaponSelect = 0;
						currentWeapon = weapons[weaponSelect];
					}
				}
				if ((gp.ButtonsDown & GamePadButtons.R) != 0) {
					if (weaponSelect > 0)
					{
						weaponSelect--;
						currentWeapon = weapons[weaponSelect];
					}
					else
					{
						weaponSelect = weapons.Count - 1;
						currentWeapon = weapons[weaponSelect];	
					}
				}
				
				//Updates current weapon only
				weapons [weaponSelect].Update (Position, p.Rotation, gp, ElapsedTime);
			}
			//New coordinates for player based on angle and player speed
			newX = (float) Math.Sin(-Rotation * (3.14/180)) * RunSpeed;
			newY = (float) Math.Cos(-Rotation * (3.14/180)) * RunSpeed;
			
			//forward movement
			if((gp.Buttons & GamePadButtons.Up) != 0){
				//right/left wall detection for forward movement
				if((Position.X + newX < Graphics.Screen.Rectangle.Width || newX > 0) && (Position.X - newX > 0 || newX < 0))
					Position -= new Vector3(newX, 0, 0);
				
				//top/bottom wall detection for forward movement
				if((Position.Y + newY < Graphics.Screen.Rectangle.Height || newY > 0) && (Position.Y - newY > 0 || newY < 0))
					Position -= new Vector3(0, newY, 0);
				
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
				if((Position.X - newX < Graphics.Screen.Rectangle.Width || newX < 0) && (Position.X + newX > 0 || newX > 0))
					Position += new Vector3(newX, 0, 0);
				
				//top/bottom wall detection for reverse movement
				if((Position.Y - newY < Graphics.Screen.Rectangle.Height || newY < 0) && (Position.Y + newY > 0 || newY > 0))
					Position += new Vector3(0, newY, 0);
				
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
			
			p.Position = Position;
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
			Alpha -= .003f;
			DamageScreen.SetColor(1,1,1, alpha);
			
			//moneyL.Text = "$" + Money;
			scoreL.Text = "Score: " + score + " $" + Money;
		}
		
		public override void Render()
		{
			//Renders current weapon only
			if(weapons.Count > 0)
				weapons[weaponSelect].Render();
			
			//Sprite sheet rendering
			p.SetTextureCoord (CellSize * ActiveFrame, 0, CellSize * (ActiveFrame + 1) - 1, CellSize);
			p.Render ();	
			
			DamageScreen.Render();
			
			//Renders the healthbar with a size proportionate to total health.
			healthBar.SetTextureCoord(0, 0, (2000 / (MAX_HEALTH)) * health - 1, 200);
			healthBar.Width = ((2000/MAX_HEALTH) *  health - 1);
			healthBar.Render ();
			
			//Empty outline for health bar
			healthEmpty.Render();	
			UISystem.SetScene(s);
			UISystem.Render ();
			
		}
		
	}
}

