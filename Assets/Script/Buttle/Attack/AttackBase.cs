using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBase
{
    //DoAttackを実行する（Tはこのメソッドで指定する）
    public abstract void Attack(Vector2Int _centerPos, Vector2Int _fowardDir, StatusBase _status);

    //攻撃を実行 TはEnemyCoreBaseかCrystalCoreBase
    protected void DoAttack<T>(Vector2Int _centerPos, Vector2Int _fowardDir, StatusBase _status){
        //攻撃対象を決める
        List<T> _attackTargetList = DecideAttackTarget<T>(_centerPos, _fowardDir, _status._attackStatus._attackRange[_status._level]);
        int _damage = CalculateDamage(_status._attackStatus._attack[_status._level]);
        //攻撃と効果を反映
        foreach(var _attackTarget in _attackTargetList){
            ApplyDamage<T>(_attackTarget, _damage);
            ApplyEffect<T>(_attackTarget, _status);
        }
    }
    // 攻撃対象をreturn
    protected abstract List<T> DecideAttackTarget<T>(Vector2Int _centerPos, Vector2Int _forwardDir, int _attackRange);
    //ダメージ計算
    protected abstract int CalculateDamage(int _attack);
    //ダメージ適用
    protected abstract void ApplyDamage<T>(T _applyCore, int _damage);
    //効果適用
    protected abstract void ApplyEffect<T>(T _applyCore, StatusBase _status);
}

