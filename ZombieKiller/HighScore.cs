using System;
using System.Collections.Generic;
using System.Text;

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
			score = Int32.Parse(values[1]);
		}
				
		public int CompareTo(object o)
		{
			HighScore h = o as HighScore;
			
			if(h.Score > score)
				return -1;
			if(h.Score < score)
				return 1;
			return 0;
		}
		
		public override string ToString ()
		{
			 return Name + "," + Score;
		}
	}
}

