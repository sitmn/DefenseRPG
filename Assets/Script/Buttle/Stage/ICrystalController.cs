using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICrystalController
{
    ACrystalStatus _crystalStatus{get;set;}
    //水晶ステータスを設定
    void SetCrystalStatus(ACrystalStatus _crystalStatus, CrystalParam _crystalParam);
}
