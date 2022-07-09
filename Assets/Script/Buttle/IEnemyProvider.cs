using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemyProvider{
    Vector2 _moveDir{get;}
    bool _serarchPlayerFlag{get; set;}
}
