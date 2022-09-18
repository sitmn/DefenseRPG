using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftDownButton : UIButtonBase
{
    public void AwakeManager(){
        _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        SetTransparentButton();
    }
}
