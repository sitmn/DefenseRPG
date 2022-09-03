using System;
using UnityEngine;
using UniRx;


//水晶の動作クラス
public class ShippingCrystalCore:CrystalCoreBase
{
    //配置時、マップ上に情報をセット
    public override void SetOnMap(){
        _crystalPos = new Vector2Int(StageMove.UndoElementStageMove(MapManager.CastMapPos(_crystalTr.position).x), MapManager.CastMapPos(_crystalTr.position).y);
        if(MapManager._map != null && MapManager.GetMap(_crystalPos)._crystalCore == null){
            MapManager.GetMap(_crystalPos)._moveCost = _crystalStatus._crystalParam._moveCost[_crystalStatus._level];
            MapManager.GetMap(_crystalPos)._crystalCore = this;
            //輸送クリスタルの位置をセット
            MapManager.SetShippingCrystalPos(new Vector2Int(StageMove.UndoElementStageMove(MapManager.CastMapPos(_crystalTr.position).x),MapManager.CastMapPos(_crystalTr.position).y));
        }
    }
    //破壊または持ち上げ時、マップ上から情報を削除
    public override void SetOffMap(){
        MapManager.GetMap(_crystalPos)._moveCost = 1;
        MapManager.GetMap(_crystalPos)._crystalCore = null;
    }

    //クリスタルを削除し、ゲームオーバ
    public override void ObjectDestroy(){
        //オブジェクト削除
        Destroy(this.gameObject);
        //ゲームオーバー
        GameManager.GameOver();
    }

    //クリスタルの行動
    public override void CrystalAction(){
        Attack();
    }

    //攻撃や効果を発動
    protected override void Attack(){
        //リフト中のクリスタルは攻撃なし
        if(this == PlayerCore.GetLiftCrystalCore()){

        }else{
            //攻撃カウントになったときにエネミーに攻撃
            //if(_crystalStatus.CountAttack()) _attack.Attack(_crystalPos, new Vector2Int((int)_crystalTr.forward.x, (int)_crystalTr.forward.z), _crystalStatus);
            Debug.Log("Attack");
        }
    }
}
