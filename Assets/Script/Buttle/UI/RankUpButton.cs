using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankUpButton : UIButtonBase
{
    void Awake(){
        _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
    }
}
