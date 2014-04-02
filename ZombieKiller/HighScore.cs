using System;
using System.Collections.Generic;
using System.Text;

//Chris Antepenko & C. Blake Becker
namespace ZombieKiller
{
	public class HighScore : IComparable
	{
		private int score;
		private string name;
		
		public int Score
		{
			get { return score;}	
		}
		
		public string Name
		{
			get { return name;}	
		}
		
		public HighScore (string data)
		{
			string[] values;
			char[] delim = {','};
			values = data.Split(delim, StringSplitOptions.None);
			name = values[0];
			if(name.Equals(""))
				name = "-----";
			score = Int32.Parse(values[1]);
		}
				
		public int CompareTo(object o)
		{
			HighScore h = o as HighScore;
			if(h.Score != score)
			{
				if(h.Score > score)
					return -1;
				if(h.Score < score)
					return 1;
				return 0;
			}
			else
				return String.Compare(h.Name, Name);				
		}
		
		public override string ToString ()
		{
			 return Name + "," + Score;
		}
	}
}

