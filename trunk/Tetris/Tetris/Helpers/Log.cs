#region Using directives
using System;
using System.IO;
#endregion

namespace XnaTetris.Helpers
{
  /// <summary>
  /// Log will create automatically a log file and write
  /// log/error/debug info for simple runtime error checking, very useful
  /// for minor errors, such as finding not files.
  /// The application can still continue working, but this log provides
  /// an easy support to find out what files are missing (in this example).
  /// </summary>
  public class Log
  {
    #region Variables
    /// <summary>
    /// Writer
    /// </summary>
    private static readonly StreamWriter writer;

    /// <summary>
    /// Log filename
    /// </summary>
    private const string LogFilename = "Log.txt";
    #endregion

    #region Static constructor to create log file
    /// <summary>
    /// Static constructor
    /// </summary>
    static Log()
    {
      try
      {
        // Open file
        FileStream file = FileHelper.OpenFileForCurrentPlayer(
          LogFilename, FileMode.OpenOrCreate, FileAccess.Write);

        if (file.Length > 2 * 1024 * 1024)
        {
          file.Close();
          file = FileHelper.OpenFileForCurrentPlayer(
            LogFilename, FileMode.Create, FileAccess.Write);
        }

        writer = file.Length == 0 ? new StreamWriter(file, System.Text.Encoding.UTF8) : new StreamWriter(file);

        writer.BaseStream.Seek(0, SeekOrigin.End);
        writer.AutoFlush = true;

        // Add some info about this session
        writer.WriteLine("");
        writer.WriteLine("/// Session started at: " + StringHelper.WriteIsoDateAndTime(DateTime.Now));
        writer.WriteLine("/// XnaTetris");
        writer.WriteLine("");
      }
      catch
      {
        // Ignore any file exceptions
      }
    }
    #endregion

    #region Write log entry
    /// <summary>
    /// Writes a LogType and info/error message string to the Log file
    /// </summary>
    static public void Write(string message)
    {
      // Can't continue without valid writer
      if (writer == null)
      {
        return;
      }

      try
      {
        DateTime ct = DateTime.Now;
        string s = "[" + ct.Hour.ToString("00") + ":" +
          ct.Minute.ToString("00") + ":" +
          ct.Second.ToString("00") + "] " +
          message;

        writer.WriteLine(s);
      }
      catch
      {
        // Ignore any file exceptions
      }
    }
    #endregion
  }
}
