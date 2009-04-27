using Microsoft.Xna.Framework;
using XnaTetris.Helpers;
using XnaTetris.Game;
using XnaTetris.Algorithms;
using XnaTetris.Interface;

namespace XnaTetris
{
  public class LinesGame : BaseGame
  {
    #region Constants
    public const int GRID_RECTANGLE_X_COORDINATE = 270;
    public const int GRID_RECTANGLE_Y_COORDINATE = 10;
    public const int PENALTY_FOR_WRONG_SWAP = 5000;
    public const int PENALTY_FOR_RESTART = 5000;
    #endregion

    #region Variables

    private GameField gameField;
    private Menu menu;

    private int curLevelNumber;

    #endregion

    #region Properties
    public long Timer { get; set; }
    public int OverallScore { get; set; }
    public int LevelScore { get; set; }
    public Serv.GameState GameState { get; private set; }
    public Level CurrentLevel { get; private set; }

    public double ElapsedGameMs { get; private set; }

    /// <summary>
    /// true if the game has just found lines
    /// </summary>
    public bool IsRemoveProcess { get; set; }
    public bool IsRestartProcess { get; set; }
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
      // Load all our content in one time
      content.RootDirectory = "Content";
      ContentSpace.LoadAllContent(content);

      // create scenes
      menu = new Menu(this, new Rectangle(0, 0, 800, 600));
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
      ContentSpace.GetSprite("SkyBackground").Render();

      if (menu.Visible)
        menu.Draw(gameTime);
      if (gameField.Visible)
        gameField.Draw(gameTime);
      if (CurrentLevel != null && CurrentLevel.StartWindow != null)
        if (CurrentLevel.StartWindow.Visible)
          CurrentLevel.StartWindow.Draw(gameTime);

      base.Draw(gameTime);
    }

    #endregion

    #region Loose and win level

    private void CheckForLoose()
    {
      if (Timer <= 0)
      {
        if (LevelScore <= CurrentLevel.LevelScore)
          ExitToMenu();
        else
        {
          GameState = Serv.GameState.GameStateLevelEnd;
         // StartNextLevel();
          ShowLevelDialog();
        }
      }
    }

    private void ShowLevelDialog()
    {
      CurrentLevel = new Level(++curLevelNumber, this);
      Components.Add(CurrentLevel.StartWindow);
      CurrentLevel.StartWindow.Show();
      gameField.Hide();
    }

    public void StartNextLevel()
    {
      LevelScore = 0;
      GameState = Serv.GameState.GameStateRunning;
      Timer = CurrentLevel.Time * 1000;

      CurrentLevel.StartWindow.Hide();
      Components.Remove(CurrentLevel.StartWindow);

      gameField.Show();
      gameField.BlockGrid.Restart();
    }

    #endregion

    #region Interface Events

    internal void Start()
    {
      GameState = Serv.GameState.GameStateRunning;
      OverallScore = 0;
      menu.Hide();
      gameField.Show();

      ShowLevelDialog();
    }

    public void ShowHiScores()
    {
      GameScene hiScores = new HiScores(this);

      GameState = Serv.GameState.GameStateRunning;
      Components.Add(hiScores);
      hiScores.Show();
      menu.Hide();
    }

    public void ExitToMenu()
    {
      OverallScore = 0;
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
