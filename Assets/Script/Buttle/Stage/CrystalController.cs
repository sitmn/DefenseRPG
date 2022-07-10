using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//水晶の動作クラス
public class CrystalController : MonoBehaviour, ICrystalController
{
    private Transform _crystalTr;


    void Awake(){
        _crystalTr = this.gameObject.GetComponent<Transform>();
    }

    //配置時、マップに移動不可情報を入れる
    void Start(){
        Vector2Int pos = AStarMap.CastMapPos(_crystalTr.position);
        AStarMap.astarMas[pos.x,pos.y].moveCost = 0;
    }
    
    //破壊または持ち上げ時、移動不可解除
    void OnDisable(){

    }
}
