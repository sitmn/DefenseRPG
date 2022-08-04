using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    //プレイヤーが装備しているクリスタル
    [SerializeField]
    public ACrystalStatus[] _crystalStatus;

    //プレイヤーが装備しているクリスタルのマテリアル
    [SerializeField]
    public Material[] _material;

}
