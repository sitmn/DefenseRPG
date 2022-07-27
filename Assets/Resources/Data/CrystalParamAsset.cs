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
    [SerializeField]
    public Material _material;
}
