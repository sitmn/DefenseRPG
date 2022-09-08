using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIButtonBase : MonoBehaviour
{
    protected CanvasGroup _canvasGroup;
    //UIを非透明状態に変更
    public void SetOpacityButton(){
        this._canvasGroup.alpha = 1f;
    }
    //UIを透明状態に変更
    public void SetTransparentButton(){
        this._canvasGroup.alpha = 0.5f;
    }
}
