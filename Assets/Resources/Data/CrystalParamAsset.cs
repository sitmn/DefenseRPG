using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrystalParamData", menuName = "ScriptableObjects/CreateCrystalParamAsset")]
public class CrystalParamAsset : ScriptableObject{
    public List<CrystalParam> CrystalParamList = new List<CrystalParam>();
}

[System.Serializable]
public class CrystalParam{
    //CrystalStatuクラスを継承したクラス名
    [SerializeField]
    public string _crystalCoreName;
    //攻撃用クラス名
    [SerializeField]
    public string _crystalAttackName;
    //最大HP
    [SerializeField]
    public int _maxHp;
    //攻撃間隔
    [SerializeField]
    public int _attackMaxCount;
    //攻撃力
    [SerializeField]
    public int _attack;
    //攻撃範囲
    [SerializeField]
    public int _attackRange;
    //効果倍率
    [SerializeField]
    public float _effectRate;
    //効果時間
    [SerializeField]
    public int _effectTime;
    //移動コスト（エネミーの移動探索用）
    [SerializeField]
    public int _moveCost;
    //クリスタル起動時コスト
    [SerializeField]
    public int _launchCost;
    //クリスタルのマテリアル
    [SerializeField]
    public Material _material;
}
