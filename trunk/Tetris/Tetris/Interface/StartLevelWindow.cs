using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;

namespace XnaTetris.Interface
{
  public class StartLevelWindow : GameScene
  {
    #region Variables

    private readonly Rectangle rect = new Rectangle(0, 0, 1024, 768);
    private Button btnOk;
    private ConvertTaggedTextHelper helper;

    #endregion

    #region Properties

    public LinesGame LinesGame { get { return Game as LinesGame; } }

    #endregion

    #region Constructor

    public StartLevelWindow(LinesGame setGame)
			: base(setGame)
		{
      InitButtons();
      //helper = new ConvertTaggedTextHelper(rect);
    }

    private void InitButtons()
    {
      btnOk = new Button(LinesGame, new Rectangle(472, 700, 80, 40), ContentSpace.GetSprite("LevelOkButton"),
                            ContentSpace.GetSprite("LevelHiOkButton"));
      btnOk.ButtonAction += btnOk_ButtonAction;
      Components.Add(btnOk);
    }

    #endregion

    #region Draw

    public override void Draw(GameTime gameTime)
    {
      ContentSpace.GetSprite("LevelBackground").Render(rect);
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
            new Vector2(ttor.Pos.X * BaseGame.Width / 1024, ttor.Pos.Y * BaseGame.Height / 768), 
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
      if (Game is LinesGame)
        (Game as LinesGame).StartNextLevel();
    }

    public void SetNewDescription(string filename)
    {
      helper.ConvertTaggedText(filename);
    }
  }
}
