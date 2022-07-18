using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IPlayerMove _playerMove;
    public ICrystalStatus[] EqueipmentCrystal{get;set;}
    [SerializeField]
    private ICrystalStatus[] _equipmentCrystal = new ICrystalStatus[3];
    
    void Awake(){
        _playerMove = this.gameObject.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        _playerMove.Move();
    }
}
