using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRepairCrystal
{
    //水晶修復アクション有効化フラグ
    bool RepairActiveFlag{get;}
    //水晶修復アクション実行中フラグ
    bool RepairActionFlag{get;}
    //水晶修復アクションを有効化
    void RepairEnable();
    //水晶修復アクションを無効化
    void RepairDisable();
}
