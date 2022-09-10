using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;

public class SelectLaunchButton : UIButtonBase{
    public void AwakeManager(){
        _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
    }
}
