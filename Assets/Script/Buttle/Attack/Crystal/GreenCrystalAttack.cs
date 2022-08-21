using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCrystalAttack : IAttack
{
    //攻撃対象を決めて、攻撃と効果を反映
    public void DoAttack(Vector2Int _centerPos, Vector2Int _fowardDir, AttackStatus _crystalStatus /*int _attackRange, int _attack, int _effectTime*/){
        //攻撃対象を決める
        List<PlayerCore> _attackTargetList = DecideAttackTarget<PlayerCore>(_centerPos, _fowardDir, _crystalStatus._attackRange);
        
        //攻撃と効果を反映
        foreach(var _attackTarget in _attackTargetList){
            ApplyEffect<PlayerCore>(_attackTarget, _crystalStatus);
        }
    }

    // 攻撃対象をreturn
    public List<T> DecideAttackTarget<T>(Vector2Int _centerPos, Vector2Int _forwardDir, int _attackRange){
        List<T> _applyCoreList = new List<T>();
        _applyCoreList.AddRange(AStarMap.GetAroundCore<T>(_centerPos, _forwardDir, _attackRange));
        return _applyCoreList;
    }
    //ダメージ計算
    public int CalculateDamage(int _attack){
        
        return _attack;
    }
    //ダメージ適用
    public void ApplyDamage<T>(T _applyCore, int _damage){

    }
    //効果適用
    public void ApplyEffect<T>(T _applyCore, AttackStatus _crystalStatus){
        if(typeof(T) == typeof(PlayerCore)) ((PlayerCore)(object)_applyCore)._playerMove.SpeedUp(_crystalStatus._effectRate, _crystalStatus._effectTime);
    }
}
