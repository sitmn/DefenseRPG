using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCrystalStatus : ACrystalStatus
{
    [SerializeField]
    private int _attack = 1;
    [SerializeField]
    private int _attackRange = 5;
    [SerializeField]
    private float _speedDecreaseRate = 0.5f;
    [SerializeField]
    private int _effectTime = 3;

    //配置時のクリスタル効果
    public override void SetEffect(Vector2Int pos, int _effectMaxCount){
        if(!SetEffectCount(_effectMaxCount)) return;

        Attack(pos);
    }

    //周囲に攻撃
    private void Attack(Vector2Int pos){
        List<IStageObject> _enemyList = AStarMap.AroundSearchAll(pos, _attackRange);
        for(int i = 0; i < _enemyList.Count ; i++){
            _enemyList[i]._hp.Value -= _attack;
            SpeedDecrease(_enemyList[i]);
        }
    }

    //移動速度減少効果
    private void SpeedDecrease(IStageObject _enemyController){
        _enemyController.SpeedDown(_speedDecreaseRate, _effectTime);
    }
}
