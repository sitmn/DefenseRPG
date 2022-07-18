using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCrystalStatus : MonoBehaviour, ICrystalStatus
{
    public int CrystalNo => _crystalNo;
    private int _crystalNo{get;}

    //配置時のクリスタル効果
    public void SetEffect(){
        
    }

    //リフト時のクリスタル効果
    public void LiftEffect(){

    }
}
