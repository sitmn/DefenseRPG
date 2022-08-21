using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : IAttack
{
    //攻撃対象を決めて、攻撃と効果を反映
    public void DoAttack(Vector2Int _centerPos, Vector2Int _fowardDir, AttackStatus _enemyStatus /*int _attackRange, int _attack, int _effectTime*/){
        //攻撃対象を決める
        List<ACrystalCore> _attackTargetList = DecideAttackTarget<ACrystalCore>(_centerPos, _fowardDir, _enemyStatus._attackRange);
        int _damage = CalculateDamage(_enemyStatus._attack);
        //攻撃と効果を反映
        foreach(var _attackTarget in _attackTargetList){
            ApplyDamage<ACrystalCore>(_attackTarget, _damage);
            ApplyEffect<ACrystalCore>(_attackTarget, _enemyStatus);
        }
    }

    // 攻撃対象をreturn
    public List<T> DecideAttackTarget<T>(Vector2Int _centerPos, Vector2Int _forwardDir, int _attackRange){
        List<T> _applyCoreList = new List<T>();
        _applyCoreList.AddRange(AStarMap.GetFowardCore<T>(_centerPos, _forwardDir, _attackRange));
        return _applyCoreList;
    }
    //ダメージ計算
    public int CalculateDamage(int _attack){
        
        return _attack;
    }
    //ダメージ適用
    public void ApplyDamage<T>(T _applyCore, int _damage){
        if(typeof(T) == typeof(ACrystalCore)) ((ACrystalCore)(object)_applyCore).Hp -= _damage;
    }
    //効果適用
    public void ApplyEffect<T>(T _applyCore, AttackStatus _enemyStatus){

    }
}
