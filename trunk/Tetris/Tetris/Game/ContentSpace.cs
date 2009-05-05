using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;

namespace XnaTetris.Game
{
	class ContentSpace
	{
	  private static bool IsContentLoaded;

    private static readonly Rectangle srcMenuBtnGeneral = new Rectangle(0, 0, 150, 50);
    private static readonly Rectangle srcMenuBtnHilight = new Rectangle(150, 0, 150, 50);
    private static readonly Rectangle srcLevelWindowGeneral = new Rectangle(0, 0, 80, 40);
    private static readonly Rectangle srcLevelWindowHilight = new Rectangle(80, 0, 80, 40);


	  private static Dictionary<string, SpriteHelper> sprites = new Dictionary<string, SpriteHelper>();
    private static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

	  private ContentSpace()
    {
    }

    public static void LoadAllContent(ContentManager content)
    {
      if (!IsContentLoaded)
      {
        try
        {
          sprites.Add("SkyBackground", new SpriteHelper(content.Load<Texture2D>("skybackground"), null));
          sprites.Add("BackgroundSmallBox", new SpriteHelper(content.Load<Texture2D>("BackgroundSmallBox"), null));
          sprites.Add("BackgroundBigBox", new SpriteHelper(content.Load<Texture2D>("BackgroundBigBox"), null));
          sprites.Add("PauseButton", new SpriteHelper(content.Load<Texture2D>("PauseButton"), srcMenuBtnGeneral));
          sprites.Add("PauseHiButton", new SpriteHelper(content.Load<Texture2D>("PauseButton"), srcMenuBtnHilight));
          sprites.Add("ExitButton", new SpriteHelper(content.Load<Texture2D>("ExitButton"), srcMenuBtnGeneral));
          sprites.Add("ExitHiButton", new SpriteHelper(content.Load<Texture2D>("ExitButton"), srcMenuBtnHilight));
          sprites.Add("MenuButtonStart", new SpriteHelper(content.Load<Texture2D>("MenuButtonStart"), srcMenuBtnGeneral));
          sprites.Add("MenuHiButtonStart", new SpriteHelper(content.Load<Texture2D>("MenuButtonStart"), srcMenuBtnHilight));
          sprites.Add("MenuButtonExit", new SpriteHelper(content.Load<Texture2D>("MenuButtonExit"), srcMenuBtnGeneral));
          sprites.Add("MenuHiButtonExit",  new SpriteHelper(content.Load<Texture2D>("MenuButtonExit"), srcMenuBtnHilight));
          sprites.Add("MenuButtonHelp", new SpriteHelper(content.Load<Texture2D>("MenuButtonHelp"), srcMenuBtnGeneral));
          sprites.Add("MenuHiButtonHelp", new SpriteHelper(content.Load<Texture2D>("MenuButtonHelp"), srcMenuBtnHilight));
          sprites.Add("MenuButtonHiScore", new SpriteHelper(content.Load<Texture2D>("MenuButtonHiScore"), srcMenuBtnGeneral));
          sprites.Add("MenuHiButtonHiScore", new SpriteHelper(content.Load<Texture2D>("MenuButtonHiScore"), srcMenuBtnHilight));
          sprites.Add("MenuButtonAuthors", new SpriteHelper(content.Load<Texture2D>("MenuButtonAuthors"), srcMenuBtnGeneral));
          sprites.Add("MenuHiButtonAuthors", new SpriteHelper(content.Load<Texture2D>("MenuButtonAuthors"), srcMenuBtnHilight));
          
          sprites.Add("LevelBackground", new SpriteHelper(content.Load<Texture2D>("LevelBackground"), null));
          sprites.Add("MenuBackground", new SpriteHelper(content.Load<Texture2D>("MenuBackground"), null));
          sprites.Add("OkButton", new SpriteHelper(content.Load<Texture2D>("OkButton"), srcLevelWindowGeneral));
          sprites.Add("HiOkButton", new SpriteHelper(content.Load<Texture2D>("OkButton"), srcLevelWindowHilight));

          sprites.Add("WhiteBlock", new SpriteHelper(content.Load<Texture2D>("BlocksGeneral"), new Rectangle(0, 0, 64, 64)));
          sprites.Add("BlueBlock", new SpriteHelper(content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64, 0, 64, 64)));
          sprites.Add("GreenBlock", new SpriteHelper(content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64 * 2, 0, 64, 64)));
          sprites.Add("YellowBlock", new SpriteHelper(content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64 * 3, 0, 64, 64)));
          sprites.Add("RedBlock", new SpriteHelper(content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64 * 4, 0, 64, 64)));
          sprites.Add("GrayBlock", new SpriteHelper(content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64 * 5, 0, 64, 64)));

          sprites.Add("WhiteHiBlock", new SpriteHelper(content.Load<Texture2D>("BlocksHilight"), new Rectangle(0, 0, 64, 64)));
          sprites.Add("BlueHiBlock", new SpriteHelper(content.Load<Texture2D>("BlocksHilight"), new Rectangle(64, 0, 64, 64)));
          sprites.Add("GreenHiBlock", new SpriteHelper(content.Load<Texture2D>("BlocksHilight"), new Rectangle(64 * 2, 0, 64, 64)));
          sprites.Add("YellowHiBlock", new SpriteHelper(content.Load<Texture2D>("BlocksHilight"), new Rectangle(64 * 3, 0, 64, 64)));
          sprites.Add("RedHiBlock", new SpriteHelper(content.Load<Texture2D>("BlocksHilight"), new Rectangle(64 * 4, 0, 64, 64)));
          sprites.Add("GrayHiBlock", new SpriteHelper(content.Load<Texture2D>("BlocksHilight"), new Rectangle(64 * 5, 0, 64, 64)));

          sprites.Add("SelectionBlock", new SpriteHelper(content.Load<Texture2D>("Selection"), null));
          sprites.Add("HelpBlock", new SpriteHelper(content.Load<Texture2D>("HelpBlock"), null));
          sprites.Add("InvulBlock", new SpriteHelper(content.Load<Texture2D>("InvulBlock"), null));
          sprites.Add("NeutralBlock", new SpriteHelper(content.Load<Texture2D>("NeutralBlock"), null));

          sprites.Add("BlueBackground", new SpriteHelper(content.Load<Texture2D>("BlueBackground"), null));
          sprites.Add("RedBackground", new SpriteHelper(content.Load<Texture2D>("RedBackground"), null));
          sprites.Add("GreenBackground", new SpriteHelper(content.Load<Texture2D>("GreenBackground"), null));
          sprites.Add("YellowBackground", new SpriteHelper(content.Load<Texture2D>("YellowBackground"), null));
          sprites.Add("WhiteBackground", new SpriteHelper(content.Load<Texture2D>("WhiteBackground"), null));
          sprites.Add("GrayBackground", new SpriteHelper(content.Load<Texture2D>("GrayBackground"), null));

          sprites.Add("Caret", new SpriteHelper(content.Load<Texture2D>("Caret"), null));
          sprites.Add("TextBackground", new SpriteHelper(content.Load<Texture2D>("TextBackground"), null));

          // add fonts
          fonts.Add("NormalFont", content.Load<SpriteFont>("normalfont"));
          fonts.Add("BigFont", content.Load<SpriteFont>("bigfont"));
          fonts.Add("SmallFont", content.Load<SpriteFont>("smallfont"));
        }
        catch (Exception ex)
        {
          throw new ContentLoadException("Content do not loaded", ex);
        }
        IsContentLoaded = true;
      }
    }

    public static SpriteHelper GetSprite(string name)
    {
      if (sprites.ContainsKey(name))
        return sprites[name];
      throw new ContentLoadException("This sprite was not loaded");
    }

    public static SpriteFont GetFont(string name)
    {
      if (fonts.ContainsKey(name))
        return fonts[name];
      throw new ContentLoadException("This font was not loaded");
    }
	}
}
