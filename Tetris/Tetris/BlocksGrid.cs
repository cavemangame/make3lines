using System;
using Microsoft.Xna.Framework;
using XnaTetris.Blocks;
using XnaTetris.Game;
using XnaTetris.Helpers;
using XnaTetris.Algorithms;

namespace XnaTetris
{
  class BlocksGrid : DrawableGameComponent
  {
    #region Constants
    public const int GRID_WIDTH = 8;
    public const int GRID_HEIGHT = 8;
    #endregion

    #region Variables
    private readonly BlocksGridHelper blocksGridHelper;
    private int lastSwapX1, lastSwapY1, lastSwapX2, lastSwapY2;

    /// <summary>
    /// true if current movement is swap (user action)
    /// </summary>
    private bool isSwap;

    /// <summary>
    /// true if current movement is undo after unsuccessful movement
    /// </summary>
    private bool isUndo;
    #endregion

    #region Properties
    public Block[,] Grid { get; private set; }
    public Rectangle GridRectangle { get; private set; }
    public int BlockWidth { get; private set; }
    public int BlockHeight { get; private set; }
    public LinesGame LinesGame { get { return Game as LinesGame; }}

    /// <summary>
    /// how much blocks are moving now
    /// </summary>
    public int ActiveBlocks { get; private set; }

    #endregion

    #region Constructor

    public BlocksGrid(Microsoft.Xna.Framework.Game game, Rectangle GridRectangle)
      : base(game)
    {
      this.GridRectangle = GridRectangle;
      BlockWidth = this.GridRectangle.Width / GRID_WIDTH;
      BlockHeight = this.GridRectangle.Height / GRID_HEIGHT;
      Grid = new Block[GRID_WIDTH, GRID_HEIGHT];
      blocksGridHelper = new BlocksGridHelper(this);
    }
    #endregion

    #region Initialize
    public override void Initialize()
    {
      Restart();

      base.Initialize();
    }
    #endregion

    #region Restart
    public void Restart()
    {
      for (int x = 0; x < GRID_WIDTH; x++)
      {
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
          // repeat until the board is in stable state
          while (true)
          {
            Block curBlock = GetNewRandomBlock(x, y);

            Grid[x, y] = curBlock;

            // TODO: use blocksGridHelper
            if ((x < 2 || !curBlock.Type.Equals(Grid[x - 1, y].Type) || !curBlock.Type.Equals(Grid[x - 2, y].Type)) &&
                (y < 2 || !curBlock.Type.Equals(Grid[x, y - 1].Type) || !curBlock.Type.Equals(Grid[x, y - 2].Type)))
            {
              break;
            }
          }
        }
      }
    }
    #endregion

    #region Update
    public override void Update(GameTime gameTime)
    {
      if (LinesGame.GameState == Serv.GameState.GameStateRunning && Input.MouseLeftButtonJustPressed)
      {
        UpdateClickedBlock(Input.MousePos, gameTime);
      }
      foreach (Block block in Grid)
      {
        block.Update(gameTime);
      }
    }

    private void UpdateClickedBlock(Point point, GameTime gameTime)
    {
      // don't allow to do anything until the board is in stable state
      if (ActiveBlocks != 0)
      {
        return;
      }

      foreach (Block block in Grid)
      {
        if (block.PointInBlock(Serv.CorrectPositionWithGameScale(point)))
        {
          int count = blocksGridHelper.ClickedBlocksCount();

          if (count == 0)
          {
            block.ClickToBlock(gameTime);
          }
          else if (count == 1)
          {
            int xN, yN;

            if (blocksGridHelper.NeighbourClickedBlock(block.X, block.Y, out xN, out yN))
            {
              isSwap = true;
              block.ClickToBlock(gameTime);
              lastSwapX1 = block.X;
              lastSwapX2 = xN;
              lastSwapY1 = block.Y;
              lastSwapY2 = yN;
              SwapBlocks(lastSwapX1, lastSwapY1, lastSwapX2, lastSwapY2, gameTime);
            }
            else
            {
              CleanClickedStates();
            }
          }
          else
          {
            throw new Exception("ClickedBlocksCount() should not return value greater then 1");
          }

          break;
        }
      }
    }

    #endregion

    #region render
    public override void Draw(GameTime gameTime)
    {
      for (int x = 0; x < GRID_WIDTH; x++)
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
          Grid[x, y].Draw(gameTime);
        }
    }
    #endregion

    #region functions
    private void SwapBlocks(int x1, int y1, int x2, int y2, GameTime gameTime)
    {
      Grid[x1, y1].MakeMove(gameTime, blocksGridHelper.GetRectangle(x2, y2), x2, y2);
      Grid[x2, y2].MakeMove(gameTime, blocksGridHelper.GetRectangle(x1, y1), x1, y1);

      Block temp = Grid[x1, y1];

      Grid[x1, y1] = Grid[x2, y2];
      Grid[x2, y2] = temp;

      Grid[x1, y1].IsClicked = false;
      Grid[x2, y2].IsClicked = false;
    }

    private void CleanClickedStates()
    {
      for (int x = 0; x < GRID_WIDTH; x++)
      {
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
          Grid[x, y].IsClicked = false;
        }
      }
    }

    public Block GetNewRandomBlock(int x, int y)
    {
      Block block = (BlockFactory.GetBlockFactory(LinesGame).GetNewBlock(
        BlockFactory.GetRandomBlockType(), blocksGridHelper.GetRectangle(x, y), x, y));

      block.StartMove += BlocksGrid_StartMove;
      block.EndMove += BlocksGrid_EndMove;

      return block;
    }

    public Block GetNewRandomBlock(int x, int y, BlockFactory.BlockType oldType, int luck)
    {
      Block block = (BlockFactory.GetBlockFactory(LinesGame).GetNewBlock(
        RandomBlockHelper.GenerateNewBlockType(oldType, luck),
        blocksGridHelper.GetRectangle(x, y), x, y));
      
      block.StartMove += BlocksGrid_StartMove;
      block.EndMove += BlocksGrid_EndMove;

      return block;
    }

    void BlocksGrid_StartMove(object sender, EventArgs e)
    {
      if (!(sender is Block))
      {
        return;
      }

      ActiveBlocks += 1;
    }

    void BlocksGrid_EndMove(object sender, EventArgs e)
    {
      if (!(sender is Block))
      {
        return;
      }

      ActiveBlocks -= 1;
      if (ActiveBlocks != 0)
      {
        return;
      }

      if (isUndo)
      {
        isUndo = false;
        return;
      }

      Block block = sender as Block;
      GameTime gameTime = block.blockGameTime;
      bool successfulMovement = blocksGridHelper.FindAndDestroyLines(gameTime);

      if (isSwap && !successfulMovement)
      {
        // undo the movement
        isUndo = true;
        LinesGame.Timer -= 5000;
        SwapBlocks(lastSwapX1, lastSwapY1, lastSwapX2, lastSwapY2, gameTime);
      }
      isSwap = false;
    }

    public void EnableComponents(bool isEnable)
    {
      Enabled = isEnable;
      for (int x = 0; x < GRID_WIDTH; x++)
        for (int y = 0; y < GRID_HEIGHT; y++)
          Grid[x, y].Enabled = isEnable;
    }
    #endregion
  }
}
