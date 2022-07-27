using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICrystalController
{
    //水晶ステータスを設定
    void SetCrystalType(ACrystalStatus _crystalStatus, Material _material);
}
