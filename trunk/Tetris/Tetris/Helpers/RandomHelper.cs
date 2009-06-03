#region Using directives
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
#endregion

namespace XnaTetris.Helpers
{
  public class RandomHelper
  {
    private static Random globalRandomGenerator = GenerateNewRandomGenerator();

    #region Generate a new random generator
    /// <summary>
    /// Generate a new random generator with help of
    /// WindowsHelper.GetPerformanceCounter.
    /// Also used for all GetRandom methods here.
    /// </summary>
    /// <returns>Random</returns>
    public static Random GenerateNewRandomGenerator()
    {
      globalRandomGenerator = new Random((int)DateTime.Now.Ticks);
      //needs Interop: (int)WindowsHelper.GetPerformanceCounter());
      return globalRandomGenerator;
    } // GenerateNewRandomGenerator()
    #endregion

    #region Get random float and byte methods
    public static int GetRandomInt(int max)
    {
      return globalRandomGenerator.Next(max);
    } // GetRandomInt(max)

    public static int GetRandomInt(int min, int max)
    {
      return globalRandomGenerator.Next(min, max);
    } // GetRandomInt(max)

    public static float GetRandomFloat(float min, float max)
    {
      return (float)globalRandomGenerator.NextDouble() * (max - min) + min;
    } // GetRandomFloat(min, max)

    public static byte GetRandomByte(byte min, byte max)
    {
      return (byte)(globalRandomGenerator.Next(min, max));
    } // GetRandomByte(min, max)

    public static Vector2 GetRandomVector2(float min, float max)
    {
      return new Vector2(
        GetRandomFloat(min, max),
        GetRandomFloat(min, max));
    } // GetRandomVector2(min, max)

    public static Vector3 GetRandomVector3(float min, float max)
    {
      return new Vector3(
        GetRandomFloat(min, max),
        GetRandomFloat(min, max),
        GetRandomFloat(min, max));
    } // GetRandomVector3(min, max)

    public static Color GetRandomColor()
    {
      return new Color(new Vector3(
        GetRandomFloat(0.25f, 1.0f),
        GetRandomFloat(0.25f, 1.0f),
        GetRandomFloat(0.25f, 1.0f)));
    } // GetRandomColor()
    #endregion
  } // class RandomHelper
} // namespace XnaTetris.Helpers
