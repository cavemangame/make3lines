using System;
using Microsoft.Xna.Framework;
using XnaTetris.Game;


namespace XnaTetris.Interface
{
  public class HiScores : GameScene
  {
    #region Variables

    private readonly Rectangle backgroundRect;
    private Button btnOk;

    #endregion

    #region Properties

    public LinesGame LinesGame { get { return Game as LinesGame; } }

    #endregion

    #region Constructor

    public HiScores(Microsoft.Xna.Framework.Game setGame)
			: base(setGame)
		{
      backgroundRect = new Rectangle(0, 0, LinesGame.Width, LinesGame.Height);
      InitButtons();
    }

    #endregion

    #region Draw

    public override void Draw(GameTime gameTime)
    {
      // TODO: use own background
      ContentSpace.GetSprite("LevelBackground").Render(backgroundRect);

      base.Draw(gameTime);
    }

    #endregion

    private void InitButtons()
    {
      btnOk = new Button(LinesGame, new Rectangle(360, 550, 80, 40),
                         ContentSpace.GetSprite("OkButton"),
                         ContentSpace.GetSprite("HiOkButton"));
      btnOk.ButtonAction += btnOk_ButtonAction;
      Components.Add(btnOk);
    }

    private void btnOk_ButtonAction(object sender, EventArgs e)
    {
      Hide();
      LinesGame.ShowMenu();
    }
  }
}
