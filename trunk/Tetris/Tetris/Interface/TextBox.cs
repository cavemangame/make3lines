using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    private string text = String.Empty;
    private float textScale = 1.0f;
    private Color textColor = Color.White;

    public event EventHandler EnterKeyPressed;

    #endregion

    #region Properties

    public string Text { get { return text; } }

    #endregion

    #region Constructor

    public TextBox(Microsoft.Xna.Framework.Game game, Rectangle setRect, SpriteHelper bgSprite)
      : base(game, setRect)
    {
      this.bgSprite = bgSprite;
    }

    public TextBox(Microsoft.Xna.Framework.Game game, Rectangle setRect, SpriteHelper bgSprite, Color color)
      : this(game, setRect, bgSprite)
    {
      textColor = color;
    }

    public TextBox(Microsoft.Xna.Framework.Game game, Rectangle setRect, SpriteHelper bgSprite, Color color, float scale)
      : this(game, setRect, bgSprite)
    {
      textScale = scale;
    }

    #endregion

    #region Draw
    public override void Draw(GameTime gameTime)
    {
      bgSprite.Render(BoundingRect);

      TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"), text, BoundingRect.X + 2,
                                  BoundingRect.Y + 1, textColor, textScale);

      if (IsWriteState)
      {
        ContentSpace.GetSprite("Caret").Render(
          new Rectangle(BoundingRect.X + caretPosition, BoundingRect.Y + 1, 6, BoundingRect.Height - 2));
      }

      base.Draw(gameTime);
    }

    #endregion

    #region Update

    public override void Update(GameTime gameTime)
    {
      if (IsWriteState)
      {
        if (Input.KeyboardEnterJustPressed)
        {
          HandleEnterKey();
        }
        Input.HandleKeyboardInput(ref text);
        caretPosition = (int)(ContentSpace.GetFont("NormalFont").MeasureString(text).X*textScale + 2);
      }
      base.Update(gameTime);
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

    private void HandleEnterKey()
    {
      EnterKeyPressed(this, EventArgs.Empty);
    }

    #endregion

  }
}
