using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " PlayerParamData", menuName = "ScriptableObjects/CreatePlayerParamAsset")]
public class PlayerParamAsset : ScriptableObject{
    public List<PlayerParam> PlayerParamList = new List<PlayerParam>();
}

[System.Serializable]
public class PlayerParam{
    //最大HP
    [SerializeField]
    public int _maxHp;
    //水晶回復間隔
    [SerializeField]
    public int _healMaxCount;
    //移動速度
    [SerializeField]
    public float _moveSpeed;
}