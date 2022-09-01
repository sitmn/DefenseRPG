using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

//アクションを実行するためのコスト
public class ActionCost : MonoBehaviour
{
    /**画面表示テスト**/
    [SerializeField]
    private Text _text;

    //現在のコスト
    [System.NonSerialized]
    public ReactiveProperty<int> _cost;
    //増えていくコスト量
    private int _increaseActionCost;
    //コストが増える時間
    private float _increaseActionCostTime;

    public void AwakeManager(SystemParam _systemParam){
        SetParam(_systemParam);
        CreateCostStream();
        CreateIncreaseCostStream();
    }

    //値の初期化
    private void SetParam(SystemParam _systemParam){
        this._cost = new ReactiveProperty<int>(_systemParam._initialActionCost);
        this._increaseActionCost = _systemParam._increaseActionCost;
        this._increaseActionCostTime = _systemParam._increaseActionCostTime;
    }

    //コストストリーム生成
    private void CreateCostStream(){
        _cost.Subscribe((x) => {
            DisplayCost();
        }).AddTo(this);
    }

    //コスト回復ストリーム生成
    private void CreateIncreaseCostStream(){
        Observable.Timer(System.TimeSpan.Zero, System.TimeSpan.FromSeconds(_increaseActionCostTime))
            .Subscribe((_) => {
                IncreaseActionCost();
            }).AddTo(this);
    }

    //コストが足りているか
    public bool EnoughCrystalCost(int _consumeActionCost){
        Debug.Log(_cost.Value + "-" + _consumeActionCost);
        return _cost.Value - _consumeActionCost >= 0;
    }
    //アクションによるコストの消費
    public void ConsumeCrystalCost(int _consumeActionCost){
        _cost.Value -= _consumeActionCost;
    }

    //コストを自動回復
    private void IncreaseActionCost(){
        //コストの自動回復
        _cost.Value += _increaseActionCost;
    }

    //コスト画面表示
    private void DisplayCost(){
        _text.text = "Cost:" + _cost.Value;
    }
}
