using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace XnaTetris.Game
{
  /// <summary>
  /// Parent class for all game scenes
  /// </summary>
  public class GameScene : DrawableGameComponent
  {
    // components can't have child components by default. Emulate it!
    public List<GameComponent> Components
    {
      get; private set;
    }


    public GameScene(Microsoft.Xna.Framework.Game game)
      : base(game)
    {
      Components = new List<GameComponent>();

      Visible = false;
      Enabled = false;
    }

    public override void Initialize()
    {
      base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
      // Refresh all child components (if Enabled)
      for (int i = 0; i < Components.Count; i++)
      {
        if (Components[i].Enabled)
        {
          Components[i].Update(gameTime);
        }
      }

      base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      // Draw all child components (if visible)
      for (int i = 0; i < Components.Count; i++)
      {
        GameComponent gc = Components[i];

        if ((gc is DrawableGameComponent) && ((DrawableGameComponent) gc).Visible)
        {
          ((DrawableGameComponent) gc).Draw(gameTime);
        }
      }

      base.Draw(gameTime);
    }

    public virtual void Show()
    {
      Visible = true;
      Enabled = true;
    }

    public virtual void Hide()
    {
      Visible = false;
      Enabled = false;
    }

  }
}