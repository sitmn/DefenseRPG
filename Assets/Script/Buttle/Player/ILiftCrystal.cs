using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiftCrystal
{
    //リフトダウンチェック（正面にエネミーとクリスタルがないか？）
    bool StageObjCheck();
    //リフトアップ有効化（正面のクリスタルをMapから取り除き、プレイヤーの頭上へ移動）
    void LiftUpEnable();
    //リフトアップ無効化（正面のクリスタルをMapから取り除き、プレイヤーの頭上へ移動）
    void LiftUpDisable();
    //リフトダウン有効化（プレイヤー頭上から正面にクリスタルを移動、Mapへ格納）
    void LiftDownEnable();
    //リフトダウン無効化（プレイヤー頭上から正面にクリスタルを移動、Mapへ格納）
    void LiftDownDisable();
    //リフトアップ中のオブジェクト
    Transform CrystalTr{get;}
    //リフトアップ有効化フラグ
    bool LiftUpActiveFlag{get;}
    //リフトダウン有効化フラグ
    bool LiftDownActiveFlag{get;}
    //リフトアップ有効化フラグ
    bool LiftUpActionFlag{get;}
    //リフトダウン有効化フラグ
    bool LiftDownActionFlag{get;}
}
