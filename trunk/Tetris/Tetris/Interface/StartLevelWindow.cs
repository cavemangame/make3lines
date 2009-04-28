using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;
using System.Xml;

namespace XnaTetris.Interface
{
  public class StartLevelWindow : GameScene
  {
    #region Variables
    private readonly Rectangle backgroundRect;
    private Button btnOk;
    private readonly ConvertTaggedTextHelper helper;
    #endregion

    #region Properties
    public LinesGame LinesGame { get { return Game as LinesGame; } }
    #endregion

    #region Constructor
    public StartLevelWindow(Microsoft.Xna.Framework.Game setGame, XmlNode loadNode)
			: base(setGame)
		{
      backgroundRect = new Rectangle(0, 0, LinesGame.Width, LinesGame.Height);
      InitButtons();
      helper = new ConvertTaggedTextHelper(backgroundRect, loadNode);
    }

    private void InitButtons()
    {
      btnOk = new Button(LinesGame, new Rectangle(360, 550, 80, 40), ContentSpace.GetSprite("OkButton"),
                            ContentSpace.GetSprite("HiOkButton"));
      btnOk.ButtonAction += btnOk_ButtonAction;
      Components.Add(btnOk);
    }
    #endregion

    #region Draw
    public override void Draw(GameTime gameTime)
    {
      ContentSpace.GetSprite("LevelBackground").Render(backgroundRect);
      if (helper != null)
      {
        foreach (SpriteToRender stor in helper.Sprites)
        {
          stor.Sprite.Render(stor.Rect);
        }

        SpriteBatch spriteBatch = new SpriteBatch(LinesGame.GraphicsDevice);
        spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

        foreach (TextToRender ttor in helper.Texts)
        {
          spriteBatch.DrawString(ttor.Font, ttor.Text,
            new Vector2(ttor.Pos.X, ttor.Pos.Y), 
            ttor.TextColor, 0f,
            new Vector2(0, 0), ttor.Scale,
            SpriteEffects.None, 0);

        }
        spriteBatch.End();
      }

      base.Draw(gameTime);
    }
    #endregion

    private void btnOk_ButtonAction(object sender, EventArgs e)
    {
      Hide();
      LinesGame.StartNextLevel();
    }
  }
}
