#region Using directives
using System;
using System.Collections;
#endregion

namespace XnaTetris.Helpers
{
#if !XBOX360
  /// <summary>
  /// Enum helper
  /// </summary>
  class EnumHelper
  {
    #region Enum enumerator class
    /// <summary>
    /// Enum enumerator helper for GetEnumerator,
    /// this allow us to enumerate enums just like collections.
    /// </summary>
    public class EnumEnumerator : IEnumerator, IEnumerable
    {
      public Type enumType;
      public int index;
      public int enumLength;

      public EnumEnumerator(Type setEnumType)
      {
        enumType = setEnumType;
        index = -1;
        enumLength = GetSize(enumType);
      }

      public object Current
      {
        get
        {
          if (index >= 0 &&
            index < enumLength)
            return Enum.GetValues(enumType).GetValue(new[] { index });
          // Just return first entry if index is invalid
          return Enum.GetValues(enumType).GetValue(new[] { 0 });
        }
      }

      public bool MoveNext()
      {
        index++;

        return index < enumLength;
      }

      public void Reset()
      {
        index = -1;
      }

      public IEnumerator GetEnumerator()
      {
        return this;
      }
    }
    #endregion

    #region Get size
    /// <summary>
    /// Get number of elements of a enum (accessing enum by type)
    /// </summary>
    public static int GetSize(Type enumType)
    {
      return Enum.GetNames(enumType).Length;
    }
    #endregion

    #region Get enumerator
    public static EnumEnumerator GetEnumerator(Type enumType)
    {
      return new EnumEnumerator(enumType);
    }
    #endregion

    #region Search enumerator
    public static object SearchEnumerator(Type type, string name)
    {
      foreach (object objEnum in GetEnumerator(type))
        if (StringHelper.Compare(objEnum.ToString(), name))
          return objEnum;

      // Else not found, just return first!
      return GetEnumerator(type).Current;
    }
    #endregion

    #region Get all enum names
    /// <summary>
    /// Get all names from an enum.
    /// E.g. If we have an enum with 3 values (A, B and C),
    /// then return "A, B, C".
    /// </summary>
    /// <param name="type">Enum type, will be passed to
    /// GetEnumerator</param>
    /// <returns>String with all enum names</returns>
    public static string GetAllEnumNames(Type type)
    {
      // Simplified version
      return StringHelper.WriteArrayData(GetEnumerator(type));

      /*same code, but more complicated:
      List<string> returnList = new List<string>();
      foreach (Enum enumValue in GetEnumerator(type))
        returnList.Add(enumValue.ToString());
      return StringHelper.WriteArrayData(returnList);
       */
    }
    #endregion
  }
#endif
}
