using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;


namespace XnaTetris.Interface
{
  public class GameField : GameScene
  {
    public const int GRID_RECTANGLE_X_COORDINATE = 310;
    public const int GRID_RECTANGLE_Y_COORDINATE = 35;
    public const int GRID_RECTANGLE_HEIGHT = 700;
    public const int GRID_RECTANGLE_WIDTH = 700;
    private readonly Rectangle rectPauseButton = new Rectangle(55, 600, 200, 50);
    private readonly Rectangle rectExitButton = new Rectangle(55, 670, 200, 50);

    private Button btnPause, btnExit;

    #region Properties

    public LinesGame LinesGame { get { return Game as LinesGame; } }
    public BlocksGrid BlockGrid { get; private set; }

    #endregion

    public GameField(LinesGame game)
      : base(game)
    {
      BlockGrid = new BlocksGrid(LinesGame, 
        new Rectangle(GRID_RECTANGLE_X_COORDINATE, GRID_RECTANGLE_Y_COORDINATE,
                      GRID_RECTANGLE_WIDTH, GRID_RECTANGLE_HEIGHT));

      btnPause = new Button(LinesGame, rectPauseButton, ContentSpace.GetSprite("PauseButton"));
      btnPause.ButtonAction += btnPause_ButtonAction;
      btnExit = new Button(LinesGame, rectExitButton, ContentSpace.GetSprite("ExitButton"));
      btnExit.ButtonAction += btnExit_ButtonAction;

      Components.Add(BlockGrid);
      Components.Add(btnPause);
      Components.Add(btnExit);
    }

    public override void Update(GameTime gameTime)
    {
      // TODO: Add your update code here

      base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      // Draw background boxes for all the components
      ContentSpace.GetSprite("BackgroundBigBox").Render(new Rectangle(300, 25, 720, 720));
      ContentSpace.GetSprite("BackgroundSmallBox").Render(new Rectangle(25, 25, 260, 720));

      TextureFont.WriteText(40, 50, String.Format("Score: {0}", LinesGame.Score));
      TextureFont.WriteText(40, 90, String.Format("Remain: {0}",
        Math.Max(LinesGame.CurrentLevel.maxScore - LinesGame.Score, 0)));
      TextureFont.WriteText(40, 140, LinesGame.CurrentLevel.LevelString);
      TextureFont.WriteText(40, 180, Serv.GetTimeString(LinesGame.Timer));

      if (LinesGame.GameState == Serv.GameState.GameStatePause)
        TextureFont.WriteText(610, 370, "PAUSE", Color.AliceBlue);


      base.Draw(gameTime);
    }

    void btnExit_ButtonAction(object sender, EventArgs e)
    {
      LinesGame.ExitToMenu();
    }

    void btnPause_ButtonAction(object sender, EventArgs e)
    {
      LinesGame.SetPauseUnpause();
    }
  }
}