using System;
using UnityEngine;
using UniRx;

public abstract class CrystalCoreBase:MonoBehaviour
{
    //クリスタルランク
    protected Transform _crystalTr;
    protected Vector2Int _crystalPos;
    protected AttackBase _attack;
    [System.NonSerialized]
    public CrystalStatus _crystalStatus;
    protected ReactiveProperty<int> _hp = new ReactiveProperty<int>();
    public int Hp{
        get{
            return _hp.Value;
        }
        set{
            if(value > _crystalStatus._crystalParam._maxHp[_crystalStatus._level]){
                _hp.Value = _crystalStatus._crystalParam._maxHp[_crystalStatus._level];
            }else{
                _hp.Value = value;
            }
        }
    }

    //配置時、マップ上に情報をセット
    public void SetOnMap(){
        _crystalPos = new Vector2Int(StageMove.UndoElementStageMove(MapManager.CastMapPos(_crystalTr.position).x), MapManager.CastMapPos(_crystalTr.position).y);
        if(MapManager._map != null && MapManager.GetMap(_crystalPos)._crystalCore == null){
            MapManager.GetMap(_crystalPos)._moveCost = _crystalStatus._crystalParam._moveCost[_crystalStatus._level];
            MapManager.GetMap(_crystalPos)._crystalCore = this;
        }
    }
    //破壊または持ち上げ時、マップ上から情報を削除
    public void SetOffMap(){
        MapManager.GetMap(_crystalPos)._moveCost = 1;
        MapManager.GetMap(_crystalPos)._crystalCore = null;
    }

    //クリスタル起動時のクリスタルステータスをセット
    public void SetCrystalStatus(CrystalParam _crystalParam){
        _crystalStatus = new CrystalStatus(_crystalParam);
        this.gameObject.GetComponent<Renderer>().material = _crystalParam._material[_crystalStatus._level];
        //現在のHPをセット
        _hp.Value = _crystalParam._maxHp[_crystalStatus._level];
        //攻撃方法をセット
        Type classObj = Type.GetType(_crystalParam._crystalAttackName);
        _attack = (AttackBase)Activator.CreateInstance(classObj);
    }

    //クリスタルの初期化
    public abstract void InitializeCore(CrystalParam _crystalParam);
    //Hpが0になったときの処理
    public abstract void ObjectDestroy();
    //フレーム毎のクリスタルの行動
    public abstract void CrystalAction();
    //クリスタルの攻撃
    protected abstract void Attack();
}
