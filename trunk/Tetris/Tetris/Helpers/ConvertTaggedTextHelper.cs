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

  public class ConvertTaggedTextHelper
  {
    #region Constants

    private const string textName = "text";
    private const string imageName = "image";
    private const int elememtVertSpan = 10; // расстояние м/у "строками"
    private const int elememtLeftSpan = 10; // расстояние м/у элементами, в частности, отступ от левого края
    private const int defaultWidth = 64;
    private const int defaultHeight = 64;

    #endregion

    #region Variables

    private Rectangle boundingRect;

    private int curX, curY, maxY; // текущая позиция "курсора"

    public List<SpriteToRender> Sprites { get; set;}
    public List<TextToRender> Texts { get; set; }

    #endregion

    #region Constructor

    public ConvertTaggedTextHelper(Rectangle setRect, XmlNode node)
    {
      boundingRect = setRect;
      curX = boundingRect.X;
      curY = boundingRect.Y;
      maxY = curY;
      Sprites = new List<SpriteToRender>();
      Texts = new List<TextToRender>();
      ConvertTaggedText(node);
    }

    public ConvertTaggedTextHelper(Rectangle setRect)
    {
      boundingRect = setRect;
      Sprites = new List<SpriteToRender>();
      Texts = new List<TextToRender>();
    }

    #endregion

    private void ConvertTaggedText(XmlNode loadNode)
    {
      Sprites.Clear();
      Texts.Clear();

      // all nodes in one upper level
      foreach (XmlNode node in loadNode.ChildNodes)
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
      bool wrap = true;
      int top = 0;
      int left = 0;
      string text = node.InnerText;
      if (text != null && text.Length == 0)
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
              scale = Convert.ToSingle(attr.Value);
              break;
            }
          case "color":
            {
              
              color = ColorHelper.ColorFromString(attr.Value);
              break;
            }
          case "left":
            {
              left = Convert.ToInt32(attr.Value);
              pos.X = curX;
              break;
            }
          case "top":
            {
              top += Convert.ToInt32(attr.Value);
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
        pos.X = elememtLeftSpan + boundingRect.X;
        curX = (int)pos.X;
        pos.Y = maxY + elememtVertSpan;
        curY = (int)pos.Y;
      }

      curX += left;
      curY += top;

      int curPos = 0;
      int textLen = text.Length;
      if (font == null)
        font = ContentSpace.GetFont("SmallFont");

      while (curPos < textLen)
      {
        if (font.MeasureString(text.Substring(0, ++curPos)).Length() * scale + curX > 
          boundingRect.Right-elememtLeftSpan)
        {
          Texts.Add(new TextToRender(font, new Vector2(curX, curY), scale, color, text.Substring(0, curPos)));
          curX = elememtLeftSpan + boundingRect.X;
          if (curY + (int) (font.MeasureString("A").Y * scale) > maxY)
            maxY = curY + (int) (font.MeasureString("A").Y * scale);
          curY = maxY + elememtVertSpan;
          text = text.Substring(curPos, textLen - curPos);
          textLen -= curPos;
          curPos = 0;
        }
      }
      if (text.Length > 0)
      {
        Texts.Add(new TextToRender(font, new Vector2(curX, curY), scale, color, text));
        curX += (int)(font.MeasureString(text).Length() * scale);
        if (curY + (int) (font.MeasureString("A").Y * scale) > maxY)
          maxY = curY + (int)(font.MeasureString("A").Y * scale);
      }
    }

    private void ParseNodeAsImage(XmlNode node)
    {
      //- left, top, width, height, wrap 
      bool wrap = true;
      int top = 0;
      int left = 0;

      string contentName = node.InnerText;
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
              left = Convert.ToInt32(attr.Value);
              rect.X = curX;
              break;
            }
          case "top":
            {
              top = Convert.ToInt32(attr.Value);
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
      if (wrap || (curX + rect.Width > boundingRect.Right - elememtLeftSpan)) 
      {
        curX = rect.X = elememtLeftSpan + boundingRect.X;
        curY = rect.Y = (maxY + elememtVertSpan);
      }

      curX += left;
      curY += top;

      if (curY + rect.Height > maxY)
        maxY = curY + rect.Height;

      Sprites.Add(new SpriteToRender(sprite, 
        new Rectangle(curX, curY, rect.Width, rect.Height)));

      curX += rect.Width;
    }
  }
}
