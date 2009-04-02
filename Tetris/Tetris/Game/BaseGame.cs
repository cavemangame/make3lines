// Project: XnaTetris, File: BaseGame.cs
// Namespace: XnaTetris.Game, Class: BaseGame
// Path: C:\code\XnaBook\XnaTetris\Game, Author: Abi
// Code lines: 16, Size of file: 298 Bytes
// Creation date: 20.11.2006 14:03
// Last modified: 20.11.2006 15:22
// Generated with Commenter by abi.exDream.com

#region Using directive
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using XnaTetris.Sounds;
using XnaTetris.Graphics;
using XnaTetris.Helpers;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace XnaTetris.Game
{
	/// <summary>
	/// Base game class for all the basic game support.
	/// Connects all our helper classes together and makes our live easier!
	/// </summary>
	public class BaseGame : Microsoft.Xna.Framework.Game
	{
		#region Variables
		protected GraphicsDeviceManager graphics;
		protected ContentManager content;

		/// <summary>
		/// Resolution of our game.
		/// </summary>
		protected static int width, height;

		/// <summary>
		/// Font for rendering text
		/// </summary>
		TextureFont font = null;
		#endregion

		#region Properties
		public static int Width
		{
			get
			{
				return width;
			} // get
		} // Width

		public static int Height
		{
			get
			{
				return height;
			} // get
		} // Height
		#endregion

		#region Constructor
		public BaseGame()
		{
			graphics = new GraphicsDeviceManager(this);
			content = new ContentManager(Services);
		} // BaseGame

		protected override void Initialize()
		{
			// Remember resolution
			width = graphics.GraphicsDevice.Viewport.Width;
			height = graphics.GraphicsDevice.Viewport.Height;

			base.Initialize();
		} // Initialize()

		/// <summary>
		/// Load all graphics content (just our background texture).
		/// Use this method to make sure a device reset event is handled correctly.
		/// </summary>
		/// <param name="loadAllContent">Load everything?</param>
		protected override void LoadGraphicsContent(bool loadAllContent)
		{
			if (loadAllContent)
			{
				// Create font
				font = new TextureFont(graphics.GraphicsDevice, content);
			} // if
			base.LoadGraphicsContent(loadAllContent);
		} // LoadGraphicsContent(loadAllContent)

		/// <summary>
		/// Unload graphic content if the device gets lost.
		/// </summary>
		/// <param name="unloadAllContent">Unload everything?</param>
		protected override void UnloadGraphicsContent(bool unloadAllContent)
		{
			if (unloadAllContent == true)
			{
				content.Unload();
				SpriteHelper.Dispose();
			} // if

			base.UnloadGraphicsContent(unloadAllContent);
		} // UnloadGraphicsContent(loadAllContent)
		#endregion

		#region Update
		protected override void Update(GameTime gameTime)
		{
			Sound.Update();

			Input.Update();

			if (Input.KeyboardEscapeJustPressed ||
				Input.GamePadBackJustPressed)
				this.Exit();
			
			base.Update(gameTime);
		} // Update(gameTime)
		#endregion

		#region Draw
		protected override void Draw(GameTime gameTime)
		{
			// Draw all sprites and fonts
			SpriteHelper.DrawSprites(width, height);
			font.WriteAll();

			base.Draw(gameTime);
		} // Draw(gameTime)
		#endregion
	} // class BaseGame
} // namespace XnaTetris.Game
