using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCrystalStatus : ICrystalStatus
{
    [SerializeField]
    private int _attack = 0;
    [SerializeField]
    private int _attackRange = 4;
    [SerializeField]
    private float _speedUpRate = 0.3f;
    [SerializeField]
    private int _effectTime = 3;

    void Start(){
        _effectMaxCount = 10;
    }

    //配置時のクリスタル効果
    public override void SetEffect(Vector2Int pos){
        if(!SetEffectCount()) return;

        Attack(pos);
    }

    //周囲に速度バフエリア
    private void Attack(Vector2Int pos){
        if(Vector2Int.Distance(pos, AStarMap._playerPos) <= _attackRange){
            SpeedUp();
        }
    }

    //移動速度減少効果
    private void SpeedUp(){
        AStarMap._playerController._playerMove.SpeedUp(_speedUpRate, _effectTime);
    }
}
