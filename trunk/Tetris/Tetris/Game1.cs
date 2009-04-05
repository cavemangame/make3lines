using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;
using XnaTetris.Graphics;
using XnaTetris.Game;
using XnaTetris.Algorithms;
using XnaTetris.Interface;

namespace XnaTetris
{
  public class LinesGame : BaseGame
  {
    #region Constants

    private readonly Rectangle rectPauseButton = new Rectangle(55, 600, 200, 50);
    private readonly Rectangle rectExitButton = new Rectangle(55, 670, 200, 50);

    #endregion

    #region Variables

    private readonly BlocksGrid blocksGrid;

    // graphics
    Texture2D backgroundTexture, backgroundSmallBoxTexture, backgroundBigBoxTexture,
      buttonPauseTexture, buttonExitTexture;
    SpriteHelper background, backgroundSmallBox, backgroundBigBox, buttonPause, buttonExit;


    /// <summary>
    ///  Timer and Level variables
    /// </summary>
    public long timer;
    private Level currentLevel;
    private int curLevelNumber;

    private int elapsedGameMs;

    /// <summary>
    /// Interface
    /// </summary>
    private Button btnPause, btnExit;

    #endregion

    #region Properties

    public int Score { get; set; }
    internal bool IsMoving { get; set; }
    public Serv.GameState GameState { get; set; }

    #endregion

    #region Constructor

    public LinesGame()
    {
      blocksGrid = new BlocksGrid(this, new Rectangle(310, 35, 700, 700));
      Components.Add(blocksGrid);
    }

    #endregion

    #region Init and Load methods

    protected override void Initialize()
    {
      IsMouseVisible = true;
      Score = 0;
      StartNextLevel();

      base.Initialize();
    }

    protected override void LoadContent()
    {
      // Load all our content
      content.RootDirectory = "Content";
      backgroundTexture = content.Load<Texture2D>("skybackground");
      backgroundSmallBoxTexture = content.Load<Texture2D>("BackgroundSmallBox");
      backgroundBigBoxTexture = content.Load<Texture2D>("BackgroundBigBox");
      buttonPauseTexture = content.Load<Texture2D>("PauseButton");
      buttonExitTexture = content.Load<Texture2D>("ExitButton");

      // Create all sprites
      background = new SpriteHelper(backgroundTexture, null);
      backgroundSmallBox = new SpriteHelper(backgroundSmallBoxTexture, null);
      backgroundBigBox = new SpriteHelper(backgroundBigBoxTexture, null);
      buttonPause = new SpriteHelper(buttonPauseTexture, null);
      buttonExit = new SpriteHelper(buttonExitTexture, null);

      // Create interface elements
      btnPause = new Button(this, rectPauseButton, buttonPause);
      btnPause.ButtonAction += btnPause_ButtonAction;
      btnExit = new Button(this, rectExitButton, buttonExit);
      btnExit.ButtonAction += btnExit_ButtonAction;
      Components.Add(btnPause);
      Components.Add(btnExit);

      base.LoadContent();
    } // LoadGraphicsContent(loadAllContent)

    #endregion

    #region Update

    protected override void Update(GameTime gameTime)
    {
      int frameMs = (int)gameTime.ElapsedGameTime.TotalMilliseconds;
      elapsedGameMs += frameMs;

      if (Input.KeyboardSpaceJustPressed)
        SetPauseUnpause();

      if (GameState == Serv.GameState.GameStateRunning)
        timer -= frameMs;

      CheckForLoose();

      base.Update(gameTime);
    }// Update(gameTime)

    #endregion

    #region Draw

    protected override void Draw(GameTime gameTime)
    {
      // Render background
      background.Render();

      // Draw background boxes for all the components
      backgroundBigBox.Render(new Rectangle(300, 25, 720, 720));
      backgroundSmallBox.Render(new Rectangle(25, 25, 260, 720));

      if (GameState == Serv.GameState.GameStateRunning || GameState == Serv.GameState.GameStatePause)
        blocksGrid.Draw(gameTime);

      btnPause.Draw(gameTime);
      btnExit.Draw(gameTime);

      TextureFont.WriteText(40, 50, String.Format("Score: {0}", Score));
      TextureFont.WriteText(40, 90, String.Format("Remain: {0}", currentLevel.maxScore - Score));
      TextureFont.WriteText(40, 140, currentLevel.LevelString);
      TextureFont.WriteText(40, 180, Serv.GetTimeString(timer));

      if (GameState == Serv.GameState.GameStatePause)
        TextureFont.WriteText(610, 370, "PAUSE", Color.AliceBlue);

      base.Draw(gameTime);
    }

    #endregion

    private void CheckForLoose()
    {
      if (timer <= 0)
      {
        if (Score <= currentLevel.maxScore)
          ExitGame();
        else
        {
          GameState = Serv.GameState.GameStateLevelEnd;
          StartNextLevel();
        }
      }
    }

    private void ExitGame()
    {
      Exit();
    }

    private void SetPauseUnpause()
    {
      if (GameState == Serv.GameState.GameStateRunning)
        PauseGame();
      else if (GameState == Serv.GameState.GameStatePause)
        RunGame();
    }

    private void PauseGame()
    {
      GameState = Serv.GameState.GameStatePause;
    }

    private void RunGame()
    {
      GameState = Serv.GameState.GameStateRunning;
    }

    private void StartNextLevel()
    {
      curLevelNumber++;
      currentLevel = LevelGenerator.GetLevel(curLevelNumber);
      blocksGrid.Restart();
      GameState = Serv.GameState.GameStateRunning;
      timer = currentLevel.time;
    }

    #region Interface Events

    void btnExit_ButtonAction(object sender, EventArgs e)
    {
      Exit();
    }

    void btnPause_ButtonAction(object sender, EventArgs e)
    {
      SetPauseUnpause();
    }

    #endregion

    #region Start game

    public static void StartGame()
    {
      using (LinesGame game = new LinesGame())
      {
        game.Run();
      } // using
    } // StartGame()

    #endregion

    #region Unit tests
#if DEBUG
    #region Test Empty Grid
    public static void TestEmptyGrid()
    {
      TestGame.Start("TestEmptyGrid",
          delegate
          {
            // Render background
            TestGame.game.background.Render();

            // Draw background box
            TestGame.game.backgroundBigBox.Render(new Rectangle(300, 25, 720, 720));

            // Show TetrisGrid component inside that box
            TestGame.game.blocksGrid.Draw(new GameTime());
          });
    } // TestEmptyGrid()
    #endregion

    #region TestBackgroundBoxes
    public static void TestBackgroundBoxes()
    {
      TestGame.Start("TestBackgroundBoxes",
                     delegate
                     {
                       // Render background
                       TestGame.game.background.Render();

                       // Draw background boxes for all the components
                       TestGame.game.backgroundBigBox.Render(new Rectangle(297, 28, 703, 703));
                       TestGame.game.backgroundSmallBox.Render(new Rectangle(27, 25, 260, 705));
                     });
    } // TestBackgroundBoxes()
    #endregion
    #region TestScoreboard
    public static void TestScoreboard()
    {
      int level = 3, score = 350, highscore = 1542, lines = 13;
      TestGame.Start("TestScoreboard",
          delegate
          {
            // Draw background box
            TestGame.game.backgroundSmallBox.Render(new Rectangle(
                (512 + 240) - 15, 40 - 10, 290 - 30, 190));

            // Show current level, score, etc.
            TextureFont.WriteText(512 + 240, 50, "Level: ");
            TextureFont.WriteText(512 + 420, 50, (level + 1).ToString());
            TextureFont.WriteText(512 + 240, 90, "Score: ");
            TextureFont.WriteText(512 + 420, 90, score.ToString());
            TextureFont.WriteText(512 + 240, 130, "Lines: ");
            TextureFont.WriteText(512 + 420, 130, lines.ToString());
            TextureFont.WriteText(512 + 240, 170, "Highscore: ");
            TextureFont.WriteText(512 + 420, 170, highscore.ToString());
          });
    } // TestScoreboard()
    #endregion
#endif
    #endregion
  }
}
