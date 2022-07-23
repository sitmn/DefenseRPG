using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class IStageObject:MonoBehaviour
{
    public ReactiveProperty<int> _hp;
}
