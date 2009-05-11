using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace PingPongLive.GameInterface
{
  /// <summary>
  /// Parent class for all game scenes
  /// </summary>
  public class GameScene : DrawableGameComponent
  {
    // components can't have child components by default. Emulate it!
    public List<GameComponent> Components { get; private set; }

    public GameScene(Game game)
      : base(game)
    {
      Components = new List<GameComponent>();
      Hide();
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
