using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICrystalStatus
{
    //クリスタルのタイプ（装備1~3,0は黒クリスタル）
    int CrystalNo{get;}
    //配置時クリスタル効果
    /*赤：近くの敵に攻撃
    **青：周囲の敵にデバフ攻撃
    **緑：周囲に速度バフエリア展開
    **黒：周囲の敵に速度バフ展開
    */
    void SetEffect();

    //リフト時クリスタル効果
    /*赤：進行方向に遠距離範囲攻撃
    **青：周囲の敵に強デバフ攻撃
    **緑：プレイヤーに速度バフ付与
    **黒：持ち上げ不可
    */
    void LiftEffect();
}
