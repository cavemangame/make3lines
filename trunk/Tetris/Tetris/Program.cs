using System;

namespace XnaTetris
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			//LinesGame.TestBackgroundBoxes();
			//LinesGame.TestEmptyGrid();
			//LinesGame.TestNextBlock();
			using (LinesGame game = new LinesGame())
			{
				game.Run();
			}
		}
	}
}

