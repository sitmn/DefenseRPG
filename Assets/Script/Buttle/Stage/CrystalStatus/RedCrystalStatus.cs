using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCrystalStatus : ACrystalStatus
{
    //配置時のクリスタル効果
    public override void SetEffect(Vector2Int pos){
        if(!SetEffectCount()) return;

        Attack(pos);
    }

    //周囲に攻撃
    private void Attack(Vector2Int pos){
        List<IStageObject> _enemyList = AStarMap.AroundSearch(pos, _attackRange);
        for(int i = 0; i < _enemyList.Count ; i++){
            _enemyList[i]._hp.Value -= _attack;
        }
    }
}
