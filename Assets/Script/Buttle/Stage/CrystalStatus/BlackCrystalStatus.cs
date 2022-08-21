using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCrystalStatus
{
    //初期ステータス
    public BlackCrystalStatus(){
        
    }

    // //配置時のクリスタル効果
    // public override void SetEffect(Vector2Int pos){
    //     if(!SetEffectCount()) return;

    //     Strength(pos);
    // }

    // //周囲エネミーにバフ
    // private void Strength(Vector2Int pos){
    //     List<AEnemyCore> _enemyList = AStarMap.AroundSearchAll(pos, _attackRange);
    //     for(int i = 0; i < _enemyList.Count ; i++){
    //         SpeedUp(_enemyList[i]);
    //     }
    // }

    // //移動速度減少効果
    // private void SpeedUp(AEnemyCore _enemyController){
    //     _enemyController.SpeedUp(_effectRate, _effectTime);
    // }
}
