using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
  /// <summary>
  /// Неуязвимый блок. Нельзя двигать. Не сочетается ни с одним цветом.
  /// </summary>
  class BlockInvulnerable : Block
  {
    public BlockInvulnerable(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, SpriteHelper setHiBlock, int x, int y, int multiplier)
			: base(setGame, setBlockRect, setBlock, setHiBlock, x, y, multiplier)
		{
		}

		public override BlockFactory.BlockType Type
		{
			get { return BlockFactory.BlockType.Invul; }
		}

    public override Color ScoreColor
    {
      get { return Color.White; }
    }

    public override int GetScore(int N)
    {
      return 0;
    }

    public override void AddScore(int N)
    {
    }
  }
}
