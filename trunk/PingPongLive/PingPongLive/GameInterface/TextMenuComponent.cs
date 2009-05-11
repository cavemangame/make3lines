using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PingPongLive.GameInterface
{
  class TextMenuComponent : DrawableGameComponent
  {
    protected SpriteBatch spriteBatch = null;
    protected readonly SpriteFont regularFont, selectedFont;
    private readonly StringCollection menuItems;

    // Используется для поддержки ввода
    protected KeyboardState oldKeyboardState;

    // Для звуковых эффектов
    //protected AudioComponent audioComponent;


    #region Properties

    public int Width { get; private set; }
    public int Height { get; private set; }
    public int SelectedIndex { get; set; }
    public Color RegularColor { get; set; }
    public Color SelectedColor { get; set; }
    public Vector2 Position { get; set; }

    #endregion

    #region Constructor

    public TextMenuComponent(Game game, SpriteFont normalFont, SpriteFont selectedFont)
      : base(game)
    {
      RegularColor = Color.White;
      SelectedColor = Color.Red;

      regularFont = normalFont;
      this.selectedFont = selectedFont;
      menuItems = new StringCollection();

      spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

      // Получаем текущий audiocomponent и воспроизводим фоновую музыку
      //audioComponent = (AudioComponent)
      //Game.Services.GetService(typeof(AudioComponent));

      oldKeyboardState = Keyboard.GetState();
    }

    public TextMenuComponent(Game game) : base(game)
    {
    }

    #endregion

    public void SetMenuItems(string[] items)
    {
      menuItems.Clear();
      menuItems.AddRange(items);
      CalculateBounds();
    }

    protected void CalculateBounds()
    {
      Width = 0;
      Height = 0;

      foreach (string item in menuItems)
      {
        Vector2 size = selectedFont.MeasureString(item);

        if (size.X > Width)
        {
          Width = (int)size.X;
        }

        Height += selectedFont.LineSpacing;
      }
    }

    #region Draw
    public override void Draw(GameTime gameTime)
    {
      float y = Position.Y;

      for (int i = 0; i < menuItems.Count; i++)
      {
        SpriteFont font;
        Color theColor;

        if (i == SelectedIndex)
        {
          font = selectedFont;
          theColor = SelectedColor;
        }
        else
        {
          font = regularFont;
          theColor = RegularColor;
        }
        // Рисуем тень текста
        spriteBatch.DrawString(font, menuItems[i],
              new Vector2(Position.X + 1, y + 1), Color.Black);

        // Рисуем текстовый элемент
        spriteBatch.DrawString(font, menuItems[i],
              new Vector2(Position.X, y), theColor);
        y += font.LineSpacing;
      }
      base.Draw(gameTime);
    }
    #endregion

    #region Update
    public override void Update(GameTime gameTime)
    {
      KeyboardState keyboardState = Keyboard.GetState();
      bool down, up;

      down = (oldKeyboardState.IsKeyDown(Keys.Down) &&
             (keyboardState.IsKeyUp(Keys.Down)));
      up = (oldKeyboardState.IsKeyDown(Keys.Up) &&
           (keyboardState.IsKeyUp(Keys.Up)));

      if (down || up)
      {
        //audioComponent.PlayCue("menu_scroll");
      }
      if (down)
      {
        SelectedIndex++;
        if (SelectedIndex == menuItems.Count)
        {
          SelectedIndex = 0;
        }
      }
      if (up)
      {
        SelectedIndex--;
        if (SelectedIndex == -1)
        {
          SelectedIndex = menuItems.Count - 1;
        }
      }
      oldKeyboardState = keyboardState;

      base.Update(gameTime);
    }
    #endregion

  }
}
