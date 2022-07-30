using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICrystalController
{
    ACrystalStatus _crystalStatus{get;set;}
    //水晶ステータスを設定
    void SetCrystalType(ACrystalStatus _crystalStatus, Material _material, int _maxHp, int _effectMaxCount, int _moveCost);
}
