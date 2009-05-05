using Microsoft.Xna.Framework;
using XnaTetris.Game;
using XnaTetris.Helpers;

namespace XnaTetris.Interface
{
  public class TextBox : LinesControl
  {
    #region Variables

    private bool IsWriteState;
    private readonly SpriteHelper bgSprite;
    private int caretPosition = 2;

    #endregion

    #region Constructor

    public TextBox(Microsoft.Xna.Framework.Game game, Rectangle setRect, SpriteHelper bgSprite)
      : base(game, setRect)
    {
      this.bgSprite = bgSprite;
    }

    #endregion

    #region Draw
    public override void Draw(GameTime gameTime)
    {
      bgSprite.Render(BoundingRect);

      if (IsWriteState)
      {
        ContentSpace.GetSprite("Caret").Render(
          new Rectangle(BoundingRect.X + caretPosition, BoundingRect.Y+1, 6, BoundingRect.Height-2));
      }

      base.Draw(gameTime);
    }
    #endregion


    #region Events

    protected override void HandleMouseClick()
    {
      IsWriteState = true;
    }

    protected override void HandleOutMouseClick()
    {
      IsWriteState = false;
    }

    #endregion

  }
}
