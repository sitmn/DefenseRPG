using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class IStageObject:MonoBehaviour
{
    public ReactiveProperty<int> _hp;
    public abstract void SpeedDown(float _decreaseRate, int _decreaseTime);
    public abstract void SpeedUp(float _decreaseRate, int _decreaseTime);
}
