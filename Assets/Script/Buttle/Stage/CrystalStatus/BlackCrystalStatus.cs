using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCrystalStatus : ACrystalStatus
{
    [SerializeField]
    private int _attack = 0;
    [SerializeField]
    private int _attackRange = 3;
    [SerializeField]
    private float _speedUpRate = 0.1f;
    [SerializeField]
    private int _effectTime = 3;

    public BlackCrystalStatus(){
        _effectMaxCount = 50;
    }

    //配置時のクリスタル効果
    public override void SetEffect(Vector2Int pos){
        if(!SetEffectCount()) return;

        Strength(pos);
    }

    //周囲エネミーにバフ
    private void Strength(Vector2Int pos){
        List<IStageObject> _enemyList = AStarMap.AroundSearchAll(pos, _attackRange);
        for(int i = 0; i < _enemyList.Count ; i++){
            SpeedUp(_enemyList[i]);
        }
    }

    //移動速度減少効果
    private void SpeedUp(IStageObject _enemyController){
        _enemyController.SpeedUp(_speedUpRate, _effectTime);
    }
}
