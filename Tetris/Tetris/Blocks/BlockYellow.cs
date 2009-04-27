using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
	class BlockYellow : Block
	{
		public BlockYellow(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, SpriteHelper setHiBlock, int x, int y)
			: base(setGame, setBlockRect, setBlock, setHiBlock, x, y)
		{
		}

		public override BlockFactory.BlockType Type
		{
			get { return BlockFactory.BlockType.Yellow; }
		}

    public override Color ScoreColor
    {
      get { return Color.Orange; }
    }

		public override int GetScore(int N)
		{
			switch (N)
			{
				case 3:
					return 40;
				case 4:
					return 140;
				case 5:
					return 180;
				default:
					return 0;
			}
		}

    public override void AddScore(int N)
    {
      LinesGame.Score.YellowScore += N;
    }
	}
}
