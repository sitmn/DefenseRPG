using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

//アクションを実行するためのコスト
public class ActionCost : MonoBehaviour
{
    //現在のコスト
    [System.NonSerialized]
    public int _cost;
    //増えていくコスト量
    private int _increaseActionCost;
    //コストが増える時間
    private float _increaseActionCostTime;

    public void AwakeManager(SystemParam _systemParam){
        SetParam(_systemParam);
        CreateIncreaseCostStream();
    }

    //値の初期化
    private void SetParam(SystemParam _systemParam){
        this._cost = _systemParam._initialActionCost;
        this._increaseActionCost = _systemParam._increaseActionCost;
        this._increaseActionCostTime = _systemParam._increaseActionCostTime;
    }

    //コスト回復ストリーム生成
    private void CreateIncreaseCostStream(){
        Observable.Timer(System.TimeSpan.Zero, System.TimeSpan.FromSeconds(_increaseActionCostTime))
            .Subscribe((_) => {
                _cost += _increaseActionCost;
            }).AddTo(this);
    }

    //コストが足りているか
    public bool EnoughCrystalCost(int _consumeActionCost){
        Debug.Log(_cost + "-" + _consumeActionCost);
        return _cost - _consumeActionCost >= 0;
    }
    //アクションによるコストの消費
    public void ConsumeCrystalCost(int _consumeActionCost){
        _cost -= _consumeActionCost;
    }
}
