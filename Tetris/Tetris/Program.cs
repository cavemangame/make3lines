namespace XnaTetris
{
  static class Program
  {
    static void Main()
    {
      using (LinesGame game = new LinesGame())
      {
        game.Run();
      }
    }
  }
}
