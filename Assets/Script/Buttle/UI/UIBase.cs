using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// 戦闘画面のUIボタンの抽象クラス
/// </summary>
public abstract class UIButtonBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// アルファ値変更用
    /// </summary>
    protected CanvasGroup _canvasGroup;
    /// <summary>
    /// ボタンサイズ初期値
    /// </summary>
    protected Vector3 _baseScale;

    /// <summary>
    /// クラスの初期化
    /// </summary>
    public void AwakeManager(){
        _baseScale = transform.localScale;

        _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        SetTransparentButton();
    }
    
    /// <summary>
    /// UIを非透明状態に変更
    /// </summary>
    public void SetOpacityButton(){
        this._canvasGroup.alpha = 1f;
    }
    /// <summary>
    /// UIを透明状態に変更
    /// </summary>
    public void SetTransparentButton(){
        this._canvasGroup.alpha = ConstManager._disableButtonAlpha;
    }

    /// <summary>
    /// ボタンを押したとき実行されるメソッド、ボタンを縮小する
    /// </summary>
    /// <param name="eventData">未使用</param>
    public void OnPointerDown(PointerEventData eventData){
        ChangeButtonScale(ConstManager._buttonShrinkRate, ConstManager._buttonscalingTime);
    }

    /// <summary>
    /// ボタンを離したとき実行されるメソッド、ボタンの縮小を戻す
    /// </summary>
    /// <param name="eventData">未使用</param>
    public void OnPointerUp(PointerEventData eventData){
        ChangeButtonScale(1, ConstManager._buttonscalingTime);
    }

    /// <summary>
    /// ボタンの拡大縮小メソッド
    /// </summary>
    /// <param name="_afterScaleRate">ボタン拡大縮小後のスケール倍率</param>
    /// <param name="_scalingTime">ボタン拡大縮小の開始から終了の時間</param>
    public void ChangeButtonScale(float _afterScaleRate, float _scalingTime){
        transform.DOScale(_baseScale * _afterScaleRate, _scalingTime)
            .SetEase(Ease.OutBounce)
            .Play();
    }
}
