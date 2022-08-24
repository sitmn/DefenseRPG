using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBase
{
    //攻撃を実行 TはEnemyCoreBaseかCrystalCoreBase
    public void DoAttack<T>(Vector2Int _centerPos, Vector2Int _fowardDir, AttackStatus _attackStatus){
        //攻撃対象を決める
        List<T> _attackTargetList = DecideAttackTarget<T>(_centerPos, _fowardDir, _attackStatus._attackRange);
        int _damage = CalculateDamage(_attackStatus._attack);
        //攻撃と効果を反映
        foreach(var _attackTarget in _attackTargetList){
            ApplyDamage<T>(_attackTarget, _damage);
            ApplyEffect<T>(_attackTarget, _attackStatus);
        }
    }
    // 攻撃対象をreturn
    protected abstract List<T> DecideAttackTarget<T>(Vector2Int _centerPos, Vector2Int _forwardDir, int _attackRange);
    //ダメージ計算
    protected abstract int CalculateDamage(int _attack);
    //ダメージ適用
    protected abstract void ApplyDamage<T>(T _applyCore, int _damage);
    //効果適用
    protected abstract void ApplyEffect<T>(T _applyCore, AttackStatus _attackStatus);
}

public struct AttackStatus{
    //攻撃力
    public int _attack;
    //攻撃範囲
    public int _attackRange;
    //特殊効果倍率
    public float _effectRate;
    //特殊効果時間
    public int _effectTime;

    public AttackStatus(int _attack, int _attackRange, float _effectRate, int _effectTime){
        this._attack = _attack;
        this._attackRange = _attackRange;
        this._effectRate = _effectRate;
        this._effectTime = _effectTime;
    }
}