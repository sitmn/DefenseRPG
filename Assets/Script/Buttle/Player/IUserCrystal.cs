using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IUserCrystal
{
    //クリスタル起動有効化
    void LaunchEnable();
    //クリスタル起動無効化
    void LaunchDisable();
    //クリスタル起動有効無効状態
    bool LaunchActiveFlag{get;}
    //クリスタル起動アクション状態
    bool LaunchActionFlag{get;}
    //正面にクリスタルがあるか判定
    bool CrystalCheck();
    //正面が黒クリスタルか判定
    bool BlackCrystalCheck();
}
