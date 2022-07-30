using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public IPlayerMove _playerMove;
    public ACrystalStatus[] EqueipmentCrystal{get;set;}
    [SerializeField]
    private ACrystalStatus[] _equipmentCrystal = new ACrystalStatus[3];

    private IUserCrystal _useCrystal;
    private ILiftCrystal _liftCrystal;
    
    void Awake(){
        _playerMove = this.gameObject.GetComponent<PlayerMove>();
        _useCrystal = this.gameObject.GetComponent<UseCrystal>();
        _liftCrystal = this.gameObject.GetComponent<LiftCrystal>();
        AStarMap._playerController = this;
    }

    // Update is called once per frame
    void Update()
    {
        //正面に黒水晶がなければ、水晶起動アクション無効化
        if(_useCrystal.LaunchActiveFlag && (!_useCrystal.CrystalCheck() || !_useCrystal.BlackCrystalCheck())){
            _useCrystal.LaunchDisable();
        }
        //正面に水晶がない、または、水晶リフト中であれば、水晶リフトアップアクション無効化
        if((_liftCrystal.LiftUpActiveFlag && !_useCrystal.CrystalCheck())){
            _liftCrystal.LiftUpDisable();
        }
        //リフト中でない、または、正面にオブジェクトが存在すれば、水晶リフトダウンアクション無効化
        if(_liftCrystal.LiftDownActiveFlag && (_liftCrystal.CrystalTr == null || _liftCrystal.StageObjCheck())){
            _liftCrystal.LiftDownDisable();
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
        }else if(_liftCrystal.LiftUpActionFlag){//水晶リフトアップ中か？
            //水晶持ち上げ処理
            return;
        }else if(_liftCrystal.LiftDownActionFlag){//水晶リフトダウン中か？
            //水晶持ち上げ処理
            return;
        }else{//行動なし、入力待機中
            if(_useCrystal.CrystalCheck()){//水晶アクションが可能か？水晶が正面にあるか？
                if(!_useCrystal.LaunchActiveFlag && _useCrystal.BlackCrystalCheck()){//黒水晶かつ水晶起動が無効であれば有効化
                    _useCrystal.LaunchEnable();
                }if(!_liftCrystal.LiftUpActiveFlag && !_useCrystal.BlackCrystalCheck() && _liftCrystal.CrystalTr == null){//黒水晶ではなく、水晶修理及び持ち上げが無効、かつ、リフト中でなければ有効化
                    _liftCrystal.LiftUpEnable();
                }
            }else if(!_liftCrystal.LiftDownActiveFlag && _liftCrystal.CrystalTr != null && !_liftCrystal.StageObjCheck()){//水晶と敵が正面にない時、水晶リフト中であればリフトダウンを有効化
                _liftCrystal.LiftDownEnable();
            }
            _playerMove.NextMovePos();
            //移動先を確定させた場合、アクションを無効化
            if(_playerMove.MoveFlag()){
                if(_useCrystal.LaunchActiveFlag){
                    _useCrystal.LaunchDisable();
                }else if(_liftCrystal.LiftUpActiveFlag){
                    _liftCrystal.LiftUpDisable();
                }else if(_liftCrystal.LiftDownActiveFlag){
                    _liftCrystal.LiftDownDisable();
                }
            }
        }
        
    }

    
}
