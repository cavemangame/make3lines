using System;
using Microsoft.Xna.Framework;
using XnaTetris.Blocks;
using XnaTetris.Game;

namespace XnaTetris.Algorithms
{
  class BlocksGridHelper
  {
    #region Constants

    #endregion

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
    /// <returns>amount of blocks (without itself)</returns>
    private int BlocksFormingLinesCount(int x, int y)
    {
      Block block = blocksGrid.Grid[x, y];
      int rightmostX = x;
      int bottommestY = y;
      int sameTypeBlocksCount;
      int result = 0;

      while (rightmostX + 1 < BlocksGrid.GRID_WIDTH &&
             block.Type.Equals(blocksGrid.Grid[rightmostX + 1, y].Type))
      {
        ++ rightmostX;
      }
      sameTypeBlocksCount = SameTypeLeftBlocksCount(rightmostX, y);
      if (sameTypeBlocksCount >= 2)
      {
        result += sameTypeBlocksCount;
      }

      while (bottommestY + 1 < BlocksGrid.GRID_HEIGHT &&
             block.Type.Equals(blocksGrid.Grid[x, bottommestY + 1].Type))
      {
        ++ bottommestY;
      }
      sameTypeBlocksCount = SameTypeUpBlocksCount(x, bottommestY);
      if (sameTypeBlocksCount >= 2)
      {
        result += sameTypeBlocksCount;
      }

      return result;
    }

    /// <summary>
    /// Counts how many blocks will create lines after swap
    /// </summary>
    /// <param name="x1">first block's x coordinate</param>
    /// <param name="y1">first block's y coordinate</param>
    /// <param name="x2">second block's x coordinate</param>
    /// <param name="y2">second block's y coordinate</param>
    /// <returns>blocks amount</returns>
    private int BlocksFormingLinesAfterSwap(int x1, int y1, int x2, int y2)
    {
      if (blocksGrid.Grid[x1, y1].Type.Equals(blocksGrid.Grid[x2, y2].Type))
      {
        return 0;
      }

      SwapBlocks(x1, y1, x2, y2);

      int blocksCount;
      int totalBlocks = 0;

      blocksCount = BlocksFormingLinesCount(x1, y1);
      if (blocksCount != 0)
      {
        totalBlocks += blocksCount + 1;
      }
      blocksCount = BlocksFormingLinesCount(x2, y2);
      if (blocksCount != 0)
      {
        totalBlocks += blocksCount + 1;
      }

      SwapBlocks(x1, y1, x2, y2);

      return totalBlocks;
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

          int blockHorizCount = 0;
          int blockVertCount = 0;

          int horizMultiplier = 1;
          int vertMultiplier = 1;

          bool isHorizontalLine = false;
          bool isVerticalLine = false;

          // can this block be rightmost?
          if (x == BlocksGrid.GRID_WIDTH - 1 || !block.Type.Equals(blocksGrid.Grid[x + 1, y].Type))
          {
            int sameTypeLeftBlocksCount = SameTypeLeftBlocksCount(x, y);

            if (sameTypeLeftBlocksCount >= 2)
            {
              result = true;
              blockHorizCount += sameTypeLeftBlocksCount + 1;
              for (int i = 0; i <= sameTypeLeftBlocksCount; ++ i)
              {
                blocksGrid.Grid[x - i, y].IsDestroyed = true;
                horizMultiplier *= blocksGrid.Grid[x - i, y].Multiplier;
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
              blockVertCount += sameTypeUpBlocksCount + 1;

              for (int i = 0; i <= sameTypeUpBlocksCount; ++ i)
              {
                blocksGrid.Grid[x, y - i].IsDestroyed = true;
                vertMultiplier *= blocksGrid.Grid[x, y - i].Multiplier;
              }
              isVerticalLine = true;
            }
          }

          if (isHorizontalLine)
          {
            AddScore(x, y, blockHorizCount, horizMultiplier);
            GeneratePopupScore(x, y, blockHorizCount, horizMultiplier, true);
          }
          if (isVerticalLine)
          {
            AddScore(x, y, blockVertCount, vertMultiplier);
            GeneratePopupScore(x, y, blockVertCount, vertMultiplier, false);
          }
        }
      }
      return result;
    }

    private void AddScore(int x, int y, int blockCount, int multiplier)
    {
      int score = blocksGrid.Grid[x, y].GetScore(blockCount) * multiplier;
      blocksGrid.LinesGame.Score.OverallScore += score;
      blocksGrid.LinesGame.LevelScore += score;
      blocksGrid.LinesGame.GameField.LevelScore.OverallScore += score;
      blocksGrid.Grid[x, y].AddScore(blockCount);
    }

    /// <summary>
    /// Генерит всплывающие над убитой линией очки
    /// </summary>
    /// <param name="x">X - крайнего блока</param>
    /// <param name="y">Y - крайнего блока</param>
    /// <param name="blocksForScoreCount">число прибитых блоков в линии</param>
    /// <param name="multiplier">Мультипликатор</param>
    /// <param name="isHorizontalLine"></param>
    private void GeneratePopupScore(int x, int y, int blocksForScoreCount, int multiplier, bool isHorizontalLine)
    {
      // find popup text position == center of center of destroyed blocks line
      int xx, yy;
      if (isHorizontalLine)
      {
        xx = (GetRectangle(x, y).Right - GetRectangle(x - blocksForScoreCount + 1, y).Left) / 2
          + GetRectangle(x - blocksForScoreCount + 1, y).Left;
        yy = GetRectangle(x, y).Center.Y;
      }
      else
      {
        xx = GetRectangle(x, y).Center.X;
        yy = (GetRectangle(x, y).Bottom - GetRectangle(x, y - blocksForScoreCount + 1).Top) / 2
          + GetRectangle(x, y - blocksForScoreCount + 1).Top;
      }

      string text = blocksGrid.Grid[x, y].GetScore(blocksForScoreCount).ToString();
      if (multiplier > 1) 
        text += String.Format("x{0}", multiplier);
      blocksGrid.AddDestroyPopupText(new Vector2(xx, yy),
                                     text,
                                     blocksGrid.Grid[x, y].ScoreColor);
    }

    public bool FindBestMovement(out int x1, out int y1, out int x2, out int y2)
    {
      int bestBlocksCount = 0;

      x1 = x2 = y1 = y2 = -1;
      for (int y = 0; y < BlocksGrid.GRID_HEIGHT; ++ y)
      {
        for (int x = 0; x < BlocksGrid.GRID_WIDTH; ++ x)
        {
          int swapX1, swapY1, swapX2, swapY2;
          int totalBlocks;

          // swap with left block
          swapX1 = x - 1;
          swapY1 = y;
          swapX2 = x;
          swapY2 = y;
          if (swapX1 >= 0)
          {
            totalBlocks = BlocksFormingLinesAfterSwap(swapX1, swapY1, swapX2, swapY2);
            if (totalBlocks > bestBlocksCount)
            {
              bestBlocksCount = totalBlocks;
              x1 = swapX1;
              y1 = swapY1;
              x2 = swapX2;
              y2 = swapY2;
            }
          }

          // swap with top block
          swapX1 = x;
          swapY1 = y - 1;
          swapX2 = x;
          swapY2 = y;
          if (swapY1 >= 0)
          {
            totalBlocks = BlocksFormingLinesAfterSwap(swapX1, swapY1, swapX2, swapY2);
            if (totalBlocks > bestBlocksCount)
            {
              bestBlocksCount = totalBlocks;
              x1 = swapX1;
              y1 = swapY1;
              x2 = swapX2;
              y2 = swapY2;
            }
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
            blocksGrid.Grid[x, y].MakeMove(GetRectangle(x, bottommestDestroyedY), x, bottommestDestroyedY);
            -- bottommestDestroyedY;
          }
        }

        for (int y = destroyedBlocksCount - 1; y >= 0; -- y)
        {
          // TODO: add luck?
          Block block = blocksGrid.GetNewRandomBlock(x, y - destroyedBlocksCount);

          blocksGrid.Grid[x, y] = block;
          block.MakeMove(GetRectangle(x, y), x, y);
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
      return new Rectangle(blocksGrid.GridRectangle.X + 8 + x * (blocksGrid.BlockWidth-2),
                           blocksGrid.GridRectangle.Y + 8 + y * (blocksGrid.BlockHeight-2),
                           blocksGrid.BlockWidth-2,
                           blocksGrid.BlockHeight-2);
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

    public void SwapBlocks(int x1, int y1, int x2, int y2)
    {
      Block temp = blocksGrid.Grid[x1, y1];

      blocksGrid.Grid[x1, y1] = blocksGrid.Grid[x2, y2];
      blocksGrid.Grid[x2, y2] = temp;
    }
  }
}
