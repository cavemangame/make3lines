using System.Collections.Generic;
using XnaTetris.Blocks;
using XnaTetris.Helpers;


namespace XnaTetris.Game
{
  class RandomBlockHelper
  {
    // ������� ������ � ������������ �� ������
    public static BlockFactory.BlockType GenerateNewBlockType(Dictionary<BlockFactory.BlockType, int> allocation)
    {
      int len = 0; //��������� ����������� � ��������, �������� �� �������

      foreach (KeyValuePair<BlockFactory.BlockType, int> pair in allocation)
        len += pair.Value;

      int shot = RandomHelper.GetRandomInt(len - 1); //��������, ������� � ����� ������� ������

      int curPos = 0;
      foreach (KeyValuePair<BlockFactory.BlockType, int> pair in allocation)
      {
        if (curPos + pair.Value > shot)
          return pair.Key;
        curPos += pair.Value;
      }

      return BlockFactory.BlockType.Black;
    }

    // ���������� ������, ��� ��������� ��-�� � luck = 1
    public static BlockFactory.BlockType GenerateNewBlockType(BlockFactory.BlockType type, int luck)
    {
      Dictionary<BlockFactory.BlockType, int> allocation = new Dictionary<BlockFactory.BlockType, int>();
      EnumHelper.EnumEnumerator enumerator = EnumHelper.GetEnumerator(typeof(BlockFactory.BlockType));

      while (enumerator.MoveNext())
      {
        BlockFactory.BlockType curType = (BlockFactory.BlockType)enumerator.Current;
        if (curType == type)
          allocation.Add(curType, luck);
        else
          allocation.Add(curType, 1);
      }

      return GenerateNewBlockType(allocation);
    }
  }
}
