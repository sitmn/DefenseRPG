using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMove
{
    //移動または移動入力受付
    void Move();
    //次の移動場所決定（移動入力受付
    void NextMovePos();
    //移動状態
    bool MoveFlag();
    //移動速度上昇
    void SpeedUp(float _upRate, int _upTime);
}
