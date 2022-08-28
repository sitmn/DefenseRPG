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
}