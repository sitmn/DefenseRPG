using System;
using UnityEngine;
using UniRx;


//水晶の動作クラス
public class CrystalCore:CrystalCoreBase
{
    //Statusやストリームの生成
    public override void InitializeCore(CrystalParam _crystalParam){
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
    public override void ObjectDestroy(){
        //水晶行動指示用リストから削除
        CrystalListCore.RemoveCrystalCoreInList(this);
        //マップ上から削除
        SetOffMap();
        //オブジェクト削除
        Destroy(this.gameObject);
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
            if(_crystalStatus.CountAttack()) _attack.Attack(_crystalPos, new Vector2Int((int)_crystalTr.forward.x, (int)_crystalTr.forward.z), _crystalStatus);
        }
    }
}
