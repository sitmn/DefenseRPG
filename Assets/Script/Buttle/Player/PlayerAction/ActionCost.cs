using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

//アクションを実行するためのコスト
public class ActionCost : MonoBehaviour
{
    //現在のコスト
    [System.NonSerialized]
    public ReactiveProperty<int> _cost;
    private UIManager _UIManager;
    private SystemParam _systemParam;
    
    //クラスの初期化
    public void AwakeManager(SystemParam _systemParam, UIManager _UIManager){
        SetParam(_systemParam, _UIManager);
        CreateCostStream();
        CreateIncreaseCostStream();
    }

    //値の初期化
    private void SetParam(SystemParam _systemParam, UIManager _UIManager){
        this._cost = new ReactiveProperty<int>(_systemParam._initialActionCost);
        this._systemParam = _systemParam;
        this._UIManager = _UIManager;
    }

    //コストストリーム生成
    private void CreateCostStream(){
        _cost.Subscribe((x) => {
            DisplayCost();
        }).AddTo(this);
    }

    //コスト回復ストリーム生成
    private void CreateIncreaseCostStream(){
        Observable.Timer(System.TimeSpan.Zero, System.TimeSpan.FromSeconds(_systemParam._increaseActionCostTime))
            .Subscribe((_) => {
                IncreaseCrystalCost(_systemParam._increaseActionCost);
            }).AddTo(this);
    }

    //コストが足りているか
    public bool EnoughCrystalCost(int _consumeActionCost){
        return _cost.Value - _consumeActionCost >= 0;
    }
    //アクションによるコストの消費
    public void ConsumeCrystalCost(int _consumeActionCost){
        _cost.Value -= _consumeActionCost;
    }

    //コストを回復
    private void IncreaseCrystalCost(int _increaseCost){
        //コストの自動回復
        _cost.Value += _increaseCost;
        if(_cost.Value > _systemParam._maxCost) _cost.Value = _systemParam._maxCost;
    }

    //コスト画面表示
    private void DisplayCost(){
        _UIManager._costGauge.SetCostGaugeValue((float)_cost.Value / (float)_systemParam._maxCost);
    }
}
