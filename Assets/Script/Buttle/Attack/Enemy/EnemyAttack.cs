using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : AttackBase
{
    // 攻撃対象をreturn
    protected override List<T> DecideAttackTarget<T>(Vector2Int _centerPos, Vector2Int _forwardDir, int _attackRange){
        List<T> _applyCoreList = new List<T>();
        _applyCoreList.AddRange(TargetCore.GetFowardCore<T>(_centerPos, _forwardDir, _attackRange));
        return _applyCoreList;
    }
    //ダメージ計算
    protected override int CalculateDamage(int _attack){
        int _damage = AttackDamage.NormalDamage(_attack);
        return _damage;
    }
    //ダメージ適用
    protected override void ApplyDamage<T>(T _applyCore, int _damage){
        if(typeof(T) == typeof(ACrystalCore)) ((ACrystalCore)(object)_applyCore).Hp -= _damage;
    }
    //効果適用
    protected override void ApplyEffect<T>(T _applyCore, AttackStatus _attackStatus){

    }
}
