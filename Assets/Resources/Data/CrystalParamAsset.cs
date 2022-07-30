using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrystalParamData", menuName = "ScriptableObjects/CreateCrystalParamAsset")]
public class CrystalParamAsset : ScriptableObject{
    public List<CrystalParam> CrystalParamList = new List<CrystalParam>();
}

[System.Serializable]
public class CrystalParam{
    //ICrystalStatuクラスを継承したクラス名
    [SerializeField]
    public string _crystalControllerName;
    //最大HP
    [SerializeField]
    public int _maxHp;
    //攻撃間隔
    [SerializeField]
    public int _effectMaxCount;
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
    [SerializeField]
    public Material _material;
}
