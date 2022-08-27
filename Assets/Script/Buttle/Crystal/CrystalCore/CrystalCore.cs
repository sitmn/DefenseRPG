using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


//水晶の動作クラス
public class CrystalCore:MonoBehaviour
{
    private Transform _crystalTr;
    private Vector2Int _crystalPos;
    public AttackBase _attack;
    public CrystalStatus _crystalStatus;
    public ReactiveProperty<int> _hp;
    public int Hp{
        get{
            return _hp.Value;
        }
        set{
            if(value > _maxHp){
                _hp.Value = _maxHp;
            }else{
                _hp.Value = value;
            }
        }
    }

    public int _maxHp;
    //Statusやストリームのセット
    public void InitializeCore(CrystalParam _crystalParam){
        _crystalTr = this.gameObject.GetComponent<Transform>();
        
        SetCrystalStatus(_crystalParam);
        SetHPStream();
        SetOnAStarMap();
    }
    
    private void SetHPStream(){
        _hp.Subscribe((x) => {
            if(x <= 0) ObjectDestroy();
        }).AddTo(this);
    }

    public void ObjectDestroy(){
        //水晶行動指示用リストから削除
        CrystalListCore.RemoveCrystalCoreInList(this);
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
    public void SetCrystalStatus(CrystalParam _crystalParam){
        _crystalStatus = new CrystalStatus(_crystalParam);
        this.gameObject.GetComponent<Renderer>().material = _crystalParam._material;
        //HPの最大値と現在のHPをセット
        this._maxHp = _crystalParam._maxHp;
        this.Hp = this._maxHp;
        //攻撃間隔のステータスをセット
        _crystalStatus._attackMaxCount = _crystalParam._attackMaxCount;
        //移動コスト
        _crystalStatus._moveCost = _crystalParam._moveCost;
        
        //攻撃方法をセット
        Type classObj = Type.GetType(_crystalParam._crystalAttackName);
        _attack = (AttackBase)Activator.CreateInstance(classObj);
    }

    //攻撃や効果を発動
    public void Attack(){
        //リフト中のクリスタルは攻撃なし
        if(this == PlayerCore.GetLiftCrystalCore()){

        }else{
            //攻撃カウントになったときにエネミーに攻撃
            if(_crystalStatus.CountAttack()) _attack.Attack(_crystalPos, new Vector2Int((int)_crystalTr.forward.x, (int)_crystalTr.forward.z), _crystalStatus._attackStatus);
        }
    }
}
