using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaTetris.Helpers;

namespace XnaTetris.Game
{
	class ContentSpace
	{
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
	}
}
