using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public IPlayerMove _playerMove;
    public CrystalStatus[] EqueipmentCrystal{get;set;}
    [SerializeField]
    private CrystalStatus[] _equipmentCrystal = new CrystalStatus[3];

    private IUserCrystal _useCrystal;
    private ILiftCrystal _liftCrystal;
    private IRepairCrystal _repairCrystal;

    private Transform _playerTr;
    
    void Awake(){
        _playerMove = this.gameObject.GetComponent<PlayerMove>();
        _useCrystal = this.gameObject.GetComponent<UseCrystal>();
        _liftCrystal = this.gameObject.GetComponent<LiftCrystal>();
        _repairCrystal = this.gameObject.GetComponent<RepairCrystal>();

        _playerTr = this. gameObject.GetComponent<Transform>();
        AStarMap._playerCore = this;
    }

    // Update is called once per frame
    public void UpdateManager()
    {
        if(GameManager._gameOverFlag) return;

        //正面に黒水晶がなければ、水晶起動アクション無効化
        if(_useCrystal.LaunchActiveFlag && (!_useCrystal.CrystalCheck() || !_useCrystal.BlackCrystalCheck())){
            _useCrystal.LaunchDisable();
        }
        //正面に水晶がない、または、水晶リフト中であれば、水晶リフトアップアクション無効化
        if(_liftCrystal.LiftUpActiveFlag && (!_useCrystal.CrystalCheck() || _useCrystal.BlackCrystalCheck())){
            _liftCrystal.LiftUpDisable();
        }
        //リフト中でない、または、水晶と敵が正面または、その隣のマスに敵がいるまたは、範囲外へのリフトダウンになる時、水晶リフトダウンアクション無効化
        if(_liftCrystal.LiftDownActiveFlag && (_liftCrystal.CrystalTr == null || _liftCrystal.StageObjCheck() || AStarMap.OutOfReferenceCheck(new Vector2Int(AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z)))){
            _liftCrystal.LiftDownDisable();
        }
        //水晶が正面にない時、水晶修理アクション無効化
        if(_repairCrystal.RepairActiveFlag && (!_useCrystal.CrystalCheck() || _useCrystal.BlackCrystalCheck())){
            _repairCrystal.RepairDisable();
        }
        //水晶起動、修理、持ち上げアクション中か？
        //移動中か？
        if(_playerMove.MoveFlag()){
            _playerMove.Move();
            return;
        }else if(_useCrystal.LaunchActionFlag){//水晶起動中か？
            //水晶起動処理
            return;
        }else if(_repairCrystal.RepairActionFlag){//水晶修理中か？
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
                }if(!_repairCrystal.RepairActiveFlag && !_useCrystal.BlackCrystalCheck() && _liftCrystal.CrystalTr == null){//黒水晶ではなく、水晶修理及び持ち上げが無効、かつ、リフト中でなければ有効化
                    _repairCrystal.RepairEnable();
                }
            }else if(!_liftCrystal.LiftDownActiveFlag && _liftCrystal.CrystalTr != null && !_liftCrystal.StageObjCheck() && !AStarMap.OutOfReferenceCheck(new Vector2Int(AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z))){//水晶と敵が正面にないかつ、その隣のマスに敵がいない、かつステージの範囲外へのリフトダウンでない時、水晶リフト中であればリフトダウンを有効化
                _liftCrystal.LiftDownEnable();
            }
            //移動場所を設定
            _playerMove.NextMovePos();
            //移動先を確定させた場合、アクションを無効化
            if(_playerMove.MoveFlag()){
                InputInvalid();
            }
        }
    }


    //アクションを無効化
    public void InputInvalid(){
        if(_useCrystal.LaunchActiveFlag) _useCrystal.LaunchDisable();
        if(_liftCrystal.LiftUpActiveFlag) _liftCrystal.LiftUpDisable();
        if(_liftCrystal.LiftDownActiveFlag) _liftCrystal.LiftDownDisable();
        if(_repairCrystal.RepairActiveFlag) _repairCrystal.RepairDisable();
    }
}
