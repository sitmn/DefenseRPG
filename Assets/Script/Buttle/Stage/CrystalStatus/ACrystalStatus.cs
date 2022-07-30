using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class ACrystalStatus : MonoBehaviour, ICrystalStatus
{
    //効果間隔用のカウント
    private int _effectCount;
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

    public abstract void SetEffect(Vector2Int pos, int _effectMaxCount);

    //効果カウントのカウントと初期化
    public bool SetEffectCount(int _effectMaxCount){
        _effectCount++;
        bool _effectLaunch = false;
        if(_effectCount >= _effectMaxCount){
            _effectCount = 0;
            _effectLaunch = true;
        }
        
        return _effectLaunch;        
    }


    //リフト時クリスタル効果 ⇨ 一旦効果なしで
    /*赤：進行方向に遠距離範囲攻撃
    **青：周囲の敵に強デバフ攻撃
    **緑：プレイヤーに速度バフ付与
    **黒：持ち上げ不可
    */
    void LiftEffect(){

    }

    public Material _material{get;set;}
}
