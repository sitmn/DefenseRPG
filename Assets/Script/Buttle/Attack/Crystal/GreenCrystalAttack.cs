using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCrystalAttack : AttackBase
{
    // 攻撃対象をreturn
    protected override List<PlayerCore> DecideAttackTarget<PlayerCore>(Vector2Int _centerPos, Vector2Int _forwardDir, int _attackRange){
        List<PlayerCore> _applyCoreList = new List<PlayerCore>();
        _applyCoreList.AddRange(TargetCore.GetAroundCore<PlayerCore>(_centerPos, _forwardDir, _attackRange));
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
        if(typeof(T) == typeof(PlayerCore)) StatusUpDown.ApplySpeedBuff<T>(_applyCore, _attackStatus);
    }
}
