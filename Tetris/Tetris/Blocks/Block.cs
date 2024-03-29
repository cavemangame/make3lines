using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;
using XnaTetris.Particles;

namespace XnaTetris.Blocks
{
  public abstract class Block : DrawableGameComponent
  {
    #region ���������

    private const int BLINK_TIME = 250;

    #endregion

    #region ����������

    private readonly SpriteHelper block;
    private readonly SpriteHelper hiblock;
    private SpriteBatch spriteBatch;
    private BlockAnimator blockAnimator;

    private bool isMoving;
    private bool isHelped;

    public event EventHandler StartMove;
    public event EventHandler EndMove;
    public GameTime blockGameTime;

    public long lastBlinkTime;
    private bool isHelpShow;

    #endregion

    #region Properties

    public int Multiplier { get; private set; }

    #endregion

    #region �����������

    protected Block(Microsoft.Xna.Framework.Game setGame, Rectangle setBlockRect, SpriteHelper setBlock, 
      SpriteHelper setHiBlock, int x, int y, int multiplier)
      : base(setGame)
    {
      BlockRectangle = setBlockRect;
      block = setBlock;
      hiblock = setHiBlock;
      X = x;
      Y = y;
      Multiplier = multiplier;
      spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

    }

    protected Block(Microsoft.Xna.Framework.Game setGame, Rectangle setBlockRect, SpriteHelper setBlock, 
       int x, int y, int multiplier)
      : this(setGame, setBlockRect, setBlock, setBlock, x, y, multiplier)
    {
    }

    protected Block(Microsoft.Xna.Framework.Game setGame, Rectangle setBlockRect, SpriteHelper setBlock,
       int x, int y)
      : this(setGame, setBlockRect, setBlock, x, y, 1)
    {
    }

    #endregion

    #region ��������
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

    public override void Initialize()
    {
      base.Initialize();
    }

    #region Draw

    public override void Draw(GameTime gameTime)
    {
      if (PointInBlock(Input.MousePos) && LinesGame.IsBoardInStableState())
      {
        hiblock.Render(spriteBatch, BlockRectangle);
      }
      else
      {
        RenderVisiblePart();
      }

      if (IsClicked)
      {
        ContentSpace.GetSprite("SelectionBlock").Render(spriteBatch, BlockRectangle);
      }

      if (Multiplier > 1 && !IsDestroyed)
      {
        TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"), 
          String.Format("x{0}", Multiplier), BlockRectangle.X + 15, BlockRectangle.Y + 10, ScoreColor, 1.25f);
      }

      if (IsHelped)
      {
        if ((long)LinesGame.ElapsedGameMs - lastBlinkTime >= BLINK_TIME)
        {
          isHelpShow = !isHelpShow;
          lastBlinkTime = (long) LinesGame.ElapsedGameMs;
        }
        if (isHelpShow)
          ContentSpace.GetSprite("HelpBlock").Render(spriteBatch, BlockRectangle);
      }

      base.Draw(gameTime);
    }

    #endregion

    #region Update

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

    #endregion

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
        block.Render(spriteBatch, BlockRectangle);
        return;
      }

      float c = (float)block.sourceRect.Height / BlockRectangle.Height;
      Rectangle gfxRect = new Rectangle(block.sourceRect.X, (int)((block.sourceRect.Y + minY - BlockRectangle.Y) * c),
                 block.sourceRect.Width, (int)((BlockRectangle.Height - minY + BlockRectangle.Y) * c));
      Rectangle visibleRect = new Rectangle(BlockRectangle.X, minY,
                                      BlockRectangle.Width, BlockRectangle.Height - minY + BlockRectangle.Y);

      block.Render(spriteBatch, visibleRect, gfxRect);
    }

    public abstract int GetScore(int N);

    public abstract void AddScore(int N);

  }
}
