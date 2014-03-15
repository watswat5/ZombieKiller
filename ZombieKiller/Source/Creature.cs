using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

/*Chris Antepenko*/

namespace ZombieKiller
{
	//Creatures have basic properties like a Graphics Context, a Collisions class and a Sprite.
	//All subtypes of creature can collide with each other.
	public abstract class Creature
	{
		public static Random rnd = new Random();
		//Graphics
		private GraphicsContext graphics;
		public GraphicsContext Graphics
		{
			get { return graphics;}	
		}
		
		//Collisions
		private Collisions collision;
		public Collisions Collide
		{
			get { return collision;}	
		}
		
		//Main sprite
		public Sprite p;
		
		//Movement Speed
		private float runSpeed;
		public float RunSpeed
		{
			get { return runSpeed;}
			set {runSpeed = value;}
		}
		
		//Active SpriteSheet Frame
		private int activeFrame;
		public int ActiveFrame
		{
			get { return activeFrame;}
			set { activeFrame = value;}
		}
		
		//Timer for frame
		private long frameTime;
		public long FrameTime
		{
			get { return frameTime;}
			set { frameTime = value;}
		}
		
		private int FRAME_MAX = 8;
		public int FrameMax
		{	
			get { return FRAME_MAX;}
		}
		private int FRAME_CELL_SIZE = 40;
		public int CellSize
		{	
			get { return FRAME_CELL_SIZE;}
			set { FRAME_CELL_SIZE = value;}
		}
		private int FRAME_DURATION = 50;
		public int FrameDuration
		{	
			get { return FRAME_DURATION;}
			set { FRAME_DURATION = value;}
		}
		
		//Is this alive?
		private bool isAlive;
		public bool IsAlive
		{
			get { return isAlive;}
			set { isAlive = value;}
		}
		public Vector2 Scale {
			set { p.Scale = value;}	
		}

		private float rotation;
		public float Rotation {
			get { return rotation;}	
			set { rotation = value;}
		}
		
		public Texture2D Texture {
			set { p.Texture = value;}	
		}

		public Creature (GraphicsContext gc, Vector3 position, Texture2D tex, Collisions col)
		{
			graphics = gc;
			collision = col;
			
			//Object sprite
			p = new Sprite (graphics, tex);
			p.Position.X = position.X;
			p.Position.Y = position.Y;
			p.Height = FRAME_CELL_SIZE;
			p.Width = FRAME_CELL_SIZE;
			p.Center = new Vector2 (0.5f, 0.5f);
			p.Scale.X = 0.75f;
			p.Scale.Y = 0.75f;
			p.Rotation = 0;
			
			//Sprite sheet variables
			activeFrame = 1;
			frameTime = 0;
			isAlive = true;
		}
		
		//A plain object does not move.
		public virtual void Update (long ElapsedTime)
		{
			
		}
		
		//Sprite sheet rendering. Objects without sprite sheets can have their own overriden Render method.
		public virtual void Render ()
		{
			p.SetTextureCoord (FRAME_CELL_SIZE * activeFrame, 0, FRAME_CELL_SIZE * (activeFrame + 1) - 1, FRAME_CELL_SIZE);
			p.Render ();		
		}
	}
}

