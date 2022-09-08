using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftUpButton : UIButtonBase
{
    void Awake(){
        _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
    }
}
