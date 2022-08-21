using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class ACrystalCore:MonoBehaviour
{
    public CrystalStatus _crystalStatus;
    public ReactiveProperty<int> _hp;
    public int Hp{
        get{
            return _hp.Value;
        }
        set{
            if(value > _maxHp){
                _hp.Value = _maxHp;
            }else{
                _hp.Value = value;
            }
        }
    }

    public int _maxHp;
    public abstract void SetCrystalStatus(CrystalParam _crystalParam);
    public abstract void SpeedDown(float _decreaseRate, int _decreaseTime);
    public abstract void SpeedUp(float _decreaseRate, int _decreaseTime);
    public abstract void ObjectDestroy();
}
