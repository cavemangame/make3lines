using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Blocks;
using XnaTetris.Game;
using XnaTetris.Helpers;
using XnaTetris.Algorithms;
using XnaTetris.Interface;

namespace XnaTetris
{
  class BlocksGrid : DrawableGameComponent
  {
    #region Constants
    public const int GRID_WIDTH = 8;
    public const int GRID_HEIGHT = 8;
    public const int IDLE_TIME = 5000;
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

    /// <summary>
    /// List of PopupText that currently showing over grid
    /// </summary>
    private readonly List<PopupText> popupTexts = new List<PopupText>();

    /// <summary>
    /// Idle Time what user don't move right 
    /// </summary>
    public long StartIdleTime { get; set;}

    /// <summary>
    /// Check for showing help at this moment
    /// </summary>
    private bool isShowingHelp;

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
            Block curBlock = GetNewRandomBlock(x, y - GRID_HEIGHT);

            Grid[x, y] = curBlock;

            if (blocksGridHelper.SameTypeUpBlocksCount(x, y) < 2 &&
                blocksGridHelper.SameTypeLeftBlocksCount(x, y) < 2)
            {
              break;
            }
          }
        }
      }

      int dummy1, dummy2, dummy3, dummy4;

      if (!blocksGridHelper.FindBestMovement(out dummy1, out dummy2, out dummy3, out dummy4))
      {
        Restart();
      }

      for (int x = 0; x < GRID_WIDTH; x++)
      {
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
          Grid[x, y].MakeMove(blocksGridHelper.GetRectangle(x, y), x, y);
        }
      }

      StartIdleTime = (long)LinesGame.ElapsedGameMs;
    }
    #endregion

    #region Update
    public override void Update(GameTime gameTime)
    {
      if (Input.MouseLeftButtonJustPressed)
      {
        if (isShowingHelp)
        {
          isShowingHelp = false;
          StartIdleTime = (long)LinesGame.ElapsedGameMs;
          CleanHelpedStates();
        }
        UpdateClickedBlock(Input.MousePos, gameTime);
      }
      foreach (Block block in Grid)
      {
        block.Update(gameTime);
      }

     /* foreach (PopupText popupText in popupTexts)
      {
        popupText.Update(gameTime);
      }*/
    }

    private void UpdateClickedBlock(Point point, GameTime gameTime)
    {
      // don't allow to do anything until the board is in stable state
      if (!LinesGame.IsBoardInStableState())
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
              SwapBlocks(lastSwapX1, lastSwapY1, lastSwapX2, lastSwapY2);
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

      foreach (PopupText popupText in popupTexts)
      {
        popupText.Draw(gameTime);
      }

      if ((long)LinesGame.ElapsedGameMs - StartIdleTime >= IDLE_TIME)
        FindAndShowHelp();
    }

    #endregion

    #region functions
    private void SwapBlocks(int x1, int y1, int x2, int y2)
    {
      Grid[x1, y1].MakeMove(blocksGridHelper.GetRectangle(x2, y2), x2, y2);
      Grid[x2, y2].MakeMove(blocksGridHelper.GetRectangle(x1, y1), x1, y1);
      blocksGridHelper.SwapBlocks(x1, y1, x2, y2);
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

    private void CleanHelpedStates()
    {
      for (int x = 0; x < GRID_WIDTH; x++)
      {
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
          Grid[x, y].IsHelped = false;
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

    private void BlocksGrid_StartMove(object sender, EventArgs e)
    {
      if (!(sender is Block))
      {
        return;
      }

      ActiveBlocks += 1;
    }

    private void BlocksGrid_EndMove(object sender, EventArgs e)
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

      // the board is in a stable state here

      if (isUndo)
      {
        isUndo = false;
        return;
      }

      bool successfulMovement = blocksGridHelper.FindLines();

      if (successfulMovement)
      {
        LinesGame.IsRemoveProcess = true;
      }
      else if (isSwap)
      {
        // undo the movement
        isUndo = true;
        LinesGame.Timer -= LinesGame.PENALTY_FOR_WRONG_SWAP;
        SwapBlocks(lastSwapX1, lastSwapY1, lastSwapX2, lastSwapY2);
      }
      else
      {
        int dummy1, dummy2, dummy3, dummy4;

        if (!blocksGridHelper.FindBestMovement(out dummy1, out dummy2, out dummy3, out dummy4))
        {
          LinesGame.Timer -= LinesGame.PENALTY_FOR_RESTART;
          for (int x = 0; x < GRID_WIDTH; x++)
          {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
              Grid[x, y].IsDestroyed = true;
            }
          }
          LinesGame.IsRestartProcess = true;
        }
      }
      isSwap = false;
    }

    public void RemoveLines(GameTime gameTime)
    {
      blocksGridHelper.RemoveLines(gameTime);
    }

    public void ReadyToRestart()
    {
      Restart();
    }

    public void AddDestroyPopupText(Vector2 pos, string text, Color color)
    {
      Vector2 measure = LinesGame.NormalFont.MeasureString(text);
      Vector2 corrPos = new Vector2(pos.X-measure.X/2, pos.Y-measure.Y/2);
      PopupText popup = new PopupText(Game, (long)LinesGame.ElapsedGameMs, text, 3000, corrPos, new Vector2(corrPos.X, corrPos.Y - 30),
        LinesGame.NormalFont, 1.2f, 0.8f, color, 255, 0);
      popup.EndDrawing += popup_EndDrawing;
      Game.Components.Add(popup);
      popupTexts.Add(popup);
    }

    void popup_EndDrawing(object sender, EventArgs e)
    {
      if ((sender is PopupText) && popupTexts.Contains(sender as PopupText))
      {
        popupTexts.Remove(sender as PopupText);
        Game.Components.Remove(sender as PopupText);
      }
    }

    private void FindAndShowHelp()
    {
      int x1, y1, x2, y2;
      if (blocksGridHelper.FindBestMovement(out x1, out y1, out x2, out y2))
      {
        isShowingHelp = true;
        Grid[x1, y1].IsHelped = true;
        Grid[x2, y2].IsHelped = true;
      }
    }

    #endregion
  }
}
