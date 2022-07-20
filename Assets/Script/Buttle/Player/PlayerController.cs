using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IPlayerMove _playerMove;
    public ICrystalStatus[] EqueipmentCrystal{get;set;}
    [SerializeField]
    private ICrystalStatus[] _equipmentCrystal = new ICrystalStatus[3];

    private IUserCrystal _useCrystal;
    
    void Awake(){
        _playerMove = this.gameObject.GetComponent<PlayerMove>();
        _useCrystal = this.gameObject.GetComponent<UseCrystal>();
    }

    // Update is called once per frame
    void Update()
    {
        //正面にクリスタルがなければ、水晶アクション無効化
        if(!_useCrystal.CrystalCheck() && _useCrystal.LaunchActiveFlag){//水晶が正面にない時、水晶アクションが有効になっていれば全て無効
                _useCrystal.LaunchDisable();
        }
        //水晶起動、修理、持ち上げアクション中か？
        //移動中か？
        if(_playerMove.MoveFlag()){
            _playerMove.Move();
            return;
        }else if(_useCrystal.LaunchActionFlag){//水晶起動中か？
            //水晶起動処理
            return;
        }else if(false){//水晶修理中か？
            //水晶修理処理
            return;
        }else if(false){//水晶持ち上げ中か？
            //水晶持ち上げ処理
            return;
        }else{//行動なし、入力待機中
            if(_useCrystal.CrystalCheck()){//水晶アクションが可能か？水晶が正面にあるか？
                if(!_useCrystal.LaunchActiveFlag){//黒水晶で、水晶起動が無効であれば有効化
                    _useCrystal.LaunchEnable();
                }else if(false){//それ以外の水晶なら、水晶修理及び持ち上げが無効であれば有効化

                }
            }/*else if(_useCrystal.LaunchActiveFlag){//水晶が正面にない時、水晶アクションが有効になっていれば全て無効
                _useCrystal.LaunchDisable();
            }*/
            _playerMove.NextMovePos();
        }
        
    }

    
}
