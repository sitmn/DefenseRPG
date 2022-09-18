using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankUpButton : UIButtonBase
{
    public void AwakeManager(){
        _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        SetTransparentButton();
    }
}
