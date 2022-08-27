using System.Collections.Generic;
using UnityEngine;

public class BlackCrystalAttack : AttackBase
{
    public override void Attack(Vector2Int _centerPos, Vector2Int _fowardDir, AttackStatus _attackStatus)
    {
        DoAttack<EnemyCoreBase>(_centerPos, _fowardDir, _attackStatus);
    }

    // 攻撃対象をreturn
    protected override List<T> DecideAttackTarget<T>(Vector2Int _centerPos, Vector2Int _forwardDir, int _attackRange){
        List<T> _applyCoreList = new List<T>();
        _applyCoreList.AddRange(TargetCore.GetAroundCoreAll<T>(_centerPos, _forwardDir, _attackRange));
        return _applyCoreList;
    }
    //ダメージ計算
    protected override int CalculateDamage(int _attack){
        
        return _attack;
    }
    //ダメージ適用
    protected override void ApplyDamage<T>(T _applyCore, int _damage){

    }
    //効果適用
    protected override void ApplyEffect<T>(T _applyCore, AttackStatus _attackStatus){
        if(typeof(T) == typeof(EnemyCoreBase)) StatusUpDown.ApplySpeedBuff<T>(_applyCore, _attackStatus);
    }
}
