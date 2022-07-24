using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//水晶の動作クラス
public class CrystalController : IStageObject,ICrystalController
{
    private Transform _crystalTr;
    private Vector2Int _crystalPos;
    private ICrystalStatus _crystalStatus;

    void Awake(){
        _crystalTr = this.gameObject.GetComponent<Transform>();
    }

    //配置時、マップに移動不可情報とクラスを入れる
    void Start(){
        if(AStarMap.astarMas[_crystalPos.x,_crystalPos.y].obj.Count == 0){
            _crystalPos = AStarMap.CastMapPos(_crystalTr.position);
            AStarMap.astarMas[_crystalPos.x,_crystalPos.y].moveCost = 0;
            AStarMap.astarMas[_crystalPos.x,_crystalPos.y].obj.Add(this);
        }
        
    }
    //配置時、マップに移動不可情報とクラスを入れる
    void OnEnable(){
        if(AStarMap.astarMas != null && AStarMap.astarMas[_crystalPos.x,_crystalPos.y].obj.Count == 0){
            _crystalPos = AStarMap.CastMapPos(_crystalTr.position);
            AStarMap.astarMas[_crystalPos.x,_crystalPos.y].moveCost = 0;
            AStarMap.astarMas[_crystalPos.x,_crystalPos.y].obj.Add(this);
        }
    }
    
    //破壊または持ち上げ時、移動不可解除
    void OnDisable(){
        _crystalPos = AStarMap.CastMapPos(_crystalTr.position);
        AStarMap.astarMas[_crystalPos.x,_crystalPos.y].moveCost = 1;
        AStarMap.astarMas[_crystalPos.x,_crystalPos.y].obj.Remove(this);
    }

    //クリスタル起動時のクリスタルステータスをセット
    public void SetCrystalType(ICrystalStatus _crystalStatus){
        this._crystalStatus = _crystalStatus;
        this.gameObject.GetComponent<Renderer>().material = _crystalStatus._material;
    }

    //クリスタルの効果発動
    public void SetEffect(){
        if(_crystalStatus != null){
            _crystalStatus.SetEffect(_crystalPos);
        }
    }

    public override void SpeedDown(float _decreaseRate, int _decreaseTime){

    }
        //CrystalControllerListで、全クリスタルが常に能力を発動
        //攻撃りょく、攻撃速度、攻撃範囲、効果はCrystalStatusが保持
        //⇨ CrystalStatusはnewする必要があるのでは？
}
