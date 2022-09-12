using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    //HPのスライダー
    private Slider _hpSlider;
    [SerializeField]
    //HPバーをセットする対象のTransform
    private Transform _targetTr;
    [SerializeField]
    //HPバーの座標
    private RectTransform _rect;
    //HPバーのゲームオブジェクト
    [SerializeField]
    private GameObject _hpBar;

    public void AwakeManager(){
        // this._rect = this.gameObject.GetComponent<RectTransform>();
        // _hpSlider = this.gameObject.GetComponent<Slider>();
        CreateMoveHPBarStream();
        SetHPBarValue(1);
    }

    //画面移動に合わせてHPBarの移動
    private void CreateMoveHPBarStream(){
        this.UpdateAsObservable().Subscribe((_) => {
            if(_hpBar.activeSelf == true) SetPosition();
        }).AddTo(this);
    }

    //HPバーを対象の上部にセット
    public void SetPosition(){
        Vector2 _pos = Camera.main.WorldToScreenPoint(this._targetTr.position + new Vector3(0, ConstManager._hpBarPosOffsetY, ConstManager._hpBarPosOffsetZ));
        this._rect.position = _pos;
    }

    //HPバーを更新
    public void SetHPBarValue(float _hpRaito){
        if(_hpRaito < 1){
            _hpBar.SetActive(true);
        }
        _hpSlider.value = _hpRaito;
        if(_hpRaito == 1){
            _hpBar.SetActive(false);
        }
    }
}
