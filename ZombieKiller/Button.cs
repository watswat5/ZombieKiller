using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;
using Sce.PlayStation.Core.Imaging;

namespace ZombieKiller
{
	public class Button
	{
		private GraphicsContext graphics;
		
		private Sprite p;
		private string text;
		
		private Scene s;
		private Label l;
		
		private Vector3 position;
		public Vector3 Position
		{
			get { return position;}
			set { position = value; p.Position = value; l.SetPosition(value.X - p.Width/2f, value.Y - p.Height/2f);}
		}
		
		private bool bDown;
		public bool ButtonDown
		{
			get { return bDown;}
		}
		
		private Vector4 bCol;
		public Vector4 ButtonColor
		{
			set { p.SetColor(value); bCol = value;}
		}
		
		public Vector4 TextColor
		{
			set { l.TextColor= (new UIColor(value.X, value.Y, value.Z, value.A));}
		}
		
		public UIFont Font
		{
			get { return l.Font;}
			set { l.Font = value;}
		}
		
		public Button (GraphicsContext g, Vector3 pos, Vector2 size, string text)
		{
			graphics = g;
			UISystem.Initialize(graphics);
			
			Image img = new Image(ImageMode.Rgba,new ImageSize((int)size.X, (int)size.Y),new ImageColor(255,255,255,255));
		    img.DrawRectangle(new ImageColor(200,100,100,255), new ImageRect((int)pos.X, (int)pos.Y, img.Size.Width, img.Size.Height));		  
		    
			Texture2D texture = new Texture2D(img.Size.Width,img.Size.Height,false,PixelFormat.Rgba);
		    
			texture.SetPixels(0,img.ToBuffer());
			
		    img.Dispose();                       
			
			p = new Sprite(g, texture);
			p.Center = new Vector2(0.5f, 0.5f);
			
			s = new Scene();
			l = new Label();
			l.Text = text;
			l.TextColor = new UIColor(0,0,0,1);
			l.Width = p.Width;
			l.Height = p.Height;
			s.RootWidget.AddChildLast(l);
			
			Position = pos;
			this.text = text;
		}
		
		public void Update(GamePadData gp)
		{
			p.SetColor(bCol + .3f);
			if((gp.ButtonsDown & GamePadButtons.Cross) != 0)
				bDown = true;
			else
				bDown = false;
		}
		
		public void Render()
		{
			p.Render();
			UISystem.SetScene(s);
			UISystem.Render();
			p.SetColor(bCol);
		}
	}
}

