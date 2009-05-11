using System;

namespace PingPongLive
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main(string[] args)
    {
      using (PingPongLive game = new PingPongLive())
      {
        game.Run();
      }
    }
  }
}

