using System;
using Microsoft.Xna.Framework;
using XnaTetris.Game;

namespace XnaTetris.Interface
{
  class StartLevelWindow : GameScene
  {
    #region Variables

    private readonly Rectangle rect = new Rectangle(0, 0, 1024, 768);


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

      base.Draw(gameTime);
    }

    #endregion


    private void btnOk_ButtonAction(object sender, EventArgs e)
    {
      if (Game is LinesGame)
        (Game as LinesGame).StartNextLevel();
    }
  }
}
