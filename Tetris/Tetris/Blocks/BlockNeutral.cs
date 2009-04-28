using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
  /// <summary>
  /// нейтральный блок, подходит для сборки с любым цветом
  /// </summary>
  class BlockNeutral : Block
  {
    public BlockNeutral(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, SpriteHelper setHiBlock, int x, int y, int multiplier)
			: base(setGame, setBlockRect, setBlock, setHiBlock, x, y, multiplier)
		{
		}

		public override BlockFactory.BlockType Type
		{
			get { return BlockFactory.BlockType.Neutral; }
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
