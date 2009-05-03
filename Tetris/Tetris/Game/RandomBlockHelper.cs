using System.Collections.Generic;
using XnaTetris.Blocks;
using XnaTetris.Helpers;


namespace XnaTetris.Game
{
  class RandomBlockHelper
  {
    // ������� ������ � ������������ �� ������
    public static BlockFactory.BlockType GenerateNewBlockType(Dictionary<BlockFactory.BlockType, float> allocation)
    {
      float len = 0; //��������� ����������� � ��������, �������� �� �������

      foreach (var pair in allocation)
        len += pair.Value;

      float shot = RandomHelper.GetRandomFloat(0f, len); //��������, ������� � ����� ������� ������

      float curPos = 0;
      foreach (var pair in allocation)
      {
        if (curPos + pair.Value > shot)
          return pair.Key;
        curPos += pair.Value;
      }

      return BlockFactory.BlockType.White;
    }

    // ���������� ������, ��� ��������� ��-�� � luck = 1
    public static BlockFactory.BlockType GenerateNewBlockType(BlockFactory.BlockType type, float luck)
    {
      var allocation = new Dictionary<BlockFactory.BlockType, float>();
      var enumerator = EnumHelper.GetEnumerator(typeof(BlockFactory.BlockType));

      while (enumerator.MoveNext())
      {
        var curType = (BlockFactory.BlockType)enumerator.Current;
        if (curType == type)
          allocation.Add(curType, luck);
        else
          allocation.Add(curType, 1);
      }

      return GenerateNewBlockType(allocation);
    }

    /// True - ���� ����� ���� � ������������ chance �� �������
    public static bool CheckChance(float chance)
    {
      if (chance >= 1)
        return true;
      if (chance <= 0)
        return false;
      int i = RandomHelper.GetRandomInt((int) (1.0f/chance));
      if (i == 1) // ���� �����, ��� ��� ����� �� ������� 1��
        return true;
      return false;
    }
  }
}
