using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
	class BlockGreen : Block
	{
		public BlockGreen(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, SpriteHelper setHiBlock, int x, int y, int multiplier)
			: base(setGame, setBlockRect, setBlock, setHiBlock, x, y, multiplier)
		{
		}

		public override BlockFactory.BlockType Type
		{
			get { return BlockFactory.BlockType.Green; }
		}

    public override Color ScoreColor
    {
      get { return Color.DarkGreen; }
    }

		public override int GetScore(int N)
		{
			switch (N)
			{
				case 3:
					return 30;
				case 4:
					return 100;
				case 5:
					return 230;
				default:
					return 0;
			}
		}

    public override void AddScore(int N)
    {
      LinesGame.Score.GreenScore += N;
      LinesGame.GameField.LevelScore.GreenScore += N;
    }
	}
}
