using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWindowButton : ButtonBase
{
    //buttonNo:1,2で使用
    [SerializeField]
    private CanvasGroup nextScreen;
    public CanvasGroup NextScreen{
        get{return nextScreen;}
    }
}
