using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;
using System.Xml;

namespace XnaTetris.Interface
{
  public class LevelWindow : GameScene
  {
    #region Variables
    private readonly Rectangle backgroundRect;
    private Button btnOk;
    private readonly ConvertTaggedTextHelper helper;

    public event EventHandler BtnOkClick;
    #endregion

    #region Properties
    public LinesGame LinesGame { get { return Game as LinesGame; } }
    #endregion

    #region Constructor
    public LevelWindow(Microsoft.Xna.Framework.Game setGame, XmlNode loadNode)
			: base(setGame)
		{
      backgroundRect = new Rectangle(100, 100, 600, 400);
      InitButtons();
      helper = new ConvertTaggedTextHelper(backgroundRect, loadNode);
    }

    public LevelWindow(Microsoft.Xna.Framework.Game setGame, Rectangle setRect, ConvertTaggedTextHelper helper)
      : base(setGame)
    {
      backgroundRect = setRect;
      InitButtons();
      this.helper = helper;
    }

    private void InitButtons()
    {
      var btnOkPos = new Rectangle(backgroundRect.X + backgroundRect.Width/2 - 40, 
        backgroundRect.Bottom - 50, 80, 40);
      btnOk = new Button(LinesGame, btnOkPos, ContentSpace.GetSprite("OkButton"),
                            ContentSpace.GetSprite("HiOkButton"));
      btnOk.ButtonAction += btnOk_ButtonAction;
      Components.Add(btnOk);
    }
    #endregion

    #region Draw
    public override void Draw(GameTime gameTime)
    {
      ContentSpace.GetSprite("LevelBackground").Render(spriteBatch, backgroundRect);
      if (helper != null)
      {
        foreach (SpriteToRender stor in helper.Sprites)
        {
          stor.Sprite.Render(spriteBatch, stor.Rect);
        }

        foreach (TextToRender ttor in helper.Texts)
        {
          TextHelper.DrawShadowedText(ttor.Font, ttor.Text, (int)ttor.Pos.X, (int)ttor.Pos.Y, 
            ttor.TextColor, ttor.Scale);
        }
      }

      base.Draw(gameTime);
    }
    #endregion

    private void btnOk_ButtonAction(object sender, EventArgs e)
    {
      Hide();
      BtnOkClick(this, e);
    }
  }
}
