using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    //攻撃を実行
    void DoAttack(Vector2Int _centerPos, Vector2Int _fowardDir, ACrystalStatus _crystalStatus);
    // 攻撃対象をreturn
    List<T> DecideAttackTarget<T>(Vector2Int _centerPos, Vector2Int _forwardDir, int _attackRange);
    //ダメージ計算
    int CalculateDamage(int _attack);
    //ダメージ適用
    void ApplyDamage<T>(T _applyCore, int _damage);
    //効果適用
    void ApplyEffect<T>(T _applyCore, ACrystalStatus _crystalStatus);
}
