using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
	class BlockWhite : Block
	{
		public BlockWhite(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, SpriteHelper setHiBlock, int x, int y, int multiplier)
			: base(setGame, setBlockRect, setBlock, setHiBlock, x, y, multiplier)
		{
		}

		public override BlockFactory.BlockType Type
		{
			get { return BlockFactory.BlockType.White; }
		}

	  public override Color ScoreColor
	  {
	    get { return Color.DarkGray;}
	  }

		public override int GetScore(int N)
		{
			switch (N)
			{
				case 3:
					return 50;
				case 4:
					return 100;
				case 5:
					return 200;
				default:
					return 0;
			}
		}

    public override void AddScore(int N)
    {
      LinesGame.Score.WhiteScore += N;
    }
	}
}
