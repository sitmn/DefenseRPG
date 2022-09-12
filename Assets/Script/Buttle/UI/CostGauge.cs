using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostGauge : MonoBehaviour
{
    //Costゲージ
    private Slider _costGauge;
    //Costゲージの初期化
    public void AwakeManager(SystemParam _systemParam){
        this._costGauge = this.gameObject.GetComponent<Slider>();
        SetCostGaugeValue((float)_systemParam._initialActionCost / (float)_systemParam._maxCost);
    }
    //Costゲージを更新
    public void SetCostGaugeValue(float _costRaito){
        _costGauge.value = _costRaito;
    }
}
