using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
  public abstract class Block : DrawableGameComponent
  {
    #region Константы

    private const int BLINK_TIME = 250;

    #endregion

    #region Переменные

    private readonly SpriteHelper block;
    private readonly SpriteHelper hiblock;
    private BlockAnimator blockAnimator;

    private bool isMoving;
    private bool isHelped;

    public event EventHandler StartMove;
    public event EventHandler EndMove;
    public GameTime blockGameTime;

    public long lastBlinkTime;
    private bool isHelpShow;
    #endregion

    #region Конструктор

    protected Block(Microsoft.Xna.Framework.Game setGame, Rectangle setBlockRect, SpriteHelper setBlock, 
      SpriteHelper setHiBlock, int x, int y)
      : base(setGame)
    {
      BlockRectangle = setBlockRect;
      block = setBlock;
      hiblock = setHiBlock;
      X = x;
      Y = y;
    }

    protected Block(Microsoft.Xna.Framework.Game setGame, Rectangle setBlockRect, SpriteHelper setBlock, 
       int x, int y)
      : this(setGame, setBlockRect, setBlock, setBlock, x, y)
    {
    }

    #endregion

    #region Свойства
    public abstract BlockFactory.BlockType Type { get; }
    public LinesGame LinesGame { get { return Game as LinesGame; } }
    public bool IsClicked { get; set; }
    public bool IsHelped
    {
      get { return isHelped; }
      set
      {
        isHelped = value;
        if (value)
        {
          isHelpShow = true;
          lastBlinkTime = (long) LinesGame.ElapsedGameMs;
        }
      }
    }
    public Rectangle BlockRectangle { get; set; }
    public bool IsDestroyed { get; set; }
    public Serv.MoveDirection CurrentDir { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public abstract Color ScoreColor { get; }

    #endregion

    public override void Draw(GameTime gameTime)
    {
      if (PointInBlock(Serv.CorrectPositionWithGameScale(Input.MousePos)) &&
          LinesGame.IsBoardInStableState())
      {
        hiblock.Render(BlockRectangle);
      }
      else
      {
        RenderVisiblePart();
      }

      if (IsClicked)
      {
        ContentSpace.GetSprite("SelectionBlock").Render(BlockRectangle);
      }
      if (IsHelped)
      {
        if ((long)LinesGame.ElapsedGameMs - lastBlinkTime >= BLINK_TIME)
        {
          isHelpShow = !isHelpShow;
          lastBlinkTime = (long) LinesGame.ElapsedGameMs;
        }
        if (isHelpShow)
          ContentSpace.GetSprite("HelpBlock").Render(BlockRectangle);
      }

      base.Draw(gameTime);
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      blockGameTime = gameTime;

      if (isMoving)
      {
        BlockRectangle = blockAnimator.CurrentRectangle(gameTime);
        if (blockAnimator.IsMoveEnded(gameTime))
        {
          isMoving = false;
          EndMove(this, EventArgs.Empty);
        }
      }
    }

    public bool PointInBlock(Point point)
    {
      bool result;
      BlockRectangle.Contains(ref point, out result);
      return result;
    }

    public void ClickToBlock(GameTime gameTime)
    {
      IsClicked = !IsClicked;
    }

    public void MakeMove(Rectangle destRectangle, int destX, int destY)
    {
      int movePathLength = Math.Max(Math.Abs(X - destX), Math.Abs(Y - destY));

      X = destX;
      Y = destY;
      isMoving = true;
      blockAnimator = new BlockAnimator(BlockRectangle, destRectangle, LinesGame.ElapsedGameMs, movePathLength > 1);
      StartMove(this, EventArgs.Empty);
    }

    private void RenderVisiblePart()
    {
      const int minY = LinesGame.GRID_RECTANGLE_Y_COORDINATE;

      if (BlockRectangle.Y + BlockRectangle.Height <= minY)
      {
        return;
      }
      if (BlockRectangle.Y >= minY)
      {
        if ((LinesGame.IsRemoveProcess || LinesGame.IsRestartProcess) && IsDestroyed)
        {
          if (BlockRectangle.Width - 2 <= 0)
          {
            LinesGame.RemoveVisualizationWasFinished(blockGameTime);
            return;
          }
          BlockRectangle = new Rectangle(BlockRectangle.X + 1, BlockRectangle.Y + 1,
                                         BlockRectangle.Width - 2, BlockRectangle.Height - 2);
        }
        block.Render(BlockRectangle);
        return;
      }

      float c = (float)block.GfxRect.Height / BlockRectangle.Height;
      Rectangle gfxRect = new Rectangle(block.GfxRect.X, (int)((block.GfxRect.Y + minY - BlockRectangle.Y) * c),
                                        block.GfxRect.Width, (int)((BlockRectangle.Height - minY + BlockRectangle.Y) * c));
      Rectangle visibleRect = new Rectangle(BlockRectangle.X, minY,
                                      BlockRectangle.Width, BlockRectangle.Height - minY + BlockRectangle.Y);

      block.Render(visibleRect, Color.White, gfxRect);
    }

    public abstract int GetScore(int N);
  }
}
