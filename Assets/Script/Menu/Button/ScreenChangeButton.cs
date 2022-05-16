using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenChangeButton : ButtonBase
{
    //buttonNo:0で使用
    [SerializeField]
    private CanvasGroup nextScreen;
    public CanvasGroup NextScreen{
        get{return nextScreen;}
    }

    //buttonNo:1で使用　所持キャラリストの判別で使用
    private int characterPossessionNo;
    public int CharacterPossession{
        get{return characterPossessionNo;}
        set{characterPossessionNo = value;}
    }

    //buttonNo:1で使用　編成中キャラリストの判別で使用
    private int characterOrgNo;
    public int CharacterOrgNo{
        get{return characterOrgNo;}
        set{characterOrgNo = value;}
    }
}
