using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    //プレイヤーが装備しているクリスタル
    [SerializeField]
    public ICrystalStatus[] _crystalStatus;

    //プレイヤーが装備しているクリスタルのマテリアル
    [SerializeField]
    public Material[] _material;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
