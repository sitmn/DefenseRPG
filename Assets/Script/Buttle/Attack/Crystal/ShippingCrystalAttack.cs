using System.Collections.Generic;
using UnityEngine;

//ShippingCrystalは攻撃なし
public class ShippingCrystalAttack : AttackBase
{
    //ShippingCrystalは攻撃なし
    public override void Attack(Vector2Int _centerPos, Vector2Int _fowardDir, StatusBase _status)
    {
        
    }
    // 攻撃対象をreturn
    protected override List<T> DecideAttackTarget<T>(Vector2Int _centerPos, Vector2Int _forwardDir, int _attackRange){
        List<T> _applyCoreList = new List<T>();
        return _applyCoreList;
    }
    //ダメージ計算
    protected override int CalculateDamage(int _attack){
        int _damage = AttackDamage.NormalDamage(_attack);
        return _damage;
    }
    //ダメージ適用
    protected override void ApplyDamage<T>(T _applyCore, int _damage){
    }
    //効果適用
    protected override void ApplyEffect<T>(T _applyCore, StatusBase _status){

    }
}
