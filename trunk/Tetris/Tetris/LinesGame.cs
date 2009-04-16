using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;
using XnaTetris.Game;
using XnaTetris.Algorithms;
using XnaTetris.Interface;

namespace XnaTetris
{
  public class LinesGame : BaseGame
  {
    #region Constants
    public const int GRID_RECTANGLE_X_COORDINATE = 310;
    public const int GRID_RECTANGLE_Y_COORDINATE = 35;
    public const int GRID_RECTANGLE_HEIGHT = 700;
    public const int GRID_RECTANGLE_WIDTH = 700;
    public const int PENALTY_FOR_WRONG_SWAP = 5000;
    public const int PENALTY_FOR_RESTART = 5000;
    #endregion

    #region Variables

    private GameField gameField;
    private Menu menu;
    private StartLevelWindow levelDialog;

    private int curLevelNumber;

    #endregion

    #region Properties
    public long Timer { get; set; }
    public int Score { get; set; }
    public Serv.GameState GameState { get; private set; }
    public SpriteFont NormalFont { get; private set; }
    public SpriteFont BigFont { get; private set; }
    public SpriteFont SmallFont { get; private set; }
    public Level CurrentLevel { get; private set; }

    public double ElapsedGameMs { get; private set; }

    /// <summary>
    /// true if the game has just found lines
    /// </summary>
    public bool IsRemoveProcess { get; set; }
    public bool IsRestartProcess { get; set; }
    #endregion

    #region Constructor

    public LinesGame()
    {
    }

    #endregion

    #region Init and Load methods

    protected override void Initialize()
    {
      IsMouseVisible = true;
      GameState = Serv.GameState.GameStateMenu;
      base.Initialize();
    }

    protected override void LoadContent()
    {
      // Load all our content
      content.RootDirectory = "Content";

      // Create all sprites
      ContentSpace.background = new SpriteHelper(content.Load<Texture2D>("skybackground"), null);
      ContentSpace.levelDialogBackground = new SpriteHelper(content.Load<Texture2D>("LevelBackground"), null);

      NormalFont = content.Load<SpriteFont>("normalfont");
      BigFont = content.Load<SpriteFont>("bigfont");
      SmallFont = content.Load<SpriteFont>("smallfont");

      menu = new Menu(this, new Rectangle(0, 0, 1024, 768),
            new SpriteHelper(content.Load<Texture2D>("MenuBackground"), null));
      gameField = new GameField(this);
      Components.Add(menu);
      Components.Add(gameField);

      menu.Show();
      base.LoadContent();
    }

    #endregion

    #region Update
    protected override void Update(GameTime gameTime)
    {
      if (gameField.Enabled && IsBoardInStableState())
      {
        long frameMs = (long)gameTime.ElapsedGameTime.TotalMilliseconds;

        if (Input.KeyboardSpaceJustPressed)
        {
          SetPauseUnpause();
        }

        if (GameState == Serv.GameState.GameStateRunning)
        {
          Timer -= frameMs;
        }
        if (GameState == Serv.GameState.GameStateRunning)
        {
          CheckForLoose();
        }
      }

      base.Update(gameTime);
    }
    #endregion

    #region Draw

    protected override void Draw(GameTime gameTime)
    {
      ElapsedGameMs = gameTime.TotalRealTime.TotalMilliseconds;
        // Render background
      ContentSpace.background.Render();

      if (menu.Visible)
        menu.Draw(gameTime);
      if (gameField.Visible)
        gameField.Draw(gameTime);

      base.Draw(gameTime);
    }

    #endregion

    #region Loose and win level

    private void CheckForLoose()
    {
      if (Timer <= 0)
      {
        if (Score <= CurrentLevel.maxScore)
          ExitToMenu();
        else
        {
          GameState = Serv.GameState.GameStateLevelEnd;
          StartNextLevel();
          //ShowLevelDialog();
        }
      }
    }

    private void ShowLevelDialog()
    {
     /* levelDialog = new StartLevelWindow(this, new Rectangle(300, 200, 400, 300), 
        ContentSpace.levelDialogBackground);
      Components.Add(levelDialog);
      blocksGrid.Enabled = false;
      btnExit.Enabled = btnPause.Enabled = false;*/
    }

    public void StartNextLevel()
    {
      curLevelNumber++;
      CurrentLevel = LevelGenerator.GetLevel(curLevelNumber);
      GameState = Serv.GameState.GameStateRunning;
      Timer = CurrentLevel.time;

      //Components.Remove(levelDialog);
      gameField.Show();
      gameField.BlockGrid.Restart();
    }

    #endregion

    #region Interface Events

    void btnExit_ButtonAction(object sender, EventArgs e)
    {
      ExitToMenu();
    }

    void btnPause_ButtonAction(object sender, EventArgs e)
    {
      SetPauseUnpause();
    }

    internal void Start()
    {
      GameState = Serv.GameState.GameStateRunning;
      Score = 0;
      menu.Hide();
      gameField.Show();
      StartNextLevel();
    }

    public void ExitToMenu()
    {
      Score = 0;
      Timer = 0;
      curLevelNumber = 0;
      menu.Show();
      gameField.Hide();
      GameState = Serv.GameState.GameStateMenu;
    }

    public void SetPauseUnpause()
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

    #endregion

    #region Start game

    public static void StartGame()
    {
      using (LinesGame game = new LinesGame())
      {
        game.Run();
      }
    }

    #endregion


    #region Some help methods

    /// <summary>
    /// Determines if there are moving blocks on the board or not
    /// </summary>
    /// <returns>true if there are no moving blocks</returns>
    public bool IsBoardInStableState()
    {
      return gameField.BlockGrid.ActiveBlocks == 0 && !IsRemoveProcess && !IsRestartProcess;
    }

    /// <summary>
    /// It's time to remove them all
    /// </summary>
    /// <param name="gameTime"></param>
    public void RemoveVisualizationWasFinished(GameTime gameTime)
    {
      if (IsRemoveProcess)
      {
        IsRemoveProcess = false;
        gameField.BlockGrid.RemoveLines(gameTime);
      }
      if (IsRestartProcess)
      {
        IsRestartProcess = false;
        gameField.BlockGrid.ReadyToRestart();
      }
    }

    #endregion
  }
}
