using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCrystalAttack : IAttack
{
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

    //攻撃対象を決めて、攻撃と効果を反映
    public void DoAttack(Vector2Int _centerPos, Vector2Int _fowardDir, ACrystalStatus _crystalStatus /*int _attackRange, int _attack, int _effectTime*/){
        //攻撃対象を決める
        List<AEnemyCore> _attackTargetList = DecideAttackTarget<AEnemyCore>(_centerPos, _fowardDir, _crystalStatus._attackRange);
        Debug.Log("U");
        //攻撃と効果を反映
        foreach(var _attackTarget in _attackTargetList){
            Debug.Log("F");
            ApplyEffect<AEnemyCore>(_attackTarget, _crystalStatus);
        }
    }

    // 攻撃対象をreturn
    public List<T> DecideAttackTarget<T>(Vector2Int _centerPos, Vector2Int _forwardDir, int _attackRange){
        List<T> _applyCoreList = new List<T>();
        _applyCoreList.AddRange(AStarMap.GetAroundCoreAll<T>(_centerPos, _forwardDir, _attackRange));
        Debug.Log(_attackRange);
        return _applyCoreList;
    }
    //ダメージ計算
    public int CalculateDamage(int _attack){
        
        return _attack;
    }
    //ダメージ適用
    public void ApplyDamage<T>(T _applyCore, int _damage){
        if(typeof(T) == typeof(AEnemyCore)) ((AEnemyCore)(object)_applyCore).Hp -= _damage;
        if(typeof(T) == typeof(ACrystalCore)) ((ACrystalCore)(object)_applyCore).Hp -= _damage;
    }
    //効果適用
    public void ApplyEffect<T>(T _applyCore, ACrystalStatus _crystalStatus){
        Debug.Log("RRR");
        if(typeof(T) == typeof(AEnemyCore)) ((AEnemyCore)(object)_applyCore).SpeedUp(_crystalStatus._effectRate, _crystalStatus._effectTime);
    }
}
