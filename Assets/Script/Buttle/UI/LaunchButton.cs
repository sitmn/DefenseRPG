using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchButton : UIButtonBase
{
    void Awake(){
        _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
    }
}
