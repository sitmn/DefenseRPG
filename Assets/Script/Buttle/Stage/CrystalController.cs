using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

//水晶の動作クラス
public class CrystalController : AStageObject,ICrystalController
{
    //HP、最大HPはIStage Objectが保持

    private Transform _crystalTr;
    private Vector2Int _crystalPos;
    public ACrystalStatus _crystalStatus{get;set;}

    void Awake(){
        _crystalTr = this.gameObject.GetComponent<Transform>();
        _crystalStatus = new BlackCrystalStatus();
        //黒水晶のHPをセット
        _maxHp = 999;
        Hp = _maxHp;
    }

    //配置時、マップに移動不可情報とクラスを入れる
    void Start(){
        if(AStarMap.astarMas[_crystalPos.x,_crystalPos.y].obj.Count == 0){
            _crystalPos = AStarMap.CastMapPos(_crystalTr.position);
            AStarMap.astarMas[_crystalPos.x,_crystalPos.y].moveCost = 0;
            AStarMap.astarMas[_crystalPos.x,_crystalPos.y].obj.Add(this);
        }
        SetHPStream();
    }

    private void SetHPStream(){
        _hp.Subscribe((x) => {
            if(x <= 0) CrystalDestroy();
        }).AddTo(this);
    }

    void OnEnable(){
        SetOnAStarMap();
    }
    void OnDisable(){
        SetOffAStarMap();
    }
    void OnDestroy(){
        SetOffAStarMap();
    }

    private void CrystalDestroy(){
        //水晶行動指示用リストから削除
        CrystalListController.RemoveCrystalInList(this);
        Destroy(this.gameObject);
    }

    //配置時、マップに移動不可情報とクラスを入れる
    public void SetOnAStarMap(){
        if(AStarMap.astarMas != null && AStarMap.astarMas[StageMove.UndoElementStageMove(_crystalPos.x),_crystalPos.y].obj.Count == 0){
            _crystalPos = AStarMap.CastMapPos(_crystalTr.position);
            AStarMap.astarMas[StageMove.UndoElementStageMove(_crystalPos.x),_crystalPos.y].moveCost = _crystalStatus._moveCost;
            AStarMap.astarMas[StageMove.UndoElementStageMove(_crystalPos.x),_crystalPos.y].obj.Add(this);
        }
    }
    //破壊または持ち上げ時、移動不可解除
    public void SetOffAStarMap(){
        _crystalPos = AStarMap.CastMapPos(_crystalTr.position);
        AStarMap.astarMas[StageMove.UndoElementStageMove(_crystalPos.x),_crystalPos.y].moveCost = 1;
        AStarMap.astarMas[StageMove.UndoElementStageMove(_crystalPos.x),_crystalPos.y].obj.Remove(this);
    }

    //クリスタル起動時のクリスタルステータスをセット
    public void SetCrystalType(ACrystalStatus _crystalStatus, Material _material, int _maxHp, int _effectMaxCount,int _attack,int _attackRange, float _effectRate, int _effectTime, int _moveCost){
        this._crystalStatus = _crystalStatus;
        this.gameObject.GetComponent<Renderer>().material = _material;
        //HPの最大値と現在のHPをセット
        this._maxHp = _maxHp;
        this.Hp = this._maxHp;
        //攻撃間隔のステータスをセット
        _crystalStatus._effectMaxCount = _effectMaxCount;
        //攻撃力と攻撃範囲のステータスをセット
        _crystalStatus._attack = _attack;
        _crystalStatus._attackRange = _attackRange;
        //特殊効果倍率と持続時間のステータスをセット
        _crystalStatus._effectRate = _effectRate;
        _crystalStatus._effectTime = _effectTime;
        //移動コスト
        _crystalStatus._moveCost = _moveCost;
        //既に設定してあるマスの移動コストのみ変更
        AStarMap.astarMas[_crystalPos.x,_crystalPos.y].moveCost = _moveCost;
    }

    //セット中クリスタルの効果発動
    public void SetEffect(){
        _crystalStatus.SetEffect(_crystalPos);
    }
    //リフト中クリスタルの効果発動
    public void LiftEffect(){

    }

    public override void SpeedDown(float _decreaseRate, int _decreaseTime){

    }
    public override void SpeedUp(float _upRate, int _upTime){

    }
        //CrystalControllerListで、全クリスタルが常に能力を発動
        //攻撃りょく、攻撃速度、攻撃範囲、効果はCrystalStatusが保持
        //⇨ CrystalStatusはnewする必要があるのでは？
}
