using System;
using System.Collections.Generic;
using System.Text;

namespace XnaTetris.Algorithms
{
	public class Level
	{
		public long time;
		public int number;
		public int maxScore;

		public Level(int setTime, int setNumber, int setMaxScore)
		{
			time = setTime*1000;
			number = setNumber;
			maxScore = setMaxScore;
		}

		public string LevelString
		{
			get { return String.Format("Level:  {0}", number) ;}
		}
	}

	public class LevelGenerator
	{
		public static Level GetLevel(int number)
		{
			switch (number)
			{
				case 1: return new Level(50, 1, 1500);
				case 2: return new Level(50, 2, 4000);
				case 3: return new Level(30, 3, 5000);
				case 4: return new Level(30, 4, 6500);
				case 5: return new Level(30, 5, 8500);
				case 6: return new Level(50, 6, 11000);
				case 7: return new Level(50, 7, 14000);
				case 8: return new Level(10, 8, 15000);
				default: return new Level(Int32.MaxValue, 0, Int32.MaxValue);
			}
		}
	}
}
