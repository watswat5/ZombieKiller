using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;

//Chris Antepenko & C. Blake Becker
namespace ZombieKiller
{
	//Portable class for text entry.
	//Requires 2 textures for keyboard and selected key.
	public class Keyboard
	{
		private GraphicsContext graphics;
		private Sprite keyboard;
		private Sprite key;
		
		private Sprite bg;
		
		//Every key and position
		private KeyValuePair<Vector3, string>[,] letters;
		
		//Position in array
		private int curR, curC;
		
		//Position on screen
		private float height;
		
		//Is text entry complete?
		private bool finished;
		public bool Finished
		{
			get { return finished;}
		}
		
		//Should the text be displayed while typing?
		private bool preview;
		public bool Preview
		{
			get { return preview;}
			set { preview = value;}
		}
		
		//Selected characters
		private List<string> selections;
		
		//Final string
		private string result;
		
		private int maxLetters;
		public int MaxLetters
		{
			get { return maxLetters;}
			set { maxLetters = value;}
		}
		
		//Used for preview
		private Scene s;
		private Label l;
		
		//Current input
		public string Text
		{
			get { return l.Text;}	
		}
		
		public Keyboard (GraphicsContext g, float h)
		{
			graphics = g;
			UISystem.Initialize(graphics);
			
			bg = new Sprite(graphics, new Texture2D("/Application/Assets/hsscreen.png", false));
			                
			s = new Scene();
			l = new Label();
			
			s.RootWidget.AddChildLast(l);
						
			height = h;
			
			maxLetters = 10;
			
			l.Y = height - 35;
			l.X = 10;
			l.Width = 544;
			l.TextColor = new UIColor(0,0,0,1);
			
			keyboard = new Sprite(graphics, new Texture2D("/Application/Assets/Keyboard.png", false)); 
			keyboard.Position.Y += height;
			key = new Sprite(graphics, new Texture2D("/Application/Assets/key.png", false));
			
			keyboard.Width = graphics.Screen.Rectangle.Width;
			
			letters = new KeyValuePair<Vector3, string>[3,10];
			
			selections = new List<string>();
			
			preview = true;
			
			InitLetters();
		}
		
		private void InitLetters()
		{
			//Row 1
			letters[0,0] = (new KeyValuePair<Vector3, string>(new Vector3(4, 0 + height, 0), "Q"));
			letters[0,1] = (new KeyValuePair<Vector3, string>(new Vector3(98, 0 + height, 0), "W"));
			letters[0,2] = (new KeyValuePair<Vector3, string>(new Vector3(192, 0 + height, 0), "E"));
			letters[0,3] = (new KeyValuePair<Vector3, string>(new Vector3(289, 0 + height, 0), "R"));
			letters[0,4] = (new KeyValuePair<Vector3, string>(new Vector3(384, 0 + height, 0), "T"));
			letters[0,5] = (new KeyValuePair<Vector3, string>(new Vector3(478, 0 + height, 0), "Y"));
			letters[0,6] = (new KeyValuePair<Vector3, string>(new Vector3(573, 0 + height, 0), "U"));
			letters[0,7] = (new KeyValuePair<Vector3, string>(new Vector3(667, 0 + height, 0), "I"));
			letters[0,8] = (new KeyValuePair<Vector3, string>(new Vector3(761, 0 + height, 0), "O"));
			letters[0,9] = (new KeyValuePair<Vector3, string>(new Vector3(855, 0 + height, 0), "P"));
			
			//Row 2
			letters[1,0] = (new KeyValuePair<Vector3, string>(new Vector3(4, 70 + height, 0), "A"));
			letters[1,1] = (new KeyValuePair<Vector3, string>(new Vector3(98, 70 + height, 0), "S"));
			letters[1,2] = (new KeyValuePair<Vector3, string>(new Vector3(192, 70 + height, 0), "D"));
			letters[1,3] = (new KeyValuePair<Vector3, string>(new Vector3(289, 70 + height, 0), "F"));
			letters[1,4] = (new KeyValuePair<Vector3, string>(new Vector3(384, 70 + height, 0), "G"));
			letters[1,5] = (new KeyValuePair<Vector3, string>(new Vector3(478, 70 + height, 0), "H"));
			letters[1,6] = (new KeyValuePair<Vector3, string>(new Vector3(573, 70 + height, 0), "J"));
			letters[1,7] = (new KeyValuePair<Vector3, string>(new Vector3(667, 70 + height, 0), "K"));
			letters[1,8] = (new KeyValuePair<Vector3, string>(new Vector3(761, 70 + height, 0), "L"));
			letters[1,9] = (new KeyValuePair<Vector3, string>(new Vector3(855, 70 + height, 0), "_"));
			
			//Row 3
			letters[2,0] = (new KeyValuePair<Vector3, string>(new Vector3(4, 137 + height, 0), ":"));
			letters[2,1] = (new KeyValuePair<Vector3, string>(new Vector3(98, 137 + height, 0), "Z"));
			letters[2,2] = (new KeyValuePair<Vector3, string>(new Vector3(192, 137 + height, 0), "X"));
			letters[2,3] = (new KeyValuePair<Vector3, string>(new Vector3(289, 137 + height, 0), "C"));
			letters[2,4] = (new KeyValuePair<Vector3, string>(new Vector3(384, 137 + height, 0), "V"));
			letters[2,5] = (new KeyValuePair<Vector3, string>(new Vector3(478, 137 + height, 0), "B"));
			letters[2,6] = (new KeyValuePair<Vector3, string>(new Vector3(573, 137 + height, 0), "N"));
			letters[2,7] = (new KeyValuePair<Vector3, string>(new Vector3(667, 137 + height, 0), "M"));
			letters[2,8] = (new KeyValuePair<Vector3, string>(new Vector3(761, 137 + height, 0), "-"));
			letters[2,9] = (new KeyValuePair<Vector3, string>(new Vector3(855, 137 + height, 0), "/"));
		}
		
		public void Update(GamePadData gp)
		{
			if((gp.ButtonsDown & GamePadButtons.Up) != 0)
			{
				if(curR > 0)
					curR--;
				else
					curR = letters.GetLength(0) - 1;
			}
			if((gp.ButtonsDown & GamePadButtons.Down) != 0)
			{
				if(curR < letters.GetLength(0) - 1)
					curR++;
				else
					curR = 0;
			}
			if((gp.ButtonsDown & GamePadButtons.Left) != 0)
			{
				if(curC > 0)
					curC--;
				else
					curC = letters.GetLength(1) - 1;
			}
			if((gp.ButtonsDown & GamePadButtons.Right) != 0)
			{
				if(curC < letters.GetLength(1) - 1)
					curC++;
				else
					curC = 0;
			}
			if((gp.ButtonsDown & GamePadButtons.Start) != 0)
			{
				finished = true;
			}
			if((gp.ButtonsDown & GamePadButtons.Cross) != 0 && selections.Count <= maxLetters)
			{
				selections.Add (letters[curR, curC].Value);
			}
			if((gp.ButtonsDown & GamePadButtons.Circle) != 0 && selections.Count > 0)
			{
				selections.RemoveAt(selections.Count - 1);
			}
			key.Position = letters[curR, curC].Key;
			string s = "";
			foreach(string a in selections)
				s = s + a;
			l.Text = s;
		}
		
		public string ReturnResult()
		{
			foreach(string s in selections)
			{
				result = result + s;	
			}
			
			return result;
		}
		
		public void Render()
		{
			bg.Render();
			keyboard.Render();	
			key.Render();
			if(preview)
			{
				UISystem.SetScene(s);
				UISystem.Render();
			}
		}
	}
}

