using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrystalParamData", menuName = "ScriptableObjects/CreateCrystalParamAsset")]
public class CrystalParamAsset : ScriptableObject{
    public List<CrystalParam> CrystalParamList = new List<CrystalParam>();
    //AttackStatusをインスタンス化
    public void SetAttackStatus(){
        for(int i = 0; i < CrystalParamList.Count; i++){
            CrystalParamList[i]._attackStatus = new AttackStatus(CrystalParamList[i]._attack, CrystalParamList[i]._attackRange, CrystalParamList[i]._effectRate, CrystalParamList[i]._effectTime);
        }
    }
}

[System.Serializable]
public class CrystalParam{
    //CrystalStatuクラスを継承したクラス名
    [SerializeField]
    public string _crystalCoreName;
    //攻撃用クラス名
    [SerializeField]
    public string _crystalAttackName;
    //最大HP(Rank毎の値を格納)
    [SerializeField]
    public List<int> _maxHp;
    //攻撃間隔(Rank毎の値を格納)
    [SerializeField]
    public List<int> _attackMaxCount;
    //攻撃力(Rank毎の値を格納)
    [SerializeField]
    public List<int> _attack;
    //攻撃範囲(Rank毎の値を格納)
    [SerializeField]
    public List<int> _attackRange;
    //効果倍率(Rank毎の値を格納)
    [SerializeField]
    public List<float> _effectRate;
    //効果時間(Rank毎の値を格納)
    [SerializeField]
    public List<int> _effectTime;
    //移動コスト（エネミーの移動探索用,Rank毎の値を格納）
    [SerializeField]
    public List<int> _moveCost;
    //クリスタル起動時コスト(Rank毎の値を格納,要素0は起動コスト、以降はRankUpコスト)
    [SerializeField]
    public List<int> _cost;
    //クリスタルのマテリアル
    [SerializeField]
    public List<Material> _material;
    //攻撃関連のステータス
    public AttackStatus _attackStatus;
}
