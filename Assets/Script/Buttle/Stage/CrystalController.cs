using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


//水晶の動作クラス
public class CrystalController : ACrystalCore,ICrystalController
{
    //HP、最大HPはIStage Objectが保持

    private Transform _crystalTr;
    private Vector2Int _crystalPos;
    public ACrystalStatus _crystalStatus{get;set;}
    public IAttack _attack{get;set;}

    void Awake(){
        _crystalTr = this.gameObject.GetComponent<Transform>();
        _crystalStatus = new BlackCrystalStatus();
        //黒水晶のHPをセット
        _maxHp = 999;
        Hp = _maxHp;
        _attack = new BlackCrystalAttack();
    }

    //配置時、マップに移動不可情報とクラスを入れる
    void Start(){
        _crystalPos = new Vector2Int(StageMove.UndoElementStageMove(AStarMap.CastMapPos(_crystalTr.position).x), AStarMap.CastMapPos(_crystalTr.position).y);
        if(AStarMap.astarMas[_crystalPos.x,_crystalPos.y]._crystalCore == null){
            // _crystalPos = AStarMap.CastMapPos(_crystalTr.position);
            // AStarMap.astarMas[StageMove.UndoElementStageMove(_crystalPos.x),_crystalPos.y].moveCost = 0;
            // AStarMap.astarMas[StageMove.UndoElementStageMove(_crystalPos.x),_crystalPos.y].obj.Add(this);
            SetOnAStarMap();
        }
        SetHPStream();
    }

    private void SetHPStream(){
        _hp.Subscribe((x) => {
            if(x <= 0) ObjectDestroy();
        }).AddTo(this);
    }

    public override void ObjectDestroy(){
        //水晶行動指示用リストから削除
        CrystalListController.RemoveCrystalInList(this);
        SetOffAStarMap();
        Destroy(this.gameObject);
    }

    //配置時、マップに移動不可情報とクラスを入れる
    public void SetOnAStarMap(){
        _crystalPos = new Vector2Int(StageMove.UndoElementStageMove(AStarMap.CastMapPos(_crystalTr.position).x), AStarMap.CastMapPos(_crystalTr.position).y);
        if(AStarMap.astarMas != null && AStarMap.astarMas[_crystalPos.x,_crystalPos.y]._crystalCore == null){
            AStarMap.astarMas[_crystalPos.x,_crystalPos.y]._moveCost = _crystalStatus._moveCost;
            AStarMap.astarMas[_crystalPos.x,_crystalPos.y]._crystalCore = this;
        }
        
    }
    //破壊または持ち上げ時、移動不可解除
    public void SetOffAStarMap(){
        AStarMap.astarMas[_crystalPos.x,_crystalPos.y]._moveCost = 1;
        AStarMap.astarMas[_crystalPos.x,_crystalPos.y]._crystalCore = null;
    }

    //クリスタル起動時のクリスタルステータスをセット
    public void SetCrystalStatus(ACrystalStatus _crystalStatus, CrystalParam _crystalParam){
        this._crystalStatus = _crystalStatus;
        this.gameObject.GetComponent<Renderer>().material = _crystalParam._material;
        //HPの最大値と現在のHPをセット
        this._maxHp = _crystalParam._maxHp;
        this.Hp = this._maxHp;
        //攻撃間隔のステータスをセット
        _crystalStatus._effectMaxCount = _crystalParam._effectMaxCount;
        //攻撃力と攻撃範囲のステータスをセット
        _crystalStatus._attack = _crystalParam._attack;
        _crystalStatus._attackRange = _crystalParam._attackRange;
        //特殊効果倍率と持続時間のステータスをセット
        _crystalStatus._effectRate = _crystalParam._effectRate;
        _crystalStatus._effectTime = _crystalParam._effectTime;
        //移動コスト
        _crystalStatus._moveCost = _crystalParam._moveCost;
        //既に設定してあるマスの移動コストのみ変更
        AStarMap.astarMas[StageMove.UndoElementStageMove(_crystalPos.x),_crystalPos.y]._moveCost = _crystalParam._moveCost;
        //攻撃方法をセット
        Type classObj = Type.GetType(_crystalParam._crystalAttackName);
        _attack = (IAttack)Activator.CreateInstance(classObj);
        
    }

    //攻撃や効果を発動
    public void Attack(){
        //リフト中のクリスタルは攻撃なし
        if(this == LiftCrystal._crystalController){

        }else{
            //攻撃カウントになったときに攻撃を実行
            if(_crystalStatus.CountAttack()) _attack.DoAttack(_crystalPos, new Vector2Int((int)_crystalTr.forward.x, (int)_crystalTr.forward.z), _crystalStatus);
        }
    // }
    // //セット中クリスタルの効果発動
    // public void SetEffect(){
        
    // }
    // //リフト中クリスタルの効果発動
    // public void LiftEffect(){

    // }
    }
    public override void SpeedDown(float _decreaseRate, int _decreaseTime){

    }
    public override void SpeedUp(float _upRate, int _upTime){

    }
        //CrystalControllerListで、全クリスタルが常に能力を発動
        //攻撃りょく、攻撃速度、攻撃範囲、効果はCrystalStatusが保持
        //⇨ CrystalStatusはnewする必要があるのでは？
}
