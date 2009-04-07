#region Using directives
using Microsoft.Xna.Framework.Storage;
using System.Collections.Generic;
using System.IO;
using System.Text;
#endregion

namespace XnaTetris.Helpers
{
  /// <summary>
  /// File helper class to get text lines, number of text lines, etc.
  /// </summary>
  public class FileHelper
  {
    #region LoadGameContentFile
    /// <summary>
    /// Load game content file, returns null if file was not found.
    /// </summary>
    /// <param name="relativeFilename">Relative filename</param>
    /// <returns>File stream</returns>
    public static FileStream LoadGameContentFile(string relativeFilename)
    {
      string fullPath = Path.Combine(
        StorageContainer.TitleLocation, relativeFilename);

      if (!File.Exists(fullPath))
      {
        return null;
      }
      return File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }
    #endregion

    #region OpenOrCreateFileForCurrentPlayer
    /// <summary>
    /// XNA user device, asks for the saving location on the Xbox360,
    /// theirfore remember this device for the time we run the game.
    /// </summary>
    private const StorageDevice xnaUserDevice = null;

    public static StorageDevice XnaUserDevice
    {
      get
      {
        return xnaUserDevice;
      }
    }

    /// <summary>
    /// Open or create file for current player. Basically just creates a
    /// FileStream using the specified FileMode flag, but on the Xbox360
    /// we have to ask the user first where he wants to.
    /// Basically used for the GameSettings and the Log class.
    /// </summary>
    /// <param name="filename">Filename</param>
    /// <param name="mode">Mode</param>
    /// <param name="access">Access</param>
    /// <returns>File stream</returns>
    public static FileStream OpenFileForCurrentPlayer(string filename, FileMode mode, FileAccess access)
    {
      StorageContainer container = XnaUserDevice.OpenContainer("XnaTetris");
      string fullFilename = Path.Combine(container.Path, filename);

      return new FileStream(fullFilename, mode, access, FileShare.ReadWrite);
    }
    #endregion

    #region Get text lines
    /// <summary>
    /// Returns the number of text lines we got in a file.
    /// </summary>
    static public string[] GetLines(string filename)
    {
      try
      {
        StreamReader reader = new StreamReader(
          new FileStream(filename, FileMode.Open, FileAccess.Read), Encoding.UTF8);
        List<string> lines = new List<string>();
        
        do
        {
          lines.Add(reader.ReadLine());
        } while (reader.Peek() > -1);
        reader.Close();
        return lines.ToArray();
      }
      catch
      {
        // Failed to read, just return null
        return null;
      }
    }
    #endregion

    #region Create text file
    public static void CreateTextFile(string filename, string textForFile, Encoding fileEncoding)
    {
      StreamWriter textWriter = new StreamWriter(
        new FileStream(filename, FileMode.Create, FileAccess.Write), fileEncoding);
      string[] textLines = StringHelper.SplitMultilineText(textForFile);

      foreach (string line in textLines)
      {
        textWriter.WriteLine(line);
      }
      textWriter.Close();
    }
    #endregion
  }
}
