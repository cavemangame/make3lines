using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
	class BlockRed : Block
	{
		public BlockRed(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, SpriteHelper setHiBlock, int x, int y, int multiplier)
			: base(setGame, setBlockRect, setBlock, setHiBlock, x, y, multiplier)
		{
		}

		public override BlockFactory.BlockType Type
		{
			get { return BlockFactory.BlockType.Red; }
		}

    public override Color ScoreColor
    {
      get { return Color.DarkRed; }
    }

		public override int GetScore(int N)
		{
			switch (N)
			{
				case 3:
					return 60;
				case 4:
					return 120;
				case 5:
					return 170;
				default:
					return 0;
			}
		}

    public override void AddScore(int N)
    {
      LinesGame.Score.RedScore += N;
      LinesGame.GameField.LevelScore.RedScore += N;
    }
	}
}
