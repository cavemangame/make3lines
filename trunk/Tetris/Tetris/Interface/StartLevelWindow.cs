using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;

namespace XnaTetris.Interface
{
  class StartLevelWindow : DrawableGameComponent
  {
    #region Variables

    private readonly Rectangle rect;
    private readonly SpriteHelper background;

    private static readonly Rectangle srcGeneral = new Rectangle(0, 0, 80, 40);
    private static readonly Rectangle srcHilight = new Rectangle(80, 0, 80, 40);


    private Button btnOk;

    #endregion

    #region Constructor

    public StartLevelWindow(LinesGame setGame, Rectangle setRect, SpriteHelper setBackground)
			: base(setGame)
		{
			rect = setRect;
			background = setBackground;
      InitButtons(setGame);
    }

    private void InitButtons(LinesGame setGame)
    {
      LoadTexturesAndSprites(setGame);

      btnOk = new Button(setGame, new Rectangle(420, 300, 80, 40), ContentSpace.buttonLevelDialogOk,
                            ContentSpace.buttonHiLevelDialogOk);
      btnOk.ButtonAction += btnOk_ButtonAction;
      Game.Components.Add(btnOk);
    }


    private static void LoadTexturesAndSprites(LinesGame game)
    {
      if (game != null)
      {
        // Load menu content
        ContentSpace.buttonLevelDialogOk =
          new SpriteHelper(game.Content.Load<Texture2D>("LevelOkButton"), srcGeneral);
        ContentSpace.buttonHiLevelDialogOk =
          new SpriteHelper(game.Content.Load<Texture2D>("LevelOkButton"), srcHilight);
      }
    }

    #endregion

    #region Draw

    public override void Draw(GameTime gameTime)
    {
      background.Render(rect);
      btnOk.Draw(gameTime);

      base.Draw(gameTime);
    }

    #endregion

    #region Update

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
    }

    public void EnableComponents(bool isEnable)
    {
      Enabled = btnOk.Enabled = isEnable;
    }

    #endregion


    private void btnOk_ButtonAction(object sender, EventArgs e)
    {
      if (Game is LinesGame)
        (Game as LinesGame).StartNextLevel();
    }
  }
}
