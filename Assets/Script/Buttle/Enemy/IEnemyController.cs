using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

interface IEnemyController
{
    IReactiveProperty<Vector2Int> EnemyPos{get;}
    List<Vector2Int> TrackPos{get;}
    void Move(Vector2Int _moveDir);
}
