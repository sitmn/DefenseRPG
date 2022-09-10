using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchButton : UIButtonBase
{
    public void AwakeManager(){
        _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
    }
}
