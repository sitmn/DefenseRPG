using System;
using UnityEngine;
using UniRx;


//水晶の動作クラス
public class CrystalCore:MonoBehaviour
{
    //クリスタルランク
    private int _crystalRank;
    private Transform _crystalTr;
    private Vector2Int _crystalPos;
    private AttackBase _attack;
    [System.NonSerialized]
    public CrystalStatus _crystalStatus;
    private ReactiveProperty<int> _hp;
    public int Hp{
        get{
            return _hp.Value;
        }
        set{
            if(value > _crystalStatus._crystalParam._maxHp[_crystalRank]){
                _hp.Value = _crystalStatus._crystalParam._maxHp[_crystalRank];
            }else{
                _hp.Value = value;
            }
        }
    }

    //private int _maxHp;
    //Statusやストリームの生成
    public void InitializeCore(CrystalParam _crystalParam){
        _crystalTr = this.gameObject.GetComponent<Transform>();
        //クリスタルステータスのセット
        SetCrystalStatus(_crystalParam);
        //HPストリームの生成
        CreateHPStream();
        //マップ上に情報セット
        SetOnMap();
    }
    //HPのストリーム生成
    private void CreateHPStream(){
        _hp.Subscribe((x) => {
            if(x <= 0) ObjectDestroy();
        }).AddTo(this);
    }
    //クリスタルの削除
    public void ObjectDestroy(){
        //水晶行動指示用リストから削除
        CrystalListCore.RemoveCrystalCoreInList(this);
        //マップ上から削除
        SetOffMap();
        //オブジェクト削除
        Destroy(this.gameObject);
    }

    //配置時、マップ上に情報をセット
    public void SetOnMap(){
        _crystalPos = new Vector2Int(StageMove.UndoElementStageMove(MapManager.CastMapPos(_crystalTr.position).x), MapManager.CastMapPos(_crystalTr.position).y);
        if(MapManager._map != null && MapManager.GetMap(_crystalPos)._crystalCore == null){
            MapManager.GetMap(_crystalPos)._moveCost = _crystalStatus._crystalParam._moveCost[_crystalRank];
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
        this._crystalRank = 0;
        _crystalStatus = new CrystalStatus(_crystalParam);
        this.gameObject.GetComponent<Renderer>().material = _crystalParam._material;
        //現在のHPをセット
        _hp = new ReactiveProperty<int>(_crystalParam._maxHp[_crystalRank]);
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
