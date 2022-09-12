using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " SystemParamData", menuName = "ScriptableObjects/CreateSystemParamAsset")]
public class SystemParamAsset : ScriptableObject{
    public List<SystemParam> SystemParamList = new List<SystemParam>();
}

[System.Serializable]
public class SystemParam{
    //ステージ幅x
    [SerializeField]
    public int max_pos_x;
    //ステージ奥行きz
    [SerializeField]
    public int max_pos_z;
    //ステージ移動速度
    [SerializeField]
    public int _stageMoveMaxCount;
    //プレイヤーアクションの初期コスト
    [SerializeField]
    public int _initialActionCost;
    //プレイヤーアクションの回復コスト
    [SerializeField]
    public int _increaseActionCost;
    //プレイヤーアクション回復までの時間
    [SerializeField]
    public float _increaseActionCostTime;
    //コストの最大上限
    [SerializeField]
    public int _maxCost;
}