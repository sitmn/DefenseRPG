using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        SetPosition();
    }

    //HPバーを対象の下部にセット
    public void SetPosition(){
        Vector2 _pos = Camera.main.WorldToScreenPoint(this._targetTr.position + new Vector3(0, ConstManager._hpBarPosOffsetY, ConstManager._hpBarPosOffsetZ));
        //　カメラと同じ向きに設定
        //transform.rotation = Camera.main.transform.rotation;
        //_pos.y -= Screen.height / 10;
        this._rect.position = _pos;
    }

    //HPバーを更新
    public void SetHPBarValue(float _hpRaito){
        _hpSlider.value = _hpRaito;
    }
}
