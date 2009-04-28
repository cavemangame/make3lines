using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using XnaTetris.Helpers;
using XnaTetris.Game;
using XnaTetris.Algorithms;
using XnaTetris.Interface;
using XnaTetris.Sounds;

namespace XnaTetris
{
  public class LinesGame : Microsoft.Xna.Framework.Game
  {
    #region Constants
    public const int GRID_RECTANGLE_X_COORDINATE = 270;
    public const int GRID_RECTANGLE_Y_COORDINATE = 10;
    public const int PENALTY_FOR_WRONG_SWAP = 5000;
    public const int PENALTY_FOR_RESTART = 5000;
    public const int PREFERRED_RESOLUTION_WIDTH = 800;
    public const int PREFERRED_RESOLUTION_HEIGHT = 600;
    #endregion

    #region Variables
    private readonly GraphicsDeviceManager graphics;
    private readonly ContentManager content;
    private TextureFont font;
    private TextHelper textHelper;

    // components
    private GameScene menu;
    private GameField gameField;
    private GameScene hiScores;
    
    private int curLevelNumber;
    #endregion

    #region Properties
    /// <summary>
    /// Resolution of our game
    /// </summary>
    public static int Width { get; private set; }
    public static int Height { get; private set; }
    public long Timer { get; set; }
    public int LevelScore { get; set; }
    public Serv.GameState GameState { get; private set; }
    public Level CurrentLevel { get; private set; }
    public Scores Score { get; private set; }
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
      graphics = new GraphicsDeviceManager(this)
                   {
                     PreferredBackBufferWidth = PREFERRED_RESOLUTION_WIDTH,
                     PreferredBackBufferHeight = PREFERRED_RESOLUTION_HEIGHT,
                     IsFullScreen = false
                   };
      graphics.ApplyChanges();
      content = new ContentManager(Services) {RootDirectory = "Content"};
      Score = new Scores();
    }
    #endregion

    #region Init and Load methods
    protected override void Initialize()
    {
      Width = graphics.GraphicsDevice.Viewport.Width;
      Height = graphics.GraphicsDevice.Viewport.Height;
      IsMouseVisible = true;

      base.Initialize();
    }

    protected override void LoadContent()
    {
      font = new TextureFont(graphics.GraphicsDevice, content);
      textHelper = new TextHelper(graphics.GraphicsDevice);
      ContentSpace.LoadAllContent(content);

      menu = new Menu(this, new Rectangle(0, 0, Width, Height));
      Components.Add(menu);
      gameField = new GameField(this);
      Components.Add(gameField);
      hiScores = new HiScores(this);
      Components.Add(hiScores);

      base.LoadContent();

      ShowMenu();
    }

    protected override void UnloadContent()
    {
      content.Unload();
      SpriteHelper.Dispose();

      base.UnloadContent();
    }
    #endregion

    #region Update
    protected override void Update(GameTime gameTime)
    {
      Sound.Update();
      Input.Update();

      if (Input.KeyboardEscapeJustPressed || Input.GamePadBackJustPressed)
      {
        Exit();
      }

      if (gameField.Enabled && IsBoardInStableState())
      {
        long frameMs = (long)gameTime.ElapsedGameTime.TotalMilliseconds;

        if (Input.KeyboardSpaceJustPressed)
        {
            gameField.PauseAction();
        }

        if (!gameField.Paused)
        {
          Timer -= frameMs;
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

      SpriteHelper.DrawSprites();
      font.WriteAll();
      textHelper.WriteAll();

      base.Draw(gameTime);
    }
    #endregion

    #region Loose and win level
    private void CheckForLoose()
    {
      if (Timer <= 0)
      {
        gameField.Hide();
        if (LevelScore <= CurrentLevel.LevelScore)
        {
          ShowMenu();
        }
        else
        {
          GameState = Serv.GameState.GameStateLevelEnd;
          ShowLevelDialog();
        }
      }
    }

    private void ShowLevelDialog()
    {
      CurrentLevel = new Level(++curLevelNumber, this);
      Components.Add(CurrentLevel.StartWindow);
      CurrentLevel.StartWindow.Show();
    }

    public void StartNextLevel()
    {
      LevelScore = 0;
      GameState = Serv.GameState.GameStateRunning;
      Timer = CurrentLevel.Time * 1000;

      Components.Remove(CurrentLevel.StartWindow);

      gameField.Show();
      gameField.BlockGrid.Restart();
    }
    #endregion

    #region Interface Events
    public void Start()
    {
      GameState = Serv.GameState.GameStateRunning;
      Score.Reset();

      ShowLevelDialog();
    }

    public void ShowHiScores()
    {
      GameState = Serv.GameState.GameStateRunning;
      hiScores.Show();
    }

    public void ShowMenu()
    {
      GameState = Serv.GameState.GameStateMenu;
      Timer = 0;
      curLevelNumber = 0;
      menu.Show();
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
