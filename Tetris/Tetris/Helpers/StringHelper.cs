#region Using directives
using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
#endregion

namespace XnaTetris.Helpers
{
  /// <summary>
  /// StringHelper: Provides additional or simplified string functions.
  /// This class does also offer a lot of powerful functions and
  /// allows complicated string operations.
  /// Easy functions at the beginning, harder ones later.
  /// </summary>
  public class StringHelper
  {
    #region Constants
    public const string NEW_LINE = "\r\n";
    #endregion

    #region Constructor (it is not allowed to instantiate)
    private StringHelper() {}
    #endregion

    #region Comparing, Counting and Extraction
    /// <summary>
    /// Check if a string (s1, can be longer as s2) begins with another
    /// string (s2), only if s1 begins with the same string data as s2,
    /// true is returned, else false. The string compare is case insensitive.
    /// </summary>
    static public bool BeginsWith(string s1, string s2)
    {
      return String.Compare(s1, 0, s2, 0, s2.Length, true, CultureInfo.CurrentCulture) == 0;
    }

    /// <summary>
    /// Helps to compare strings, uses case insensitive comparison.
    /// String.Compare is also gay because we have always to check for == 0.
    /// </summary>
    static public bool Compare(string s1, string s2)
    {
      return String.Compare(s1, s2, true, CultureInfo.CurrentCulture) == 0;
    }

    /// <summary>
    /// Helps to compare strings, uses case insensitive comparison.
    /// String.Compare is also gay because we have always to check for == 0.
    /// This overload allows multiple strings to be checked, if any of
    /// them matches we are good to go (e.g. ("hi", {"hey", "hello", "hi"})
    /// will return true).
    /// </summary>
    static public bool Compare(string s1, string[] anyMatch)
    {
      if (anyMatch == null)
      {
        throw new ArgumentNullException("anyMatch", "Unable to execute method without valid anyMatch array.");
      }

      foreach (string match in anyMatch)
      {
        if (String.Compare(s1, match, true, CultureInfo.CurrentCulture) == 0)
        {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Is a specific name in a list of strings?
    /// </summary>
    static public bool IsInList(string name, ArrayList list, bool ignoreCase)
    {
      if (list == null)
      {
        throw new ArgumentNullException("list", "Unable to execute method without valid list.");
      }

      foreach (string listEntry in list)
      {
        if (String.Compare(name, listEntry, ignoreCase, CultureInfo.CurrentCulture) == 0)
        {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Is a specific name in a list of strings?
    /// </summary>
    static public bool IsInList(string name, string[] list, bool ignoreCase)
    {
      if (list == null)
      {
        throw new ArgumentNullException("list", "Unable to execute method without valid list.");
      }

      foreach (string listEntry in list)
      {
        if (String.Compare(name, listEntry, ignoreCase, CultureInfo.CurrentCulture) == 0)
        {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Count words in a text (words are only separated by ' ' (spaces))
    /// </summary>
    static public int CountWords(string text)
    {
      if (text == null)
      {
        throw new ArgumentNullException("text", "Unable to execute method without valid text.");
      }

      return text.Split(new[] { ' ' }).Length;
    }

    public static bool CompareCharCaseInsensitive(char c1, char c2)
    {
      return char.ToLower(c1) == char.ToLower(c2);
    }

    public static string GetLastWord(string text)
    {
      if (text == null)
      {
        throw new ArgumentNullException("text", "Unable to execute method without valid text.");
      }

      string[] words = text.Split(new[] { ' ' });

      if (words.Length > 0)
      {
        return words[words.Length - 1];
      }
      return text;
    }

    public static string RemoveLastWord(string text)
    {
      string lastWord = GetLastWord(text);

      if (text == lastWord)
      {
        return "";
      }
      if (lastWord.Length == 0 || text.Length == 0 || text.Length - lastWord.Length - 1 <= 0)
      {
        return text;
      }
      return text.Substring(0, text.Length - lastWord.Length - 1);
    }

    static public string GetAllSpacesAndTabsAtBeginning(string text)
    {
      if (text == null)
      {
        throw new ArgumentNullException("text", "Unable to execute method without valid text.");
      }

      StringBuilder ret = new StringBuilder();

      for (int pos = 0; pos < text.Length; pos++)
      {
        if (text[pos] == ' ' || text[pos] == '\t')
        {
          ret.Append(text[pos]);
        }
        else
        {
          break;
        }
      }
      return ret.ToString();
    }

    static public int GetTabDepth(string text)
    {
      for (int textPos = 0; textPos < text.Length; textPos++)
      {
        if (text[textPos] != '\t')
        {
          return textPos;
        }
      }
      return text.Length;
    }

    public static string CheckStringWordLength(string originalText, int maxLength)
    {
      if (originalText == null)
      {
        throw new ArgumentNullException("originalText", "Unable to execute method without valid text.");
      }

      string[] splitted = originalText.Split(new[] { ' ' });
      string ret = "";

      foreach (string word in splitted)
      {
        if (word.Length <= maxLength)
        {
          ret += word + " ";
        }
        else
        {
          for (int i = 0; i < word.Length / maxLength; i++)
          {
            ret += word.Substring(i * maxLength, maxLength) + " ";
          }
        }
      }
      return ret.TrimEnd();
    }
    #endregion

    #region String contains (for case insensitive compares)
    /// <summary>
    /// Is searchName contained in textToCheck, will check case insensitive,
    /// for a normal case sensitive test use textToCheck.Contains(searchName)
    /// </summary>
    /// <param name="textToCheck">Text to check</param>
    /// <param name="searchName">Search name</param>
    /// <returns>Bool</returns>
    public static bool Contains(string textToCheck, string searchName)
    {
      return textToCheck.ToLower().Contains(searchName.ToLower());
    }

    /// <summary>
    /// Is any of the names in searchNames contained in textToCheck,
    /// will check case insensitive, for a normal case sensitive test
    /// use textToCheck.Contains(searchName).
    /// </summary>
    /// <param name="textToCheck">String to check</param>
    /// <param name="searchNames">Search names</param>
    /// <returns>Bool</returns>
    public static bool Contains(string textToCheck, string[] searchNames)
    {
      string stringToCheckLower = textToCheck.ToLower();

      foreach (string name in searchNames)
      {
        if (stringToCheckLower.Contains(name.ToLower()))
        {
          return true;
        }
      }
      return false;
    }
    #endregion

    #region Write data
    /// <summary>
    /// Returns a string with the array data, byte array version.
    /// </summary>
    static public string WriteArrayData(byte[] byteArray)
    {
      StringBuilder ret = new StringBuilder();

      if (byteArray != null)
      {
        for (int i = 0; i < byteArray.Length; i++)
        {
          ret.Append((ret.Length == 0 ? "" : ", ") +
                     byteArray[i].ToString(CultureInfo.InvariantCulture.NumberFormat));
        }
      }
      return ret.ToString();
    }

    /// <summary>
    /// Returns a string with the array data, int array version.
    /// </summary>
    static public string WriteArrayData(int[] intArray)
    {
      StringBuilder ret = new StringBuilder();

      if (intArray != null)
      {
        for (int i = 0; i < intArray.Length; i++)
        {
          ret.Append((ret.Length == 0 ? "" : ", ") +
                     intArray[i].ToString(CultureInfo.InvariantCulture.NumberFormat));
        }
      }
      return ret.ToString();
    }

    /// <summary>
    /// Returns a string with the array data, general array version.
    /// </summary>
    static public string WriteArrayData(Array array)
    {
      StringBuilder ret = new StringBuilder();

      if (array != null)
      {
        for (int i = 0; i < array.Length; i++)
        {
          ret.Append((ret.Length == 0 ? "" : ", ") +
                     (array.GetValue(i) == null ? "null" : array.GetValue(i).ToString()));
        }
      }
      return ret.ToString();
    }

    /// <summary>
    /// Returns a string with the array data, general array version
    /// with maxLength bounding (will return string with max. this
    /// number of entries).
    /// </summary>
    static public string WriteArrayData(Array array, int maxLength)
    {
      StringBuilder ret = new StringBuilder();

      if (array != null)
      {
        for (int i = 0; i < array.Length && i < maxLength; i++)
        {
          ret.Append((ret.Length == 0 ? "" : ", ") + array.GetValue(i));
        }
      }
      return ret.ToString();
    }

    /// <summary>
    /// Returns a string with the array data, ArrayList version.
    /// </summary>
    static public string WriteArrayData(ArrayList array)
    {
      StringBuilder ret = new StringBuilder();

      if (array != null)
      {
        foreach (object obj in array)
        {
          ret.Append((ret.Length == 0 ? "" : ", ") + obj);
        }
      }
      return ret.ToString();
    }

    /// <summary>
    /// Returns a string with the array data, CollectionBase version.
    /// </summary>
    static public string WriteArrayData(CollectionBase collection)
    {
      StringBuilder ret = new StringBuilder();

      if (collection != null)
      {
        foreach (object obj in collection)
        {
          ret.Append((ret.Length == 0 ? "" : ", ") + obj);
        }
      }
      return ret.ToString();
    }

#if !XBOX360
    /// <summary>
    /// Returns a string with the array data, StringCollection version.
    /// </summary>
    static public string WriteArrayData(StringCollection textCollection)
    {
      StringBuilder ret = new StringBuilder();

      if (textCollection != null)
      {
        foreach (string s in textCollection)
        {
          ret.Append((ret.Length == 0 ? "" : ", ") + s);
        }
      }
      return ret.ToString();
    }
#endif

    /// <summary>
    /// Returns a string with the array data, enumerable class version.
    /// </summary>
    static public string WriteArrayData(IEnumerable enumerableClass)
    {
      StringBuilder ret = new StringBuilder();

      if (enumerableClass != null)
      {
        foreach (object obj in enumerableClass)
        {
          ret.Append((ret.Length == 0 ? "" : ", ") + obj);
        }
      }
      return ret.ToString();
    }

    /// <summary>
    /// Write into space string, useful for writing parameters without
    /// knowing the length of each string, e.g. when writing numbers
    /// (-1, 1.45, etc.). You can use this function to give all strings
    /// the same width in a table. Maybe there is already a string function
    /// for this, but I don't found any useful stuff.
    /// </summary>
    static public string WriteIntoSpaceString(string message, int spaces)
    {
      if (message == null)
      {
        throw new ArgumentNullException("message", "Unable to execute method without valid text.");
      }

      if (message.Length >= spaces)
      {
        return message;
      }

      // Create string with number of specified spaces
      char[] ret = new char[spaces];

      // Copy data
      for (int i = 0; i < message.Length; i++)
      {
        ret[i] = message[i];
      }
      // Fill rest with spaces
      for (int i = message.Length; i < spaces; i++)
      {
        ret[i] = ' ';
      }

      return new string(ret);
    }

    /// <summary>
    /// Year-Month-Day
    /// </summary>
    public static string WriteIsoDate(DateTime date)
    {
      return date.Year + "-" +
        date.Month.ToString("00") + "-" +
        date.Day.ToString("00");
    }

    /// <summary>
    /// Year-Month-Day Hour:Minute
    /// </summary>
    public static string WriteIsoDateAndTime(DateTime date)
    {
      return date.Year + "-" +
        date.Month.ToString("00") + "-" +
        date.Day.ToString("00") + " " +
        date.Hour.ToString("00") + ":" +
        date.Minute.ToString("00");
    }

    public static string WriteInternetTime(DateTime time, bool daylightSaving)
    {
      return "@" + (((int)(time.ToUniversalTime().AddHours(
        daylightSaving ? 1 : 0).TimeOfDay.
        TotalSeconds * 100000 / (24 * 60 * 60))) / 100.0f).ToString(NumberFormatInfo.InvariantInfo);
    }
    #endregion

    #region Convert methods
    /// <summary>
    /// Convert string data to int array, string must be in the form
    /// "1, 3, 8, 7", etc. WriteArrayData is the complementar function.
    /// </summary>
    /// <returns>int array, will be null if string is invalid!</returns>
    static public int[] ConvertStringToIntArray(string s)
    {
      if (string.IsNullOrEmpty(s))
        return null;

      string[] splitted = s.Split(new[] { ' ' });
      int[] ret = new int[splitted.Length];
      for (int i = 0; i < ret.Length; i++)
      {
        try
        {
          ret[i] = Convert.ToInt32(splitted[i]);
        }
        catch { } // ignore
      }
      return ret;
    }

    /// <summary>
    /// Convert string data to float array, string must be in the form
    /// "1.5, 3.534, 8.76, 7.49", etc. WriteArrayData is the complementar
    /// function.
    /// </summary>
    /// <returns>float array, will be null if string is invalid!</returns>
    static public float[] ConvertStringToFloatArray(string s)
    {
      if (string.IsNullOrEmpty(s))
        return null;

      string[] splitted = s.Split(new[] { ' ' });
      float[] ret = new float[splitted.Length];
      for (int i = 0; i < ret.Length; i++)
      {
        try
        {
          ret[i] = Convert.ToSingle(splitted[i],
            CultureInfo.InvariantCulture);
        }
        catch { } // ignore
      }
      return ret;
    }
    #endregion

    #region File stuff
    /// <summary>
    /// Extracts filename from full path+filename, cuts of extension
    /// if cutExtension is true. Can be also used to cut of directories
    /// from a path (only last one will remain).
    /// </summary>
    static public string ExtractFilename(string pathFile, bool cutExtension)
    {
      if (pathFile == null)
        return "";

      // Update 2006-09-29: also checking for normal slashes, needed
      // for support reading 3ds max stuff.
      string[] fileName = pathFile.Split(new[] { '\\', '/' });
      if (fileName.Length == 0)
      {
        if (cutExtension)
          return CutExtension(pathFile);
        return pathFile;
      } // if (fileName.Length)

      if (cutExtension)
        return CutExtension(fileName[fileName.Length - 1]);
      return fileName[fileName.Length - 1];
    } // ExtractFilename(pathFile, cutExtension)

    /// <summary>
    /// Get directory of path+File, if only a path is given we will cut off
    /// the last sub path!
    /// </summary>
    static public string GetDirectory(string pathFile)
    {
      if (pathFile == null)
        return "";
      int i = pathFile.LastIndexOf("\\");
      if (i >= 0 && i < pathFile.Length)
        // Return directory
        return pathFile.Substring(0, i);
      // No sub directory found (parent of some dir is "")
      return "";
    }

    /// <summary>
    /// Same as GetDirectory(): Get directory of path+File,
    /// if only a path is given we will cut of the last sub path!
    /// </summary>
    static public string CutOneFolderOff(string path)
    {
      // GetDirectory does exactly what we need!
      return GetDirectory(path);
    }

    /// <summary>
    /// Splits a path into all parts of its directories,
    /// e.g. "maps\\sub\\kekse" becomes
    /// {"maps\\sub\\kekse","maps\\sub","maps"}
    /// </summary>
    static public string[] SplitDirectories(string path)
    {
      ArrayList localList = new ArrayList {path};

      do
      {
        path = CutOneFolderOff(path);
        if (path.Length > 0)
          localList.Add(path);
      } while (path.Length > 0);

      return (string[])localList.ToArray(typeof(string));
    }

    /// <summary>
    /// Remove first directory of path (if one exists).
    /// e.g. "maps\\mymaps\\hehe.map" becomes "mymaps\\hehe.map"
    /// Also used to cut first folder off, especially useful for relative
    /// paths. e.g. "maps\\test" becomes "test"
    /// </summary>
    static public string RemoveFirstDirectory(string path)
    {
      int i = path.IndexOf("\\");
      if (i >= 0 && i < path.Length)
        // Return rest of path
        return path.Substring(i + 1);
      // No first directory found, just return original path
      return path;
    }

    /// <summary>
    /// Check if a folder is a direct sub folder of a main folder.
    /// True is only returned if this is a direct sub folder, not if
    /// it is some sub folder few levels below.
    /// </summary>
    static public bool IsDirectSubfolder(string subfolder, string mainFolder)
    {
      // First check if subFolder is really a sub folder of mainFolder
      if (subfolder != null &&
        subfolder.StartsWith(mainFolder))
      {
        // Same order?
        if (subfolder.Length < mainFolder.Length + 1)
          // Then it ain't a sub folder!
          return false;
        // Ok, now check if this is direct sub folder or some sub folder
        // of mainFolder sub folder
        string folder = subfolder.Remove(0, mainFolder.Length + 1);
        // Check if this is really a direct sub folder
        for (int i = 0; i < folder.Length; i++)
          if (folder[i] == '\\')
            // No, this is a sub folder of mainFolder sub folder
            return false;
        // Ok, this is a direct sub folder of mainFolder!
        return true;
      } 
      // Not even any sub folder!
      return false;
    }

    /// <summary>
    /// Cut of extension, e.g. "hi.txt" becomes "hi"
    /// </summary>
    static public string CutExtension(string file)
    {
      if (file == null)
        return "";
      int l = file.LastIndexOf('.');
      if (l > 0)
        return file.Remove(l, file.Length - l);
      return file;
    }

    /// <summary>
    /// Get extension (the stuff behind that '.'),
    /// e.g. "test.bmp" will return "bmp"
    /// </summary>
    static public string GetExtension(string file)
    {
      if (file == null)
        return "";
      int l = file.LastIndexOf('.');
      if (l > 0 && l < file.Length)
        return file.Remove(0, l + 1);
      return "";
    }
    #endregion

    #region String splitting and getting it back together
    /// <summary>
    /// Performs basically the same job as String.Split, but does
    /// trim all parts, no empty parts are returned, e.g.
    /// "hi  there" returns "hi", "there", String.Split would return
    /// "hi", "", "there".
    /// </summary>
    public static string[] SplitAndTrim(string text, char separator)
    {
      ArrayList ret = new ArrayList();

      string[] splitted = text.Split(new[] { separator });
      foreach (string s in splitted)
        if (s.Length > 0)
          ret.Add(s);
      return (string[])ret.ToArray(typeof(string));
    }

    /// <summary>
    /// Splits a multi line string to several strings and
    /// returns the result as a string array.
    /// Will also remove any \r, \n or space character
    /// at the end of each line!
    /// </summary>
    public static string[] SplitMultilineText(string text)
    {
      if (text == null)
        throw new ArgumentNullException("text",
          "Unable to execute method without valid text.");

      ArrayList ret = new ArrayList();
      // Supports any format, only \r, only \n, normal \n\r,
      // crazy \r\n or even mixed \n\r with any format
      string[] splitted1 = text.Split(new[] { '\n' });
      string[] splitted2 = text.Split(new[] { '\r' });
      string[] splitted =
        splitted1.Length >= splitted2.Length ?
      splitted1 : splitted2;

      foreach (string s in splitted)
      {
        // Never add any \r or \n to the single lines
        if (s.EndsWith("\r") ||
          s.EndsWith("\n"))
          ret.Add(s.Substring(0, s.Length - 1));
        else if (s.StartsWith("\n") ||
          s.StartsWith("\r"))
          ret.Add(s.Substring(1));
        else
          ret.Add(s);
      }

      return (string[])ret.ToArray(typeof(string));
    }

    static public string BuildStringFromLines(string[] lines, int startLine, int startOffset,
      int endLine, int endOffset, string separator)
    {
      if (lines == null)
        throw new ArgumentNullException("lines",
          "Unable to execute method without valid lines.");

      // Check if all values are in range (correct if not)
      if (startLine >= lines.Length)
        startLine = lines.Length - 1;
      if (endLine >= lines.Length)
        endLine = lines.Length - 1;
      if (startLine < 0)
        startLine = 0;
      if (endLine < 0)
        endLine = 0;
      if (startOffset >= lines[startLine].Length)
        startOffset = lines[startLine].Length - 1;
      if (endOffset >= lines[endLine].Length)
        endOffset = lines[endLine].Length - 1;
      if (startOffset < 0)
        startOffset = 0;
      if (endOffset < 0)
        endOffset = 0;

      StringBuilder builder = new StringBuilder((endLine - startLine) * 80);
      for (int lineNumber = startLine; lineNumber <= endLine; lineNumber++)
      {
        if (lineNumber == startLine)
          builder.Append(lines[lineNumber].Substring(startOffset));
        else if (lineNumber == endLine)
          builder.Append(lines[lineNumber].Substring(0, endOffset + 1));
        else
          builder.Append(lines[lineNumber]);

        if (lineNumber != endLine)
          builder.Append(separator);
      } 
      return builder.ToString();
    } 

    static public string BuildStringFromLines(
      string[] lines, string separator)
    {
      StringBuilder builder = new StringBuilder(lines.Length * 80);
      for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
      {
        builder.Append(lines[lineNumber]);
        if (lineNumber != lines.Length - 1)
          builder.Append(separator);
      }
      return builder.ToString();
    }

    static public string BuildStringFromLines(string[] lines)
    {
      return BuildStringFromLines(lines, NEW_LINE);
    }

    static public string BuildStringFromLines(string[] lines, int startLine, int endLine, string separator)
    {
      // Check if all values are in range (correct if not)
      if (startLine < 0)
        startLine = 0;
      if (endLine < 0)
        endLine = 0;
      if (startLine >= lines.Length)
        startLine = lines.Length - 1;
      if (endLine >= lines.Length)
        endLine = lines.Length - 1;

      StringBuilder builder = new StringBuilder((endLine - startLine) * 80);
      for (int lineNumber = startLine; lineNumber <= endLine; lineNumber++)
      {
        builder.Append(lines[lineNumber]);
        if (lineNumber != endLine)
          builder.Append(separator);
      }
      return builder.ToString();
    }

    public enum CutMode
    {
      Begin,
      End,
      BothEnds
    }

    public static string MaxStringLength(string originalText, int maxLength, CutMode cutMode)
    {
      if (originalText.Length <= maxLength)
        return originalText;

      if (cutMode == CutMode.Begin)
        return originalText.Substring(
          originalText.Length - maxLength, maxLength);
      if (cutMode == CutMode.End)
        return originalText.Substring(0, maxLength);
      // logic: if ( cutMode == CutModes.BothEnds )
      return originalText.Substring((originalText.Length - maxLength) / 2, maxLength);
    }

    /// <summary>
    /// Get left part of everything to the left of the first
    /// occurrence of a character.
    /// </summary>
    public static string GetLeftPartAtFirstOccurence(
      string sourceText, char ch)
    {
      if (sourceText == null)
        throw new ArgumentNullException("sourceText",
          "Unable to execute this method without valid string.");

      int index = sourceText.IndexOf(ch);
      if (index == -1)
        return sourceText;

      return sourceText.Substring(0, index);
    }

    /// <summary>
    /// Get right part of everything to the right of the first
    /// occurrence of a character.
    /// </summary>
    public static string GetRightPartAtFirstOccurrence(
      string sourceText, char ch)
    {
      if (sourceText == null)
        throw new ArgumentNullException("sourceText",
          "Unable to execute this method without valid string.");

      int index = sourceText.IndexOf(ch);
      if (index == -1)
        return "";

      return sourceText.Substring(index + 1);
    }

    /// <summary>
    /// Get left part of everything to the left of the last
    /// occurrence of a character.
    /// </summary>
    public static string GetLeftPartAtLastOccurrence(
      string sourceText, char ch)
    {
      if (sourceText == null)
        throw new ArgumentNullException("sourceText",
          "Unable to execute this method without valid string.");

      int index = sourceText.LastIndexOf(ch);
      if (index == -1)
        return sourceText;

      return sourceText.Substring(0, index);
    }

    /// <summary>
    /// Get right part of everything to the right of the last
    /// occurrence of a character.
    /// </summary>
    public static string GetRightPartAtLastOccurrence(string sourceText, char ch)
    {
      if (sourceText == null)
        throw new ArgumentNullException("sourceText",
          "Unable to execute this method without valid string.");

      int index = sourceText.LastIndexOf(ch);
      if (index == -1)
        return sourceText;

      return sourceText.Substring(index + 1);
    }

    public static string CreatePasswordString(string originalText)
    {
      if (originalText == null)
        throw new ArgumentNullException("originalText",
          "Unable to execute this method without valid string.");

      string passwordString = "";
      for (int i = 0; i < originalText.Length; i++)
        passwordString += "*";
      return passwordString;
    }

    /// <summary>
    /// Helper function to convert letter to lowercase. Could someone
    /// tell me the reason why there is no function for that in char?
    /// </summary>
    public static char ToLower(char letter)
    {
      return letter.ToString().ToLower(CultureInfo.InvariantCulture)[0];
    }

    /// <summary>
    /// Helper function to convert letter to uppercase. Could someone
    /// tell me the reason why there is no function for that in char?
    /// </summary>
    public static char ToUpper(char letter)
    {
      return letter.ToString().ToUpper(
        CultureInfo.InvariantCulture)[0];
    }

    /// <summary>
    /// Helper function to check if this is an lowercase letter.
    /// </summary>
    public static bool IsLowercaseLetter(char letter)
    {
      return letter == ToLower(letter);
    }

    /// <summary>
    /// Helper function to check if this is an uppercase letter.
    /// </summary>
    public static bool IsUppercaseLetter(char letter)
    {
      return letter == ToUpper(letter);
    }

    /// <summary>
    /// Helper function for SplitFunctionNameToWordString to detect
    /// abbreviations in the function name
    /// </summary>
    private static int GetAbbreviationLengthInFunctionName(
      string functionName, int startPos)
    {
      StringBuilder abbreviation = new StringBuilder();
      // Go through string until we reach a lower letter or it ends
      for (int pos = startPos; pos < functionName.Length; pos++)
      {
        // Quit if its not an uppercase letter
        if (!IsUppercaseLetter(functionName[pos]))
          break;
        // Else just add letter
        abbreviation.Append(functionName[pos]);
      }

      // Abbreviation has to be at least 2 letters long.
      if (abbreviation.Length >= 2)
      {
        // If not at end of functionName, last letter belongs to next name,
        // e.g. "TW" is not a abbreviation in "HiMrTWhatsUp",
        // "AB" isn't one either in "IsABall",
        // but "PQ" is in "PQList" and "AB" is in "MyAB"
        if (startPos + abbreviation.Length >= functionName.Length)
          // Ok, then return full abbreviation length
          return abbreviation.Length;
        // Else return length - 1 because of next word
        return abbreviation.Length - 1;
      }

      // No Abbreviation, just return 1
      return 1;
    }

    /// <summary>
    /// Checks if letter is space ' ' or any punctuation (. , : ; ' " ! ?)
    /// </summary>
    public static bool IsSpaceOrPunctuation(char letter)
    {
      return
        letter == ' ' ||
        letter == '.' ||
        letter == ',' ||
        letter == ':' ||
        letter == ';' ||
        letter == '\'' ||
        letter == '\"' ||
        letter == '!' ||
        letter == '?' ||
        letter == '*';
    }

    /// <summary>
    /// Splits a function name to words, e.g.
    /// "MakeDamageOnUnit" gets "Make damage on unit".
    /// Will also detect abbreviation like TCP and leave them
    /// intact, e.g. "CreateTCPListener" gets "Create TCP listener".
    /// </summary>
    public static string SplitFunctionNameToWordString(string functionName)
    {
      if (string.IsNullOrEmpty(functionName))
        return "";

      string ret = "";
      // Go through functionName and find big letters!
      for (int pos = 0; pos < functionName.Length; pos++)
      {
        char letter = functionName[pos];
        // First letter is always big!
        if (pos == 0 ||
          pos == 1 && IsUppercaseLetter(functionName[1]) &&
          IsUppercaseLetter(functionName[0]) ||
          pos == 2 && IsUppercaseLetter(functionName[2]) &&
          IsUppercaseLetter(functionName[1]) &&
          IsUppercaseLetter(functionName[0]))
          ret += ToUpper(letter);
        // Found uppercase letter?
        else if (IsUppercaseLetter(letter) &&
          //also support numbers and other symbols not lower/upper letter:
          //StringHelper.IsLowercaseLetter(letter) == false &&
          // But don't allow space or any punctuation (. , : ; ' " ! ?)
          !IsSpaceOrPunctuation(letter) && !ret.EndsWith(" "))
        {
          // Could be new word, but we have to check if its an abbreviation
          int abbreviationLength = GetAbbreviationLengthInFunctionName(
            functionName, pos);
          // Found valid abbreviation?
          if (abbreviationLength > 1)
          {
            // Then add it
            ret += " " + functionName.Substring(pos, abbreviationLength);
            // And advance pos (abbreviation is longer than 1 letter)
            pos += abbreviationLength - 1;
          } // if (abbreviationLength)
          // Else just add new word (in lower letter)
          else
            ret += " " + ToLower(letter);
        }
        else
          // Just add letter
          ret += letter;
      }
      return ret;
    }
    #endregion

    #region Remove character
    public static void RemoveCharacter(ref string text, char characterToBeRemoved)
    {
      if (text == null)
        throw new ArgumentNullException("text",
          "Unable to execute method without valid text.");

      if (text.Contains(characterToBeRemoved.ToString()))
        text = text.Replace(characterToBeRemoved.ToString(), "");
    }
    #endregion

    #region Kb/mb name generator
    /// <summary>
    /// Write bytes, KB, MB, GB, TB message.
    /// 1 KB = 1024 Bytes
    /// 1 MB = 1024 KB = 1048576 Bytes
    /// 1 GB = 1024 MB = 1073741824 Bytes
    /// 1 TB = 1024 GB = 1099511627776 Bytes
    /// E.g. 100 will return "100 Bytes"
    /// 2048 will return "2.00 KB"
    /// 2500 will return "2.44 KB"
    /// 1534905 will return "1.46 MB"
    /// 23045904850904 will return "20.96 TB"
    /// </summary>
    public static string WriteBigByteNumber(
      long bigByteNumber, string decimalSeperator)
    {
      if (bigByteNumber < 0)
        return "-" + WriteBigByteNumber(-bigByteNumber);

      if (bigByteNumber <= 999)
        return bigByteNumber + " Bytes";
      if (bigByteNumber <= 999 * 1024)
      {
        double fKB = bigByteNumber / 1024.0;
        return (int)fKB + decimalSeperator + ((int)(fKB * 100.0f) % 100).ToString("00") + " KB";
      }
      if (bigByteNumber <= 999 * 1024 * 1024)
      {
        double fMB = bigByteNumber / (1024.0 * 1024.0);
        return (int)fMB + decimalSeperator +
          ((int)(fMB * 100.0f) % 100).ToString("00") + " MB";
      } // if
      // this is very big ^^ will not fit into int
      if (bigByteNumber <= 999L * 1024L * 1024L * 1024L)
      {
        double fGB = bigByteNumber / (1024.0 * 1024.0 * 1024.0);
        return (int)fGB + decimalSeperator +
          ((int)(fGB * 100.0f) % 100).ToString("00") + " GB";
      }
      double fTB = bigByteNumber / (1024.0 * 1024.0 * 1024.0 * 1024.0);
      return (int)fTB + decimalSeperator + ((int)(fTB * 100.0f) % 100).ToString("00") + " TB";
    }

    /// <summary>
    /// Write bytes, KB, MB, GB, TB message.
    /// 1 KB = 1024 Bytes
    /// 1 MB = 1024 KB = 1048576 Bytes
    /// 1 GB = 1024 MB = 1073741824 Bytes
    /// 1 TB = 1024 GB = 1099511627776 Bytes
    /// E.g. 100 will return "100 Bytes"
    /// 2048 will return "2.00 KB"
    /// 2500 will return "2.44 KB"
    /// 1534905 will return "1.46 MB"
    /// 23045904850904 will return "20.96 TB"
    /// </summary>
    public static string WriteBigByteNumber(long bigByteNumber)
    {
      string decimalSeperator = CultureInfo.CurrentCulture.
        NumberFormat.CurrencyDecimalSeparator;
      return WriteBigByteNumber(bigByteNumber, decimalSeperator);
    }
    #endregion

    #region Try parse methods that are not available on the XBox360!
    public static bool IsNumericFloat(string str)
    {
      return IsNumericFloat(str, CultureInfo.InvariantCulture.NumberFormat);
    }

    /// <summary>
    /// Allow only one decimal point, used for IsNumericFloat.
    /// </summary>
    /// <param name="str">Input string to check</param>
    /// <param name="numberFormat">Used number format, e.g.
    /// CultureInfo.InvariantCulture.NumberFormat</param>
    /// <return>True if check succeeded, false otherwise</return>
    private static bool AllowOnlyOneDecimalPoint(string str, NumberFormatInfo numberFormat)
    {
      char[] strInChars = str.ToCharArray();
      bool hasGroupSeperator = false;
      int decimalSeperatorCount = 0;
      for (int i = 0; i < strInChars.Length; i++)
      {
        if (numberFormat.CurrencyDecimalSeparator.IndexOf(strInChars[i]) == 0)
        {
          decimalSeperatorCount++;
        }

        // has float group seperators  ?
        if (numberFormat.CurrencyGroupSeparator.IndexOf(strInChars[i]) == 0)
        {
          hasGroupSeperator = true;
        }
      }

      if (hasGroupSeperator)
      {
        // If first digit is the group seperator or begins with 0,
        // there is something wrong, the group seperator is used as a comma.
        if (str.StartsWith(numberFormat.CurrencyGroupSeparator) ||
          strInChars[0] == '0')
          return false;

        // look only at the digits in front of the decimal point
        string[] splittedByDecimalSeperator = str.Split(
          numberFormat.CurrencyDecimalSeparator.ToCharArray());

        #region Invert the digits for modulo check
        //   ==> 1.000 -> 000.1  ==> only after 3 digits 
        char[] firstSplittedInChars = splittedByDecimalSeperator[0].ToCharArray();
        int arrayLength = firstSplittedInChars.Length;
        char[] firstSplittedInCharsInverted = new char[arrayLength];
        for (int i = 0; i < arrayLength; i++)
        {
          firstSplittedInCharsInverted[i] =
            firstSplittedInChars[arrayLength - 1 - i];
        }
        #endregion

        // group seperators are only allowed between 3 digits -> 1.000.000
        for (int i = 0; i < arrayLength; i++)
        {
          if (i % 3 != 0 && numberFormat.CurrencyGroupSeparator.IndexOf(
            firstSplittedInCharsInverted[i]) == 0)
          {
            return false;
          }
        }
      }
      if (decimalSeperatorCount > 1)
        return false;

      return true;
    }

    /// <summary>
    /// Checks if string is numeric float value
    /// </summary>
    /// <param name="str">Input string</param>
    /// <param name="numberFormat">Used number format, e.g.
    /// CultureInfo.InvariantCulture.NumberFormat</param>
    /// <returns>True if str can be converted to a float,
    /// false otherwise</returns>
    public static bool IsNumericFloat(string str, NumberFormatInfo numberFormat)
    {
      // Can't be a float if string is not valid!
      if (String.IsNullOrEmpty(str))
        return false;

      //not supported by Convert.ToSingle:
      //if (str.EndsWith("f"))
      //	str = str.Substring(0, str.Length - 1);

      // Only 1 decimal point is allowed
      if (!AllowOnlyOneDecimalPoint(str, numberFormat))
        return false;

      // + allows in the first,last,don't allow in middle of the string
      // - allows in the first,last,don't allow in middle of the string
      // $ allows in the first,last,don't allow in middle of the string
      // , allows in the last,middle,don't allow in first char of the string
      // . allows in the first,last,middle, allows in all the indexs
      bool retVal = false;

      // If string is just 1 letter, don't allow it to be a sign
      if (str.Length == 1 && "+-$.,".IndexOf(str[0]) >= 0)
        return false;

      for (int i = 0; i < str.Length; i++)
      {
        // For first indexchar
        char pChar = Convert.ToChar(str.Substring(i, 1));

        retVal = false;
        if (str.IndexOf(pChar) == 0)
        {
          retVal = ("+-$.0123456789".IndexOf(pChar) >= 0) ? true : false;
        }
        // For middle characters
        if ((!retVal) && (str.IndexOf(pChar) > 0) &&
          (str.IndexOf(pChar) < (str.Length - 1)))
        {
          retVal = (",.0123456789".IndexOf(pChar) >= 0) ? true : false;
        } // if ()
        // For last characters
        if ((!retVal) && (str.IndexOf(pChar) == (str.Length - 1)))
        {
          retVal = ("+-$,.0123456789".IndexOf(pChar) >= 0) ? true : false;
        } // if ()

        if (!retVal)
          break;
      }

      return retVal;
    }

    /// <summary>
    /// Try to convert to float. Will not modify value if that does not work. 
    /// This uses also always the invariant culture.
    /// </summary>
    /// <param name="value">Value</param>
    /// <param name="textToConvert">Text to convert</param>
    public static void TryToConvertToFloat(ref float value, string textToConvert)
    {
      TryToConvertToFloat(ref value, textToConvert, NumberFormatInfo.InvariantInfo);
    }

    /// <summary>
    /// Try to convert to float. Will not modify value if that does not work.
    /// </summary>
    /// <param name="value">Value</param>
    /// <param name="textToConvert">Text to convert</param>
    /// <param name="format">Format for converting</param>
    public static void TryToConvertToFloat(
      ref float value, string textToConvert, NumberFormatInfo format)
    {
      // Basically the same as float.TryParse(), but faster!
      if (IsNumericFloat(textToConvert, format))
      {
        value = Convert.ToSingle(textToConvert, format);
      }
    }

    /// <summary>
    /// Check if string is numeric integer. A decimal point is not accepted.
    /// </summary>
    /// <param name="str">String to check</param>
    public static bool IsNumericInt(string str)
    {
      // Can't be an int if string is not valid!
      if (String.IsNullOrEmpty(str))
        return false;

      // Go through every letter in str
      int strPos = 0;
      foreach (char ch in str)
      {
        // Only 0-9 are allowed
        if ("0123456789".IndexOf(ch) < 0 &&
          // Allow +/- for first char
          (strPos > 0 || (ch != '-' && ch != '+')))
          return false;
        strPos++;
      }

      // All fine, return true, this is a number!
      return true;
    }
    #endregion

  } // class StringHelper
} // namespace XnaTetris.Helpers
