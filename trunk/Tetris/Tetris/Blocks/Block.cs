using System;
using Microsoft.Xna.Framework;
using XnaTetris.Game;
using XnaTetris.Helpers;
using Microsoft.Xna.Framework.Graphics;

namespace XnaTetris.Blocks
{
  abstract class Block : DrawableGameComponent
  {
    #region Переменные

    private readonly SpriteHelper block;
    private BlockAnimator blockAnimator;

    private bool isMoving;

    private float depth;

    public event EventHandler EndMove;
    public GameTime blockGameTime;

    #endregion

    #region Конструктор

    public Block(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, int x, int y)
      : base(setGame)
    {
      BlockRectangle = setBlockRect;
      block = setBlock;
      X = x;
      Y = y;
    }

    #endregion

    #region Свойства
    public abstract BlockFactory.BlockType Type { get; }

    public bool IsClicked { get; set; }
    public Rectangle BlockRectangle { get; set; }
    public bool IsDestroyed { get; set; }
    public Serv.MoveDirection CurrentDir { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    #endregion

    public override void Draw(GameTime gameTime)
    {
      Color color = IsClicked ? Color.LightGray : Color.White;

      block.Render(BlockRectangle, color);

      base.Draw(gameTime);
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      blockGameTime = gameTime;

      if (isMoving)
      {
        if (blockAnimator.IsMoveEnded(gameTime))
        {
          isMoving = false;
          EndMove(this, EventArgs.Empty);
        }
        else
        {
          BlockRectangle = blockAnimator.CurrentRectangle(gameTime);
        }
      }
    }

    #region virtual methods

    public virtual void SetBlockSprite()
    {

    }

    public virtual int GetScore(int N)
    {
      return (N - 3) * 5 + N * 5;
    }

    public virtual bool PointInBlock(Point point)
    {
      bool result;
      BlockRectangle.Contains(ref point, out result);
      return result;
    }

    public virtual void ClickToBlock(GameTime gameTime)
    {
      IsClicked = !IsClicked;
    }

    public virtual void MakeMove(GameTime gameTime, Rectangle newRect, float setDepth)
    {
      isMoving = true;
      blockAnimator = new BlockAnimator(BlockRectangle, newRect, gameTime);
      depth = setDepth;
    }

    public virtual void MakeMove(GameTime gameTime, Rectangle newRect, int setx, int sety, float setDepth)
    {
      X = setx;
      Y = sety;
      MakeMove(gameTime, newRect, setDepth);
    }

    #endregion
  }
}
