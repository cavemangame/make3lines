using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PingPongLive.GameInterface
{
  public enum DrawMode
  {
    Center = 1,
    Stretch,
    Free,
  };

  class ImageComponent : DrawableGameComponent
  {
    protected readonly Texture2D texture;
    protected readonly DrawMode drawMode;
    protected SpriteBatch spriteBatch;
    protected Rectangle imageRect;


    public ImageComponent(Game game, Texture2D texture, DrawMode drawMode, Rectangle? rect) : base(game)
    {
      this.texture = texture;
      this.drawMode = drawMode;

      // Получаем текущий пакет спрайтов
      spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

      // Создаем прямоугольник с размерами и местоположением изображения
      switch (drawMode)
      {
        case DrawMode.Center:
          imageRect = new Rectangle((Game.Window.ClientBounds.Width -
                  texture.Width) / 2, (Game.Window.ClientBounds.Height -
                  texture.Height) / 2, texture.Width, texture.Height);
          break;
        case DrawMode.Stretch:
          imageRect = new Rectangle(0, 0, Game.Window.ClientBounds.Width,
                  Game.Window.ClientBounds.Height);
          break;
        case DrawMode.Free:
          {
            if (rect.HasValue)
              imageRect = rect.Value;
            else
              imageRect = new Rectangle(0, 0, Game.Window.ClientBounds.Width,
                  Game.Window.ClientBounds.Height);
          }
          break;

      }
    }

    public override void Draw(GameTime gameTime)
    {
      spriteBatch.Draw(texture, imageRect, Color.White);
      base.Draw(gameTime);
    }

  }
}
