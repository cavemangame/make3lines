using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;


namespace XnaTetris.Interface
{
  public class GameField : GameScene
  {
    public const int GRID_RECTANGLE_X_COORDINATE = 270;
    public const int GRID_RECTANGLE_Y_COORDINATE = 10;
    public const int GRID_RECTANGLE_HEIGHT = 512;
    public const int GRID_RECTANGLE_WIDTH = 512;
    private readonly Rectangle rectPauseButton = new Rectangle(40, 400, 180, 50);
    private readonly Rectangle rectExitButton = new Rectangle(40, 460, 180, 50);

    private readonly Button btnPause;
    private readonly Button btnExit;

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

      btnPause = new Button(LinesGame, rectPauseButton, ContentSpace.GetSprite("PauseButton"), ContentSpace.GetSprite("PauseHiButton"));
      btnPause.ButtonAction += btnPause_ButtonAction;
      btnExit = new Button(LinesGame, rectExitButton, ContentSpace.GetSprite("ExitButton"), ContentSpace.GetSprite("ExitHiButton"));
      btnExit.ButtonAction += btnExit_ButtonAction;

      Components.Add(BlockGrid);
      Components.Add(btnPause);
      Components.Add(btnExit);
    }

    public override void Draw(GameTime gameTime) 
    {
      // Draw background boxes for all the components
      ContentSpace.GetSprite("BackgroundBigBox").Render(new Rectangle(270, 10, 512, 512));
      ContentSpace.GetSprite("BackgroundSmallBox").Render(new Rectangle(10, 10, 240, 512));

      TextHelper.DrawText(ContentSpace.GetFont("NormalFont"), 
        String.Format("Score: {0}", LinesGame.OverallScore), 20, 20, Color.White);
      TextHelper.DrawText(ContentSpace.GetFont("NormalFont"),
        String.Format("Level Score: {0}", LinesGame.LevelScore), 20, 60, Color.WhiteSmoke);
      TextHelper.DrawText(ContentSpace.GetFont("NormalFont"),
        String.Format("Remain: {0}", Math.Max(LinesGame.CurrentLevel.LevelScore - LinesGame.LevelScore, 0)), 
        20, 100, Color.WhiteSmoke);
      TextHelper.DrawText(ContentSpace.GetFont("NormalFont"),
         LinesGame.CurrentLevel.LevelString, 20, 140, Color.LightCoral);
      TextHelper.DrawText(ContentSpace.GetFont("NormalFont"),
         Serv.GetTimeString(LinesGame.Timer), 20, 180, Color.LightPink);

      
      if (LinesGame.GameState == Serv.GameState.GameStatePause)
        TextHelper.DrawText(ContentSpace.GetFont("NormalFont"), "PAUSE", 340, 280, Color.LightPink);

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