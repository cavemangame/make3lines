namespace XnaTetris.Algorithms
{
  public class Scores
  {
    public long RedScore { get; set; }
    public long GreenScore { get; set; }
    public long BlueScore { get; set; }
    public long WhiteScore { get; set; }
    public long YellowScore { get; set; }
    public long GrayScore { get; set; }
    public long OverallScore { get; set; }

    public void Reset()
    {
      RedScore = 0;
      GreenScore = 0;
      BlueScore = 0;
      WhiteScore = 0;
      YellowScore = 0;
      GrayScore = 0;
      OverallScore = 0;
    }
  }
}
