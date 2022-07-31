using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParamData", menuName = "ScriptableObjects/CreateEnemyParamAsset")]
public class EnemyParamAsset : ScriptableObject{
    public List<EnemyParam> EnemyParamList = new List<EnemyParam>();
}

[System.Serializable]
public class EnemyParam{
    //敵キャラ名
    [SerializeField]
    public string _enemyName;
    //最大HP
    [SerializeField]
    public int _maxHp;
    //攻撃間隔
    [SerializeField]
    public int _attackMaxCount;
    //攻撃力
    [SerializeField]
    public int _attack;
    //移動速度
    [SerializeField]
    public float _moveSpeed;
    //索敵範囲
    [SerializeField]
    public float _searchDestination;
}
