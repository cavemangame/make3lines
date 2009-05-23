using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;


namespace XnaTetris.Interface
{
  public class HelpWindow : GameScene
  {
    #region Variables

    private readonly Rectangle backgroundRect;
    private Button btnOk;

    #endregion

    #region Properties

    public LinesGame LinesGame { get { return Game as LinesGame; } }

    #endregion

    #region Constructor

    public HelpWindow(Microsoft.Xna.Framework.Game setGame)
      : base(setGame)
    {
      backgroundRect = new Rectangle(0, 0, LinesGame.Width, LinesGame.Height);
      InitButtons();
    }

    #endregion

    #region Draw

    public override void Draw(GameTime gameTime)
    {
      ContentSpace.GetSprite("HelpBackground").Render(backgroundRect);

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
