using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;

namespace XnaTetris.Helpers
{

  public class SpriteToRender
  {
    public SpriteHelper Sprite { get; private set; }
    public Rectangle Rect { get; private set; }

    public SpriteToRender(SpriteHelper sprite, Rectangle rect)
    {
      Sprite = sprite;
      Rect = rect;
    }
  }

  public class TextToRender
  {
    public SpriteFont Font { get; private set; }
    public Vector2 Pos { get; private set; }
    public float Scale { get; private set; }
    public Color TextColor { get; private set; }
    public string Text { get; private set; }

    public TextToRender(SpriteFont font, Vector2 pos, float scale, Color color, string text)
    {
      Font = font;
      Pos = pos;
      Scale = scale;
      TextColor = color;
      Text = text;
    }
  }

  /// <summary>
  /// Convert tagged xml text to SpriteBatch
  /// </summary>
  public class ConvertTaggedTextHelper
  {
    #region Constants

    private const string textName = "text";
    private const string imageName = "image";
    private const int elememtVertSpan = 20; // расстояние м/у "строками"
    private const int elememtLeftSpan = 20; // расстояние м/у элементами, в частности, отступ от левого края
    private const int defaultWidth = 64;
    private const int defaultHeight = 64;

    #endregion

    #region Variables

    private Rectangle boundingRect;
    private readonly ContentManager content;

    private int curX, curY; // текущая позиция "курсора"

    public List<SpriteToRender> Sprites { get; private set;}
    public List<TextToRender> Texts { get; private set; }

    #endregion

    #region Constructor

    public ConvertTaggedTextHelper(ContentManager setContent, Rectangle setRect)
    {
      boundingRect = setRect;
      content = setContent;
    }

    #endregion

    public void ConvertTaggedText(XmlDocument doc)
    {
      if (doc == null || doc.DocumentElement == null)
        throw new ArgumentNullException("doc");

      Sprites.Clear();
      Texts.Clear();

      // all nodes in one upper level
      foreach (XmlNode node in doc.DocumentElement.ChildNodes)
      {
        ParseNode(node);
      }
    }

    private void ParseNode(XmlNode node)
    {
      if (0 == String.CompareOrdinal(node.Name, textName))
        ParseNodeAsText(node);
      else if (0 == String.CompareOrdinal(node.Name, imageName))
        ParseNodeAsImage(node);
    }

    private void ParseNodeAsText(XmlNode node)
    {
      bool wrap = false;
      string text = node.Value;
      if (text.Length == 0)
        return;

      SpriteFont font = null;
      float scale = 1.0f;
      Vector2 pos = new Vector2(curX + elememtLeftSpan, curY);
      Color color = Color.White;

     // - name - название шрифта из папки content (сейчас bigfont, normalfont, smallfont)
		 // - scale - растяжение/сжатие текста относительно обычного размера
		 // - color - цвет
		 // - left - расстояние от левого края диалога
		 // - top - расстояние от предыдущего элемента (строки или текста) диалога
		 // - wrap - false - значит этот текст не начинается со следующей строки, 
     // а продолжается за предыдущим элементом
      foreach (XmlAttribute attr in node.Attributes)
      {
        switch (attr.Name)
        {
          case "name":
            {
              font = ContentSpace.GetFont(attr.Value);
              break;
            }
          case "scale":
            {
              scale = Convert.ToInt32(attr.Value);
              break;
            }
          case "color":
            {
              color = (Color)EnumHelper.SearchEnumerator(typeof(Color), attr.Value);
              break;
            }
          case "left":
            {
              curX += Convert.ToInt32(attr.Value);
              pos.X = curX;
              break;
            }
          case "top":
            {
              curY += Convert.ToInt32(attr.Value);
              pos.Y = curY;
              break;
            }
          case "wrap":
            {
              wrap = Convert.ToBoolean(attr.Value);
              break;
            }
          default:
            break;
        }
      }

      //TODO: сделать перенос строк
      if (wrap)
      {
        pos.X = elememtLeftSpan;
        curX = (int) pos.X;
        pos.Y = (curY + elememtVertSpan);
        curY = (int)pos.Y;
      }

      Texts.Add(new TextToRender(font, pos, scale, color, text));
    }

    private void ParseNodeAsImage(XmlNode node)
    {
      //- left, top, width, height, wrap 
      bool wrap = false;
      string contentName = node.Value;
      if (contentName.Length == 0)
        return;


      SpriteHelper sprite = ContentSpace.GetSprite(contentName);

      // умолчательные значения прямоугольника картинки
      Rectangle rect = new Rectangle(curX + elememtLeftSpan, curY, defaultWidth, defaultHeight);

      foreach (XmlAttribute attr in node.Attributes)
      {
        switch (attr.Name)
        {
          case "width":
            {
              rect.Width = Convert.ToInt32(attr.Value);
              break;
            }
          case "height":
            {
              rect.Height = Convert.ToInt32(attr.Value);
              break;
            }
          case "left":
            {
              curX += Convert.ToInt32(attr.Value);
              rect.X = curX;
              break;
            }
          case "top":
            {
              curY += Convert.ToInt32(attr.Value);
              rect.Y = curY;
              break;
            }
          case "wrap":
            {
              wrap = Convert.ToBoolean(attr.Value);
              break;
            }
          default :
            break;
        }
      }

      // коли картинка не влезает, либо так задумано - переносим ее на следующую строку
      if (wrap || (curX + rect.Width > boundingRect.Right)) 
      {
        curX = rect.X = elememtLeftSpan;
        curY = rect.Y = (curY + elememtVertSpan);
      }

      Sprites.Add(new SpriteToRender(sprite, rect));
    }
  }
}
