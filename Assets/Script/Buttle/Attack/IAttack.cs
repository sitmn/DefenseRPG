using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    //攻撃を実行
    void DoAttack(Vector2Int _centerPos, Vector2Int _fowardDir, AttackStatus _attackStatus);
    // 攻撃対象をreturn
    List<T> DecideAttackTarget<T>(Vector2Int _centerPos, Vector2Int _forwardDir, int _attackRange);
    //ダメージ計算
    int CalculateDamage(int _attack);
    //ダメージ適用
    void ApplyDamage<T>(T _applyCore, int _damage);
    //効果適用
    void ApplyEffect<T>(T _applyCore, AttackStatus _attackStatus);
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