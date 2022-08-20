using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class ACrystalStatus : MonoBehaviour
{
    // //水晶の最大HP
    // public int _maxHp;
    //水晶の攻撃間隔
    public int _effectMaxCount;
    //効果間隔用のカウント
    public int _effectCount;
    //攻撃力
    public int _attack;
    //攻撃範囲
    public int _attackRange;
    //特殊効果倍率
    public float _effectRate;
    //特殊効果時間
    public int _effectTime;
    //水晶の移動コスト(エネミーの移動経路探索用)
    public int _moveCost;

    //クリスタルのタイプ（装備1~3,0は黒クリスタル）
    public int CrystalNo{get;}
    //配置時クリスタル効果
    /*赤：近くの敵に攻撃
    **青：周囲の敵にデバフ攻撃
    **緑：周囲に速度バフエリア展開
    **黒：周囲の敵に速度バフ展開
    */

    void Awake(){
        _effectCount = 0;
    }

    //効果カウントのカウントと初期化
    public bool CountAttack(){
        _effectCount++;
        bool _effectLaunch = false;
        if(_effectCount >= _effectMaxCount){
            _effectCount = 0;
            _effectLaunch = true;
        }
        
        return _effectLaunch;        
    }

    public Material _material{get;set;}

    
}
