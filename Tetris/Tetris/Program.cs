namespace XnaTetris
{
  static class Program
  {
    static void Main()
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
