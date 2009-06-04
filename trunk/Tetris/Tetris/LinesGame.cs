using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
    private SpriteBatch spriteBatch;
    private readonly ContentManager content;
    private TextureFont font;
    private TextHelper textHelper;

    // components
    private GameScene menu;
    private GameScene hiScores;
    private GameScene help;

    public Player Player { get; set;}
    
    #endregion

    #region Properties
    /// <summary>
    /// Resolution of our game
    /// </summary>
    public static int Width { get; private set; }
    public static int Height { get; private set; }
    public long Timer { get; set; }
    public int LevelScore { get; set; }
    public Serv.GameState GameState { get; set; }
    public Scores Score { get; set; }
    public double ElapsedGameMs { get; private set; }
    public GameField GameField { get; private set; }

    /// <summary>
    /// true if the game has just found lines
    /// </summary>
    public bool IsRemoveProcess { get; set; }
    public bool IsRestartProcess { get; set; }

    public string CurrentPlayerName { get; set; }

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
      CurrentPlayerName = Serv.PlayerName;
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
      spriteBatch = new SpriteBatch(GraphicsDevice);
      Services.AddService(typeof(SpriteBatch), spriteBatch);

      font = new TextureFont(graphics.GraphicsDevice, content);
      textHelper = new TextHelper(this);
      ContentSpace.LoadAllContent(content);

      menu = new Menu(this, new Rectangle(0, 0, Width, Height));
      Components.Add(menu);
      GameField = new GameField(this);
      Components.Add(GameField);
      hiScores = new HiScores(this);
      Components.Add(hiScores);
      help = new HelpWindow(this);
      Components.Add(help);

      base.LoadContent();

      ShowMenu();
    }

    protected override void UnloadContent()
    {
      content.Unload();
      spriteBatch.Dispose();

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
        if (Player != null)
          Serv.PlayerName = Player.PlayerName;
        Exit();
      }

      if (GameField.Enabled && IsBoardInStableState() && GameState == Serv.GameState.GameStateRunning)
      {
        long frameMs = (long)gameTime.ElapsedGameTime.TotalMilliseconds;

        if (Input.KeyboardSpaceJustPressed)
        {
            GameField.PauseAction();
        }

        if (!GameField.Paused)
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

      spriteBatch.Begin();
      base.Draw(gameTime);
      spriteBatch.End();

      font.WriteAll();
      textHelper.WriteAll();
    }
    #endregion

    #region Loose and win level
    private void CheckForLoose()
    {
      if (Timer <= 0 && GameState == Serv.GameState.GameStateRunning)
      {
        GameState = Serv.GameState.GameStateBetweenLevel;
        if (LevelScore <= GameField.CurrentLevel.LevelScore)
        {
          GameField.GameOver();
        }
        else
        {
          GameState = Serv.GameState.GameStateBetweenLevel;
          Player.PlayerScore.Copy(Score);
          GameField.EndLevel();
        }
      }
    }
    #endregion

    #region Interface Events

    public void Start()
    {
      GameState = Serv.GameState.GameStateRunning;
      Score.Copy(Player.PlayerScore);
      GameField.StartNewGame();
    }

    public void ShowHiScores()
    {
      GameState = Serv.GameState.GameStateMenu;
      hiScores.Show();
    }

    public void ShowMenu()
    {
      GameState = Serv.GameState.GameStateMenu;
      Timer = 0;
      menu.Show();
    }

    public void ShowHelp()
    {
      GameState = Serv.GameState.GameStateMenu;
      help.Show();
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
      return GameField.BlockGrid.ActiveBlocks == 0 && !IsRemoveProcess && !IsRestartProcess;
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
        GameField.BlockGrid.RemoveLines(gameTime);
      }
      if (IsRestartProcess)
      {
        IsRestartProcess = false;
        GameField.BlockGrid.ReadyToRestart();
      }
    }
    #endregion
  }
}
