using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XnaTetris.Game
{
  /// <summary>
  /// Parent class for all game scenes
  /// </summary>
  public class GameScene : DrawableGameComponent
  {
    protected SpriteBatch spriteBatch;

    // components can't have child components by default. Emulate it!
    public List<GameComponent> Components { get; private set; }

    public GameScene(Microsoft.Xna.Framework.Game game)
      : base(game)
    {
      spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); 
      Components = new List<GameComponent>();
      Hide();
    }

    public override void Initialize()
    {
      base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
      // Refresh all child components (if Enabled)
      foreach (GameComponent component in Components)
      {
        if (component.Enabled)
        {
          component.Update(gameTime);
        }
      }

      base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      // Draw all child components (if Visible)
      foreach (GameComponent component in Components)
      {
        DrawableGameComponent drawableComponent = component as DrawableGameComponent;

        if (drawableComponent != null && drawableComponent.Visible)
        {
          drawableComponent.Draw(gameTime);
        }
      }

      base.Draw(gameTime);
    }

    public void Show()
    {
      Visible = true;
      Enabled = true;
    }

    public void Hide()
    {
      Visible = false;
      Enabled = false;
    }
  }
}
