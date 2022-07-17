using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//水晶の動作クラス
public class CrystalController : MonoBehaviour, ICrystalController, IStageObject
{
    private Transform _crystalTr;


    void Awake(){
        _crystalTr = this.gameObject.GetComponent<Transform>();
    }

    //配置時、マップに移動不可情報とクラスを入れる
    void Start(){
        Vector2Int pos = AStarMap.CastMapPos(_crystalTr.position);
        AStarMap.astarMas[pos.x,pos.y].moveCost = 0;
        AStarMap.astarMas[pos.x,pos.y].obj = this;
    }
    //配置時、マップに移動不可情報とクラスを入れる
    void OnEnable(){
        if(AStarMap.astarMas != null){
            Vector2Int pos = AStarMap.CastMapPos(_crystalTr.position);
            AStarMap.astarMas[pos.x,pos.y].moveCost = 0;
            AStarMap.astarMas[pos.x,pos.y].obj = this;
        }
    }
    
    //破壊または持ち上げ時、移動不可解除
    void OnDisable(){
        Vector2Int pos = AStarMap.CastMapPos(_crystalTr.position);
        AStarMap.astarMas[pos.x,pos.y].moveCost = 1;
        AStarMap.astarMas[pos.x,pos.y].obj = null;
    }
}
