using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;

namespace XnaTetris.Interface
{
  class StartLevelWindow : GameScene
  {
    #region Variables

    private readonly Rectangle rect = new Rectangle(0, 0, 1024, 768);

    private readonly Rectangle srcGeneral = new Rectangle(0, 0, 80, 40);
    private readonly Rectangle srcHilight = new Rectangle(80, 0, 80, 40);

    private Button btnOk;

    #endregion

    #region Properties

    public LinesGame LinesGame { get { return Game as LinesGame; } }

    #endregion

    #region Constructor

    public StartLevelWindow(LinesGame setGame)
			: base(setGame)
		{
      InitButtons();
    }

    private void InitButtons()
    {
      LoadTexturesAndSprites();

      btnOk = new Button(LinesGame, new Rectangle(472, 700, 80, 40), ContentSpace.buttonLevelDialogOk,
                            ContentSpace.buttonHiLevelDialogOk);
      btnOk.ButtonAction += btnOk_ButtonAction;
      Components.Add(btnOk);
    }


    private void LoadTexturesAndSprites()
    {
      if (LinesGame != null)
      {
        ContentSpace.levelDialogBackground = 
          new SpriteHelper(LinesGame.Content.Load<Texture2D>("LevelBackground"), null);
        ContentSpace.buttonLevelDialogOk =
          new SpriteHelper(LinesGame.Content.Load<Texture2D>("LevelOkButton"), srcGeneral);
        ContentSpace.buttonHiLevelDialogOk =
          new SpriteHelper(LinesGame.Content.Load<Texture2D>("LevelOkButton"), srcHilight);
      }
    }

    #endregion

    #region Draw

    public override void Draw(GameTime gameTime)
    {
      ContentSpace.levelDialogBackground.Render(rect);

      base.Draw(gameTime);
    }

    #endregion

    #region Update

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
    }

    #endregion


    private void btnOk_ButtonAction(object sender, EventArgs e)
    {
      if (Game is LinesGame)
        (Game as LinesGame).StartNextLevel();
    }
  }
}
