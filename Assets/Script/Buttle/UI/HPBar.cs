using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class HPBar : MonoBehaviour
{
    //HPのスライダー
    private Slider _hpSlider;
    [SerializeField]
    //HPバーをセットする対象のTransform
    private Transform _targetTr;
    private RectTransform _rect;

    public void AwakeManager(){
        this._rect = this.gameObject.GetComponent<RectTransform>();
        _hpSlider = this.gameObject.GetComponent<Slider>();
        SetHPBarValue(1);
        CreateMoveHPBarStream();
    }

    //画面移動に合わせてHPBarの移動
    private void CreateMoveHPBarStream(){
        this.UpdateAsObservable().Subscribe((_) => {
            SetPosition();
        }).AddTo(this);
    }

    //HPバーを対象の上部にセット
    public void SetPosition(){
        //if(MapManager.IsOutOfReference(MapManager.CastMapPos(this._targetTr.position))) return;

        Vector2 _pos = Camera.main.WorldToScreenPoint(this._targetTr.position + new Vector3(0, ConstManager._hpBarPosOffsetY, ConstManager._hpBarPosOffsetZ));
        this._rect.position = _pos;
        if(_rect.position.x < 100){
Debug.Log(_rect.position + "A");
        }
        
    }

    //HPバーを更新
    public void SetHPBarValue(float _hpRaito){
        _hpSlider.value = _hpRaito;
    }
}
