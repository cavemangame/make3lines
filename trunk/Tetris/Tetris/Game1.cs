using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using XnaTetris.Helpers;
using XnaTetris.Graphics;
using XnaTetris.Game;
using XnaTetris.Sounds;
using XnaTetris.Algorithms;

namespace XnaTetris
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class LinesGame : BaseGame
	{
		#region Variables

		private readonly BlocksGrid blocksGrid;

		private int score;
		private bool isMoving = false;
		private Serv.GameState gameState;
		// graphics
		SpriteBatch spriteBatch;
		Texture2D backgroundTexture, backgroundSmallBoxTexture, backgroundBigBoxTexture;
		SpriteHelper  background, backgroundSmallBox, backgroundBigBox;

		/// <summary>
		///  Timer and Level variables
		/// </summary>
		public long timer;
		private Level currentLevel;
		private int curLevelNumber = 0;

		#endregion

		#region Properties

		public int Score
		{
			get { return score; }
			set { score = value; }
		}

		internal bool IsMoving
		{
			get { return isMoving; }
			set { isMoving = value; }
		}

		public Serv.GameState GameState
		{
			get { return gameState; }
			set { gameState = value; }
		}

		#endregion

		public LinesGame()
		{
			blocksGrid = new BlocksGrid(this, new Rectangle(310, 35, 700, 700));
			this.Components.Add(blocksGrid);
		}

		#region Init and Load methods

		protected override void Initialize()
		{
			base.Initialize();
			IsMouseVisible = true;
			score = 0;
			StartNextLevel();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void LoadGraphicsContent(bool loadAllContent)
		{
			if (loadAllContent)
			{
				// Load all our content
				content.RootDirectory = "Content";
				backgroundTexture = content.Load<Texture2D>("SpaceBackground");
				backgroundSmallBoxTexture = content.Load<Texture2D>("BackgroundSmallBox");
				backgroundBigBoxTexture = content.Load<Texture2D>("BackgroundBigBox");

				// Create all sprites
				background = new SpriteHelper(backgroundTexture, null);
				backgroundSmallBox = new SpriteHelper(backgroundSmallBoxTexture, null);
				backgroundBigBox = new SpriteHelper(backgroundBigBoxTexture, null);
			} // if
			base.LoadGraphicsContent(loadAllContent);
		} // LoadGraphicsContent(loadAllContent)

		#endregion

		protected override void UnloadContent()
		{
		}

		#region Update
		int elapsedGameMs = 0;

		protected override void Update(GameTime gameTime)
		{
			int oldGameMs = elapsedGameMs;
			int frameMs = (int)gameTime.ElapsedGameTime.TotalMilliseconds;
			elapsedGameMs += frameMs;

			if (Input.KeyboardSpaceJustPressed)
				if (GameState == Serv.GameState.GameStateRunning)
					GameState = Serv.GameState.GameStatePause;
				else if (GameState == Serv.GameState.GameStatePause)
					GameState = Serv.GameState.GameStateRunning;

			if (GameState == Serv.GameState.GameStateRunning)
				timer -= frameMs;

			CheckForLoose();

			base.Update(gameTime);
		} // Update(gameTime)

		#endregion

		protected override void Draw(GameTime gameTime)
		{
			// Render background
			background.Render();

			// Draw background boxes for all the components
			backgroundBigBox.Render(new Rectangle(300, 25, 720, 720));
			backgroundSmallBox.Render(new Rectangle(25, 25, 260, 720));

			if (GameState == Serv.GameState.GameStateRunning || GameState == Serv.GameState.GameStatePause)
				blocksGrid.Draw(gameTime);

			TextureFont.WriteText(10, 10, String.Format("Score: {0}", score));
			TextureFont.WriteText(40, 50, currentLevel.LevelString);
			TextureFont.WriteText(40, 90, Serv.GetTimeString(timer));

		  if (GameState == Serv.GameState.GameStatePause)
				TextureFont.WriteText(610, 370, "PAUSE", Color.AliceBlue);

			base.Draw(gameTime);
		}

		private void CheckForLoose()
		{
			if (timer <= 0)
			{
				if (score <= currentLevel.maxScore)
					Exit();
				else
				{
					GameState = Serv.GameState.GameStateLevelEnd;
					StartNextLevel();
				}
			}
		}

		private void StartNextLevel()
		{
			curLevelNumber++;
			currentLevel = LevelGenerator.GetLevel(curLevelNumber);
			blocksGrid.Restart();
			GameState = Serv.GameState.GameStateRunning;
			timer = currentLevel.time;
		}

		#region Start game
		static bool isUnitTest = true;

		public static void StartGame()
		{
			isUnitTest = false;
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
