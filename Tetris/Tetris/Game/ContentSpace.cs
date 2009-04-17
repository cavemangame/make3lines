using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;

namespace XnaTetris.Game
{
	class ContentSpace
	{
	  private static bool IsContentLoaded = false;

    private static readonly Rectangle srcMenuBtnGeneral = new Rectangle(0, 0, 200, 50);
    private static readonly Rectangle srcMenuBtnHilight = new Rectangle(200, 0, 200, 50);
    private static readonly Rectangle srcLevelWindowGeneral = new Rectangle(0, 0, 80, 40);
    private static readonly Rectangle srcLevelWindowHilight = new Rectangle(80, 0, 80, 40);

    public static SpriteHelper menuButtonStart, menuButtonExit, menuButtonHelp, menuButtonHiScore, 
      menuButtonAuthors;

    public static SpriteHelper menuHiButtonStart, menuHiButtonExit, menuHiButtonHelp, menuHiButtonHiScore,
      menuHiButtonAuthors;

    public static SpriteHelper greenBlock, redBlock, blueBlock, yellowBlock, blackBlock, someBlock,
      selectionBlock, helpBlock;

    public static SpriteHelper greenHiBlock, redHiBlock, blueHiBlock, yellowHiBlock, blackHiBlock, someHiBlock;

    public static SpriteHelper background, backgroundSmallBox, backgroundBigBox, buttonPause, buttonExit;
    public static SpriteHelper buttonHiLevelDialogOk, buttonLevelDialogOk;

    public static SpriteHelper levelDialogBackground;

	  private ContentSpace()
    {
    }

    public static void LoadAllContent(ContentManager content)
    {
      if (!IsContentLoaded)
      {
        try
        {
          background = new SpriteHelper(content.Load<Texture2D>("skybackground"), null);
          backgroundSmallBox = new SpriteHelper(content.Load<Texture2D>("BackgroundSmallBox"), null);
          backgroundBigBox = new SpriteHelper(content.Load<Texture2D>("BackgroundBigBox"), null);
          buttonPause = new SpriteHelper(content.Load<Texture2D>("PauseButton"), null);
          buttonExit = new SpriteHelper(content.Load<Texture2D>("ExitButton"), null);
          menuButtonStart = new SpriteHelper(content.Load<Texture2D>("MenuButtonStart"), srcMenuBtnGeneral);
          menuHiButtonStart = new SpriteHelper(content.Load<Texture2D>("MenuButtonStart"), srcMenuBtnHilight);
          menuButtonExit = new SpriteHelper(content.Load<Texture2D>("MenuButtonExit"), srcMenuBtnGeneral);
          menuHiButtonExit = new SpriteHelper(content.Load<Texture2D>("MenuButtonExit"), srcMenuBtnHilight);
          menuButtonHelp = new SpriteHelper(content.Load<Texture2D>("MenuButtonHelp"), srcMenuBtnGeneral);
          menuHiButtonHelp = new SpriteHelper(content.Load<Texture2D>("MenuButtonHelp"), srcMenuBtnHilight);
          menuButtonHiScore = new SpriteHelper(content.Load<Texture2D>("MenuButtonHiScore"), srcMenuBtnGeneral);
          menuHiButtonHiScore = new SpriteHelper(content.Load<Texture2D>("MenuButtonHiScore"), srcMenuBtnHilight);
          menuButtonAuthors = new SpriteHelper(content.Load<Texture2D>("MenuButtonAuthors"), srcMenuBtnGeneral);
          menuHiButtonAuthors = new SpriteHelper(content.Load<Texture2D>("MenuButtonAuthors"), srcMenuBtnHilight);
          levelDialogBackground = new SpriteHelper(content.Load<Texture2D>("LevelBackground"), null);
          buttonLevelDialogOk = new SpriteHelper(content.Load<Texture2D>("LevelOkButton"), srcLevelWindowGeneral);
          buttonHiLevelDialogOk = new SpriteHelper(content.Load<Texture2D>("LevelOkButton"), srcLevelWindowHilight);

          blackBlock = new SpriteHelper(content.Load<Texture2D>("BlocksGeneral"), new Rectangle(0, 0, 64, 64));
          blueBlock = new SpriteHelper(content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64, 0, 64, 64));
          greenBlock = new SpriteHelper(content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64 * 2, 0, 64, 64));
          yellowBlock = new SpriteHelper(content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64 * 3, 0, 64, 64));
          redBlock = new SpriteHelper(content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64 * 4, 0, 64, 64));
          someBlock = new SpriteHelper(content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64 * 5, 0, 64, 64));

          blackHiBlock = new SpriteHelper(content.Load<Texture2D>("BlocksHilight"), new Rectangle(0, 0, 64, 64));
          blueHiBlock = new SpriteHelper(content.Load<Texture2D>("BlocksHilight"), new Rectangle(64, 0, 64, 64));
          greenHiBlock = new SpriteHelper(content.Load<Texture2D>("BlocksHilight"), new Rectangle(64 * 2, 0, 64, 64));
          yellowHiBlock = new SpriteHelper(content.Load<Texture2D>("BlocksHilight"), new Rectangle(64 * 3, 0, 64, 64));
          redHiBlock = new SpriteHelper(content.Load<Texture2D>("BlocksHilight"), new Rectangle(64 * 4, 0, 64, 64));
          someHiBlock = new SpriteHelper(content.Load<Texture2D>("BlocksHilight"), new Rectangle(64 * 5, 0, 64, 64));

          selectionBlock = new SpriteHelper(content.Load<Texture2D>("Selection"), null);
          helpBlock = new SpriteHelper(content.Load<Texture2D>("HelpBlock"), null);
        }
        catch (Exception ex)
        {
          throw new ContentLoadException("Content do not loaded", ex);
        }
      }
    }
	}
}
