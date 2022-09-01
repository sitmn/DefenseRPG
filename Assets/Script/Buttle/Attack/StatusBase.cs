using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusBase
{
    //レベル(0がレベル1)
    public int _level;
    //攻撃関連のステータス
    public AttackStatus _attackStatus;
}

//攻撃関連のステータス
public class AttackStatus{
    //攻撃力
    public List<int> _attack;
    //攻撃範囲
    public List<int> _attackRange;
    //特殊効果倍率
    public List<float> _effectRate;
    //特殊効果時間
    public List<int> _effectTime;

    public AttackStatus(List<int> _attack, List<int> _attackRange, List<float> _effectRate, List<int> _effectTime){
        this._attack = _attack;
        this._attackRange = _attackRange;
        this._effectRate = _effectRate;
        this._effectTime = _effectTime;
    }
}