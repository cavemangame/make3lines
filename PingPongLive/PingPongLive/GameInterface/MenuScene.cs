using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PingPongLive.Helpers;

namespace PingPongLive.GameInterface
{
  class MenuScene : GameScene
  {
    protected TextMenuComponent menu;
    private SpriteBatch spriteBatch;

    public int SelectedMenuIndex { get { return menu.SelectedIndex; } }

    public MenuScene(Game game, SpriteFont smallFont, SpriteFont largeFont, Texture2D background)
      : base(game)
    {
      Components.Add(new ImageComponent(game, background, DrawMode.Center, null));
      // Создаем меню
      string[] items = { "One Player", "Two Players", "Network Game", "Help", "Quit" };
      menu = new TextMenuComponent(game, smallFont, largeFont);
      menu.Position = new Vector2(400, 300);
      menu.SetMenuItems(items);
      Components.Add(menu);

      // Получаем текущий spritebatch
      spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
    }
  }
}
