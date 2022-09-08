using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ActionGauge : MonoBehaviour
{
    private Transform _playerTr;
    private RectTransform _rect;
    private Image _actionGauge;
    //アクションゲージ用のシーケンス
    Sequence _actionSequence;
    public void AwakeManager(Transform _playerTr){
        this._rect = this.gameObject.GetComponent<RectTransform>();
        this._playerTr = _playerTr;
        this._actionGauge = this.gameObject.GetComponent<Image>();
    }

    //アクションゲージのTweenをセット
    public void SetTween(float _actionTime){
        //アクションゲージの位置をセット
        SetPosition();
        //アクションゲージのTweenをスタート
        StartTween(_actionTime);
    }

    //プレイヤー頭上位置をセット
    private void SetPosition(){
        Vector2 _pos = Camera.main.WorldToScreenPoint(this._playerTr.position);
        //　カメラと同じ向きに設定
        //transform.rotation = Camera.main.transform.rotation;
        _pos.y += Screen.height / 10;
        this._rect.position = _pos;
    }

    //アクションゲージのTweenをスタート
    private void StartTween(float _actionTime){
        this._actionGauge.fillAmount = 0;
        this._actionSequence = DOTween.Sequence();
        this._actionSequence.Append(_actionGauge.DOFillAmount(1, _actionTime))
        .AppendInterval(0.1f)
        .AppendCallback(() => this._actionGauge.fillAmount = 0)
        .SetLink(this.gameObject)
        .Play();
    }

    //アクションゲージのTweenをキャンセル
    public void CancelTween(){
        this._actionSequence.Kill();
        this._actionGauge.fillAmount = 0;
    }
}
