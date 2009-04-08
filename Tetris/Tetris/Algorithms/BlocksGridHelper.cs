using Microsoft.Xna.Framework;
using XnaTetris.Blocks;
using XnaTetris.Game;

namespace XnaTetris.Algorithms
{
  class BlocksGridHelper
  {
    #region Variables
    private readonly BlocksGrid blocksGrid;
    #endregion

    #region Constructor
    public BlocksGridHelper(BlocksGrid blocksGrid)
    {
      this.blocksGrid = blocksGrid;
    }
    #endregion

    #region Finder functions
    public int SameTypeLeftBlocksCount(int x, int y)
    {
      Block block = blocksGrid.Grid[x, y];
      int xIndex = x - 1;
      int result = 0;

      while (xIndex >= 0 && block.Type.Equals(blocksGrid.Grid[xIndex, y].Type))
      {
        ++ result;
        -- xIndex;
      }
      return result;
    }

    public int SameTypeUpBlocksCount(int x, int y)
    {
      Block block = blocksGrid.Grid[x, y];
      int yIndex = y - 1;
      int result = 0;

      while (yIndex >= 0 && block.Type.Equals(blocksGrid.Grid[x, yIndex].Type))
      {
        ++ result;
        -- yIndex;
      }
      return result;
    }

    /// <summary>
    /// For given block finds near block of the same type which make lines
    /// </summary>
    /// <param name="x">block's x coordinate</param>
    /// <param name="y">block's y coordinate</param>
    /// <param name="horizontalBlocksCount">amount of blocks on the left and on the right sides (without itself)</param>
    /// <param name="verticalBlocksCount">amount of blocks on the top and on the bottom sides (without itself)</param>
    private void BlocksFormingLinesCount(int x, int y,
                                         out int horizontalBlocksCount, out int verticalBlocksCount)
    {
      Block block = blocksGrid.Grid[x, y];
      int rightmostX = x;
      int bottommestY = y;
      int sameTypeBlocksCount;

      horizontalBlocksCount = 0;
      verticalBlocksCount = 0;

      while (rightmostX + 1 < BlocksGrid.GRID_WIDTH &&
             block.Type.Equals(blocksGrid.Grid[rightmostX + 1, y].Type))
      {
        ++ rightmostX;
      }
      sameTypeBlocksCount = SameTypeLeftBlocksCount(rightmostX, y);
      if (sameTypeBlocksCount >= 2)
      {
        horizontalBlocksCount = sameTypeBlocksCount;
      }

      while (bottommestY + 1 < BlocksGrid.GRID_HEIGHT &&
             block.Type.Equals(blocksGrid.Grid[x, bottommestY + 1].Type))
      {
        ++ bottommestY;
      }
      sameTypeBlocksCount = SameTypeUpBlocksCount(x, bottommestY);
      if (sameTypeBlocksCount >= 2)
      {
        verticalBlocksCount += sameTypeBlocksCount;
      }
    }

    /// <summary>
    /// Tries to find any clicked block near current block
    /// </summary>
    /// <param name="x">current block x</param>
    /// <param name="y">current block y</param>
    /// <param name="xN">result block x</param>
    /// <param name="yN">result block y</param>
    /// <returns>true if there is such block</returns>
    public bool NeighbourClickedBlock(int x, int y, out int xN, out int yN)
    {
      if (x > 0 && blocksGrid.Grid[x - 1, y].IsClicked)
      {
        xN = x - 1;
        yN = y;
        return true;
      }
      if (x < BlocksGrid.GRID_WIDTH - 1 && blocksGrid.Grid[x + 1, y].IsClicked)
      {
        xN = x + 1;
        yN = y;
        return true;
      }
      if (y > 0 && blocksGrid.Grid[x, y - 1].IsClicked)
      {
        xN = x;
        yN = y - 1;
        return true;
      }
      if (y < BlocksGrid.GRID_HEIGHT - 1 && blocksGrid.Grid[x, y + 1].IsClicked)
      {
        xN = x;
        yN = y + 1;
        return true;
      }
      xN = yN = -1;
      return false;
    }

    /// <summary>
    /// Finds lines and mark lines' blocks as Destroyed
    /// </summary>
    /// <returns>true if any line was found</returns>
    public bool FindLines()
    {
      bool result = false;

      // for each block try to find line where this block is rightmost or bottommost
      for (int y = 0; y < BlocksGrid.GRID_HEIGHT; ++y)
      {
        for (int x = 0; x < BlocksGrid.GRID_WIDTH; ++x)
        {
          Block block = blocksGrid.Grid[x, y];
          int blocksForScoreCount = 0;
          bool isHorizontalLine = false;

          // can this block be rightmost?
          if (x == BlocksGrid.GRID_WIDTH - 1 || !block.Type.Equals(blocksGrid.Grid[x + 1, y].Type))
          {
            int sameTypeLeftBlocksCount = SameTypeLeftBlocksCount(x, y);

            if (sameTypeLeftBlocksCount >= 2)
            {
              result = true;
              blocksForScoreCount += sameTypeLeftBlocksCount + 1;
              for (int i = 0; i <= sameTypeLeftBlocksCount; ++ i)
              {
                blocksGrid.Grid[x - i, y].IsDestroyed = true;
              }
              isHorizontalLine = true;
            }
          }

          // can this block be bottommost?
          if (y == BlocksGrid.GRID_HEIGHT - 1 || !block.Type.Equals(blocksGrid.Grid[x, y + 1].Type))
          {
            int sameTypeUpBlocksCount = SameTypeUpBlocksCount(x, y);

            if (sameTypeUpBlocksCount >= 2)
            {
              result = true;
              blocksForScoreCount += sameTypeUpBlocksCount + 1;
              for (int i = 0; i <= sameTypeUpBlocksCount; ++ i)
              {
                blocksGrid.Grid[x, y - i].IsDestroyed = true;
              }
            }
          }

          blocksGrid.LinesGame.Score += blocksGrid.Grid[x, y].GetScore(blocksForScoreCount);
          if (blocksForScoreCount > 0)
          {
            int xx, yy;
            if (isHorizontalLine)
            {
              xx = (GetRectangle(x, y).Right - GetRectangle(x - blocksForScoreCount + 1, y).Left)/2
                + GetRectangle(x - blocksForScoreCount + 1, y).Left;
              yy = GetRectangle(x, y).Center.Y;
            }
            else
            {
              xx = GetRectangle(x, y).Center.X;
              yy = (GetRectangle(x, y).Bottom - GetRectangle(x, y - blocksForScoreCount + 1).Top) / 2
                + GetRectangle(x, y - blocksForScoreCount + 1).Top;
            }
            Point p = Serv.InvertCorrectPositionWithGameScale(new Point(xx, yy));
            blocksGrid.AddDestroyPopupText((int)blocksGrid.LinesGame.ElapsedGameMs,
                                           new Vector2(p.X, p.Y),
                                           blocksGrid.Grid[x, y].GetScore(blocksForScoreCount).ToString());
          }
        }
      }
      return result;
    }

    public bool FindBestMovement(out int x1, out int y1, out int x2, out int y2)
    {
      int bestBlocksCount = 0;

      x1 = x2 = y1 = y2 = -1;
      for (int y = 0; y < BlocksGrid.GRID_HEIGHT; ++ y)
      {
        for (int x = 0; x < BlocksGrid.GRID_WIDTH; ++ x)
        {
          // swap with left block
          if (x != 0)
          {
            SwapBlocks(x - 1, y, x, y);

            bool isSameType = blocksGrid.Grid[x - 1, y].Type.Equals(blocksGrid.Grid[x, y].Type);
            int horizontalBlocksCount, verticalBlocksCount;
            int totalBlocks = 0;
            
            BlocksFormingLinesCount(x - 1, y, out horizontalBlocksCount, out verticalBlocksCount);
            if (horizontalBlocksCount != 0 || verticalBlocksCount != 0)
            {
              totalBlocks = horizontalBlocksCount + verticalBlocksCount + 1;
            }
            BlocksFormingLinesCount(x, y, out horizontalBlocksCount, out verticalBlocksCount);
            if (!isSameType)
            {
              totalBlocks += horizontalBlocksCount;
            }
            totalBlocks += verticalBlocksCount;
            if (!isSameType && verticalBlocksCount != 0)
            {
              totalBlocks += 1;
            }

            if (totalBlocks > bestBlocksCount)
            {
              bestBlocksCount = totalBlocks;
              x1 = x - 1;
              y1 = y;
              x2 = x;
              y2 = y;
            }

            SwapBlocks(x - 1, y, x, y);
          }

          // swap with top block
          if (y != 0)
          {
            SwapBlocks(x, y - 1, x, y);

            bool isSameType = blocksGrid.Grid[x, y - 1].Type.Equals(blocksGrid.Grid[x, y].Type);
            int horizontalBlocksCount, verticalBlocksCount;
            int totalBlocks = 0;
            
            BlocksFormingLinesCount(x, y - 1, out horizontalBlocksCount, out verticalBlocksCount);
            if (horizontalBlocksCount != 0 || verticalBlocksCount != 0)
            {
              totalBlocks = horizontalBlocksCount + verticalBlocksCount + 1;
            }
            BlocksFormingLinesCount(x, y, out horizontalBlocksCount, out verticalBlocksCount);
            if (!isSameType)
            {
              totalBlocks += verticalBlocksCount;
            }
            totalBlocks += horizontalBlocksCount;
            if (!isSameType && horizontalBlocksCount != 0)
            {
              totalBlocks += 1;
            }

            if (totalBlocks > bestBlocksCount)
            {
              bestBlocksCount = totalBlocks;
              x1 = x;
              y1 = y - 1;
              x2 = x;
              y2 = y;
            }

            SwapBlocks(x, y - 1, x, y);
          }
        }
      }

      return bestBlocksCount != 0;
    }
    #endregion

    #region Destroy Blocks
    /// <summary>
    /// Remove blocks marked as Destroyed with FindLines()
    /// </summary>
    /// <param name="gameTime"></param>
    public void RemoveLines(GameTime gameTime)
    {
      for (int x = 0; x < BlocksGrid.GRID_WIDTH; ++ x)
      {
        int bottommestDestroyedY = BlocksGrid.GRID_HEIGHT;

        while (-- bottommestDestroyedY >= 0 && !blocksGrid.Grid[x, bottommestDestroyedY].IsDestroyed) {}
        if (bottommestDestroyedY == -1)
        {
          continue;
        }

        int destroyedBlocksCount = 0;

        for (int y = bottommestDestroyedY; y >= 0; -- y)
        {
          Block block = blocksGrid.Grid[x, y];

          if (block.IsDestroyed)
          {
            block.X = -1;
            block.Y = -1;
            block.BlockRectangle = Serv.EmptyRect;
            ++ destroyedBlocksCount;
          }
          else
          {
            blocksGrid.Grid[x, bottommestDestroyedY] = blocksGrid.Grid[x, y];
            blocksGrid.Grid[x, y].MakeMove(gameTime, GetRectangle(x, bottommestDestroyedY), x, bottommestDestroyedY);
            -- bottommestDestroyedY;
          }
        }

        for (int y = destroyedBlocksCount - 1; y >= 0; -- y)
        {
          // TODO: add luck?
          Block block = blocksGrid.GetNewRandomBlock(x, y - destroyedBlocksCount);

          blocksGrid.Grid[x, y] = block;
          block.MakeMove(gameTime, GetRectangle(x, y), x, y);
        }

      }
    }
    #endregion

    /// <summary>
    /// Returns Rectangle instance for given block's coordinates
    /// </summary>
    /// <param name="x">block x coordinate</param>
    /// <param name="y">block y coordinate</param>
    /// <returns>Rectangle instance for given block's coordinates</returns>
    public Rectangle GetRectangle(int x, int y)
    {
      return new Rectangle(blocksGrid.GridRectangle.X + x * blocksGrid.BlockWidth,
                           blocksGrid.GridRectangle.Y + y * blocksGrid.BlockHeight,
                           blocksGrid.BlockWidth, blocksGrid.BlockHeight);
    }

    /// <summary>
    /// Calculates how many blocks are in clicked state on the board now
    /// </summary>
    /// <returns>amount of clicked blocks</returns>
    public int ClickedBlocksCount()
    {
      int count = 0;

      for (int x = 0; x < BlocksGrid.GRID_WIDTH; x++)
      {
        for (int y = 0; y < BlocksGrid.GRID_HEIGHT; y++)
        {
          if (blocksGrid.Grid[x, y].IsClicked)
            count++;
        }
      }
      return count;
    }

    private void SwapBlocks(int x1, int y1, int x2, int y2)
    {
      Block temp = blocksGrid.Grid[x1, y1];

      blocksGrid.Grid[x1, y1] = blocksGrid.Grid[x2, y2];
      blocksGrid.Grid[x2, y2] = temp;
    }
  }
}
